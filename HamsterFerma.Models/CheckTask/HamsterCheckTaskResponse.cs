using HamsterFerma.Models.User;
using System.Text.Json.Serialization;

namespace HamsterFerma.Models.CheckTask;

public sealed class HamsterCheckTaskResponse
{
    [JsonPropertyName("task")]
    public HamsterCheckedTask Task { get; set; } = null!;
    [JsonPropertyName("clickerUser")]
    public HamsterClickerUser ClickerUser { get; set; } = null!;
}
