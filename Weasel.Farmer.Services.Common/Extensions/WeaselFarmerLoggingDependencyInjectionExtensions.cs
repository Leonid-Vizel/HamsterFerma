using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Weasel.Farmer.Services.Common.Extensions;

public static class WeaselFarmerLoggingDependencyInjectionExtensions
{
    public static void AddStandartLogging(this IHostApplicationBuilder builder)
    {
        if (builder.Environment.IsProduction())
        {
            builder.Logging.AddSystemdConsole();
        }
        else
        {
            builder.Logging.AddConsole();
        }
    }
}
