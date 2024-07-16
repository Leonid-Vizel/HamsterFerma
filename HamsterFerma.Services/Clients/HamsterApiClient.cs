using HamsterFerma.Models.BoostList;
using HamsterFerma.Models.CheckTask;
using HamsterFerma.Models.Cipher;
using HamsterFerma.Models.Config;
using HamsterFerma.Models.Taps;
using HamsterFerma.Models.Tasks;
using HamsterFerma.Models.UpgradeBuy;
using HamsterFerma.Models.Upgrades;
using HamsterFerma.Services.Configs;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HamsterFerma.Services.Clients;

public interface IHamsterApiClient
{
    Task<HamsterUpgradeListResponse?> GetUpgradeListAsync(AuthBearerConfig config);
    Task<HamsterBoostListResponse?> GetBoostListAsync(AuthBearerConfig config);
    Task<HamsterTaskListResponse?> GetTaskListAsync(AuthBearerConfig config);
    Task<HamsterCheckTaskResponse?> CheckTaskAsync(AuthBearerConfig config, HamsterCheckTaskRequest request);
    Task<HamsterCheckTaskResponse?> CheckTaskAsync(AuthBearerConfig config, string taskId);
    Task<HamsterSyncResponse?> SyncAsync(AuthBearerConfig config);
    Task<HamsterConfigResponse?> ConfigAsync(AuthBearerConfig config);
    Task<bool> ClaimDailyCipher(AuthBearerConfig config, HamsterCipherRequest request);
    Task<bool> ClaimDailyCipher(AuthBearerConfig config, string cipher);
    Task<HamsterTapResponse?> TapAsync(AuthBearerConfig config, HamsterTapRequest request);
    Task<HamsterTapResponse?> TapAsync(AuthBearerConfig config, int tapCount);
    Task<HamsterUpgradeBuyResponse?> BuyUpgradeAsync(AuthBearerConfig config, HamsterUpgradeBuyRequest request);
    Task<HamsterUpgradeBuyResponse?> BuyUpgradeAsync(AuthBearerConfig config, string upgradeId);
    Task<HamsterBoostBuyResponse?> BuyBoostAsync(AuthBearerConfig config, HamsterBoostBuyRequest request);
    Task<HamsterBoostBuyResponse?> BuyBoostAsync(AuthBearerConfig config, string boostId);
}

