using Fabaceae.CLI;
using FakeItEasy;
using Snapshooter.Xunit;
using Xunit;

namespace Fabaceae.Tests.CLI.Output.DefaultOutputService;

public sealed class WriteOutputAsync
{

    private string _filename = "out.journal";
    private Fabaceae.CLI.DefaultOutputService SUT => new(_filename);

    [Fact]
    public async Task Should_Write_Content_To_File()
    {
        // Given
        _filename = "demo.2023-01-01.2023-01-23.journal";
        var posts = new[] {
            PostWithAccount("2023-01-01", "First", -23.41M, "Assets:Bank", "Expenses:Food", "Something"),
            PostWithAccount("2023-01-02", "Second", -433.22M, "Assets:Bank", "Expenses:Car", "Something else"),
            PostWithAccount("2023-01-03", "Third", 1023.4M, "Assets:Bank", "Income:Salary", "Something else entirely"),
            PostWithAccount("2023-01-04", "Fourth", -120.5M, "Assets:Bank", "Expenses:Beer", "Something completely different")
        };

        // When
        await SUT.WriteOutputAsync(posts);

        // Then
        var file = await File.ReadAllTextAsync(_filename);
        Snapshot.Match(file);
    }

    private static KeyValuePair<Post, IAccount> PostWithAccount(string date, string description, decimal amount, string debitAccount, string creditAccount, string comment)
    {
        var post = new Post(date, description, amount, debitAccount, comment);
        var account = A.Fake<IAccount>();
        A.CallTo(() => account.FullName).Returns(creditAccount);
        return new(post, account);
    }
}