namespace Fabaceae.CLI;

internal class OutputServiceFactory
{
    public IOutputService Create(PTAEngine type, Journal journal)
        => type switch
        {
            PTAEngine.Default => new DefaultOutputService(journal.OutputFileName),
            _ => throw new NotImplementedException($"Invalid {nameof(PTAEngine)}")
        };
}
