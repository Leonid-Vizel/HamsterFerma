using System.Text.Json.Serialization;

namespace HamsterFerma.Models.User;

public sealed class HamsterUserTasks
{
    [JsonPropertyName("streak_days")]
    public HamsterUserStreakDayTask StreakDays { get; set; } = null!;
}