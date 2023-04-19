namespace Fabaceae.CLI;

using ClosedXML.Excel;

internal sealed class ExcelReader
{
    private readonly IXLWorksheet _sheet;
    private readonly ExcelReaderConfig _config;

    internal ExcelReader(IXLWorksheet sheet, ExcelReaderConfig config)
    {
        _sheet = sheet;
        _config = config;
    }

    public Journal BuildJournal(IAccount accountRoot)
        => new(accountRoot, _config.Name,
            _sheet.Rows().Skip(_config.SkipRows).Select(row =>
                new Post(
                    row.Cell(_config.DateColumnIndex).GetString(),
                    row.Cell(_config.DescriptionColumnIndex).GetString(),
                    decimal.Parse(row.Cell(_config.AmountColumnIndex).GetString()),
                    _config.AccountName,
                    _config.CommentColumnIndex.HasValue ? row.Cell(_config.CommentColumnIndex.Value).GetString() : string.Empty
                )
            ).ToArray()
        );
}
