using System.Text.Json.Serialization;

namespace SeedFerma.Models.WormsList;

public sealed class WormsListResponseData
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = null!;
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }
    [JsonPropertyName("ended_at")]
    public DateTime EndedAt { get; set; }
    [JsonPropertyName("next_worm")]
    public DateTime NextWorm { get; set; }
    [JsonPropertyName("reward")]
    public int Reward { get; set; }
    [JsonPropertyName("is_caught")]
    public bool IsCaught { get; set; }
}
