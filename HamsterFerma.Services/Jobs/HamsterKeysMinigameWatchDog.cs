using HamsterFerma.Services.Clients;
using HamsterFerma.Services.Configs;
using HamsterFerma.Services.Tools;
using Microsoft.Extensions.Logging;
using Quartz;
using System.Text.Json;

namespace HamsterFerma.Services.Jobs;

public sealed class HamsterKeysMinigameWatchDog(IHamsterApiClient client,
                                                IAuthConfigDecoder configDecoder,
                                                IHamsterKeyGameCipherGenerator keyGameCipherGen,
                                                ILogger<HamsterKeysMinigameWatchDog> logger) : IJob
{
    public static void ConfigureFor(IServiceCollectionQuartzConfigurator options, AuthBearerConfig config, TimeZoneInfo timeZone)
    {
        if (!config.AutoTask)
        {
            return;
        }
        if (string.IsNullOrEmpty(config.Token))
        {
            return;
        }
        var key = CreateKey(config.Tag);
        options.AddJob<HamsterKeysMinigameWatchDog>(key, job => job
            .UsingJobData(nameof(AuthBearerConfig), JsonSerializer.Serialize(config))
        ).AddTrigger(trigger => trigger
            .ForJob(key)
            .WithCronSchedule(config.KeysMinigameCron, x => x.InTimeZone(timeZone))
        );
    }

    public static JobKey CreateKey(string tag)
        => JobKey.Create(nameof(HamsterKeysMinigameWatchDog), tag);

    public async Task Execute(IJobExecutionContext context)
    {
        var config = configDecoder.Decode(context);
        if (config == null)
        {
            return;
        }

        var userConfig = await client.ConfigAsync(config);
        if (userConfig == null)
        {
            return;
        }
        if (userConfig.KeysMinigame.IsClaimed)
        {
            return;
        }
        var userSync = await client.SyncAsync(config);
        if (!long.TryParse(userSync?.ClickerUser.Id, out var longId))
        {
            return;
        }
        await client.StartDailyKeysMinigameAsync(config);
        await Task.Delay(Random.Shared.Next(15000, 20000));
        var cipher = keyGameCipherGen.Generate(longId);
        var claimResult = await client.ClaimDailyCipher(config, cipher);
        if (!claimResult)
        {
            logger.LogInformation($"[Tag: {config.Tag}] Keys minigame was not claimed!");
            return;
        }
        logger.LogInformation($"[Tag: {config.Tag}] Keys minigame claimed successfully!");
    }
}
