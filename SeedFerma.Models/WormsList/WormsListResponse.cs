using System.Text.Json.Serialization;

namespace SeedFerma.Models.WormsList;

public sealed class WormsListResponse
{
    [JsonPropertyName("data")]
    public WormsListResponseData Data { get; set; } = null!;
}