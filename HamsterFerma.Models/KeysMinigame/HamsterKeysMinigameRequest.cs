using System.Text.Json.Serialization;

namespace HamsterFerma.Models.KeysMinigame;

public sealed class HamsterKeysMinigameRequest
{
    [JsonPropertyName("cipher")]
    public string Cipher { get; set; } = null!;

    public HamsterKeysMinigameRequest() : base() { }
    public HamsterKeysMinigameRequest(string cipher) : this()
    {
        Cipher = cipher;
    }
}
