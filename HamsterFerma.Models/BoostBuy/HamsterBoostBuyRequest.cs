using System.Text.Json.Serialization;

namespace HamsterFerma.Models.UpgradeBuy;

public sealed class HamsterBoostBuyRequest
{
    [JsonPropertyName("boostId")]
    public string BoostId { get; set; } = null!;
    [JsonIgnore]
    public DateTime Timestamp { get; set; }
    [JsonPropertyName("timestamp")]
    public long TimestampString => Convert.ToInt64(Timestamp.ToString("yyyyMMddHHmmssffff"));
    public HamsterBoostBuyRequest() : base() { }
    public HamsterBoostBuyRequest(string boostId) : this()
    {
        BoostId = boostId;
        Timestamp = DateTime.Now;
    }
}
