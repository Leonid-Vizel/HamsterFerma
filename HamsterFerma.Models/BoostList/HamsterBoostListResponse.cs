using System.Collections;
using System.Text.Json.Serialization;

namespace HamsterFerma.Models.BoostList;

public sealed class HamsterBoostListResponse : IEnumerable<HamsterBoost>
{
    [JsonPropertyName("boostsForBuy")]
    public List<HamsterBoost> List { get; set; } = null!;

    public HamsterBoost this[int index] => List[index];

    public IEnumerator<HamsterBoost> GetEnumerator()
        => List.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => List.GetEnumerator();
}
