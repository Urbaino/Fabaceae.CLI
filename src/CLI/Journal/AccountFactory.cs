namespace Fabaceae.CLI;
internal sealed class AccountFactory
{
    private readonly IConfigurationService _configurationService;

    public AccountFactory(IConfigurationService configurationService)
    {
        _configurationService = configurationService;
    }

    public Task<Account> CreateAsync(PTAEngine type, string directory, string filenameOverride = "")
        => type switch
        {
            PTAEngine.HLedger => CreateFromHLedger(directory, filenameOverride),
            _ => throw new NotImplementedException($"Invalid {nameof(PTAEngine)}")
        };

    private async Task<Account> CreateFromHLedger(string directory, string filenameOverride)
    {
        var fileName = string.IsNullOrWhiteSpace(filenameOverride)
            ? _configurationService.Configuration.AccountPlanFileName
            : filenameOverride;

        var path = Path.Join(directory, fileName);
        if (!File.Exists(path))
        {
            path = fileName;
            if (!File.Exists(path)) throw new FileNotFoundException("Missing Account Plan");
        }

        using (var kontoFile = File.OpenRead(path))
        {
            var rootAccount = new Account();
            var accountPlan = new StreamReader(kontoFile);

            while (!accountPlan.EndOfStream)
            {
                var line = await accountPlan.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line)) continue;

                var account = RegexExpressions.Account().Match(line);
                if (!account.Groups[0].Success) continue;
                var captures = account.Groups[1].Captures;

                IAccount parent = rootAccount;
                for (var i = 0; i < captures.Count; ++i)
                {
                    var accountName = captures[i].Value;
                    parent = parent[accountName] ?? parent.AddSubAccount(accountName);
                }
            }

            return rootAccount;
        }
    }
}
