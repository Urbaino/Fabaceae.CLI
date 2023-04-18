using System.Diagnostics.CodeAnalysis;
using ClosedXML.Excel;
using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.Console.Rendering;

internal sealed class ParseCommand : AsyncCommand<ParseCommandSettings>
{
    private readonly IExcelReaderService _readerService;
    private readonly AccountFactory _accountFactory;
    private readonly OutputServiceFactory _outputServiceFactory;

    public ParseCommand(IExcelReaderService readerService, AccountFactory accountFactory, OutputServiceFactory outputServiceFactory)
    {
        _readerService = readerService;
        _accountFactory = accountFactory;
        _outputServiceFactory = outputServiceFactory;
    }

    public override async Task<int> ExecuteAsync([NotNull] CommandContext context, [NotNull] ParseCommandSettings settings)
    {
        try
        {
            AnsiConsole.Write(Intro());

            var accountPlan = await AnsiConsole.Status()
                .Spinner(Spinner.Known.Moon)
                .StartAsync("Reading accounts...", ctx => _accountFactory.CreateAsync(PTAEngine.HLedger, Path.GetDirectoryName(settings.Path) ?? string.Empty, settings.AccountPlanFileName));

            var sheet = await AnsiConsole.Status()
                .Spinner(Spinner.Known.Moon)
                .StartAsync("Reading spreadsheet...", ctx => Task.Run(() =>
                {
                    var workbook = new XLWorkbook(settings.Path);
                    return workbook.Worksheets.First();
                }));

            Journal? journal = null;
            while (journal == null)
            {
                try
                {
                    var reader = DetermineReader(settings.Reader);
                    AnsiConsole.Write(new Rows(new Text($"Reading spreadsheet using '{reader.name}'", new Style(Color.Yellow))));
                    journal = new ExcelReader(sheet, reader).BuildJournal(accountPlan);
                }
                catch (Exception ex)
                {
                    AnsiConsole.Write(new Rows(
                        new Text("Something went wrong when trying to read the spreadsheet:", new Style(Color.Yellow)),
                        new Text(ex.Message, new Style(Color.Red)),
                        new Text("Please select another reader.", new Style(Color.Yellow))
                    ));
                }
            }

            var outputService = _outputServiceFactory.Create(PTAEngine.Default, journal);

            AnsiConsole.Write(SetupSummary(settings, journal, outputService));

            // Check if file exists and ask user to proceed
            if (outputService.OutputFileExists)
            {
                if (!AnsiConsole.Prompt(new ConfirmationPrompt("File already exists, do you wish to overwrite it?")))
                {
                    return 0;
                }
            }
            else
            {
                AnsiConsole.Prompt(new TextPrompt<string>("Press enter to begin.").AllowEmpty().Secret(null));
            }

            // Start assigning account codes
            var count = 0;
            var accountCodedPosts = new Dictionary<Post, IAccount>();
            foreach (var post in journal.Posts)
            {
                AnsiConsole.Clear();
                AnsiConsole.Write(Progress(journal, post, count));
                accountCodedPosts.Add(post, AnsiConsole.Prompt(AccountPrompt(journal)));
                ++count;
            }

            // Write the result to file
            await outputService.WriteOutputAsync(accountCodedPosts);

            // Display summary and prompt for exit
            AnsiConsole.Clear();
            AnsiConsole.Write(Summary(journal, accountCodedPosts));
            AnsiConsole.Prompt(new TextPrompt<string>("Press enter to exit.").AllowEmpty().Secret(null));

            return 0;
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteException(ex);
            AnsiConsole.Prompt(new TextPrompt<string>("Press enter to exit.").AllowEmpty().Secret(null));
            return 1;
        }
    }

    private Rows Intro()
        => new Rows(
            new FigletText("Fabaceae")
                .LeftJustified()
                .Color(Color.Cyan1),
            new Text("Time to do some accounting!", new Style(Color.Cyan1))
        );

    private Grid SetupSummary(ParseCommandSettings settings, Journal journal, IOutputService outputService)
        => new Grid().AddColumns(2)
            .AddRow(
                new Text("File", new Style(Color.Yellow)),
                new Text(settings.Path, new Style(Color.Green))
            )
            .AddRow(
                new Text("Statement type", new Style(Color.Yellow)),
                new Text(journal.FileType, new Style(Color.Green))
            )
            .AddRow(
                new Text("Posts", new Style(Color.Yellow)),
                new Text(journal.Posts.Count.ToString(), new Style(Color.Green))
              )
            .AddRow(
                new Text("Output filename", new Style(Color.Yellow)),
                new Text(journal.OutputFileName, new Style(outputService.OutputFileExists ? Color.Red : Color.Green))
            );

