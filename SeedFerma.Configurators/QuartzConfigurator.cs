using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using SeedFerma.Services.Clients;
using SeedFerma.Services.Configs;
using SeedFerma.Services.Jobs;
using SeedFerma.Services.Tools;

namespace SeedFerma.Configurators;

public static class QuartzConfigurator
{
    public static void Configure(IHostApplicationBuilder builder)
    {
        var configs = new AuthBearerConfigCollection();
        builder.Configuration.GetSection("Auth").Bind(configs);

        builder.Services
            .AddScoped<ISeedApiClient, SeedApiClient>()
            .AddSingleton<IAuthConfigDecoder, AuthConfigDecoder>()
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
                    SeedClaimerWatchDog.ConfigureFor(options, config, zone);
                    WormCatcherWatchDog.ConfigureFor(options, config, zone);
                    //SeedConsumerWatchDog.ConfigureFor(options, config, zone);
                    //SeedSellerWatchDog.ConfigureFor(options, config, zone);
                }
            }).AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            }).AddHttpClient("Seed", x =>
            {
                x.DefaultRequestHeaders.Add("accept", "*/*");
                x.DefaultRequestHeaders.Add("accept-language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7,en-GB;q=0.6,zh-TW;q=0.5,zh-CN;q=0.4,zh;q=0.3");
                x.DefaultRequestHeaders.Add("origin", "https://cf.seeddao.org");
                x.DefaultRequestHeaders.Add("priority", "u=1, i");
                x.DefaultRequestHeaders.Add("referer", "https://cf.seeddao.org/");
                x.DefaultRequestHeaders.Add("sec-ch-ua", "\"Not/A)Brand\";v=\"8\", \"Chromium\";v=\"126\", \"Google Chrome\";v=\"126\"");
                x.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
                x.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
                x.DefaultRequestHeaders.Add("sec-fetch-dest", "empty");
                x.DefaultRequestHeaders.Add("sec-fetch-mode", "cors");
                x.DefaultRequestHeaders.Add("sec-fetch-site", "same-site");
                x.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36");
            });
    }
}
