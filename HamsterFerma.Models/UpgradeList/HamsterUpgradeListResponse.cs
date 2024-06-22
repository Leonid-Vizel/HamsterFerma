using System.Text.Json.Serialization;

namespace HamsterFerma.Models.Upgrades;

public sealed class HamsterUpgradeListResponse
{
    [JsonPropertyName("upgradesForBuy")]
    public List<HamsterUpgrade> UpgradesForBuy { get; set; } = null!;

    [JsonPropertyName("dailyCombo")]
    public HamsterUserDailyCombo DailyCombo { get; set; } = null!;
}