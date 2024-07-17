using System.Text.Json.Serialization;

namespace SeedFerma.Models.SeedClaim;

public sealed class SeedClaimResponseData
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;
    [JsonPropertyName("user_id")]
    public string UserId { get; set; } = null!;
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }
    [JsonPropertyName("amount")]
    public int Amount { get; set; }
}
