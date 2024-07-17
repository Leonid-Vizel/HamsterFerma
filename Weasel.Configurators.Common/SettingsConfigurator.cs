﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Weasel.Configurators.Common;

public static class SettingsConfigurator
{
    public static void Configure(IHostApplicationBuilder builder)
    {
        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);
    }
}