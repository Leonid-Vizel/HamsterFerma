using System.Text.Json.Serialization;

namespace HamsterFerma.Models.CheckTask;

public sealed class HamsterCheckTaskRequest
{
    [JsonPropertyName("taskId")]
    public string TaskId { get; private set; }
    public HamsterCheckTaskRequest(string taskId)
    {
        TaskId = taskId;
    }
}
