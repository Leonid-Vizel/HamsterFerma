using HamsterFerma.Models.BoostList;
using HamsterFerma.Models.UpgradeBuy;
using HamsterFerma.Services.Clients;
using LinqKit;
using Microsoft.Extensions.Logging;
using Quartz;

namespace HamsterFerma.Services.Jobs;

public sealed class HamsterBoostWatchDog(IHamsterApiClient client, ILogger<HamsterBoostWatchDog> logger) : IJob
{
    public static void ConfigureFor(IServiceCollectionQuartzConfigurator options, TimeZoneInfo timeZone)
    {
        var key = CreateKey();
        options.AddJob<HamsterBoostWatchDog>(key)
            .AddTrigger(trigger => trigger.ForJob(key).WithCronSchedule("0 * * ? * * *", x => x.InTimeZone(timeZone)));
    }

    public static JobKey CreateKey()
        => JobKey.Create(nameof(HamsterBoostWatchDog));

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

        var query = PredicateBuilder.New<HamsterBoost>();
        if (client.Config.BuyUselessPerClickBoosts)
        {
            query = query.Or(x => x.EarnPerTapDelta > 0);
        }
        if (client.Config.BuyClickLimitBoosts)
        {
            query = query.Or(x => x.MaxTapsDelta > 0);
        }
        if (client.Config.BuyClickFullLimitBoosts)
        {
            query = query.Or(x => x.Price == 0);
        }

        var boosts = boostList.List
            .Where(x => x.CooldownSeconds == 0)
            .Where(query)
            .OrderBy(x => x.Price)
            .ToList();

        foreach (var boost in boosts)
        {
            userData.BalanceCoins -= boost.Price;
            if (userData.BalanceCoins <= client.Config.MinBalance && boost.Price != 0)
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
    }
}
