namespace BlumFerma.Services.Configs;

public sealed class AuthBearerConfig
{
    public string RefreshToken { get; set; } = null!;
    public string AccessToken { get; set; } = null!;
    public string Tag { get; set; } = "Default";
    public string Cron { get; set; } = "0 0 0/2 ? * * *";
    public bool AutoPlay { get; set; } = true;
    public bool AutoFarm { get; set; } = true;
    public bool AutoTask { get; set; } = true;
    public bool AutoDaily { get; set; } = true;
}
