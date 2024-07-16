using HamsterFerma.Services.Clients;
using HamsterFerma.Services.Configs;
using HamsterFerma.Services.Tools;
using Microsoft.Extensions.Logging;
using Quartz;
using System.Text;
using System.Text.Json;

namespace HamsterFerma.Services.Jobs;

public sealed class HamsterCipherWatchDog(IHamsterApiClient client,
                                          IAuthConfigDecoder configDecoder,
                                          IHamsterCipherDecoder cipherDecoder,
                                          ILogger<HamsterCipherWatchDog> logger) : IJob
{
    public static void ConfigureFor(IServiceCollectionQuartzConfigurator options, AuthBearerConfig config, TimeZoneInfo timeZone)
    {
        if (!config.AutoCipher)
        {
            return;
        }
        if (string.IsNullOrEmpty(config.Token))
        {
            return;
        }
        var key = CreateKey(config.Tag);
        options.AddJob<HamsterCipherWatchDog>(key, job => job
            .UsingJobData(nameof(AuthBearerConfig), JsonSerializer.Serialize(config))
        ).AddTrigger(trigger => trigger
            .ForJob(key)
            .WithCronSchedule(config.CipherCron, x => x.InTimeZone(timeZone))
        );
    }

    public static JobKey CreateKey(string tag)
        => JobKey.Create(nameof(HamsterCipherWatchDog), tag);

    public async Task Execute(IJobExecutionContext context)
    {
        var config = configDecoder.Decode(context);
        if (config == null)
        {
            return;
        }

        var clickerConfig = await client.ConfigAsync(config);
        if (clickerConfig == null)
        {
            return;
        }
        if (clickerConfig.DailyCipher.IsClaimed)
        {
            logger.LogInformation($"[Tag: {config.Tag}] Шифр уже расшифрован, дешифровка пропущена.");
            return;
        }
        var decoded = cipherDecoder.Decode(clickerConfig.DailyCipher.Cipher);

        var claimResult = await client.ClaimDailyCipher(config, decoded);
        if (!claimResult)
        {
            logger.LogError($"[Tag: {config.Tag}] Неудачная дешифровка! (decoded='{decoded}')!");
            return;
        }
        logger.LogInformation($"[Tag: {config.Tag}] Успешно расшифровано. Сегодняшний шифр: {decoded}. За счёт этого получено монет: {clickerConfig.DailyCipher.BonusCoins}.");
    }
}
