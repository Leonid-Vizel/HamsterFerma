using AngleSharp;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace HamsterFerma.Services.Clients;

public interface IComboApiClient
{
    Task<string[]?> GetLatestComboAsync();
}

public sealed class ComboClient(IHttpClientFactory clientFactory,
                                IMemoryCache cache) : IComboApiClient
{
    public async Task<string[]?> GetLatestComboAsync()
    {
        if (cache.TryGetValue($"{nameof(ComboClient)}_LatestCombo", out var cacheVal) && cacheVal is string[] cachedComboArray)
        {
            return cachedComboArray;
        }
        using var client = clientFactory.CreateClient("Combo");
        using var htmlResponse = await client.GetAsync("https://hamster-combo.com/");
        if (!htmlResponse.IsSuccessStatusCode)
        {
            return null;
        }
        using var htmlStream = await htmlResponse.Content.ReadAsStreamAsync();
        var config = Configuration.Default;
        using var context = BrowsingContext.New(config);
        using var doc = await context.OpenAsync(req => req.Content(htmlStream));
        return doc.QuerySelectorAll("div.hk-card > a")
            .Select(x => x.Attributes["href"]!.ToString())
            .Select(x => x!.Replace("https://hamster-combo.com/", ""))
            .Select(x => x!.Substring(0, x.Length - 1))
            .ToArray();
    }
}