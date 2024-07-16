using System.Collections;
using System.Text.Json.Serialization;

namespace HamsterFerma.Models.TaskList;

public sealed class HamsterTaskListResponse : IEnumerable<HamsterTask>
{
    [JsonPropertyName("tasks")]
    public List<HamsterTask> Tasks { get; set; } = [];

    public HamsterTask this[int index] => Tasks[index];

    public IEnumerator<HamsterTask> GetEnumerator()
        => Tasks.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => Tasks.GetEnumerator();
}
