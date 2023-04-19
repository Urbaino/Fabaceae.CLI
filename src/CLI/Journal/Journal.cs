namespace Fabaceae.CLI;

internal sealed class Journal
{
    public Journal(IAccount accounts, string fileType, IReadOnlyCollection<Post> posts)
    {
        AccountRoot = accounts;
        FileType = fileType;
        Posts = posts;
    }

    public IAccount AccountRoot { get; }
    public string FileType { get; }
    public IReadOnlyCollection<Post> Posts { get; }

    public string OutputFileName => $"{Posts.First().Date.ToShortDateString()}.{FileType.ToLowerInvariant()}.journal";
}
