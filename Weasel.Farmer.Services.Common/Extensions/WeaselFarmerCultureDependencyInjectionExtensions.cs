using Microsoft.Extensions.Hosting;
using System.Globalization;

namespace Weasel.Farmer.Services.Common.Extensions;

public static class WeaselFarmerCultureDependencyInjectionExtensions
{
    public static void ConfigureRussianCulture(this IHostApplicationBuilder builder)
    {
        CultureInfo cultureInfo = new CultureInfo("ru-RU");
        cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
        cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
        cultureInfo.NumberFormat.PercentDecimalSeparator = ".";
        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
    }
}
