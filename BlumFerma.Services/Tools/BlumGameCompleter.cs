using BlumFerma.Services.Clients;
using BlumFerma.Services.Configs;
using Microsoft.Extensions.Logging;

namespace BlumFerma.Services.Tools;

public interface IBlumGameCompleter
{

}

public sealed class BlumGameCompleter(IBlumApiClient client,
                                      ILogger<BlumGameCompleter> logger) : IBlumGameCompleter
{
    public async Task CompleteAsync(AuthBearerConfig config)
    {
        
    }
}
