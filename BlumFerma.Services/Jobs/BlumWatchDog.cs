using BlumFerma.Models.Common;
using BlumFerma.Services.Clients;
using BlumFerma.Services.Configs;
using BlumFerma.Services.Tools;
using Microsoft.Extensions.Logging;
using Quartz;
using System.Text.Json;

namespace BlumFerma.Services.Jobs;

public sealed class BlumWatchDog(IBlumApiClient client,
                                 IAuthConfigDecoder configDecoder,
                                 ILogger<BlumWatchDog> logger,
                                 IBlumTaskCompleter taskCompleter) : IJob
{
    public static void ConfigureFor(IServiceCollectionQuartzConfigurator options, AuthBearerConfig config, TimeZoneInfo timeZone)
    {
        if (!config.AutoTask)
        {
            return;
        }
        if (string.IsNullOrEmpty(config.Token))
        {
            return;
        }
        var key = CreateKey(config.Tag);
        options.AddJob<BlumWatchDog>(key, job => job
            .UsingJobData(nameof(AuthBearerConfig), JsonSerializer.Serialize(config))
        ).AddTrigger(trigger => trigger
            .ForJob(key)
            .WithCronSchedule(config.Cron, x => x.InTimeZone(timeZone))
        );
    }

    public static JobKey CreateKey(string tag)
        => JobKey.Create(nameof(BlumWatchDog), tag);

    public async Task Execute(IJobExecutionContext context)
    {
        var config = configDecoder.Decode(context);
        if (config == null)
        {
            return;
        }

        await taskCompleter.CompleteAsync(config);
    }
}