    private IRenderable Progress(Journal journal, Post post, int count)
        => new Rows(
            new BarChart()
                .Width(60)
                .AddItem("Posts", journal.Posts.Count, Color.Green)
                .AddItem("Done", count, Color.Green1),
            new Grid().AddColumns(3).AddRow(new[] {
                new Text(post.Date.ToShortDateString(), new Style(Color.Yellow)),
                new Text(post.Description, new Style(Color.Cyan1)),
                new Text(post.Amount.ToString("C"), new Style(post.Amount > 0 ? Color.Green : Color.Red))
            }));

    private IRenderable Summary(Journal journal, IEnumerable<KeyValuePair<Post, IAccount>> accountCodedPosts)
    {
        var debit = accountCodedPosts.Where(acp => acp.Key.Amount > 0);
        var credit = accountCodedPosts.Where(acp => acp.Key.Amount < 0);

        var net = accountCodedPosts.Sum(acp => acp.Key.Amount);

        var panel = new Panel(new Rows(
            new Grid().AddColumns(2)
                .AddRow(
                    new Text("Total posts", new Style(Color.Yellow)),
                    new Text(accountCodedPosts.Count().ToString(), new Style(Color.Yellow))
                )
                .AddRow(
                    new Text("Total debit", new Style(Color.Yellow)),
                    new Text(debit.Sum(acp => acp.Key.Amount).ToString("C"), new Style(Color.Green))
                )
                .AddRow(
                    new Text("Total credit", new Style(Color.Yellow)),
                    new Text(credit.Sum(acp => acp.Key.Amount).ToString("C"), new Style(Color.Red))
                )
                .AddRow(
                    new Text("Net", new Style(Color.Yellow)),
                    new Text(net.ToString("C"), new Style(net < 0 ? Color.Red : Color.Green))
                ),
            new Rule("Debit breakdown:"),
            new BreakdownChart().UseValueFormatter(d => d.ToString("C")).AddItems(ToBreakdownItems(GroupByAccount(debit), new[] { Color.Green, Color.Yellow, Color.Cyan1 })),
            new Rule("Credit breakdown:"),
            new BreakdownChart().UseValueFormatter(d => d.ToString("C")).AddItems(ToBreakdownItems(GroupByAccount(credit), new[] { Color.Red, Color.Orange1, Color.Blue }))
        )).Expand();

        panel.Header = new PanelHeader("All done!");

        return panel;

        IEnumerable<IGrouping<string, decimal>> GroupByAccount(IEnumerable<KeyValuePair<Post, IAccount>> accountCodedPosts)
            => accountCodedPosts.GroupBy(acp => acp.Value.FullName, g => g.Key.Amount);

        IEnumerable<BreakdownChartItem> ToBreakdownItems(IEnumerable<IGrouping<string, decimal>> items, IReadOnlyList<Color> colors)
           => items.Select((g, i) => new BreakdownChartItem(g.Key, (double)g.Sum(), colors[i % colors.Count]));
    }

    private SelectionPrompt<IAccount> AccountPrompt(Journal journal)
    {
        var prompt = new SelectionPrompt<IAccount>()
            .Title("Select account")
            .WrapAround()
            .Mode(SelectionMode.Leaf)
            .UseConverter(a => a.Name)
            .MoreChoicesText("[grey](Scroll for more)[/]");
        foreach (var account in journal.AccountRoot.SubAccounts)
        {
            prompt.AddChoiceGroup(account, account.SubAccounts);
        }

        return prompt;
    }

    private ExcelReaderConfig DetermineReader(string readerParam)
    {
        if (string.IsNullOrWhiteSpace(readerParam))
        {
            return AnsiConsole.Prompt(ReaderSelector());
        }

        var reader = _readerService.GetReader(readerParam);
        if (reader != null)
        {
            return reader;
        }

        AnsiConsole.MarkupLine($"[red]Selected reader could not be found in config.[/]");
        return AnsiConsole.Prompt(ReaderSelector());
    }

    private SelectionPrompt<ExcelReaderConfig> ReaderSelector()
        => new SelectionPrompt<ExcelReaderConfig>()
            .Title("Which reader do you want to use?")
            .WrapAround()
            .PageSize(12)
            .Mode(SelectionMode.Leaf)
            .MoreChoicesText("[grey](Scroll for more)[/]")
            .AddChoices(_readerService.Readers)
            .UseConverter(r => r.name);
}
