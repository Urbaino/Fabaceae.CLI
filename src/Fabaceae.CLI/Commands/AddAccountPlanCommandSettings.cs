using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

public sealed class AddAccountPlanCommandSettings : CommandSettings
{
    [Description("Ledger file name.")]
    [CommandArgument(0, "<file-name>")]
    public string FileName { get; init; } = string.Empty;

    public override ValidationResult Validate()
    {
        return base.Validate();
    }
}
