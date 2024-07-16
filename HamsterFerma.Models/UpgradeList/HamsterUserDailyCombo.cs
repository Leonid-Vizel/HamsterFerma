using System.Text.Json.Serialization;

namespace HamsterFerma.Models.UpgradeList;

public sealed class HamsterUserDailyCombo
{
    [JsonPropertyName("upgradeIds")]
    public List<string> UpgradeIds { get; set; } = null!;

    [JsonPropertyName("bonusCoins")]
    public int BonusCoins { get; set; }

    [JsonPropertyName("isClaimed")]
    public bool IsClaimed { get; set; }

    [JsonPropertyName("remainSeconds")]
    public int RemainSeconds { get; set; }
}
