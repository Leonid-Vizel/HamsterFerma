using System.Text.Json.Serialization;

namespace HexacoreFerma.Models.Common;

public class HexacoreSimpleResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }
}
