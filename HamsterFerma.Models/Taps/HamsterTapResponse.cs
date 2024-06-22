using System.Text.Json.Serialization;
using HamsterFerma.Models.User;

namespace HamsterFerma.Models.Taps;

public sealed class HamsterTapResponse
{
    [JsonPropertyName("clickerUser")]
    public HamsterClickerUser ClickerUser { get; set; } = null!;
}