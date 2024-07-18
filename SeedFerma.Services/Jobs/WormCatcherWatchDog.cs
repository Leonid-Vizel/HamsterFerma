using Microsoft.Extensions.Logging;
using Quartz;
using SeedFerma.Services.Clients;
using SeedFerma.Services.Configs;
using SeedFerma.Services.Tools;
using System.Text.Json;

namespace SeedFerma.Services.Jobs;

public sealed class WormCatcherWatchDog(ISeedApiClient client,
                                        IAuthConfigDecoder configDecoder,
                                        ILogger<WormCatcherWatchDog> logger) : IJob
{
    public static void ConfigureFor(IServiceCollectionQuartzConfigurator options, AuthBearerConfig config, TimeZoneInfo timeZone)
    {
        if (!config.AutoCatch)
        {
            return;
        }
        if (string.IsNullOrEmpty(config.TelegramData))
        {
            return;
        }
        var key = CreateKey(config.Tag);
        options.AddJob<WormCatcherWatchDog>(key, job => job
            .UsingJobData(nameof(AuthBearerConfig), JsonSerializer.Serialize(config))
        ).AddTrigger(trigger => trigger
            .ForJob(key)
            .WithCronSchedule(config.CatchCron, x => x.InTimeZone(timeZone))
        );
    }

    public static JobKey CreateKey(string tag)
        => JobKey.Create(nameof(WormCatcherWatchDog), tag);

    public async Task Execute(IJobExecutionContext context)
    {
        var config = configDecoder.Decode(context);
        if (config == null)
        {
            return;
        }

        var worms = await client.GetWormListAsync(config);
        if (worms == null)
        {
            return;
        }
        if (worms.Data.IsCaught)
        {
            return;
        }
        var caughtResult = await client.CatchAsync(config);
        if (caughtResult == null)
        {
            return;
        }
        logger.LogInformation($"[Tag: {config.Tag}] Червяк ({caughtResult.Data.Type}) пойман!");
    }
}
