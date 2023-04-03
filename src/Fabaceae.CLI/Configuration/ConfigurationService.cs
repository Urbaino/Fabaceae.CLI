using System.Text.Json;

public interface IConfigurationService
{
    IConfigState Configuration { get; }
    Task UpdateConfiguration(Action<ConfigState> action, CancellationToken cancellationToken = default);
}

internal class ConfigurationService : IConfigurationService
{
    private readonly string _configPath;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };

    public ConfigurationService()
    {
        _configPath = Path.Combine(System.AppContext.BaseDirectory, "config.json");
        _configuration = ReadConfiguration();
    }

    private ConfigState _configuration;
    public IConfigState Configuration => _configuration;

    public async Task UpdateConfiguration(Action<ConfigState> action, CancellationToken cancellationToken = default)
    {
        action(_configuration);
        await File.WriteAllTextAsync(_configPath, JsonSerializer.Serialize(Configuration, _jsonSerializerOptions), cancellationToken);
    }

    private ConfigState ReadConfiguration()
    {
        if (!File.Exists(_configPath)) return new ConfigState();

        var json = File.ReadAllText(_configPath);
        return JsonSerializer.Deserialize<ConfigState>(json, _jsonSerializerOptions) ?? throw new Exception("Failed to parse config!");
    }
}