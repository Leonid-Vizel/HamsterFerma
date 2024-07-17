using System.Text.Json.Serialization;

namespace SeedFerma.Models.MarketList;

public sealed class SeedMarketListRequest
{
    [JsonPropertyName("market_type")]
    public string MarketType { get; set; } = null!;
    [JsonPropertyName("worm_type")]
    public string? WormType { get; set; }
    [JsonPropertyName("sort_by_price")]
    public string SortByPrice { get; set; } = null!;
    [JsonPropertyName("sort_by_updated_at")]
    public string? SortByUpdatedAt { get; set; }
    [JsonPropertyName("page")]
    public int Page { get; set; } = 1;
}
