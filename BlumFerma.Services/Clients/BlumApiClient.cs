using BlumFerma.Models.Balance;
using BlumFerma.Models.Common;
using BlumFerma.Models.Refresh;
using BlumFerma.Services.Configs;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace BlumFerma.Services.Clients;

public interface IBlumApiClient
{
    Task<List<BlumTask>?> GetTasksAsync(AuthBearerConfig config);
    Task<BlumTask?> StartTaskAsync(AuthBearerConfig config, string taskId);
    Task<BlumTask?> ClaimTaskAsync(AuthBearerConfig config, string taskId);
    Task<BlumGame?> StartGameAsync(AuthBearerConfig config);
    Task<BlumBalanceResponse?> GetBalanceAsync(AuthBearerConfig config);
    Task<bool> ClaimGameAsync(AuthBearerConfig config, BlumGame game);
    Task<bool> ClaimGameAsync(AuthBearerConfig config, string gameId, ushort points);
    Task<BlumRefreshResponse?> RefreshTokenAsync(AuthBearerConfig config, bool autoSet = true);
    Task<BlumFarmingState?> StartFarmingAsync(AuthBearerConfig config);
    Task<BlumBalanceResponse?> ClaimFarmingAsync(AuthBearerConfig config);
    Task<bool> CheckDailyRewardAsync(AuthBearerConfig config, short minutesUtcOffset = -180);
}

