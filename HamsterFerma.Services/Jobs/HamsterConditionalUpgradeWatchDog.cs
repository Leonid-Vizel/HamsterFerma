using HamsterFerma.Services.Clients;
using HamsterFerma.Services.Configs;
using HamsterFerma.Services.Tools;
using Microsoft.Extensions.Logging;
using Quartz;
using System.Text.Json;

namespace HamsterFerma.Services.Jobs;

public sealed class HamsterConditionalUpgradeWatchDog(IHamsterApiClient client,
                                    IAuthConfigDecoder configDecoder,
                                    ILogger<HamsterConditionalUpgradeWatchDog> logger) : IJob
{
    public static void ConfigureFor(IServiceCollectionQuartzConfigurator options, AuthBearerConfig config, TimeZoneInfo timeZone)
    {
        if (!config.AutoConditionalUpgrade)
        {
            return;
        }
        if (string.IsNullOrEmpty(config.Token))
        {
            return;
        }
        var key = CreateKey();
        options.AddJob<HamsterConditionalUpgradeWatchDog>(key, job => job
            .UsingJobData(nameof(AuthBearerConfig), JsonSerializer.Serialize(config))
        ).AddTrigger(trigger => trigger
            .ForJob(key)
            .WithCronSchedule(config.ConditionalUpgradeCron, x => x.InTimeZone(timeZone))
        );
    }

    public static JobKey CreateKey()
        => JobKey.Create(nameof(HamsterConditionalUpgradeWatchDog));

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
        var upgrageList = await client.GetUpgradeListAsync(config);
        if (upgrageList == null)
        {
            return;
        }

        var blockedUpgrades = upgrageList.UpgradesForBuy
            .Where(x => !x.IsAvailable)
            .Where(x => !x.IsExpired)
            .Where(x => x.CooldownSeconds == 0)
            .Where(x => x.Condition?.UpgradeId != null)
            .Select(x => x.Condition?.UpgradeId)
            .Cast<string>()
            .ToList();

        if (blockedUpgrades.Count == 0)
        {
            return;
        }

        var buyUpgrades = upgrageList.UpgradesForBuy
            .Where(x => x.IsAvailable)
            .Where(x => blockedUpgrades.Contains(x.Id))
            .OrderBy(x => x.Price)
            .ToList();

        double actualBalance = userData.BalanceCoins;

        foreach (var upgrade in buyUpgrades)
        {
            userData.BalanceCoins -= upgrade.Price;
            if (userData.BalanceCoins <= config.MinBalance)
            {
                logger.LogInformation($"[Tag: {config.Tag}] [Требуется для открытия другого улучшения] Ожидаю ({actualBalance}/{upgrade.Price}) улучшение {upgrade.Name} с дельтой {upgrade.ProfitPerHourDelta}.");
                break;
            }
            if (upgrade.CooldownSeconds != null && upgrade.CooldownSeconds != 0)
            {
                logger.LogInformation($"[Tag: {config.Tag}] [Требуется для открытия другого улучшения] Ожидаю ({actualBalance}/{upgrade.Price}) улучшение {upgrade.Name} с дельтой {upgrade.ProfitPerHourDelta} через {(double)upgrade.CooldownSeconds / 60} минут.");
                continue;
            }
            logger.LogInformation($"[Tag: {config.Tag}] [Требуется для открытия другого улучшения] Покупка улучшения {upgrade.Name} по цене: {upgrade.Price} с дельтой {upgrade.ProfitPerHourDelta}.");
            await client.BuyUpgradeAsync(config, upgrade.Id);
        }
    }
}
