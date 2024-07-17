//using HamsterFerma.Services.Clients;
//using HamsterFerma.Services.Configs;
//using HamsterFerma.Services.Tools;
//using Microsoft.Extensions.Logging;
//using Quartz;
//using System.Text.Json;

//namespace SeedFerma.Services.Jobs;

//public sealed class SeedConsumerWatchDog(IHamsterApiClient client,
//                                        IAuthConfigDecoder configDecoder,
//                                        ILogger<SeedConsumerWatchDog> logger) : IJob
//{
//    private static string[] ignoreTasks =
//    [
//        "invite_friends"
//    ];

//    public static void ConfigureFor(IServiceCollectionQuartzConfigurator options, AuthBearerConfig config, TimeZoneInfo timeZone)
//    {
//        if (!config.AutoTask)
//        {
//            return;
//        }
//        if (string.IsNullOrEmpty(config.Token))
//        {
//            return;
//        }
//        var key = CreateKey(config.Tag);
//        options.AddJob<SeedConsumerWatchDog>(key, job => job
//            .UsingJobData(nameof(AuthBearerConfig), JsonSerializer.Serialize(config))
//        ).AddTrigger(trigger => trigger
//            .ForJob(key)
//            .WithCronSchedule(config.TaskCron, x => x.InTimeZone(timeZone))
//        );
//    }

//    public static JobKey CreateKey(string tag)
//        => JobKey.Create(nameof(SeedConsumerWatchDog), tag);

//    public async Task Execute(IJobExecutionContext context)
//    {
//        var config = configDecoder.Decode(context);
//        if (config == null)
//        {
//            return;
//        }

//        var taskList = await client.GetTaskListAsync(config);
//        if (taskList == null)
//        {
//            return;
//        }
//        var nonCompletedTasks = taskList.Tasks
//            .Where(x => x.ChannelId == null)
//            .Where(x => !x.IsCompleted || x.Days != null)
//            .Where(x => x.RemainSeconds == 0 || x.RemainSeconds == null)
//            .Where(x => !ignoreTasks.Contains(x.Id))
//            .ToList();
//        foreach (var task in nonCompletedTasks)
//        {
//            var completedTask = await client.CheckTaskAsync(config, task.Id);
//            if (completedTask == null)
//            {
//                return;
//            }
//            logger.LogInformation($"[Tag: {config.Tag}] Выполнено задание: {completedTask.Task.Id} ({completedTask.Task.RewardCoins} coins)!");
//        }
//    }
//}
