using System.Text.Json.Serialization;

namespace SeedFerma.Models.MarketList;

public class SeedMarketListResponseData
{
    [JsonPropertyName("total")]
    public int Total { get; set; }
    [JsonPropertyName("page")]
    public int Page { get; set; }
    [JsonPropertyName("page_size")]
    public int PageSize { get; set; }
    [JsonPropertyName("items")]
    public List<SeedMarketListResponseItem> Items { get; set; } = null!;
}

public class SeedMarketListResponseItem
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;
    [JsonPropertyName("egg_id")]
    public string? EggId { get; set; }
    [JsonPropertyName("egg_type")]
    public string? EggType { get; set; }
    [JsonPropertyName("worm_id")]
    public string? WormId { get; set; }
    [JsonPropertyName("worm_type")]
    public string? WormType { get; set; }
    [JsonPropertyName("price_gross")]
    public int PriceGross { get; set; }
    [JsonPropertyName("price_net")]
    public int PriceNet { get; set; }
    [JsonPropertyName("fee")]
    public int Fee { get; set; }
    [JsonPropertyName("status")]
    public string Status { get; set; } = null!;
    [JsonPropertyName("created_by")]
    public string CreatedBy { get; set; } = null!;
    [JsonPropertyName("bought_by")]
    public object BoughtBy { get; set; } = null!;
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }
    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }
}

public class MarketListResponse
{
    [JsonPropertyName("data")]
    public SeedMarketListResponseData Data { get; set; } = null!;
}