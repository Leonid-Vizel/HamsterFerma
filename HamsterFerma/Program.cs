using HamsterFerma.Configurators;
using Microsoft.Extensions.Hosting;

CultureConfigurator.Configure();

var builder = Host.CreateApplicationBuilder(args);

SettingsConfigurator.Configure(builder);
LoggingConfigurator.Configure(builder);
QuartzConfigurator.Configure(builder);

using var host = builder.Build();

await host.RunAsync();