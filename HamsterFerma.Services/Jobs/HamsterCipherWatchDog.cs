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
        var key = CreateKey();
        options.AddJob<HamsterCipherWatchDog>(key, job => job
            .UsingJobData(nameof(AuthBearerConfig), JsonSerializer.Serialize(config))
        ).AddTrigger(trigger => trigger
            .ForJob(key)
            .WithCronSchedule(config.CipherCron, x => x.InTimeZone(timeZone))
        );
    }

    public static JobKey CreateKey()
        => JobKey.Create(nameof(HamsterCipherWatchDog));

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
        var encoded = clickerConfig.DailyCipher.Cipher;
        var mixed = $"{encoded.Substring(0, 3)}{encoded.Substring(4)}";
        var base64Bytes = Convert.FromBase64String(mixed);
        var decoded = Encoding.UTF8.GetString(base64Bytes);

        var claimResult = await client.ClaimDailyCipher(config, decoded);
        if (!claimResult)
        {
            logger.LogError($"[Tag: {config.Tag}] Неудачная дешифровка! (encoded='{encoded}', mixed='{mixed}', decoded='{decoded}')!");
            return;
        }
        logger.LogInformation($"[Tag: {config.Tag}] Успешно расшифровано. Сегодняшний шифр: {decoded}. За счёт этого получено монет: {clickerConfig.DailyCipher.BonusCoins}.");
    }
}
