using HamsterFerma.Services.Clients;
using HamsterFerma.Services.Configs;
using HamsterFerma.Services.Tools;
using Microsoft.Extensions.Logging;
using Quartz;
using System.Text.Json;

namespace HamsterFerma.Services.Jobs;

public sealed class HamsterClickerWatchDog(IHamsterApiClient client,
                                           IAuthConfigDecoder configDecoder,
                                           ILogger<HamsterClickerWatchDog> logger) : IJob
{
    public static void ConfigureFor(IServiceCollectionQuartzConfigurator options, AuthBearerConfig config, TimeZoneInfo timeZone)
    {
        if (!config.AutoClick)
        {
            return;
        }
        if (string.IsNullOrEmpty(config.Token))
        {
            return;
        }
        var key = CreateKey(config.Tag);
        options.AddJob<HamsterClickerWatchDog>(key, job => job
            .UsingJobData(nameof(AuthBearerConfig), JsonSerializer.Serialize(config))
        ).AddTrigger(trigger => trigger
            .ForJob(key)
            .WithCronSchedule(config.ClickCron, x => x.InTimeZone(timeZone))
        );
    }

    public static JobKey CreateKey(string tag)
        => JobKey.Create(nameof(HamsterClickerWatchDog), tag);

    public async Task Execute(IJobExecutionContext context)
    {
        var config = configDecoder.Decode(context);
        if (config == null)
        {
            return;
        }

        var userData = (await client.SyncAsync(config))?.ClickerUser;
        if (userData == null)
        {
            return;
        }

        if (userData.AvailableTaps > 500)
        {
            logger.LogInformation($"[Tag: {config.Tag}] Отправка {userData.AvailableTaps} нажатий.");
            userData = (await client.TapAsync(config, userData.AvailableTaps / userData.EarnPerTap))?.ClickerUser;
            if (userData == null)
            {
                return;
            }
        }
    }
}
