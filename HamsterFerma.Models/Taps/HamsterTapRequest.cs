using System.Text.Json.Serialization;

namespace HamsterFerma.Models.Taps;

public sealed class HamsterTapRequest
{
    [JsonPropertyName("count")]
    public int Count { get; set; }
    [JsonPropertyName("availableTaps")]
    public int AvailableTaps { get; set; }
    [JsonIgnore]
    public DateTime Timestamp { get; set; }
    [JsonPropertyName("timestamp")]
    public long TimestampString => Convert.ToInt64(Timestamp.ToString("yyyyMMddHHmmssffff"));

    public HamsterTapRequest() : base() { }
    public HamsterTapRequest(int count) : this()
    {
        Count = count;
        AvailableTaps = 0;
        Timestamp = DateTime.Now;
    }
}
