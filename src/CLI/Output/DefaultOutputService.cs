namespace Fabaceae.CLI;

public class DefaultOutputService : IOutputService
{
    private readonly string _outputFileName;

    public DefaultOutputService(string outputFileName)
    {
        _outputFileName = outputFileName;
    }

    public async Task WriteOutputAsync(IEnumerable<KeyValuePair<Post, IAccount>> accountCodedPosts)
    {
        var output = accountCodedPosts.SelectMany(acp =>
        {
            var (post, account) = (acp.Key, acp.Value);

            var comment = string.IsNullOrWhiteSpace(post.Comment) ? string.Empty : $"; {post.Comment}";

            return new[] {
                $"{post.Date:yyyy-MM-dd} {post.Description}",
                $"\t{post.Account}\t\t{post.Amount:F2}{comment}",
                "\t" + account.FullName,
                string.Empty
            };
        });

        await File.WriteAllLinesAsync(_outputFileName, output);
    }

    public bool OutputFileExists => File.Exists(_outputFileName);
}