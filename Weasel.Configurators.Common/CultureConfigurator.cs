using System.Globalization;

namespace Weasel.Configurators.Common;

public static class CultureConfigurator
{
    public static void Configure()
    {
        CultureInfo cultureInfo = new CultureInfo("ru-RU");
        cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
        cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
        cultureInfo.NumberFormat.PercentDecimalSeparator = ".";
        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
    }
}
