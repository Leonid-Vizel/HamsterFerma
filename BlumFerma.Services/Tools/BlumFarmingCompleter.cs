using BlumFerma.Services.Clients;
using BlumFerma.Services.Configs;
using Microsoft.Extensions.Logging;

namespace BlumFerma.Services.Tools;

public interface IBlumFarmingCompleter
{
    Task CompleteAsync(AuthBearerConfig config);
}

public sealed class BlumFarmingCompleter(IBlumApiClient client,
                                         ILogger<BlumFarmingCompleter> logger) : IBlumFarmingCompleter
{
    public async Task CompleteAsync(AuthBearerConfig config)
    {
        var balance = await client.GetBalanceAsync(config);
        if (balance == null)
        {
            return;
        }
        if (balance.Farming != null && balance.Farming.EndTimeFormattedUtc < DateTime.UtcNow)
        {
            var claimResult = await client.ClaimFarmingAsync(config);
            if (claimResult != null)
            {
                logger.LogInformation($"[Tag: {config.Tag}] Фарминг завершён, награда получена!");
                balance = claimResult;
            }
        }
        if (balance.Farming == null)
        {
            var startResult = await client.StartFarmingAsync(config);
            if (startResult != null)
            {
                logger.LogInformation($"[Tag: {config.Tag}] Фарминг начат!");
            }
        }
    }
}
