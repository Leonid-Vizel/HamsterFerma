using BlumFerma.Models.Common;
using BlumFerma.Services.Clients;
using BlumFerma.Services.Configs;
using BlumFerma.Services.Tools;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Logging;
using System.Text.Json;

namespace BlumFerma.Services.Jobs;

public sealed class BlumWatchDog(IBlumApiClient client,
                                 IAuthConfigDecoder configDecoder,
                                 ILogger<BlumWatchDog> logger,
                                 IBlumTaskCompleter taskCompleter,
                                 IBlumGameCompleter gameCompleter,
                                 IBlumFarmingCompleter farmingCompleter,
                                 IBlumDailyRewardCompleter dailyCompleter) : IJob
{
    public static void ConfigureFor(IServiceCollectionQuartzConfigurator options, AuthBearerConfig config, TimeZoneInfo timeZone)
    {
        if (!config.AutoTask && !config.AutoFarm && !config.AutoDaily && !config.AutoPlay)
        {
            return;
        }
        if (string.IsNullOrEmpty(config.RefreshToken))
        {
            return;
        }
        var key = CreateKey(config.Tag);
        options.AddJob<BlumWatchDog>(key, job => job
            .UsingJobData(nameof(AuthBearerConfig), JsonSerializer.Serialize(config))
        ).AddTrigger(trigger => trigger
            .ForJob(key)
            .WithCronSchedule(config.Cron, x => x.InTimeZone(timeZone))
        );
    }

    public static JobKey CreateKey(string tag)
        => JobKey.Create(nameof(BlumWatchDog), tag);

    public async Task Execute(IJobExecutionContext context)
    {
        var config = configDecoder.Decode(context);
        if (config == null)
        {
            return;
        }

        logger.LogInformation($"[Tag: {config.Tag}] Начата проверка!");

        var refreshResult = await client.RefreshTokenAsync(config);
        if (refreshResult == null)
        {
            return;
        }

        if (config.AutoDaily)
        {
            await dailyCompleter.CompleteAsync(config);
        }
        if (config.AutoTask)
        {
            await taskCompleter.CompleteAsync(config);
        }
        if (config.AutoPlay)
        {
            await gameCompleter.CompleteAsync(config);
        }
        if (config.AutoFarm)
        {
            await farmingCompleter.CompleteAsync(config);
        }

        logger.LogInformation($"[Tag: {config.Tag}] Проверка окончена!");
    }
}
