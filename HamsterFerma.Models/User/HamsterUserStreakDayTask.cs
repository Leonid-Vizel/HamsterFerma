using System.Text.Json.Serialization;

namespace HamsterFerma.Models.User;

public sealed class HamsterUserStreakDayTask
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;
    [JsonPropertyName("completedAt")]
    public DateTime CompletedAt { get; set; }
    [JsonPropertyName("days")]
    public int Days { get; set; }
}