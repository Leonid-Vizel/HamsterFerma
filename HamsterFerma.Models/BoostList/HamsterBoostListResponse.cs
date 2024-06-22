using System.Text.Json.Serialization;

namespace HamsterFerma.Models.BoostList;

public sealed class HamsterBoostListResponse
{
    [JsonPropertyName("boostsForBuy")]
    public List<HamsterBoost> List { get; set; } = null!;
}
