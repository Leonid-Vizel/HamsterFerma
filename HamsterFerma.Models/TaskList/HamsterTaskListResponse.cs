using System.Text.Json.Serialization;

namespace HamsterFerma.Models.Tasks;

public sealed class HamsterTask
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;
    [JsonPropertyName("rewardCoins")]
    public int RewardCoins { get; set; }
    [JsonPropertyName("periodicity")]
    public string Periodicity { get; set; } = null!;
    [JsonPropertyName("link")]
    public string Link { get; set; } = null!;
    [JsonPropertyName("isCompleted")]
    public bool IsCompleted { get; set; }
    [JsonPropertyName("completedAt")]
    public DateTime? CompletedAt { get; set; }
    [JsonPropertyName("channelId")]
    public int? ChannelId { get; set; }
    [JsonPropertyName("days")]
    public int? Days { get; set; }
    [JsonPropertyName("remainSeconds")]
    public int? RemainSeconds { get; set; }
}

public sealed class HamsterTaskListResponse
{
    [JsonPropertyName("tasks")]
    public List<HamsterTask> Tasks { get; set; } = [];
}
