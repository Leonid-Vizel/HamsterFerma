namespace BlumFerma.Services.Configs;

public sealed class AuthBearerConfig
{
    public string Token { get; set; } = null!;
    public string Tag { get; set; } = "Default";
    public string Cron { get; set; } = "0 * * ? * * *";
    public bool AutoPlay { get; set; } = true;
    public bool AutoFarm { get; set; } = true;
    public bool AutoTask { get; set; } = true;
    public bool AutoDaily { get; set; } = true;
}
