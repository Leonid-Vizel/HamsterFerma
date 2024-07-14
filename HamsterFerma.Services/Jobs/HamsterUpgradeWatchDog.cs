using HamsterFerma.Models.UpgradeBuy;
using HamsterFerma.Services.Clients;
using Microsoft.Extensions.Logging;
using Quartz;

namespace HamsterFerma.Services.Jobs;

public class HamsterUpgradeWatchDog(IHamsterApiClient client, ILogger<HamsterUpgradeWatchDog> logger) : IJob
{
    public static void ConfigureFor(IServiceCollectionQuartzConfigurator options, TimeZoneInfo timeZone)
    {
        var key = CreateKey();
        options.AddJob<HamsterUpgradeWatchDog>(key)
            .AddTrigger(trigger => trigger.ForJob(key).WithCronSchedule("10 * * ? * * *", x => x.InTimeZone(timeZone)));
    }

    public static JobKey CreateKey()
        => JobKey.Create(nameof(HamsterUpgradeWatchDog));

    public async Task Execute(IJobExecutionContext context)
    {
        var userData = (await client.SyncAsync())?.ClickerUser;
        if (userData == null)
        {
            logger.LogError("Sync returned null!");
            return;
        }
        var upgrageList = await client.GetUpgradeListAsync();
        if (upgrageList == null)
        {
            logger.LogError("UpgradeList returned null!");
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
            if (userData.BalanceCoins <= client.Config.MinBalance)
            {
                break;
            }
            if (upgrade.CooldownSeconds != null && upgrade.CooldownSeconds != 0)
            {
                logger.LogInformation($"Ожидаю ({actualBalance}/{upgrade.Price}) улучшение {upgrade.Name} по цене: {upgrade.Price} с дельтой {upgrade.ProfitPerHourDelta} через {(double)upgrade.CooldownSeconds / 60} минут.");
                continue;
            }
            var buyRequest = new HamsterUpgradeBuyRequest(upgrade.Id);
            logger.LogInformation($"Покупка улучшения {upgrade.Name} по цене: {upgrade.Price} с дельтой {upgrade.ProfitPerHourDelta}.");
            await client.BuyUpgradeAsync(buyRequest);
        }
    }
}
