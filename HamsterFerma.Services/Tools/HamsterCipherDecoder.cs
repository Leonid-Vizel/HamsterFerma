using System.Text;

namespace HamsterFerma.Services.Tools;

public interface IHamsterCipherDecoder
{
    string Decode(string encoded);
}

public sealed class HamsterCipherDecoder : IHamsterCipherDecoder
{
    public string Decode(string encoded)
    {
        var mixed = $"{encoded.Substring(0, 3)}{encoded.Substring(4)}";
        var base64Bytes = Convert.FromBase64String(mixed);
        var decoded = Encoding.UTF8.GetString(base64Bytes);
        return decoded;
    }
}
