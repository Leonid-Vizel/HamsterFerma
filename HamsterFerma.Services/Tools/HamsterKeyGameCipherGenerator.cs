namespace HamsterFerma.Services.Tools;

public interface IHamsterKeyGameCipherGenerator
{
    string Generate(long id);
}

public sealed class HamsterKeyGameCipherGenerator : IHamsterKeyGameCipherGenerator
{
    public string Generate(long id)
    {
        var cipherStart = (long)(id / (Random.Shared.NextDouble() * 10 + 10));
        return cipherStart.ToString().PadLeft(10, '0');
    }
}
