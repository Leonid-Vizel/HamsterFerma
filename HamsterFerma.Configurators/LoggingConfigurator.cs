using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace NeuroMonitoring.Configurators;

public static class LoggingConfigurator
{
    public static void Configure(IHostApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
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
