using System.Diagnostics.CodeAnalysis;
using Spectre.Console;
using Spectre.Console.Cli;

internal sealed class AddReaderCommand : AsyncCommand<AddReaderCommandSettings>
{
    private readonly IExcelReaderService ReaderService;

    public AddReaderCommand(IExcelReaderService readerService)
    {
        ReaderService = readerService;
    }

    public override async Task<int> ExecuteAsync([NotNull] CommandContext context, [NotNull] AddReaderCommandSettings settings)
    {
        if (ReaderService.GetReader(settings.Name) != null) throw new Exception($"Cannot add reader. A reader named {settings.Name} already exists!");

        var config = new ExcelReaderConfig(
                    name: settings.Name,
                    accountName: settings.AccountName ?? AnsiConsole.Prompt(new TextPrompt<string>("Which account should be used for credit?")),
                    skipRows: settings.SkipRows ?? AnsiConsole.Prompt(new TextPrompt<int>("How many rows to skip before the first transaction?").DefaultValue(0)),
                    dateColumnIndex: settings.DateColumnIndex ?? AnsiConsole.Prompt(new TextPrompt<int>("Which column holds the dates?").DefaultValue(1)),
                    amountColumnIndex: settings.AmountColumnIndex ?? AnsiConsole.Prompt(new TextPrompt<int>("Which column holds the amounts?").DefaultValue(2)),
                    descriptionColumnIndex: settings.DescriptionColumnIndex ?? AnsiConsole.Prompt(new TextPrompt<int>("Which column holds the descriptions?").DefaultValue(3)),
                    commentColumnIndex: settings.CommentColumnIndex ?? AnsiConsole.Prompt(new TextPrompt<int?>("Which column do you want to use for comments? Leave blank for none.").DefaultValue(null))
                );

        await ReaderService.AddReaderAsync(config);
        AnsiConsole.MarkupLine($"[cyan1]Reader {config.name} added![/]");

        return 0;
    }
}