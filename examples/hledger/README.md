
# Example using hledger

## Prerequisites
To use Fabaceae you need the following, which is supplied for you in this example:

1. A file containing the account codes you use.
2. A bank statement in `.xlsx`-format.

This example assumes you have Hledger installed. More information on how to set it up can be found at [hledger.org](https://hledger.org/install.html).

## Instructions (from source)
To run this example, navigate to this folder and run the following. 

```

dotnet run --project ../../src/CLI/Fabaceae.CLI.csproj -- add reader DemoBank --account-name Assets:Bank --date-index 1 --description-index 2 --amount-index 3 --comment-index 4 --skip-rows 4

dotnet run --project ../../src/CLI/Fabaceae.CLI.csproj -- add accounts accounts.journal

dotnet run --project ../../src/CLI/Fabaceae.CLI.csproj -- statement.xlsx
```

You can omit all the optional parameters to `add reader` if you wish to add the reader data interactively:
```
add reader DemoBank 
```

## Instructions (using binary)

If you downloaded the binary you can reference that instead of via `dotnet run`:

```
fabaceae add reader DemoBank --account-name Assets:Bank --date-index 1 --description-index 2 --amount-index 3 --comment-index 4 --skip-rows 4

fabaceae add accounts accounts.journal

fabaceae statement.xlsx
```

## How it works

### Account codes

You need a file containing the accounts you use in your accounting. Supply a file name to the CLI using the `add accounts <path>` command and it will save it for later, or you can supply it at runtime using the `-a <path>` option. The account plan is assumed to be either in the same folder as the bank statement or the current working directory and will search for it in that order.

The account hierarchy is determined via `:` as shown in the example below:

```plain
account Assets:Bank
    
account Income:Salary

account Liabilities:CreditCard

account Expenses:Food
account Expenses:Rent
account Expenses:Utilities
account Expenses:Leisure    
account Expenses:Other
```

You are free to set this up however you like, with the exception that you need to use at least two levels of hierarchy.

### Reader

In order to properly read your bank statement you need to add it to the configuration using the `add reader` command. You will be prompted for the different values required, or you can supply them via options (use `add reader -h` for details). The config will be saved under the supplied name so that you can easily use it again.

### Output

When you have selected an account code for each of the transactions in your statement, a file will be created named after the earliest date in your statement followed by the reader name (`2022-12-30.demobank.journal`). You need to import this file in your PTA journal to be able to generate reports.

### Report

To generate a simple expense report from the result of this file, you can run:

```
hledger balance -f 2022-12-30.demobank.journal Expenses
```