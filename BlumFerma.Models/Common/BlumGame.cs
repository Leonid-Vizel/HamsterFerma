using System.Text.Json.Serialization;

namespace BlumFerma.Models.Common;

public sealed class BlumGame
{
    [JsonPropertyName("gameId")]
    public string GameId { get; set; } = null!;
    [JsonPropertyName("points")]
    public ushort Points { get; set; }

    public BlumGame() : base() { }
    public BlumGame(string gameId, ushort points) : this()
    {
        GameId = gameId;
        Points = points;
    }
}
