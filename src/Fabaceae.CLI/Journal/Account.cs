public interface IAccount
{
    string Name { get; }
    string FullName { get; }
    IReadOnlyCollection<IAccount> SubAccounts { get; }
    IAccount AddSubAccount(string subAccount);
    IAccount? this[string accountName] { get; }
}

internal sealed class Account : IAccount
{
    public Account() { }
    private Account(string name, string parentFullName)
    {
        Name = name;
        ParentFullName = parentFullName;
    }

    private string ParentFullName { get; } = string.Empty;
    private IList<IAccount> subAccounts = new List<IAccount>();

    public string Name { get; } = string.Empty;
    public string FullName => $"{ParentFullName}:{Name}".TrimStart(':');
    public IReadOnlyCollection<IAccount> SubAccounts => subAccounts.AsReadOnly();

    public IAccount AddSubAccount(string subAccount)
    {
        var account = new Account(subAccount, FullName);
        subAccounts.Add(account);
        return account;
    }

    public IAccount? this[string accountName]
    {
        get => subAccounts.FirstOrDefault(a => a.Name.Equals(accountName, StringComparison.OrdinalIgnoreCase));
    }
}
