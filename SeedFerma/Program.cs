using Microsoft.Extensions.Hosting;
using SeedFerma.Services.Extensions;
using Weasel.Farmer.Services.Common.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.ConfigureRussianCulture();
builder.AddStandartLogging();
builder.AddStandartAppSettings();
builder.ConfigureSeedFerma();

using var host = builder.Build();

await host.RunAsync();