public sealed class HamsterApiClient(IHttpClientFactory clientFactory,
                                     ILogger<HamsterApiClient> logger) : IHamsterApiClient
{
    private static readonly string _tokenErrorMessage = "Заполните Token, ферма не может работать без авторизационного Bearer-токена (ознакомьтесь с Readme.md)!";
    private static readonly string _codeUnsuccessfulErrorMessage = "Обращение к серверу не привело к успешному ответу!";

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
        var responseJson = await JsonSerializer.DeserializeAsync<HamsterUpgradeListResponse>(responseStream);
        return responseJson;
    }

    public async Task<HamsterBoostListResponse?> GetBoostListAsync(AuthBearerConfig config)
    {
        if (string.IsNullOrEmpty(config?.Token))
        {
            logger.LogError($"[Tag: {config?.Tag}] {_tokenErrorMessage}");
            return null;
        }
        using var client = clientFactory.CreateClient("Hamster");
        client.DefaultRequestHeaders.Add("authorization", $"Bearer {config.Token}");
        var httpResponse = await client.PostAsync("https://api.hamsterkombatgame.io/clicker/boosts-for-buy", null);
        if (!httpResponse.IsSuccessStatusCode)
        {
            logger.LogError($"[Tag: {config.Tag}] [Method: {nameof(GetBoostListAsync)}] {_codeUnsuccessfulErrorMessage}");
            return null;
        }
        using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
        var responseJson = await JsonSerializer.DeserializeAsync<HamsterBoostListResponse>(responseStream);
        return responseJson;
    }

    public async Task<HamsterTaskListResponse?> GetTaskListAsync(AuthBearerConfig config)
    {
        if (string.IsNullOrEmpty(config?.Token))
        {
            logger.LogError($"[Tag: {config?.Tag}] {_tokenErrorMessage}");
            return null;
        }
        using var client = clientFactory.CreateClient("Hamster");
        client.DefaultRequestHeaders.Add("authorization", $"Bearer {config.Token}");
        var httpResponse = await client.PostAsync("https://api.hamsterkombatgame.io/clicker/list-tasks", null);
        if (!httpResponse.IsSuccessStatusCode)
        {
            logger.LogError($"[Tag: {config.Tag}] [Method: {nameof(GetTaskListAsync)}] {_codeUnsuccessfulErrorMessage}");
            return null;
        }
        using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
        var responseJson = await JsonSerializer.DeserializeAsync<HamsterTaskListResponse>(responseStream);
        return responseJson;
    }

    public async Task<HamsterSyncResponse?> SyncAsync(AuthBearerConfig config)
    {
        if (string.IsNullOrEmpty(config?.Token))
        {
            logger.LogError($"[Tag: {config?.Tag}] {_tokenErrorMessage}");
            return null;
        }
        using var client = clientFactory.CreateClient("Hamster");
        client.DefaultRequestHeaders.Add("authorization", $"Bearer {config.Token}");
        var httpResponse = await client.PostAsync("https://api.hamsterkombatgame.io/clicker/sync", null);
        if (!httpResponse.IsSuccessStatusCode)
        {
            logger.LogError($"[Tag: {config.Tag}] [Method: {nameof(SyncAsync)}] {_codeUnsuccessfulErrorMessage}");
            return null;
        }
        using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
        var responseJson = await JsonSerializer.DeserializeAsync<HamsterSyncResponse>(responseStream);
        return responseJson;
    }

    public async Task<HamsterConfigResponse?> ConfigAsync(AuthBearerConfig config)
    {
        if (string.IsNullOrEmpty(config?.Token))
        {
            logger.LogError($"[Tag: {config?.Tag}] {_tokenErrorMessage}");
            return null;
        }
        using var client = clientFactory.CreateClient("Hamster");
        client.DefaultRequestHeaders.Add("authorization", $"Bearer {config.Token}");
        var httpResponse = await client.PostAsync("https://api.hamsterkombatgame.io/clicker/config", null);
        if (!httpResponse.IsSuccessStatusCode)
        {
            logger.LogError($"[Tag: {config.Tag}] [Method: {nameof(ConfigAsync)}] {_codeUnsuccessfulErrorMessage}");
            return null;
        }
        using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
        var responseJson = await JsonSerializer.DeserializeAsync<HamsterConfigResponse>(responseStream);
        return responseJson;
    }

    public async Task<HamsterTapResponse?> TapAsync(AuthBearerConfig config, HamsterTapRequest request)
    {
        if (string.IsNullOrEmpty(config?.Token))
        {
            logger.LogError($"[Tag: {config?.Tag}] {_tokenErrorMessage}");
            return null;
        }
        using var client = clientFactory.CreateClient("Hamster");
        client.DefaultRequestHeaders.Add("authorization", $"Bearer {config.Token}");
        var serializedRequest = JsonSerializer.Serialize(request);
        var serializedRequestContent = new StringContent(serializedRequest, Encoding.UTF8, "application/json");
        var httpResponse = await client.PostAsync("https://api.hamsterkombatgame.io/clicker/tap", serializedRequestContent);
        if (!httpResponse.IsSuccessStatusCode)
        {
            logger.LogError($"[Tag: {config.Tag}] [Method: {nameof(TapAsync)}] {_codeUnsuccessfulErrorMessage}");
            return null;
        }
        using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
        var responseJson = await JsonSerializer.DeserializeAsync<HamsterTapResponse>(responseStream);
        return responseJson;
    }

    public Task<HamsterTapResponse?> TapAsync(AuthBearerConfig config, int tapCount)
        => TapAsync(config, new HamsterTapRequest(tapCount));

    public async Task<HamsterCheckTaskResponse?> CheckTaskAsync(AuthBearerConfig config, HamsterCheckTaskRequest request)
    {
        if (string.IsNullOrEmpty(config?.Token))
        {
            logger.LogError($"[Tag: {config?.Tag}] {_tokenErrorMessage}");
            return null;
        }
        using var client = clientFactory.CreateClient("Hamster");
        client.DefaultRequestHeaders.Add("authorization", $"Bearer {config.Token}");
        var serializedRequest = JsonSerializer.Serialize(request);
        var serializedRequestContent = new StringContent(serializedRequest, Encoding.UTF8, "application/json");
        var httpResponse = await client.PostAsync("https://api.hamsterkombatgame.io/clicker/check-task", serializedRequestContent);
        if (!httpResponse.IsSuccessStatusCode)
        {
            logger.LogError($"[Tag: {config.Tag}] [Method: {nameof(CheckTaskAsync)}] {_codeUnsuccessfulErrorMessage}");
            return null;
        }
        using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
        var responseJson = await JsonSerializer.DeserializeAsync<HamsterCheckTaskResponse>(responseStream);
        return responseJson;
    }

    public Task<HamsterCheckTaskResponse?> CheckTaskAsync(AuthBearerConfig config, string taskId)
        => CheckTaskAsync(config, new HamsterCheckTaskRequest(taskId));

    public async Task<HamsterUpgradeBuyResponse?> BuyUpgradeAsync(AuthBearerConfig config, HamsterUpgradeBuyRequest request)
    {
        if (string.IsNullOrEmpty(config?.Token))
        {
            logger.LogError($"[Tag: {config?.Tag}] {_tokenErrorMessage}");
            return null;
        }
        using var client = clientFactory.CreateClient("Hamster");
        client.DefaultRequestHeaders.Add("authorization", $"Bearer {config.Token}");
        var serializedRequest = JsonSerializer.Serialize(request);
        var serializedRequestContent = new StringContent(serializedRequest, Encoding.UTF8, "application/json");
        var httpResponse = await client.PostAsync("https://api.hamsterkombatgame.io/clicker/buy-upgrade", serializedRequestContent);
        if (!httpResponse.IsSuccessStatusCode)
        {
            logger.LogError($"[Tag: {config.Tag}] [Method: {nameof(BuyUpgradeAsync)}] {_codeUnsuccessfulErrorMessage}");
            return null;
        }
        using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
        var responseJson = await JsonSerializer.DeserializeAsync<HamsterUpgradeBuyResponse>(responseStream);
        return responseJson;
    }

    public Task<HamsterUpgradeBuyResponse?> BuyUpgradeAsync(AuthBearerConfig config, string upgradeId)
        => BuyUpgradeAsync(config, new HamsterUpgradeBuyRequest(upgradeId));

    public async Task<HamsterBoostBuyResponse?> BuyBoostAsync(AuthBearerConfig config, HamsterBoostBuyRequest request)
    {
        if (string.IsNullOrEmpty(config?.Token))
        {
            logger.LogError($"[Tag: {config?.Tag}] {_tokenErrorMessage}");
            return null;
        }
        using var client = clientFactory.CreateClient("Hamster");
        client.DefaultRequestHeaders.Add("authorization", $"Bearer {config.Token}");
        var serializedRequest = JsonSerializer.Serialize(request);
        var serializedRequestContent = new StringContent(serializedRequest, Encoding.UTF8, "application/json");
        var httpResponse = await client.PostAsync("https://api.hamsterkombatgame.io/clicker/buy-boost", serializedRequestContent);
        if (!httpResponse.IsSuccessStatusCode)
        {
            logger.LogError($"[Tag: {config.Tag}] [Method: {nameof(BuyBoostAsync)}] {_codeUnsuccessfulErrorMessage}");
            return null;
        }
        using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
        var responseJson = await JsonSerializer.DeserializeAsync<HamsterBoostBuyResponse>(responseStream);
        return responseJson;
    }

    public Task<HamsterBoostBuyResponse?> BuyBoostAsync(AuthBearerConfig config, string boostId)
        => BuyBoostAsync(config, new HamsterBoostBuyRequest(boostId));

    public async Task<bool> ClaimDailyCipher(AuthBearerConfig config, HamsterCipherRequest request)
    {
        if (string.IsNullOrEmpty(config?.Token))
        {
            logger.LogError($"[Tag: {config?.Tag}] {_tokenErrorMessage}");
            return false;
        }
        using var client = clientFactory.CreateClient("Hamster");
        client.DefaultRequestHeaders.Add("authorization", $"Bearer {config.Token}");
        var serializedRequest = JsonSerializer.Serialize(request);
        var serializedRequestContent = new StringContent(serializedRequest, Encoding.UTF8, "application/json");
        var httpResponse = await client.PostAsync("https://api.hamsterkombatgame.io/clicker/claim-daily-cipher", serializedRequestContent);
        return httpResponse.IsSuccessStatusCode;
    }

    public Task<bool> ClaimDailyCipher(AuthBearerConfig config, string cipher)
        => ClaimDailyCipher(config, new HamsterCipherRequest(cipher));
}
