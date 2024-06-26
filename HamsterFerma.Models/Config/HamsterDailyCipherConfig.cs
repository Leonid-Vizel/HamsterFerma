using System.Text.Json.Serialization;

namespace HamsterFerma.Models.Config;

public class HamsterDailyCipherConfig
{
    [JsonPropertyName("cipher")]
    public string Cipher { get; set; } = null!;
    [JsonPropertyName("bonusCoins")]
    public int BonusCoins { get; set; }
    [JsonPropertyName("isClaimed")]
    public bool IsClaimed { get; set; }
    [JsonPropertyName("remainSeconds")]
    public int RemainSeconds { get; set; }
}