public sealed class BlumApiClient(IHttpClientFactory clientFactory,
                                  ILogger<BlumApiClient> logger) : IBlumApiClient
{
    private static readonly string _tokenErrorMessage = "Заполните Token, ферма не может работать без авторизационного Bearer-токена (ознакомьтесь с Readme.md)!";
    private static readonly string _codeUnsuccessfulErrorMessage = "Обращение к серверу не привело к успешному ответу!";

    public async Task<List<BlumTask>?> GetTasksAsync(AuthBearerConfig config)
    {
        if (string.IsNullOrEmpty(config?.AccessToken))
        {
            logger.LogError($"[Tag: {config?.Tag}] {_tokenErrorMessage}");
            return null;
        }
        using var client = clientFactory.CreateClient("Blum");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {config.AccessToken}");
        var httpResponse = await client.GetAsync("https://game-domain.blum.codes/api/v1/tasks");
        if (!httpResponse.IsSuccessStatusCode)
        {
            logger.LogError($"[Tag: {config.Tag}] [Method: {nameof(GetTasksAsync)}] {_codeUnsuccessfulErrorMessage}");
            return null;
        }
        using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
        var responseJson = await JsonSerializer.DeserializeAsync<List<BlumTask>>(responseStream);
        return responseJson;
    }

    public async Task<BlumTask?> StartTaskAsync(AuthBearerConfig config, string taskId)
    {
        if (string.IsNullOrEmpty(config?.AccessToken))
        {
            logger.LogError($"[Tag: {config?.Tag}] {_tokenErrorMessage}");
            return null;
        }
        using var client = clientFactory.CreateClient("Blum");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {config.AccessToken}");
        var httpResponse = await client.PostAsync($"https://game-domain.blum.codes/api/v1/tasks/{taskId}/start", null);
        if (!httpResponse.IsSuccessStatusCode)
        {
            logger.LogError($"[Tag: {config.Tag}] [Method: {nameof(StartTaskAsync)}] {_codeUnsuccessfulErrorMessage}");
            return null;
        }
        using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
        var responseJson = await JsonSerializer.DeserializeAsync<BlumTask?>(responseStream);
        return responseJson;
    }

    public async Task<BlumTask?> ClaimTaskAsync(AuthBearerConfig config, string taskId)
    {
        if (string.IsNullOrEmpty(config?.AccessToken))
        {
            logger.LogError($"[Tag: {config?.Tag}] {_tokenErrorMessage}");
            return null;
        }
        using var client = clientFactory.CreateClient("Blum");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {config.AccessToken}");
        var httpResponse = await client.PostAsync($"https://game-domain.blum.codes/api/v1/tasks/{taskId}/claim", null);
        if (!httpResponse.IsSuccessStatusCode)
        {
            logger.LogError($"[Tag: {config.Tag}] [Method: {nameof(ClaimTaskAsync)}] {_codeUnsuccessfulErrorMessage}");
            return null;
        }
        using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
        var responseJson = await JsonSerializer.DeserializeAsync<BlumTask?>(responseStream);
        return responseJson;
    }

    public async Task<BlumBalanceResponse?> GetBalanceAsync(AuthBearerConfig config)
    {
        if (string.IsNullOrEmpty(config?.AccessToken))
        {
            logger.LogError($"[Tag: {config?.Tag}] {_tokenErrorMessage}");
            return null;
        }
        using var client = clientFactory.CreateClient("Blum");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {config.AccessToken}");
        var httpResponse = await client.GetAsync($"https://game-domain.blum.codes/api/v1/user/balance");
        if (!httpResponse.IsSuccessStatusCode)
        {
            logger.LogError($"[Tag: {config.Tag}] [Method: {nameof(GetBalanceAsync)}] {_codeUnsuccessfulErrorMessage}");
            return null;
        }
        using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
        var responseJson = await JsonSerializer.DeserializeAsync<BlumBalanceResponse?>(responseStream);
        return responseJson;
    }

    public async Task<BlumFarmingState?> StartFarmingAsync(AuthBearerConfig config)
    {
        if (string.IsNullOrEmpty(config?.AccessToken))
        {
            logger.LogError($"[Tag: {config?.Tag}] {_tokenErrorMessage}");
            return null;
        }
        using var client = clientFactory.CreateClient("Blum");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {config.AccessToken}");
        var httpResponse = await client.PostAsync($"https://game-domain.blum.codes/api/v1/farming/start", null);
        if (!httpResponse.IsSuccessStatusCode)
        {
            logger.LogError($"[Tag: {config.Tag}] [Method: {nameof(StartFarmingAsync)}] {_codeUnsuccessfulErrorMessage}");
            return null;
        }
        using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
        var responseJson = await JsonSerializer.DeserializeAsync<BlumFarmingState?>(responseStream);
        return responseJson;
    }

    public async Task<BlumBalanceResponse?> ClaimFarmingAsync(AuthBearerConfig config)
    {
        if (string.IsNullOrEmpty(config?.AccessToken))
        {
            logger.LogError($"[Tag: {config?.Tag}] {_tokenErrorMessage}");
            return null;
        }
        using var client = clientFactory.CreateClient("Blum");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {config.AccessToken}");
        var httpResponse = await client.PostAsync($"https://game-domain.blum.codes/api/v1/farming/claim", null);
        if (!httpResponse.IsSuccessStatusCode)
        {
            logger.LogError($"[Tag: {config.Tag}] [Method: {nameof(ClaimFarmingAsync)}] {_codeUnsuccessfulErrorMessage}");
            return null;
        }
        using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
        var responseJson = await JsonSerializer.DeserializeAsync<BlumBalanceResponse?>(responseStream);
        return responseJson;
    }

    public async Task<BlumGame?> StartGameAsync(AuthBearerConfig config)
    {
        if (string.IsNullOrEmpty(config?.AccessToken))
        {
            logger.LogError($"[Tag: {config?.Tag}] {_tokenErrorMessage}");
            return null;
        }
        using var client = clientFactory.CreateClient("Blum");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {config.AccessToken}");
        var httpResponse = await client.PostAsync("https://game-domain.blum.codes/api/v1/game/play", null);
        if (!httpResponse.IsSuccessStatusCode)
        {
            logger.LogError($"[Tag: {config.Tag}] [Method: {nameof(StartGameAsync)}] {_codeUnsuccessfulErrorMessage}");
            return null;
        }
        using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
        var responseJson = await JsonSerializer.DeserializeAsync<BlumGame?>(responseStream);
        return responseJson;
    }

    public async Task<bool> ClaimGameAsync(AuthBearerConfig config, BlumGame game)
    {
        if (string.IsNullOrEmpty(config?.AccessToken))
        {
            logger.LogError($"[Tag: {config?.Tag}] {_tokenErrorMessage}");
            return false;
        }
        using var client = clientFactory.CreateClient("Blum");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {config.AccessToken}");
        var serializedRequest = JsonSerializer.Serialize(game);
        var serializedRequestContent = new StringContent(serializedRequest, Encoding.UTF8, "application/json");
        var httpResponse = await client.PostAsync("https://game-domain.blum.codes/api/v1/game/claim", serializedRequestContent);
        if (!httpResponse.IsSuccessStatusCode)
        {
            logger.LogError($"[Tag: {config.Tag}] [Method: {nameof(ClaimGameAsync)}] {_codeUnsuccessfulErrorMessage}");
            return false;
        }
        return true;
    }

    public Task<bool> ClaimGameAsync(AuthBearerConfig config, string gameId, ushort points)
        => ClaimGameAsync(config, new(gameId, points));

    public async Task<BlumRefreshResponse?> RefreshTokenAsync(AuthBearerConfig config, bool autoSet = true)
    {
        if (string.IsNullOrEmpty(config?.RefreshToken))
        {
            logger.LogError($"[Tag: {config?.Tag}] {_tokenErrorMessage}");
            return null;
        }
        using var client = clientFactory.CreateClient("Blum");
        var serializedRequestModel = new BlumRefreshRequest(config.RefreshToken);
        var serializedRequest = JsonSerializer.Serialize(serializedRequestModel);
        var serializedRequestContent = new StringContent(serializedRequest, Encoding.UTF8, "application/json");
        var httpResponse = await client.PostAsync("https://gateway.blum.codes/v1/auth/refresh", serializedRequestContent);
        if (!httpResponse.IsSuccessStatusCode)
        {
            logger.LogError($"[Tag: {config.Tag}] [Method: {nameof(RefreshTokenAsync)}] {_codeUnsuccessfulErrorMessage}");
            return null;
        }
        using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
        var responseJson = await JsonSerializer.DeserializeAsync<BlumRefreshResponse?>(responseStream);
        if (responseJson != null && autoSet)
        {
            config.RefreshToken = responseJson.Refresh;
            config.AccessToken = responseJson.Access;
        }
        return responseJson;
    }

    public async Task<bool> CheckDailyRewardAsync(AuthBearerConfig config, short minutesUtcOffset = -180)
    {
        if (string.IsNullOrEmpty(config?.AccessToken))
        {
            logger.LogError($"[Tag: {config?.Tag}] {_tokenErrorMessage} ");
            return false;
        }
        using var client = clientFactory.CreateClient("Blum");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {config.AccessToken}");
        var httpResponse = await client.PostAsync($"https://game-domain.blum.codes/api/v1/daily-reward?offset={minutesUtcOffset}", null);
        if (!httpResponse.IsSuccessStatusCode)
        {
            logger.LogError($"[Tag: {config.Tag}] [Method: {nameof(CheckDailyRewardAsync)}] {_codeUnsuccessfulErrorMessage}");
            return false;
        }
        return true;
    }
}
