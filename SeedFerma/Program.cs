using Microsoft.Extensions.Hosting;
using Weasel.Configurators.Common;
using SeedFerma.Services.Extensions;

CultureConfigurator.Configure();

var builder = Host.CreateApplicationBuilder(args);

SettingsConfigurator.Configure(builder);
LoggingConfigurator.Configure(builder);
builder.ConfigureSeedFerma();

using var host = builder.Build();

await host.RunAsync();