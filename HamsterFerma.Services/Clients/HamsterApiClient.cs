using HamsterFerma.Models.BoostList;
using HamsterFerma.Models.Cipher;
using HamsterFerma.Models.Config;
using HamsterFerma.Models.Taps;
using HamsterFerma.Models.UpgradeBuy;
using HamsterFerma.Models.Upgrades;
using HamsterFerma.Services.Configs;
using System.Text;
using System.Text.Json;

namespace HamsterFerma.Services.Clients;

public interface IHamsterApiClient
{
    AuthBearerConfig Config { get; }
    Task<HamsterUpgradeListResponse?> GetUpgradeListAsync();
    Task<HamsterBoostListResponse?> GetBoostListAsync();
    Task<HamsterSyncResponse?> SyncAsync();
    Task<HamsterConfigResponse?> ConfigAsync();
    Task<bool> ClaimDailyCipher(HamsterCipherRequest request);
    Task<bool> ClaimDailyCipher(string cipher);
    Task<HamsterTapResponse?> TapAsync(HamsterTapRequest request);
    Task<HamsterUpgradeBuyResponse?> BuyUpgradeAsync(HamsterUpgradeBuyRequest request);
    Task<HamsterBoostBuyResponse?> BuyBoostAsync(HamsterBoostBuyRequest request);
}

public sealed class HamsterApiClient(AuthBearerConfig config,
                                     IHttpClientFactory clientFactory) : IHamsterApiClient
{
    public AuthBearerConfig Config => config;
    public async Task<HamsterUpgradeListResponse?> GetUpgradeListAsync()
    {
        using var client = clientFactory.CreateClient("Hamster");
        client.DefaultRequestHeaders.Add("authorization", $"Bearer {config.Auth}");
        var httpResponse = await client.PostAsync("https://api.hamsterkombat.io/clicker/upgrades-for-buy", null);
        if (!httpResponse.IsSuccessStatusCode)
        {
            return null;
        }
        using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
        var responseJson = await JsonSerializer.DeserializeAsync<HamsterUpgradeListResponse>(responseStream);
        return responseJson;
    }

    public async Task<HamsterBoostListResponse?> GetBoostListAsync()
    {
        using var client = clientFactory.CreateClient("Hamster");
        client.DefaultRequestHeaders.Add("authorization", $"Bearer {config.Auth}");
        var httpResponse = await client.PostAsync("https://api.hamsterkombat.io/clicker/boosts-for-buy", null);
        if (!httpResponse.IsSuccessStatusCode)
        {
            return null;
        }
        using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
        var responseJson = await JsonSerializer.DeserializeAsync<HamsterBoostListResponse>(responseStream);
        return responseJson;
    }

    public async Task<HamsterSyncResponse?> SyncAsync()
    {
        using var client = clientFactory.CreateClient("Hamster");
        client.DefaultRequestHeaders.Add("authorization", $"Bearer {config.Auth}");
        var httpResponse = await client.PostAsync("https://api.hamsterkombat.io/clicker/sync", null);
        if (!httpResponse.IsSuccessStatusCode)
        {
            return null;
        }
        using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
        var responseJson = await JsonSerializer.DeserializeAsync<HamsterSyncResponse>(responseStream);
        return responseJson;
    }

    public async Task<HamsterConfigResponse?> ConfigAsync()
    {
        using var client = clientFactory.CreateClient("Hamster");
        client.DefaultRequestHeaders.Add("authorization", $"Bearer {config.Auth}");
        var httpResponse = await client.PostAsync("https://api.hamsterkombat.io/clicker/config", null);
        if (!httpResponse.IsSuccessStatusCode)
        {
            return null;
        }
        using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
        var responseJson = await JsonSerializer.DeserializeAsync<HamsterConfigResponse>(responseStream);
        return responseJson;
    }

    public async Task<HamsterTapResponse?> TapAsync(HamsterTapRequest request)
    {
        using var client = clientFactory.CreateClient("Hamster");
        client.DefaultRequestHeaders.Add("authorization", $"Bearer {config.Auth}");
        var serializedRequest = JsonSerializer.Serialize(request);
        var serializedRequestContent = new StringContent(serializedRequest, Encoding.UTF8, "application/json");
        var httpResponse = await client.PostAsync("https://api.hamsterkombat.io/clicker/tap", serializedRequestContent);
        if (!httpResponse.IsSuccessStatusCode)
        {
            return null;
        }
        using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
        var responseJson = await JsonSerializer.DeserializeAsync<HamsterTapResponse>(responseStream);
        return responseJson;
    }

    public async Task<HamsterUpgradeBuyResponse?> BuyUpgradeAsync(HamsterUpgradeBuyRequest request)
    {
        using var client = clientFactory.CreateClient("Hamster");
        client.DefaultRequestHeaders.Add("authorization", $"Bearer {config.Auth}");
        var serializedRequest = JsonSerializer.Serialize(request);
        var serializedRequestContent = new StringContent(serializedRequest, Encoding.UTF8, "application/json");
        var httpResponse = await client.PostAsync("https://api.hamsterkombat.io/clicker/buy-upgrade", serializedRequestContent);
        if (!httpResponse.IsSuccessStatusCode)
        {
            return null;
        }
        using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
        var responseJson = await JsonSerializer.DeserializeAsync<HamsterUpgradeBuyResponse>(responseStream);
        return responseJson;
    }

    public async Task<HamsterBoostBuyResponse?> BuyBoostAsync(HamsterBoostBuyRequest request)
    {
        using var client = clientFactory.CreateClient("Hamster");
        client.DefaultRequestHeaders.Add("authorization", $"Bearer {config.Auth}");
        var serializedRequest = JsonSerializer.Serialize(request);
        var serializedRequestContent = new StringContent(serializedRequest, Encoding.UTF8, "application/json");
        var httpResponse = await client.PostAsync("https://api.hamsterkombat.io/clicker/buy-boost", serializedRequestContent);
        if (!httpResponse.IsSuccessStatusCode)
        {
            return null;
        }
        using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
        var responseJson = await JsonSerializer.DeserializeAsync<HamsterBoostBuyResponse>(responseStream);
        return responseJson;
    }

    public async Task<bool> ClaimDailyCipher(HamsterCipherRequest request)
    {
        using var client = clientFactory.CreateClient("Hamster");
        client.DefaultRequestHeaders.Add("authorization", $"Bearer {config.Auth}");
        var serializedRequest = JsonSerializer.Serialize(request);
        var serializedRequestContent = new StringContent(serializedRequest, Encoding.UTF8, "application/json");
        var httpResponse = await client.PostAsync("https://api.hamsterkombat.io/clicker/claim-daily-cipher", serializedRequestContent);
        return httpResponse.IsSuccessStatusCode;
    }

    public Task<bool> ClaimDailyCipher(string cipher)
        => ClaimDailyCipher(new HamsterCipherRequest(cipher));
}
