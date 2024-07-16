using System.Collections;
using System.Text.Json.Serialization;

namespace HamsterFerma.Models.UpgradeList;

public sealed class HamsterUpgradeListResponse : IEnumerable<HamsterUpgrade>
{
    [JsonPropertyName("upgradesForBuy")]
    public List<HamsterUpgrade> UpgradesForBuy { get; set; } = null!;

    [JsonPropertyName("dailyCombo")]
    public HamsterUserDailyCombo DailyCombo { get; set; } = null!;

    public HamsterUpgrade this[int index] => UpgradesForBuy[index];

    public IEnumerator<HamsterUpgrade> GetEnumerator()
        => UpgradesForBuy.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => UpgradesForBuy.GetEnumerator();
}