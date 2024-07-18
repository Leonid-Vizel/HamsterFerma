using System.Text.Json.Serialization;

namespace SeedFerma.Models.WormCatch;

public sealed class WormCatchResponse
{
    [JsonPropertyName("data")]
    public WormCatchResponseData Data { get; set; } = null!;
}