﻿using System.Text.Json.Serialization;

namespace HamsterFerma.Models.Config;

public sealed class HamsterConfigResponse
{
    [JsonPropertyName("dailyCipher")]
    public HamsterDailyCipherConfig DailyCipher { get; set; } = null!;
    [JsonPropertyName("dailyKeysMiniGame")]
    public HamsterKeysMinigameConfig KeysMinigame { get; set; } = null!;
}