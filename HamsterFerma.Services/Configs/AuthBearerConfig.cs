using System.Text.Json.Serialization;

namespace HamsterFerma.Services.Configs;

public sealed class AuthBearerConfig
{
    public string Token { get; set; } = null!;
    public string Tag { get; set; } = "Default";
    public double MinBalance { get; set; } = 0;
    public double UpgradeBuffer { get; set; } = 0;
    public bool BuyClickLimitBoosts { get; set; } = false;
    public bool BuyUselessPerClickBoosts { get; set; } = false;
    public bool BuyClickFullLimitBoosts { get; set; } = true;
    public string BoostCron { get; set; } = "0 * * ? * * *";
    public bool AutoClick { get; set; } = true;
    public string ClickCron { get; set; } = "5 * * ? * * *";
    public bool AutoUpgrade { get; set; } = true;
    public string UpgradeCron { get; set; } = "10 * * ? * * *";
    public bool AutoCipher { get; set; } = true;
    public string CipherCron { get; set; } = "0 5 22 ? * * *";
    public bool AutoTask { get; set; } = true;
    public string TaskCron { get; set; } = "0 0 * ? * * *";
    [JsonIgnore]
    public bool AutoBoost => BuyClickLimitBoosts || BuyUselessPerClickBoosts || BuyClickFullLimitBoosts;
}
