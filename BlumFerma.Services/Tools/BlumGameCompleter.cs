using BlumFerma.Services.Clients;
using BlumFerma.Services.Configs;
using Microsoft.Extensions.Logging;

namespace BlumFerma.Services.Tools;

public interface IBlumGameCompleter
{
    Task CompleteAsync(AuthBearerConfig config);
}

public sealed class BlumGameCompleter(IBlumApiClient client,
                                      ILogger<BlumGameCompleter> logger) : IBlumGameCompleter
{
    public async Task CompleteAsync(AuthBearerConfig config)
    {
        var balance = await client.GetBalanceAsync(config);
        if (balance == null)
        {
            return;
        }

        if (balance.PlayPasses <= 0)
        {
            logger.LogInformation($"[Tag: {config.Tag}] Игра пропущена, недостаточно попыток!");
            return;
        }

        var game = await client.StartGameAsync(config);
        if (game == null)
        {
            return;
        }

        var delayMs = Random.Shared.Next(29000, 60000);
        logger.LogInformation($"[Tag: {config.Tag}] Игра {game.GameId} начата, ожидаю {delayMs} ms!");
        await Task.Delay(delayMs);

        game.Points = (ushort)Random.Shared.Next(175, 250);
        var claimResult = await client.ClaimGameAsync(config, game);
        if (claimResult)
        {
            logger.LogInformation($"[Tag: {config.Tag}] Игра {game.GameId} завершена с {game.Points} очками!");
        }
    }
}
