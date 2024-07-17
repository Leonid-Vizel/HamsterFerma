using System.Text.Json.Serialization;

namespace SeedFerma.Models.SeedClaim;

public sealed class SeedClaimResponse
{
    [JsonPropertyName("data")]
    public SeedClaimResponseData Data { get; set; } = null!;
}
