using BlumFerma.Models.Common;
using BlumFerma.Services.Clients;
using BlumFerma.Services.Configs;
using BlumFerma.Services.Jobs;
using Microsoft.Extensions.Logging;

namespace BlumFerma.Services.Tools;

public interface IBlumTaskCompleter
{
    Task CompleteAsync(AuthBearerConfig config);
}

public sealed class BlumTaskCompleter(IBlumApiClient client,
                                      ILogger<BlumTaskCompleter> logger) : IBlumTaskCompleter
{
    public async Task CompleteAsync(AuthBearerConfig config)
    {
        var taskList = await client.GetTasksAsync(config);
        if (taskList == null)
        {
            return;
        }

        var needToStartTasks = taskList
            .Where(x => x.Status == BlumTaskStatus.NotStarted)
            .Where(x => x.Type == BlumTaskType.SocialSubscription)
            .ToList();

        foreach (var task in needToStartTasks)
        {
            var startedTask = await client.StartTaskAsync(config, task.Id);
            if (startedTask == null)
            {
                return;
            }
            task.Status = startedTask.Status;
            logger.LogInformation($"[Tag: {config.Tag}] Запущено задание: {startedTask.Id}!");
        }

        var needToClaimTasks = taskList
            .Where(x => x.Status == BlumTaskStatus.ReadyForClaim)
            .ToList();

        foreach (var task in needToClaimTasks)
        {
            var startedTask = await client.ClaimTaskAsync(config, task.Id);
            if (startedTask == null)
            {
                return;
            }
            logger.LogInformation($"[Tag: {config.Tag}] Выполнено задание: {startedTask.Id} ({startedTask.Reward} coins)!");
        }
    }
}
