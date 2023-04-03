using System.Diagnostics.CodeAnalysis;
using Spectre.Console;
using Spectre.Console.Cli;

internal sealed class AddAccountPlanCommand : AsyncCommand<AddAccountPlanCommandSettings>
{
    private readonly IConfigurationService Configuration;

    public AddAccountPlanCommand(IConfigurationService configuration)
    {
        Configuration = configuration;
    }

    public override async Task<int> ExecuteAsync([NotNull] CommandContext context, [NotNull] AddAccountPlanCommandSettings settings)
    {
        await Configuration.UpdateConfiguration(c => c.AccountPlanFileName = settings.FileName);

        AnsiConsole.MarkupLine($"[cyan1]Account plan file name saved! Now[/] [yellow]{settings.FileName}[/][cyan1]![/]");

        return 0;
    }

}
