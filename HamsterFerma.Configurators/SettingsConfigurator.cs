using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace NeuroMonitoring.Configurators;

public static class SettingsConfigurator
{
    public static void Configure(IHostApplicationBuilder builder)
    {
        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);
    }
}
