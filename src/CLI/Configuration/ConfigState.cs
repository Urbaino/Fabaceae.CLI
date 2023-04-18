namespace Fabaceae.CLI;

public interface IConfigState
{
    string AccountPlanFileName { get; }
    IReadOnlyCollection<ExcelReaderConfig> Readers { get; }
}

public class ConfigState : IConfigState
{
    public string AccountPlanFileName { get; set; } = string.Empty;
    public IList<ExcelReaderConfig> Readers { get; set; } = new List<ExcelReaderConfig>();

    IReadOnlyCollection<ExcelReaderConfig> IConfigState.Readers => Readers.AsReadOnly();
}