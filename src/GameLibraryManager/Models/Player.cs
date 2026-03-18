namespace GameLibraryManager.Models;

public class Player
{
    public int PlayerId { get; set; }
    public string Username { get; set; }
    public List<GameStat> GameStats { get; set; }

    public Player()
    {
        Username = string.Empty;
        GameStats = new List<GameStat>();
    }

    public Player(int playerId, string username)
    {
        PlayerId = playerId;
        Username = username;
        GameStats = new List<GameStat>();
    }
}
