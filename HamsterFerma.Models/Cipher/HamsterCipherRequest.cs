using System.Text.Json.Serialization;

namespace HamsterFerma.Models.Cipher;

public sealed class HamsterCipherRequest
{
    [JsonPropertyName("cipher")]
    public string Cipher { get; private set; }
    public HamsterCipherRequest(string cipher)
    {
        Cipher = cipher;
    }
}
