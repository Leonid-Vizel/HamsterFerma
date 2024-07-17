using Microsoft.Extensions.Logging;
using SeedFerma.Models.MarketList;
using SeedFerma.Models.SeedClaim;
using SeedFerma.Models.WormsList;
using SeedFerma.Services.Configs;
using System.Text.Json;

namespace SeedFerma.Services.Clients;

public interface ISeedApiClient
{
    Task<SeedMarketListResponse?> GetMarketListAsync(AuthBearerConfig config, SeedMarketListRequest request);
    Task<SeedClaimResponse?> ClaimAsync(AuthBearerConfig config);
    Task<string?> CatchAsync(AuthBearerConfig config);
    Task<WormsListResponse?> GetWormListAsync(AuthBearerConfig config);
}

public sealed class SeedApiClient(IHttpClientFactory clientFactory,
                                  ILogger<SeedApiClient> logger) : ISeedApiClient
{
    private static readonly string _tokenErrorMessage = "Заполните TelegramData, ферма не может работать без авторизационных данных (ознакомьтесь с Readme.md)!";
    private static readonly string _codeUnsuccessfulErrorMessage = "Обращение к серверу не привело к успешному ответу!";

    public async Task<SeedClaimResponse?> ClaimAsync(AuthBearerConfig config)
    {
        if (string.IsNullOrEmpty(config?.TelegramData))
        {
            logger.LogError($"[Tag: {config?.Tag}] {_tokenErrorMessage}");
            return null;
        }
        using var client = clientFactory.CreateClient("Seed");
        client.DefaultRequestHeaders.Add("telegram-data", config.TelegramData);
        var httpResponse = await client.PostAsync("https://elb.seeddao.org/api/v1/seed/claim", null);
        if (!httpResponse.IsSuccessStatusCode)
        {
            logger.LogError($"[Tag: {config.Tag}] [Method: {nameof(ClaimAsync)}] {_codeUnsuccessfulErrorMessage}");
            return null;
        }
        using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
        var responseJson = await JsonSerializer.DeserializeAsync<SeedClaimResponse>(responseStream);
        return responseJson;
    }

    public async Task<string?> CatchAsync(AuthBearerConfig config)
    {
        if (string.IsNullOrEmpty(config?.TelegramData))
        {
            logger.LogError($"[Tag: {config?.Tag}] {_tokenErrorMessage}");
            return null;
        }
        using var client = clientFactory.CreateClient("Seed");
        client.DefaultRequestHeaders.Add("telegram-data", config.TelegramData);
        var httpResponse = await client.PostAsync("https://elb.seeddao.org/api/v1/worms/catch", null);
        if (!httpResponse.IsSuccessStatusCode)
        {
            logger.LogError($"[Tag: {config.Tag}] [Method: {nameof(CatchAsync)}] {_codeUnsuccessfulErrorMessage}");
            return null;
        }
        using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
        var a = await httpResponse.Content.ReadAsStringAsync();
        //var responseJson = await JsonSerializer.DeserializeAsync<SeedClaimResponse>(responseStream);
        return a;
    }

    public async Task<SeedClaimResponse?> BuyAsync(AuthBearerConfig config)
    {
        if (string.IsNullOrEmpty(config?.TelegramData))
        {
            logger.LogError($"[Tag: {config?.Tag}] {_tokenErrorMessage}");
            return null;
        }
        using var client = clientFactory.CreateClient("Seed");
        client.DefaultRequestHeaders.Add("telegram-data", config.TelegramData);
        var httpResponse = await client.PostAsync("https://elb.seeddao.org/api/v1/worms/catch", null);
        if (!httpResponse.IsSuccessStatusCode)
        {
            logger.LogError($"[Tag: {config.Tag}] [Method: {nameof(BuyAsync)}] {_codeUnsuccessfulErrorMessage}");
            return null;
        }
        using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
        var responseJson = await JsonSerializer.DeserializeAsync<SeedClaimResponse>(responseStream);
        return responseJson;
    }

    public async Task<WormsListResponse?> GetWormListAsync(AuthBearerConfig config)
    {
        if (string.IsNullOrEmpty(config?.TelegramData))
        {
            logger.LogError($"[Tag: {config?.Tag}] {_tokenErrorMessage}");
            return null;
        }
        using var client = clientFactory.CreateClient("Seed");
        client.DefaultRequestHeaders.Add("telegram-data", config.TelegramData);
        var httpResponse = await client.PostAsync("https://elb.seeddao.org/api/v1/worms", null);
        if (!httpResponse.IsSuccessStatusCode)
        {
            logger.LogError($"[Tag: {config.Tag}] [Method: {nameof(GetWormListAsync)}] {_codeUnsuccessfulErrorMessage}");
            return null;
        }
        using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
        var responseJson = await JsonSerializer.DeserializeAsync<WormsListResponse>(responseStream);
        return responseJson;
    }

    public async Task<SeedMarketListResponse?> GetMarketListAsync(AuthBearerConfig config, SeedMarketListRequest request)
    {
        if (string.IsNullOrEmpty(config?.TelegramData))
        {
            logger.LogError($"[Tag: {config?.Tag}] {_tokenErrorMessage}");
            return null;
        }
        using var client = clientFactory.CreateClient("Seed");
        client.DefaultRequestHeaders.Add("telegram-data", config.TelegramData);
        var queryString = JsonSerializer
            .Deserialize<IDictionary<string, object?>>(JsonSerializer.Serialize(request))?
            .Where(x => x.Value != null)
            .Select(x => $"{Uri.EscapeDataString(x.Key)}={Uri.EscapeDataString(x.Value!.ToString()!)}")
            .ToList();
        string url = "https://elb.seeddao.org/api/v1/market";
        if (queryString != null && queryString.Count > 0)
        {
            url = $"{url}?{string.Join("&", queryString)}";
        }
        var httpResponse = await client.GetAsync(url);
        if (!httpResponse.IsSuccessStatusCode)
        {
            logger.LogError($"[Tag: {config.Tag}] [Method: {nameof(GetMarketListAsync)}] {_codeUnsuccessfulErrorMessage}");
            return null;
        }
        using var responseStream = await httpResponse.Content.ReadAsStreamAsync();
        var responseJson = await JsonSerializer.DeserializeAsync<SeedMarketListResponse>(responseStream);
        return responseJson;
    }
}
