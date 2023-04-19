namespace Fabaceae.Tests.CLI.RegexExpressions;

using Xunit;

public class Account
{
    [Theory]
    [InlineData("account Assets", 1)]
    [InlineData("account Expenses", 1)]
    [InlineData("account Income", 1)]
    [InlineData("account Liabilites", 1)]
    [InlineData("account Assets:Bank", 2)]
    [InlineData("account Expenses:Food", 2)]
    [InlineData("account Income:Salary", 2)]
    [InlineData("account Liabilites:CreditCard", 2)]
    public void Should_Match(string accountName, int levels)
    {
        var match = Fabaceae.CLI.RegexExpressions.Account().Match(accountName);

        Assert.True(match.Success);
        Assert.Equal(levels, match.Groups[1].Captures.Count);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("account")]
    [InlineData("account ")]
    [InlineData("account Expenses:")]
    [InlineData("account :Income")]
    public void Should_Not_Match(string accountName)
    {
        var match = Fabaceae.CLI.RegexExpressions.Account().Match(accountName);

        Assert.False(match.Success, accountName);
    }

}