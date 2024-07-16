using System.Text.Json.Serialization;

namespace HamsterFerma.Models.UpgradeList;

public class HamsterUpgrade
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("price")]
    public int Price { get; set; }

    [JsonPropertyName("profitPerHour")]
    public int ProfitPerHour { get; set; }

    [JsonPropertyName("condition")]
    public HamsterUpgradeCondition? Condition { get; set; } = null!;

    [JsonPropertyName("section")]
    public string Section { get; set; } = null!;

    [JsonPropertyName("level")]
    public int Level { get; set; }

    [JsonPropertyName("currentProfitPerHour")]
    public int CurrentProfitPerHour { get; set; }

    [JsonPropertyName("profitPerHourDelta")]
    public int ProfitPerHourDelta { get; set; }

    [JsonPropertyName("isAvailable")]
    public bool IsAvailable { get; set; }

    [JsonPropertyName("isExpired")]
    public bool IsExpired { get; set; }

    [JsonPropertyName("cooldownSeconds")]
    public int? CooldownSeconds { get; set; }

    [JsonPropertyName("totalCooldownSeconds")]
    public int? TotalCooldownSeconds { get; set; }

    public double Ratio
    {
        get
        {
            if (!IsAvailable)
            {
                return 0;
            }
            return (double)ProfitPerHourDelta / Price;
        }
    }
    public double RatioWithCoolDown
    {
        get
        {
            if (IsAvailable && CooldownSeconds != null && CooldownSeconds.Value != 0)
            {
                return (double)ProfitPerHourDelta * 60 / (Price * CooldownSeconds.Value);
            }
            return Ratio;
        }
    }
}