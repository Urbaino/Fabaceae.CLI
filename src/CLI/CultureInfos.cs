namespace Fabaceae.CLI;

using System.Globalization;

public static class CultureInfos
{
    public static CultureInfo Default
    {
        get
        {
            var culture = CultureInfo.CreateSpecificCulture("sv-SE");
            culture.NumberFormat.NegativeSign = "-";
            return culture;
        }
    }
}