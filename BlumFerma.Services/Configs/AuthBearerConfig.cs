namespace BlumFerma.Services.Configs;

public sealed class AuthBearerConfig
{
    public string Token { get; set; } = null!;
    public string Tag { get; set; } = "Default";
    public string PlayCron { get; set; } = "0 * * ? * * *";
    public bool AutoPlay { get; set; } = true;
    public string FarmCron { get; set; } = "5 * * ? * * *";
    public bool AutoFarm { get; set; } = true;
    public string UpgradeCron { get; set; } = "10 * * ? * * *";
    public bool AutoTask { get; set; } = true;
    public string TaskCron { get; set; } = "0 0 * ? * * *";
    public bool AutoDaily { get; set; } = true;
    public string DailyCron { get; set; } = "0 0 * ? * * *";
}
