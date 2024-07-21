using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Weasel.Farmer.Services.Common.Extensions;

public static class WeaselFarmerSettingsDependencyInjectionExtensions
{
    public static void AddStandartAppSettings(this IHostApplicationBuilder builder)
    {
        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);
    }
}
