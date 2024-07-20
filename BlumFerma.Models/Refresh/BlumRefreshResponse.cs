using System.Text.Json.Serialization;

namespace BlumFerma.Models.Refresh;

public sealed class BlumRefreshResponse
{
    [JsonPropertyName("access")]
    public string Access { get; set; } = null!;
    [JsonPropertyName("refresh")]
    public string Refresh { get; set; } = null!;
}
