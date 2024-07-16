using HamsterFerma.Services.Clients;
using HamsterFerma.Services.Configs;
using HamsterFerma.Services.Tools;
using Microsoft.Extensions.Logging;
using Quartz;
using System.Text.Json;

namespace HamsterFerma.Services.Jobs;

public class HamsterUpgradeWatchDog(IHamsterApiClient client,
                                    IAuthConfigDecoder configDecoder,
                                    ILogger<HamsterUpgradeWatchDog> logger) : IJob
{
    public static void ConfigureFor(IServiceCollectionQuartzConfigurator options, AuthBearerConfig config, TimeZoneInfo timeZone)
    {
        if (!config.AutoUpgrade)
        {
            return;
        }
        if (string.IsNullOrEmpty(config.Token))
        {
            return;
        }
        var key = CreateKey(config.Tag);
        options.AddJob<HamsterUpgradeWatchDog>(key, job => job
            .UsingJobData(nameof(AuthBearerConfig), JsonSerializer.Serialize(config))
        ).AddTrigger(trigger => trigger
            .ForJob(key)
            .WithCronSchedule(config.UpgradeCron, x => x.InTimeZone(timeZone))
        );
    }

    public static JobKey CreateKey(string tag)
        => JobKey.Create(nameof(HamsterUpgradeWatchDog), tag);

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

        var bests = upgrageList.UpgradesForBuy
            .Where(x => x.IsAvailable)
            .Where(x => !x.IsExpired)
            .Where(x => !(x.ProfitPerHourDelta == 0 && x.Level == 2))
            .OrderByDescending(x => x.RatioWithCoolDown)
            .Take(1)
            .OrderBy(x => x.Price)
            .ToList();

        double actualBalance = userData.BalanceCoins;

        foreach (var upgrade in bests)
        {
            userData.BalanceCoins -= upgrade.Price;
            if (userData.BalanceCoins <= config.MinBalance + config.ConditionalUpgradeBuffer)
            {
                logger.LogInformation($"[Tag: {config.Tag}] Ожидаю ({actualBalance}/{upgrade.Price}) улучшение {upgrade.Name} с дельтой {upgrade.ProfitPerHourDelta}.");
                break;
            }
            if (upgrade.CooldownSeconds != null && upgrade.CooldownSeconds != 0)
            {
                logger.LogInformation($"[Tag: {config.Tag}] Ожидаю ({actualBalance}/{upgrade.Price}) улучшение {upgrade.Name} с дельтой {upgrade.ProfitPerHourDelta} через {(double)upgrade.CooldownSeconds / 60} минут.");
                continue;
            }
            logger.LogInformation($"[Tag: {config.Tag}] Покупка улучшения {upgrade.Name} по цене: {upgrade.Price} с дельтой {upgrade.ProfitPerHourDelta}.");
            await client.BuyUpgradeAsync(config, upgrade.Id);
        }
    }
}
