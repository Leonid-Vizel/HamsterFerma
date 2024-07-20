using System.Text.Json.Serialization;
using BlumFerma.Models.Common;

namespace BlumFerma.Models.Balance;

public sealed class BlumBalanceResponse
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
