using System.Text.Json.Serialization;

namespace BlumFerma.Models.Common;

public sealed class BlumFarmingState
{
    [JsonPropertyName("startTime")]
    public long StartTime { get; set; }
    [JsonPropertyName("endTime")]
    public long EndTime { get; set; }
    [JsonIgnore]
    public DateTime EndTimeFormattedUtc
        => new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(1721434384196);
    [JsonPropertyName("earningsRate")]
    public string EarningsRate { get; set; } = null!;
    [JsonPropertyName("balance")]
    public string Balance { get; set; } = null!;
}
