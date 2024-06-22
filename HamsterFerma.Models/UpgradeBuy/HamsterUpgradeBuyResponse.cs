using HamsterFerma.Models.User;
using System.Text.Json.Serialization;

namespace HamsterFerma.Models.UpgradeBuy;

public sealed class HamsterUpgradeBuyResponse
{
    [JsonPropertyName("clickerUser")]
    public HamsterClickerUser ClickerUser { get; set; } = null!;
}
