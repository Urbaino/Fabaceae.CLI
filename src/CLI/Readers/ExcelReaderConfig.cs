namespace Fabaceae.CLI;

public record ExcelReaderConfig(
    string Name,
    string AccountName,
    int DateColumnIndex = 0,
    int DescriptionColumnIndex = 1,
    int AmountColumnIndex = 3,
    int? CommentColumnIndex = null,
    int SkipRows = 0
);