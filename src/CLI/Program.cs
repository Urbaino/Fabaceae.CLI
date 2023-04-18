using System.Globalization;
using Fabaceae.CLI;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

var culture = CultureInfo.CreateSpecificCulture("sv-SE");
culture.NumberFormat.NegativeSign = "-";
Thread.CurrentThread.CurrentCulture = culture;

var registrations = new ServiceCollection();
registrations.AddTransient<IConfigurationService, ConfigurationService>();
registrations.AddTransient<IExcelReaderService, ExcelReaderService>();
registrations.AddTransient<OutputServiceFactory>();
registrations.AddTransient<AccountFactory>();

var app = new CommandApp<ParseCommand>(new TypeRegistrar(registrations));
app.Configure(c =>
{
    c.AddBranch("add", add =>
    {
        add.AddCommand<AddReaderCommand>("reader").WithDescription("Add a new Excel configuration.");
        add.AddCommand<AddAccountPlanCommand>("accounts").WithDescription("Set account plan file name.");
    });
});

return app.Run(args);
