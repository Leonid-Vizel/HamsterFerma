using System.Collections;
using System.Text.Json.Serialization;

namespace HamsterFerma.Models.TaskList;

public sealed class HamsterTaskListResponse
{
    [JsonPropertyName("tasks")]
    public List<HamsterTask> Tasks { get; set; } = [];

    public HamsterTask this[int index] => Tasks[index];
}
