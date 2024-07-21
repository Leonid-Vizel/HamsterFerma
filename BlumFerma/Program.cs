using BlumFerma.Services.Extensions;
using Microsoft.Extensions.Hosting;
using Weasel.Farmer.Services.Common.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.ConfigureRussianCulture();
builder.AddStandartLogging();
builder.AddStandartAppSettings();
builder.ConfigureBlumFerma();

using var host = builder.Build();

await host.RunAsync();