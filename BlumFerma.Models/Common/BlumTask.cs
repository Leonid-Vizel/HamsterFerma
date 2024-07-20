using System.Text.Json.Serialization;

namespace BlumFerma.Models.Common;

public sealed class BlumTask
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;
    [JsonPropertyName("kind")]
    public string Kind { get; set; } = null!;
    [JsonPropertyName("type")]
    public string Type { get; set; } = null!;
    [JsonPropertyName("status")]
    public string Status { get; set; } = null!;
    [JsonPropertyName("iconFileKey")]
    public string IconFileKey { get; set; } = null!;
    [JsonPropertyName("bannerFileKey")]
    public string? BannerFileKey { get; set; }
    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;
    [JsonPropertyName("productName")]
    public string? ProductName { get; set; } = null!;
    [JsonPropertyName("description")]
    public string? Description { get; set; } = null!;
    [JsonPropertyName("reward")]
    public string Reward { get; set; } = null!;
    [JsonPropertyName("isDisclaimerRequired")]
    public bool IsDisclaimerRequired { get; set; }
}
