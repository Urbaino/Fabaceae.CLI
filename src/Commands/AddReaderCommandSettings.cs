using System.ComponentModel;
using Spectre.Console.Cli;

// TODO: Enforce positive indices

public class AddReaderCommandSettings : CommandSettings
{
    [Description("Reader name")]
    [CommandArgument(0, "<name>")]
    public string Name { get; init; } = string.Empty;

    [Description("Account to credit")]
    [CommandOption("--account-name")]
    public string? AccountName { get; init; }

    [Description("Date column")]
    [CommandOption("--date-index")]
    public int? DateColumnIndex { get; init; }

    [Description("Description column")]
    [CommandOption("--description-index")]
    public int? DescriptionColumnIndex { get; init; }

    [Description("Amount column")]
    [CommandOption("--amount-index")]
    public int? AmountColumnIndex { get; init; }

    [Description("Comment column")]
    [CommandOption("--comment-index")]
    public int? CommentColumnIndex { get; init; }

    [Description("Rows to skip")]
    [CommandOption("--skip-rows")]
    public int? SkipRows { get; init; }
}