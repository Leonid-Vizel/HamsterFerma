using System.Text.Json.Serialization;

namespace HamsterFerma.Models.CheckTask;

public sealed class HamsterCheckedTask
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;
    [JsonPropertyName("rewardCoins")]
    public int RewardCoins { get; set; }
    [JsonPropertyName("periodicity")]
    public string Periodicity { get; set; } = null!;
    [JsonPropertyName("link")]
    public string Link { get; set; } = null!;
    [JsonPropertyName("completedAt")]
    public DateTime CompletedAt { get; set; }
    [JsonPropertyName("isCompleted")]
    public bool IsCompleted { get; set; }
}
