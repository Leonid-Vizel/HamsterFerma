using Microsoft.Extensions.Logging;
using Quartz;
using SeedFerma.Services.Clients;
using SeedFerma.Services.Configs;
using SeedFerma.Services.Tools;
using System.Text.Json;

namespace SeedFerma.Services.Jobs;

public sealed class SeedClaimerWatchDog(ISeedApiClient client,
                                        IAuthConfigDecoder configDecoder,
                                        ILogger<SeedClaimerWatchDog> logger) : IJob
{
    public static void ConfigureFor(IServiceCollectionQuartzConfigurator options, AuthBearerConfig config, TimeZoneInfo timeZone)
    {
        if (!config.AutoClaim)
        {
            return;
        }
        if (string.IsNullOrEmpty(config.TelegramData))
        {
            return;
        }
        var key = CreateKey(config.Tag);
        options.AddJob<SeedClaimerWatchDog>(key, job => job
            .UsingJobData(nameof(AuthBearerConfig), JsonSerializer.Serialize(config))
        ).AddTrigger(trigger => trigger
            .ForJob(key)
            .WithCronSchedule(config.ClaimCron, x => x.InTimeZone(timeZone))
        );
    }

    public static JobKey CreateKey(string tag)
        => JobKey.Create(nameof(SeedClaimerWatchDog), tag);

    public async Task Execute(IJobExecutionContext context)
    {
        var config = configDecoder.Decode(context);
        if (config == null)
        {
            return;
        }

        var claimResult = await client.ClaimAsync(config);
        if (claimResult == null)
        {
            return;
        }
        logger.LogInformation($"[Tag: {config.Tag}] Награда ({claimResult.Data.Amount}) успешно забрана!");
    }
}