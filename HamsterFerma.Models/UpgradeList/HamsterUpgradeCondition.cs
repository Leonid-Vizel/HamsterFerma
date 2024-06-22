using System.Text.Json.Serialization;

namespace HamsterFerma.Models.Upgrades;

public sealed class HamsterUpgradeCondition
{
    [JsonPropertyName("_type")]
    public string Type { get; set; } = null!;

    [JsonPropertyName("upgradeId")]
    public string UpgradeId { get; set; } = null!;

    [JsonPropertyName("level")]
    public int Level { get; set; }

    [JsonPropertyName("referralCount")]
    public int? ReferralCount { get; set; }

    [JsonPropertyName("moreReferralsCount")]
    public int? MoreReferralsCount { get; set; }
}