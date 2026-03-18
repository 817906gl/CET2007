using GameLibraryManager.Models;

namespace GameLibraryManager.Services;

public class GameLibraryManager
{
    private readonly List<Player> _players;

    public GameLibraryManager()
    {
        _players = new List<Player>();
    }

    public void AddPlayer(Player player)
    {
        if (player == null)
        {
            throw new ArgumentNullException(nameof(player));
        }

        bool duplicateExists = _players.Any(existingPlayer => existingPlayer.PlayerId == player.PlayerId);

        if (duplicateExists)
        {
            throw new InvalidOperationException("A player with this ID already exists.");
        }

        _players.Add(player);
    }

    public List<Player> GetAllPlayers()
    {
        return new List<Player>(_players);
    }

    public Player? FindPlayerById(int playerId)
    {
        return _players.FirstOrDefault(player => player.PlayerId == playerId);
    }

    public List<Player> FindPlayersByUsername(string username)
    {
        return _players
            .Where(player => player.Username.Contains(username, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public List<Player> FindPlayersByGameName(string gameName)
    {
        return _players
            .Where(player => player.GameStats.Any(stat =>
                stat.GameName.Contains(gameName, StringComparison.OrdinalIgnoreCase)))
            .ToList();
    }

    public List<Player> SortPlayersByTotalHoursPlayed()
    {
        List<Player> sortedPlayers = new List<Player>(_players);

        for (int i = 0; i < sortedPlayers.Count - 1; i++)
        {
            for (int j = 0; j < sortedPlayers.Count - i - 1; j++)
            {
                int currentHours = GetTotalHoursPlayed(sortedPlayers[j]);
                int nextHours = GetTotalHoursPlayed(sortedPlayers[j + 1]);

                if (currentHours < nextHours)
                {
                    Player temp = sortedPlayers[j];
                    sortedPlayers[j] = sortedPlayers[j + 1];
                    sortedPlayers[j + 1] = temp;
                }
            }
        }

        return sortedPlayers;
    }

    public List<Player> SortPlayersByHighestScore()
    {
        List<Player> sortedPlayers = new List<Player>(_players);

        for (int i = 0; i < sortedPlayers.Count - 1; i++)
        {
            for (int j = 0; j < sortedPlayers.Count - i - 1; j++)
            {
                int currentScore = GetHighestScore(sortedPlayers[j]);
                int nextScore = GetHighestScore(sortedPlayers[j + 1]);

                if (currentScore < nextScore)
                {
                    Player temp = sortedPlayers[j];
                    sortedPlayers[j] = sortedPlayers[j + 1];
                    sortedPlayers[j + 1] = temp;
                }
            }
        }

        return sortedPlayers;
    }

    public bool AddGameStatToPlayer(int playerId, GameStat gameStat)
    {
        if (gameStat == null)
        {
            throw new ArgumentNullException(nameof(gameStat));
        }

        Player? player = FindPlayerById(playerId);

        if (player == null)
        {
            return false;
        }

        player.GameStats.Add(gameStat);
        return true;
    }

    public bool UpdatePlayerUsername(int playerId, string newUsername)
    {
        Player? player = FindPlayerById(playerId);

        if (player == null)
        {
            return false;
        }

        player.Username = newUsername;
        return true;
    }

    public bool UpdateGameStat(int playerId, string gameName, int hoursPlayed, int highScore)
    {
        Player? player = FindPlayerById(playerId);

        if (player == null)
        {
            return false;
        }

        GameStat? gameStat = player.GameStats.FirstOrDefault(stat =>
            stat.GameName.Equals(gameName, StringComparison.OrdinalIgnoreCase));

        if (gameStat == null)
        {
            return false;
        }

        gameStat.HoursPlayed = hoursPlayed;
        gameStat.HighScore = highScore;
        return true;
    }

    private int GetTotalHoursPlayed(Player player)
    {
        int totalHours = 0;

        foreach (GameStat gameStat in player.GameStats)
        {
            totalHours += gameStat.HoursPlayed;
        }

        return totalHours;
    }

    private int GetHighestScore(Player player)
    {
        int highestScore = 0;

        foreach (GameStat gameStat in player.GameStats)
        {
            if (gameStat.HighScore > highestScore)
            {
                highestScore = gameStat.HighScore;
            }
        }

        return highestScore;
    }
}
