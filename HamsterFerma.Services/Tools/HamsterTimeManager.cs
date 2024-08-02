namespace HamsterFerma.Services.Tools;

public interface IHamsterTimeManager
{
    DateTime GetLastestKeysMinigameTime();
    DateTime GetNextKeysMinigameTime();
}

public sealed class HamsterTimeManager : IHamsterTimeManager
{
    public DateTime GetLastestKeysMinigameTime()
    {
        var today = DateTime.Today;
        if (today.Hour < 23)
        {
            return today.AddHours(-1);
        }
        return today.AddHours(23);
    }

    public DateTime GetNextKeysMinigameTime()
    {
        var today = DateTime.Today;
        if (today.Hour < 23)
        {
            return today.AddHours(23);
        }
        return today.AddDays(1).AddHours(23);
    }
}
