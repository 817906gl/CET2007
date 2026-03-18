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
}
