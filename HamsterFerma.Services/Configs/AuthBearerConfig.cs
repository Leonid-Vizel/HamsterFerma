using System.Text.Json.Serialization;

namespace HamsterFerma.Services.Configs;

public sealed class AuthBearerConfig
{
    public string Auth { get; set; } = null!;
    public double MinBalance { get; set; } = 0;
    public bool BuyClickLimitBoosts { get; set; } = false;
    public bool BuyUselessPerClickBoosts { get; set; } = false;
    public bool BuyClickFullLimitBoosts { get; set; } = true;
    public bool AutoClick { get; set; } = true;
    public bool AutoUpgrade { get; set; } = true;
    public bool AutoCipher { get; set; } = true;
    [JsonIgnore]
    public bool AutoBoost => BuyClickLimitBoosts || BuyUselessPerClickBoosts || BuyClickFullLimitBoosts;
}
