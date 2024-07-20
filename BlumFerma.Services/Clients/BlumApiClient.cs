using BlumFerma.Models.Common;
using BlumFerma.Services.Configs;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlumFerma.Services.Clients;

public interface IBlumApiClient
{
    Task<List<BlumTask>?> GetTasksAsync(AuthBearerConfig config);
    Task<BlumTask?> StartTaskAsync(AuthBearerConfig config, string taskId);
    Task<BlumTask?> ClaimTaskAsync(AuthBearerConfig config, string taskId);
    Task<BlumGame?> StartGameAsync(AuthBearerConfig config);
    Task<BlumBalance?> GetBalanceAsync(AuthBearerConfig config);
    Task<bool> ClaimGameAsync(AuthBearerConfig config, BlumGame game);
    Task<bool> ClaimGameAsync(AuthBearerConfig config, string gameId, ushort points);
}

public sealed class BlumApiClient(IHttpClientFactory clientFactory,
                                  ILogger<BlumApiClient> logger) : IBlumApiClient
{
    private static readonly string _tokenErrorMessage = "Заполните Token, ферма не может работать без авторизационного Bearer-токена (ознакомьтесь с Readme.md)!";
    private static readonly string _codeUnsuccessfulErrorMessage = "Обращение к серверу не привело к успешному ответу!";

    public async Task<List<BlumTask>?> GetTasksAsync(AuthBearerConfig config)
    {
        if (string.IsNullOrEmpty(config?.Token))
        {
            logger.LogError($"[Tag: {config?.Tag}] {_tokenErrorMessage}");
            return null;
        }
        using var client = clientFactory.CreateClient("Blum");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {config.Token}");
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
        if (string.IsNullOrEmpty(config?.Token))
        {
            logger.LogError($"[Tag: {config?.Tag}] {_tokenErrorMessage}");
            return null;
        }
        using var client = clientFactory.CreateClient("Blum");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {config.Token}");
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
        if (string.IsNullOrEmpty(config?.Token))
        {
            logger.LogError($"[Tag: {config?.Tag}] {_tokenErrorMessage}");
            return null;
        }
        using var client = clientFactory.CreateClient("Blum");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {config.Token}");
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

    public async Task<BlumBalance?> GetBalanceAsync(AuthBearerConfig config)
    {
        if (string.IsNullOrEmpty(config?.Token))
        {
            logger.LogError($"[Tag: {config?.Tag}] {_tokenErrorMessage}");
            return null;
        }
        using var client = clientFactory.CreateClient("Blum");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {config.Token}");
        var httpResponse = await client.PostAsync($"https://game-domain.blum.codes/api/v1/user/balance", null);
        if (!httpResponse.IsSuccessStatusCode)
        {
            logger.LogError($"[Tag: {config.Tag}] [Method: {nameof(GetBalanceAsync)}] {_codeUnsuccessfulErrorMessage}");
            return null;
        }
        using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
        var responseJson = await JsonSerializer.DeserializeAsync<BlumBalance?>(responseStream);
        return responseJson;
    }

    public async Task<BlumGame?> StartGameAsync(AuthBearerConfig config)
    {
        if (string.IsNullOrEmpty(config?.Token))
        {
            logger.LogError($"[Tag: {config?.Tag}] {_tokenErrorMessage}");
            return null;
        }
        using var client = clientFactory.CreateClient("Blum");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {config.Token}");
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
        if (string.IsNullOrEmpty(config?.Token))
        {
            logger.LogError($"[Tag: {config?.Tag}] {_tokenErrorMessage}");
            return false;
        }
        using var client = clientFactory.CreateClient("Blum");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {config.Token}");
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
}
