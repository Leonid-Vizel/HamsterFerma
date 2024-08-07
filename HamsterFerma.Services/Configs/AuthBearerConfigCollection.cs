﻿using System.Collections;

namespace HamsterFerma.Services.Configs;

public sealed class AuthBearerConfigCollection : IEnumerable<AuthBearerConfig>
{
    public AuthBearerConfig[] Bearers { get; set; } = [];

    public AuthBearerConfig this[int index]
        => Bearers[index];

    public IEnumerator<AuthBearerConfig> GetEnumerator()
        => ((IEnumerable<AuthBearerConfig>)Bearers).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => Bearers.GetEnumerator();
}
