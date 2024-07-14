using HamsterFerma.Services.Clients;
using Microsoft.Extensions.Logging;
using Quartz;

namespace HamsterFerma.Services.Jobs;

public sealed class HamsterTaskWatchDog(IHamsterApiClient client, ILogger<HamsterTaskWatchDog> logger) : IJob
{
    private static string[] ignoreTasks =
    [
        "invite_friends"
    ];

    public static void ConfigureFor(IServiceCollectionQuartzConfigurator options, TimeZoneInfo timeZone)
    {
        var key = CreateKey();
        options.AddJob<HamsterTaskWatchDog>(key)
            .AddTrigger(trigger => trigger.ForJob(key).WithCronSchedule("0 0 * ? * * *", x => x.InTimeZone(timeZone)));
    }

    public static JobKey CreateKey()
        => JobKey.Create(nameof(HamsterTaskWatchDog));

    public async Task Execute(IJobExecutionContext context)
    {
        var taskList = await client.GetTaskListAsync();
        if (taskList == null)
        {
            logger.LogError($"{nameof(client.GetTaskListAsync)} returned null!");
            return;
        }
        var nonCompletedTasks = taskList.Tasks
            .Where(x => x.ChannelId == null)
            .Where(x => !x.IsCompleted)
            .Where(x => x.RemainSeconds == 0 || x.RemainSeconds == null)
            .Where(x => !ignoreTasks.Contains(x.Id))
            .ToList();
        foreach (var task in nonCompletedTasks)
        {
            var completedTask = await client.CheckTaskAsync(task.Id);
            if (completedTask == null)
            {
                logger.LogError($"{nameof(client.CheckTaskAsync)} returned null (taskId:{task.Id})!");
                return;
            }
            logger.LogInformation($"Completed task: {completedTask.Task.Id} ({completedTask.Task.RewardCoins} coins)!");
        }
    }
}
