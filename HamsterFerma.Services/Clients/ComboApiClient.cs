using Microsoft.Extensions.Logging;

namespace HamsterFerma.Services.Clients;

public interface IComboApiClient
{
    Task GetLatestComboAsync();
}

public sealed class ComboApiClient(IHttpClientFactory clientFactory,
                                   ILogger<ComboApiClient> logger) : IComboApiClient
{
    public async Task GetLatestComboAsync()
    {

    }
}