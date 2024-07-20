using BlumFerma.Services.Clients;
using BlumFerma.Services.Configs;
using Microsoft.Extensions.Logging;

namespace BlumFerma.Services.Tools;

public interface IBlumDailyRewardCompleter
{
    Task CompleteAsync(AuthBearerConfig config);
}

public sealed class BlumDailyRewardCompleter(IBlumApiClient client,
                                          ILogger<BlumGameCompleter> logger) : IBlumDailyRewardCompleter
{
    public async Task CompleteAsync(AuthBearerConfig config)
    {
        if (DateTime.UtcNow.Hour > 10)
        {
            return;
        }

        var dailyResullt = await client.CheckDailyRewardAsync(config);
        if (dailyResullt)
        {
            logger.LogInformation($"[Tag: {config.Tag}] Получена ежедневная награда!");
        }
    }
}
