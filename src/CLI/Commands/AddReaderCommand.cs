namespace Fabaceae.CLI;

using System.Diagnostics.CodeAnalysis;
using Spectre.Console;
using Spectre.Console.Cli;

internal sealed class AddReaderCommand : AsyncCommand<AddReaderCommandSettings>
{
    private readonly IExcelReaderService _readerService;

    public AddReaderCommand(IExcelReaderService readerService)
    {
        _readerService = readerService;
    }

    public override async Task<int> ExecuteAsync([NotNull] CommandContext context, [NotNull] AddReaderCommandSettings settings)
    {
        if (_readerService.GetReader(settings.Name) != null) throw new Exception($"Cannot add reader. A reader named {settings.Name} already exists!");

        var config = new ExcelReaderConfig(
            Name: settings.Name,
            AccountName: settings.AccountName ?? AnsiConsole.Prompt(new TextPrompt<string>("Which account should be used for credit?")),
            DateColumnIndex: settings.DateColumnIndex ?? AnsiConsole.Prompt(new TextPrompt<int>("Which column holds the dates?").DefaultValue(1)),
            DescriptionColumnIndex: settings.DescriptionColumnIndex ?? AnsiConsole.Prompt(new TextPrompt<int>("Which column holds the descriptions?").DefaultValue(3)),
            AmountColumnIndex: settings.AmountColumnIndex ?? AnsiConsole.Prompt(new TextPrompt<int>("Which column holds the amounts?").DefaultValue(2)),
            CommentColumnIndex: settings.CommentColumnIndex ?? AnsiConsole.Prompt(new TextPrompt<int?>("Which column do you want to use for comments? Leave blank for none.").DefaultValue(null)),
            SkipRows: settings.SkipRows ?? AnsiConsole.Prompt(new TextPrompt<int>("How many rows to skip before the first transaction?").DefaultValue(0))
        );

        await _readerService.AddReaderAsync(config);
        AnsiConsole.MarkupLine($"[cyan1]Reader {config.Name} added![/]");

        return 0;
    }
}