using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

public sealed class ParseCommandSettings : CommandSettings
{
    [Description("Name of reader config to use.")]
    [CommandOption("-r|--reader")]
    public string Reader { get; init; } = string.Empty;

    [Description("Account plan file name to use.")]
    [CommandOption("-a|--account-plan")]
    public string AccountPlanFileName { get; init; } = string.Empty;

    [Description("Path to excel file.")]
    [CommandArgument(0, "<path>")]
    public string Path { get; init; } = string.Empty;

    public override ValidationResult Validate()
    {
        return base.Validate();
    }
}
