using HamsterFerma.Models.BoostList;
using HamsterFerma.Services.Clients;
using HamsterFerma.Services.Configs;
using HamsterFerma.Services.Tools;
using LinqKit;
using Microsoft.Extensions.Logging;
using Quartz;
using System.Text.Json;

namespace HamsterFerma.Services.Jobs;

public sealed class HamsterBoostWatchDog(IHamsterApiClient client,
                                         IAuthConfigDecoder configDecoder,
                                         ILogger<HamsterBoostWatchDog> logger) : IJob
{
    public static void ConfigureFor(IServiceCollectionQuartzConfigurator options, AuthBearerConfig config, TimeZoneInfo timeZone)
    {
        if (!config.AutoBoost)
        {
            return;
        }
        if (string.IsNullOrEmpty(config.Token))
        {
            return;
        }
        var key = CreateKey();
        options.AddJob<HamsterBoostWatchDog>(key, job => job
            .UsingJobData(nameof(AuthBearerConfig), JsonSerializer.Serialize(config))
        ).AddTrigger(trigger => trigger
            .ForJob(key)
            .WithCronSchedule(config.BoostCron, x => x.InTimeZone(timeZone))
        );
    }

    public static JobKey CreateKey()
        => JobKey.Create(nameof(HamsterBoostWatchDog));

    public async Task Execute(IJobExecutionContext context)
    {
        var config =  configDecoder.Decode(context);
        if (config == null)
        {
            return;
        }

        var userData = (await client.SyncAsync(config))?.ClickerUser;
        if (userData == null)
        {
            return;
        }

        var boostList = await client.GetBoostListAsync(config);
        if (boostList == null)
        {
            return;
        }

        var query = PredicateBuilder.New<HamsterBoost>();
        if (config.BuyUselessPerClickBoosts)
        {
            query = query.Or(x => x.EarnPerTapDelta > 0);
        }
        if (config.BuyClickLimitBoosts)
        {
            query = query.Or(x => x.MaxTapsDelta > 0);
        }
        if (config.BuyClickFullLimitBoosts)
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
            if (userData.BalanceCoins <= config.MinBalance && boost.Price != 0)
            {
                break;
            }
            logger.LogInformation($"[Tag: {config.Tag}] Покупка буста {boost.Id} по цене: {boost.Price}.");
            userData = (await client.BuyBoostAsync(config, boost.Id))?.ClickerUser;
            if (userData == null)
            {
                return;
            }
        }
    }
}
