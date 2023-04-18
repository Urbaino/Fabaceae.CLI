using ClosedXML.Excel;

internal sealed class ExcelReader
{
    private readonly IXLWorksheet Sheet;
    private readonly ExcelReaderConfig Config;

    internal ExcelReader(IXLWorksheet sheet, ExcelReaderConfig config)
    {
        Sheet = sheet;
        Config = config;
    }

    public Journal BuildJournal(IAccount accountRoot)
        => new Journal(accountRoot, Config.name,
            Sheet.Rows().Skip(Config.skipRows).Select(row =>
                new Post(
                    row.Cell(Config.dateColumnIndex).GetString(),
                    row.Cell(Config.descriptionColumnIndex).GetString(),
                    decimal.Parse(row.Cell(Config.amountColumnIndex).GetString()),
                    Config.accountName,
                    Config.commentColumnIndex.HasValue ? row.Cell(Config.commentColumnIndex.Value).GetString() : string.Empty
                )
            ).ToArray()
        );
}
