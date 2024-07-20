using BlumFerma.Models.Common;
using BlumFerma.Services.Configs;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BlumFerma.Services.Clients;

public interface IBlumApiClient
{
    Task<List<BlumTask>?> GetTasksAsync(AuthBearerConfig config);
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
}
