# `Fabaceae`

Following A Budget using Account Codes, Easy and Accessible to Everyone! Fabaceae is also the latin name for the Bean family of plants (shout-out to Beancount).

This project aims to simplify the process of extracting data from your bank and using it with your Plain Text Accounting (PTA) engine of choice. These tools help you analyse your data and create reports, but the process of adding that data can be tedious. Just tell `Fabaceae` where to find your accounts and how your bank statement is structured and from that day you can drag and drop your bank statements and it will be ready for analysis in no time!

I wanted to create a tool using the beautiful library [`Spectre.Console`](https://github.com/spectreconsole) that could help me quickly translate my bank statements to `.journal` files, and this is the result. I am glad that you found your way here, and I hope that it will help you too! 

If you are new to PTA, you can read more at https://plaintextaccounting.org.

## Features

- Read bank statements from `.xlsx` files.
- Load your accounts from `.ledger` files.
- Interactively match accounts with your transactions.
- Output the results to a `.ledger` file.

Also:
- Persistent config - set it up once, save time later.
- Save formats of different bank statements for easy reuse.

## Usage
 
### Setup


For a working example, please see the [example using hledger](examples/hledger/README.md).

### Running

Just supply your bank statement path as parameter and you are good to go! This also means that you can drag-and-drop the bank statement onto the executable if you are so inclined (only one at a time though).

Optionally you can supply the name of the reader you wish to use via the `-r <reader>` option, otherwise you will be prompted. Use `-h` for details.

## Building 

To build and run it from source you need the [.NET 7 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0). 

To build and run, execute the following commands in the root directory.

```
dotnet build src
dotnet run --project ./src/CLI/Fabaceae.CLI.csproj -- -h
```

If you want to deploy and use it standalone (on windows):

```
dotnet publish ./src/CLI --output=out --configuration=Release --runtime=win-x64 --self-contained -p:PublishSingleFile=true 
```

### Visual Studio Code

If you use VSCode and have Docker installed you can skip installing the SDK and instead set up a Development Container using the extension [ms-vscode-remote.remote-containers](https://marketplace.visualstudio.com/items?itemName=ms-vscode-remote.remote-containers). Simply install the extension and clone the repository and you will be asked if you wish to open the repository in a container using the provided configuration.

There are VSCode tasks set up for build and publish that you can use.

## Upcoming features

### CI and Releases

There will be binaries available for those who just want to use the tool.

### Tests

Obviously.

### Wider support for specific tools

Reading account definitions from Beancount, for example.