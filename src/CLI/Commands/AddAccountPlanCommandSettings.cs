namespace Fabaceae.CLI;

using System.ComponentModel;
using Spectre.Console.Cli;

public sealed class AddAccountPlanCommandSettings : CommandSettings
{
    [Description("Ledger file name.")]
    [CommandArgument(0, "<file-name>")]
    public string FileName { get; init; } = string.Empty;
}
