using BlumFerma.Services.Extensions;
using Microsoft.Extensions.Hosting;
using Weasel.Configurators.Common;

CultureConfigurator.Configure();

var builder = Host.CreateApplicationBuilder(args);

SettingsConfigurator.Configure(builder);
LoggingConfigurator.Configure(builder);
builder.ConfigureBlumFerma();

using var host = builder.Build();

await host.RunAsync();