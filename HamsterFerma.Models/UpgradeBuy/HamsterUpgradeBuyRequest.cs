using System.Text.Json.Serialization;

namespace HamsterFerma.Models.UpgradeBuy;

public sealed class HamsterUpgradeBuyRequest
{
    [JsonPropertyName("upgradeId")]
    public string UpgradeId { get; set; } = null!;
    [JsonIgnore]
    public DateTime Timestamp { get; set; }
    [JsonPropertyName("timestamp")]
    public long TimestampString => Convert.ToInt64(Timestamp.ToString("yyyyMMddHHmmssffff"));
    public HamsterUpgradeBuyRequest() : base() { }
    public HamsterUpgradeBuyRequest(string upgradeId) : this()
    {
        UpgradeId = upgradeId;
        Timestamp = DateTime.Now;
    }
}
