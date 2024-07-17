using SeedFerma.Configurators;
using Microsoft.Extensions.Hosting;
using Weasel.Configurators.Common;

CultureConfigurator.Configure();

var builder = Host.CreateApplicationBuilder(args);

SettingsConfigurator.Configure(builder);
LoggingConfigurator.Configure(builder);
QuartzConfigurator.Configure(builder);

using var host = builder.Build();

await host.RunAsync();