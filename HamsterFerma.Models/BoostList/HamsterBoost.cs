using System.Text.Json.Serialization;

namespace HamsterFerma.Models.BoostList;

public class HamsterBoost
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;
    [JsonPropertyName("price")]
    public int Price { get; set; }
    [JsonPropertyName("earnPerTap")]
    public int EarnPerTap { get; set; }
    [JsonPropertyName("maxTaps")]
    public int MaxTaps { get; set; }
    [JsonPropertyName("cooldownSeconds")]
    public int CooldownSeconds { get; set; }
    [JsonPropertyName("level")]
    public int Level { get; set; }
    [JsonPropertyName("maxTapsDelta")]
    public int MaxTapsDelta { get; set; }
    [JsonPropertyName("earnPerTapDelta")]
    public int EarnPerTapDelta { get; set; }
    [JsonPropertyName("maxLevel")]
    public int? MaxLevel { get; set; }
}