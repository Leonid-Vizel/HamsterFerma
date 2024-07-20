using System.Text.Json.Serialization;

namespace BlumFerma.Models.Common;

public sealed class BlumBalance
{
    [JsonPropertyName("availableBalance")]
    public string AvailableBalance { get; set; } = null!;
    [JsonPropertyName("playPasses")]
    public int PlayPasses { get; set; }
    [JsonPropertyName("timestamp")]
    public long Timestamp { get; set; }
    [JsonPropertyName("farming")]
    public BlumFarmingState? Farming { get; set; }
}
