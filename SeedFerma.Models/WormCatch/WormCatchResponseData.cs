using System.Text.Json.Serialization;

namespace SeedFerma.Models.WormCatch;

public sealed class WormCatchResponseData
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;
    [JsonPropertyName("type")]
    public string Type { get; set; } = null!;
    [JsonPropertyName("status")]
    public string Status { get; set; } = null!;
    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }
    [JsonPropertyName("reward")]
    public int Reward { get; set; }
    [JsonPropertyName("on_market")]
    public bool OnMarket { get; set; }
    [JsonPropertyName("owner_id")]
    public string OwnerId { get; set; } = null!;
    [JsonPropertyName("market_id")]
    public string? MarketId { get; set; }
    [JsonPropertyName("price")]
    public string? Price { get; set; }
}