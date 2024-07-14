using HamsterFerma.Models.Taps;
using HamsterFerma.Services.Clients;
using Microsoft.Extensions.Logging;
using Quartz;

namespace HamsterFerma.Services.Jobs;

public sealed class HamsterClickerWatchDog(IHamsterApiClient client, ILogger<HamsterClickerWatchDog> logger) : IJob
{
    public static void ConfigureFor(IServiceCollectionQuartzConfigurator options, TimeZoneInfo timeZone)
    {
        var key = CreateKey();
        options.AddJob<HamsterClickerWatchDog>(key)
            .AddTrigger(trigger => trigger.ForJob(key).WithCronSchedule("5 * * ? * * *", x => x.InTimeZone(timeZone)));
    }

    public static JobKey CreateKey()
        => JobKey.Create(nameof(HamsterClickerWatchDog));

    public async Task Execute(IJobExecutionContext context)
    {
        var userData = (await client.SyncAsync())?.ClickerUser;
        if (userData == null)
        {
            logger.LogError("Sync returned null!");
            return;
        }

        if (userData.AvailableTaps > 500)
        {
            logger.LogInformation($"Отправка {userData.AvailableTaps} нажатий.");
            var tapRequest = new HamsterTapRequest(userData.AvailableTaps);
            userData = (await client.TapAsync(tapRequest))?.ClickerUser;
            if (userData == null)
            {
                logger.LogError("Tap returned null!");
                return;
            }
        }
    }
}
