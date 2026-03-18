namespace GameLibraryManager.Models;

public class GameStat
{
    public string GameName { get; set; }
    public int HoursPlayed { get; set; }
    public int HighScore { get; set; }

    public GameStat()
    {
        GameName = string.Empty;
    }

    public GameStat(string gameName, int hoursPlayed, int highScore)
    {
        GameName = gameName;
        HoursPlayed = hoursPlayed;
        HighScore = highScore;
    }
}
