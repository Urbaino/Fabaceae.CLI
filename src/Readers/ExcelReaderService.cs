internal interface IExcelReaderService
{
    ExcelReaderConfig? GetReader(string readerParam);
    IEnumerable<ExcelReaderConfig> Readers { get; }
    Task AddReaderAsync(ExcelReaderConfig config, CancellationToken cancellationToken = default);
}

internal sealed class ExcelReaderService : IExcelReaderService
{
    private readonly IConfigurationService ConfigurationService;

    public ExcelReaderService(IConfigurationService configurationService)
    {
        ConfigurationService = configurationService;
    }

    public async Task AddReaderAsync(ExcelReaderConfig config, CancellationToken cancellationToken = default)
    {
        await ConfigurationService.UpdateConfiguration(c => c.Readers.Add(config), cancellationToken);
    }

    public ExcelReaderConfig? GetReader(string readerParam)
        => Readers.FirstOrDefault(r => r.name.Equals(readerParam, StringComparison.OrdinalIgnoreCase));

    public IEnumerable<ExcelReaderConfig> Readers => ConfigurationService.Configuration.Readers;
}
