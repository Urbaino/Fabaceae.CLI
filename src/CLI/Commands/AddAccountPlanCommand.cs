namespace Fabaceae.CLI;

using System.Diagnostics.CodeAnalysis;
using Spectre.Console;
using Spectre.Console.Cli;

internal sealed class AddAccountPlanCommand : AsyncCommand<AddAccountPlanCommandSettings>
{
    private readonly IConfigurationService _configuration;

    public AddAccountPlanCommand(IConfigurationService configuration)
    {
        _configuration = configuration;
    }

    public override async Task<int> ExecuteAsync([NotNull] CommandContext context, [NotNull] AddAccountPlanCommandSettings settings)
    {
        await _configuration.UpdateConfiguration(c => c.AccountPlanFileName = settings.FileName);

        AnsiConsole.MarkupLine($"[cyan1]Account plan file name saved! Now[/] [yellow]{settings.FileName}[/][cyan1]![/]");

        return 0;
    }
}
