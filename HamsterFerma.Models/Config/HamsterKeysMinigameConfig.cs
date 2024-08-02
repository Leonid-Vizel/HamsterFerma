using System.Text.Json.Serialization;

namespace HamsterFerma.Models.Config;

public sealed class HamsterKeysMinigameConfig
{
    [JsonPropertyName("isClaimed")]
    public bool IsClaimed { get; set; }
}
