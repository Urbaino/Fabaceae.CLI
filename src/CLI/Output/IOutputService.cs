namespace Fabaceae.CLI;

internal interface IOutputService
{
    Task WriteOutputAsync(IEnumerable<KeyValuePair<Post, IAccount>> accountCodedTransactions);
    bool OutputFileExists { get; }
}