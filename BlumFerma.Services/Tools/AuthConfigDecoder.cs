using BlumFerma.Services.Configs;
using Microsoft.Extensions.Logging;
using Quartz;
using System.Text.Json;

namespace BlumFerma.Services.Tools;

public interface IAuthConfigDecoder
{
    AuthBearerConfig? Decode(IJobExecutionContext context);
}

public sealed class AuthConfigDecoder(ILogger<AuthConfigDecoder> logger) : IAuthConfigDecoder
{
    public AuthBearerConfig? Decode(IJobExecutionContext context)
    {
        var configString = context.JobDetail.JobDataMap.GetString(nameof(AuthBearerConfig));
        if (configString == null)
        {
            logger.LogError("Авторизационный конфиг не найден!");
            return null;
        }
        var config = JsonSerializer.Deserialize<AuthBearerConfig>(configString);
        if (configString == null)
        {
            logger.LogError("Авторизационный конфиг передан некорректно!");
        }
        return config;
    }
}
