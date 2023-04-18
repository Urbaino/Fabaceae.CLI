namespace Fabaceae.CLI;

internal interface IExcelReaderService
{
    ExcelReaderConfig? GetReader(string readerParam);
    IEnumerable<ExcelReaderConfig> Readers { get; }
    Task AddReaderAsync(ExcelReaderConfig config, CancellationToken cancellationToken = default);
}

internal sealed class ExcelReaderService : IExcelReaderService
{
    private readonly IConfigurationService _configurationService;

    public ExcelReaderService(IConfigurationService configurationService)
    {
        _configurationService = configurationService;
    }

    public async Task AddReaderAsync(ExcelReaderConfig config, CancellationToken cancellationToken = default)
    {
        await _configurationService.UpdateConfiguration(c => c.Readers.Add(config), cancellationToken);
    }

    public ExcelReaderConfig? GetReader(string readerParam)
        => Readers.FirstOrDefault(r => r.Name.Equals(readerParam, StringComparison.OrdinalIgnoreCase));

    public IEnumerable<ExcelReaderConfig> Readers => _configurationService.Configuration.Readers;
}
