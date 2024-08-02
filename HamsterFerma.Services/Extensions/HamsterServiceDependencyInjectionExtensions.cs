using HamsterFerma.Services.Clients;
using HamsterFerma.Services.Configs;
using HamsterFerma.Services.Jobs;
using HamsterFerma.Services.Tools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;

namespace HamsterFerma.Services.Extensions;

public static class HamsterServiceDependencyInjectionExtensions
{
    public static void ConfigureHamsterFerma(this IHostApplicationBuilder builder)
    {
        var configs = new AuthBearerConfigCollection();
        builder.Configuration.GetSection("Auth").Bind(configs);

        builder.Services
            .AddMemoryCache()
            .AddScoped<IHamsterApiClient, HamsterApiClient>()
            .AddSingleton<IAuthConfigDecoder, AuthConfigDecoder>()
            .AddSingleton<IHamsterTimeManager, HamsterTimeManager>()
            .AddSingleton<IHamsterKeyGameCipherGenerator, HamsterKeyGameCipherGenerator>()
            .AddSingleton<IHamsterCipherDecoder, HamsterCipherDecoder>()
            .AddSingleton(configs)
            .AddQuartz(options =>
            {
                options.InterruptJobsOnShutdown = true;
                options.UseDefaultThreadPool(1, x =>
                {
                    x.MaxConcurrency = 1;
                });
                options.UseInMemoryStore();
                var zone = TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");
                foreach (var config in configs)
                {
                    HamsterClickerWatchDog.ConfigureFor(options, config, zone);
                    HamsterUpgradeWatchDog.ConfigureFor(options, config, zone);
                    HamsterConditionalUpgradeWatchDog.ConfigureFor(options, config, zone);
                    HamsterBoostWatchDog.ConfigureFor(options, config, zone);
                    HamsterCipherWatchDog.ConfigureFor(options, config, zone);
                    HamsterTaskWatchDog.ConfigureFor(options, config, zone);
                }
            }).AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            }).AddHttpClient("Hamster", x =>
            {
                x.DefaultRequestHeaders.Add("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7,en-GB;q=0.6,zh-TW;q=0.5,zh-CN;q=0.4,zh;q=0.3");
                x.DefaultRequestHeaders.Add("Connection", "keep-alive");
                x.DefaultRequestHeaders.Add("Origin", "https://hamsterkombat.io");
                x.DefaultRequestHeaders.Add("Referer", "https://hamsterkombat.io/");
                x.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "empty");
                x.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors");
                x.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-site");
                x.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36");
                x.DefaultRequestHeaders.Add("accept", "application/json");
                x.DefaultRequestHeaders.Add("sec-ch-ua", "\"Not/A)Brand\";v=\"8\", \"Chromium\";v=\"126\", \"Google Chrome\";v=\"126\"");
                x.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
                x.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
            });

        builder.Services.AddHttpClient("Combo", x =>
        {
            x.DefaultRequestHeaders.Add("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
            x.DefaultRequestHeaders.Add("accept-language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7,en-GB;q=0.6,zh-TW;q=0.5,zh-CN;q=0.4,zh;q=0.3");
            x.DefaultRequestHeaders.Add("cookie", "pll_language=en");
            x.DefaultRequestHeaders.Add("priority", "u=0, i");
            x.DefaultRequestHeaders.Add("sec-ch-ua", "\"Not)A;Brand\";v=\"99\", \"Google Chrome\";v=\"127\", \"Chromium\";v=\"127\"");
            x.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
            x.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
            x.DefaultRequestHeaders.Add("sec-fetch-dest", "document");
            x.DefaultRequestHeaders.Add("sec-fetch-mode", "navigate");
            x.DefaultRequestHeaders.Add("sec-fetch-site", "cross-site");
            x.DefaultRequestHeaders.Add("sec-fetch-user", "?1");
            x.DefaultRequestHeaders.Add("upgrade-insecure-requests", "1");
            x.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/127.0.0.0 Safari/537.36");
        });
    }
}
