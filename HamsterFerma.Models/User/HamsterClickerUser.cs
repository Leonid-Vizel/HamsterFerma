using System.Text.Json.Serialization;

namespace HamsterFerma.Models.User;

public sealed class HamsterClickerUser
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;
    [JsonPropertyName("totalCoins")]
    public double TotalCoins { get; set; }
    [JsonPropertyName("balanceCoins")]
    public double BalanceCoins { get; set; }
    [JsonPropertyName("level")]
    public int Level { get; set; }
    [JsonPropertyName("availableTaps")]
    public int AvailableTaps { get; set; }
    [JsonPropertyName("lastSyncUpdate")]
    public int LastSyncUpdate { get; set; }
    [JsonPropertyName("tasks")]
    public HamsterUserTasks Tasks { get; set; } = null!;
    [JsonPropertyName("referralsCount")]
    public int ReferralsCount { get; set; }
    [JsonPropertyName("maxTaps")]
    public int MaxTaps { get; set; }
    [JsonPropertyName("earnPerTap")]
    public int EarnPerTap { get; set; }
    [JsonPropertyName("earnPassivePerSec")]
    public double EarnPassivePerSec { get; set; }
    [JsonPropertyName("earnPassivePerHour")]
    public int EarnPassivePerHour { get; set; }
    [JsonPropertyName("lastPassiveEarn")]
    public double LastPassiveEarn { get; set; }
    [JsonPropertyName("tapsRecoverPerSec")]
    public int TapsRecoverPerSec { get; set; }
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }
}