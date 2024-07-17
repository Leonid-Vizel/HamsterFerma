using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace SeedFerma.Services.Clients;

public interface ISeedApiClient
{

}

public sealed class SeedApiClient(IHttpClientFactory clientFactory,
                                  ILogger<SeedApiClient> logger) : ISeedApiClient
{
    public async Task<HamsterUpgradeListResponse?> GetUpgradeListAsync(AuthBearerConfig config)
    {
        if (string.IsNullOrEmpty(config?.Token))
        {
            logger.LogError($"[Tag: {config?.Tag}] {_tokenErrorMessage}");
            return null;
        }
        using var client = clientFactory.CreateClient("Hamster");
        client.DefaultRequestHeaders.Add("authorization", $"Bearer {config.Token}");
        var httpResponse = await client.PostAsync("https://api.hamsterkombatgame.io/clicker/upgrades-for-buy", null);
        if (!httpResponse.IsSuccessStatusCode)
        {
            logger.LogError($"[Tag: {config.Tag}] [Method: {nameof(GetUpgradeListAsync)}] {_codeUnsuccessfulErrorMessage}");
            return null;
        }
        using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
        var a = await httpResponse.Content.ReadAsStringAsync();
        var responseJson = await JsonSerializer.DeserializeAsync<HamsterUpgradeListResponse>(responseStream);
        return responseJson;
    }
}
