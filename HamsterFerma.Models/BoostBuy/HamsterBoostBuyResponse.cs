using HamsterFerma.Models.User;
using System.Text.Json.Serialization;

namespace HamsterFerma.Models.BoostBuy;

public sealed class HamsterBoostBuyResponse
{
    [JsonPropertyName("clickerUser")]
    public HamsterClickerUser ClickerUser { get; set; } = null!;
}
