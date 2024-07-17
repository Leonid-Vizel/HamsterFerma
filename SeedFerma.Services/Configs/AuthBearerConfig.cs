using System.Text.Json.Serialization;

namespace SeedFerma.Services.Configs;

public sealed class AuthBearerConfig
{
    public string TelegramData { get; set; } = null!;
    public string Tag { get; set; } = "Default";
    public double MinBalance { get; set; } = 0;
    public string MarketCron { get; set; } = "0 * * ? * * *";
    public bool AutoMarket { get; set; } = true;
    public string ClaimCron { get; set; } = "0 0 * ? * * *";
    public bool AutoClaim { get; set; } = true;
    public string CatchCron { get; set; } = "0 1 * ? * * *";
    public bool AutoCatch { get; set; } = true;
}
