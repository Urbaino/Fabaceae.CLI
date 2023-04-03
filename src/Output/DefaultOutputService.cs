public class DefaultOutputService : IOutputService
{
    private string OutputFileName;

    public DefaultOutputService(string outputFileName)
    {
        OutputFileName = outputFileName;
    }

    public async Task WriteOutputAsync(IEnumerable<KeyValuePair<Post, IAccount>> accountCodedPosts)
    {
        var output = accountCodedPosts.SelectMany(acp =>
        {
            var (post, account) = (acp.Key, acp.Value);

            var comment = string.IsNullOrWhiteSpace(post.Comment) ? string.Empty : $"; {post.Comment}";

            return new[] {
                $"{post.Date.ToShortDateString()} {post.Description}",
                $"\t{post.Account}\t\t{post.Amount:F2}{comment}",
                "\t" + account.FullName,
                string.Empty
            };
        });

        await File.WriteAllLinesAsync(OutputFileName, output);
    }

    public bool OutputFileExists => File.Exists(OutputFileName);

}