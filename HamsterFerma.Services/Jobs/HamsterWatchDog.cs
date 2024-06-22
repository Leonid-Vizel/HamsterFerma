using HamsterFerma.Models.Taps;
using HamsterFerma.Models.UpgradeBuy;
using HamsterFerma.Services.Clients;
using Microsoft.Extensions.Logging;
using Quartz;

namespace HamsterFerma.Services.Jobs;

public sealed class HamsterWatchDog(IHamsterApiClient client, ILogger<HamsterWatchDog> logger) : IJob
{
    public static void ConfigureFor(IServiceCollectionQuartzConfigurator options, TimeZoneInfo timeZone)
    {
        var key = CreateKey();
        options.AddJob<HamsterWatchDog>(key)
            .AddTrigger(trigger => trigger.ForJob(key).WithCronSchedule("0 * * ? * * *", x => x.InTimeZone(timeZone)));
    }

    public static JobKey CreateKey()
        => JobKey.Create(nameof(HamsterWatchDog));

    public async Task Execute(IJobExecutionContext context)
    {
        var userData = (await client.SyncAsync())?.ClickerUser;
        if (userData == null)
        {
            logger.LogError("Sync returned null!");
            return;
        }

        var boostList = await client.GetBoostListAsync();
        if (boostList == null)
        {
            logger.LogError("BoostList returned null!");
            return;
        }

        var boosts = boostList.List
            .Where(x => x.Price == 0 && x.CooldownSeconds == 0)
            .OrderBy(x => x.Price)
            .ToList();

        foreach (var boost in boosts)
        {
            userData.BalanceCoins -= boost.Price;
            if (userData.BalanceCoins <= 0)
            {
                break;
            }
            var buyRequest = new HamsterBoostBuyRequest(boost.Id);
            logger.LogInformation($"Покупка буста {boost.Id} по цене: {boost.Price}.");
            userData = (await client.BuyBoostAsync(buyRequest))?.ClickerUser;
            if (userData == null)
            {
                logger.LogError("Sync returned null!");
                return;
            }
        }

        if (userData.AvailableTaps > 0)
        {
            logger.LogInformation($"Отправка {userData.AvailableTaps} нажатий.");
            var tapRequest = new HamsterTapRequest(userData.AvailableTaps);
            userData = (await client.TapAsync(tapRequest))?.ClickerUser;
            if (userData == null)
            {
                logger.LogError("Tap returned null!");
                return;
            }
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
            .Where(x => x.CooldownSeconds == null || x.CooldownSeconds == 0)
            .OrderByDescending(x => x.Ratio)
            .Take(1)
            .OrderBy(x => x.Price)
            .ToList();

        foreach (var upgrade in bests)
        {
            userData.BalanceCoins -= upgrade.Price;
            if (userData.BalanceCoins <= 0)
            {
                break;
            }
            var buyRequest = new HamsterUpgradeBuyRequest(upgrade.Id);
            logger.LogInformation($"Покупка улучшения {upgrade.Name} по цене: {upgrade.Price} с дельтой {upgrade.ProfitPerHourDelta}.");
            await client.BuyUpgradeAsync(buyRequest);
        }
    }
}
