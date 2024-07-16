using HamsterFerma.Services.Clients;
using HamsterFerma.Services.Configs;
using Microsoft.Extensions.Logging;
using Quartz;
using System.Text;

namespace HamsterFerma.Services.Jobs;

public sealed class HamsterCipherWatchDog(IHamsterApiClient client, ILogger<HamsterCipherWatchDog> logger) : IJob
{
    public static void ConfigureFor(IServiceCollectionQuartzConfigurator options, AuthBearerConfig config, TimeZoneInfo timeZone)
    {
        if (!config.AutoCipher)
        {
            return;
        }
        var key = CreateKey();
        options.AddJob<HamsterCipherWatchDog>(key)
            .AddTrigger(trigger => trigger.ForJob(key).WithCronSchedule(config.CipherCron, x => x.InTimeZone(timeZone)));
    }

    public static JobKey CreateKey()
        => JobKey.Create(nameof(HamsterCipherWatchDog));

    public async Task Execute(IJobExecutionContext context)
    {
        var config = await client.ConfigAsync();
        if (config == null)
        {
            logger.LogError("Config returned null!");
            return;
        }
        if (config.DailyCipher.IsClaimed)
        {
            logger.LogInformation("Шифр уже расшифрован, дешифровка пропущена.");
            return;
        }
        var encoded = config.DailyCipher.Cipher;
        var mixed = $"{encoded.Substring(0, 3)}{encoded.Substring(4)}";
        var base64Bytes = Convert.FromBase64String(mixed);
        var decoded = Encoding.UTF8.GetString(base64Bytes);

        var claimResult = await client.ClaimDailyCipher(decoded);
        if (!claimResult)
        {
            logger.LogError($"Неудачная дешифровка! (encoded='{encoded}', mixed='{mixed}', decoded='{decoded}')!");
            return;
        }
        logger.LogInformation($"Успешно расшифровано. Сегодняшний шифр: {decoded}. За счёт этого получено монет: {config.DailyCipher.BonusCoins}.");
    }
}
