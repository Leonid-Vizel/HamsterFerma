using System.Text.Json.Serialization;
using HamsterFerma.Models.User;

namespace HamsterFerma.Models.Sync;

public sealed class HamsterSyncResponse
{
    [JsonPropertyName("clickerUser")]
    public HamsterClickerUser ClickerUser { get; set; } = null!;
}