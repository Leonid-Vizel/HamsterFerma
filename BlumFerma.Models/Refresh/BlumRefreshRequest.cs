using System.Text.Json.Serialization;

namespace BlumFerma.Models.Refresh;

public sealed class BlumRefreshRequest
{
    [JsonPropertyName("refresh")]
    public string Refresh { get; set; } = null!;
    public BlumRefreshRequest() : base() { }
    public BlumRefreshRequest(string refresh) : this()
    {
        Refresh = refresh;
    }
}
