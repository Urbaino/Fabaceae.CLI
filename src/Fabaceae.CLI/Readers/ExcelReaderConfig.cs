public record ExcelReaderConfig(
    string name,
    string accountName,
    int dateColumnIndex = 0,
    int descriptionColumnIndex = 1,
    int amountColumnIndex = 3,
    int? commentColumnIndex = null,
    int skipRows = 0
);