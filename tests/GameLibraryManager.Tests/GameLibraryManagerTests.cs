using GameLibraryManager.Models;
using Xunit;
using Manager = GameLibraryManager.Services.GameLibraryManager;

namespace GameLibraryManager.Tests;

public class GameLibraryManagerTests
{
    [Fact]
    public void AddPlayer_ShouldStorePlayerInList()
    {
        var manager = new Manager();
        var player = new Player(1, "alice");

        manager.AddPlayer(player);
        var players = manager.GetAllPlayers();

        Assert.Single(players);
        Assert.Equal("alice", players[0].Username);
    }

    [Fact]
    public void AddPlayer_ShouldThrowException_WhenPlayerIdAlreadyExists()
    {
        var manager = new Manager();

        manager.AddPlayer(new Player(1, "alice"));

        Action act = () => manager.AddPlayer(new Player(1, "bob"));

        Assert.Throws<InvalidOperationException>(act);
    }

    [Fact]
    public void FindPlayerById_ShouldReturnPlayer_WhenPlayerExists()
    {
        var manager = new Manager();
        var player = new Player(2, "charlie");

        manager.AddPlayer(player);
        var result = manager.FindPlayerById(2);

        Assert.NotNull(result);
        Assert.Equal("charlie", result!.Username);
    }

    [Fact]
    public void FindPlayerById_ShouldReturnNull_WhenPlayerDoesNotExist()
    {
        var manager = new Manager();

        var result = manager.FindPlayerById(99);

        Assert.Null(result);
    }

    [Fact]
    public void AddGameStatToPlayer_ShouldAddStat_WhenPlayerExists()
    {
        var manager = new Manager();
        var player = new Player(3, "diana");

        manager.AddPlayer(player);

        var result = manager.AddGameStatToPlayer(3, new GameStat("Minecraft", 20, 7000));

        Assert.True(result);
        Assert.Single(player.GameStats);
        Assert.Equal("Minecraft", player.GameStats[0].GameName);
    }

    [Fact]
    public void AddGameStatToPlayer_ShouldReturnFalse_WhenPlayerDoesNotExist()
    {
        var manager = new Manager();

        var result = manager.AddGameStatToPlayer(50, new GameStat("Terraria", 10, 3000));

        Assert.False(result);
    }

    [Fact]
    public void UpdatePlayerUsername_ShouldUpdateUsername_WhenPlayerExists()
    {
        var manager = new Manager();
        var player = new Player(4, "oldname");

        manager.AddPlayer(player);

        var result = manager.UpdatePlayerUsername(4, "newname");

        Assert.True(result);
        Assert.Equal("newname", player.Username);
    }

    [Fact]
    public void UpdatePlayerUsername_ShouldReturnFalse_WhenPlayerDoesNotExist()
    {
        var manager = new Manager();

        var result = manager.UpdatePlayerUsername(100, "newname");

        Assert.False(result);
    }

    [Fact]
    public void UpdateGameStat_ShouldUpdateValues_WhenGameStatExists()
    {
        var manager = new Manager();
        var player = new Player(5, "emma");

        manager.AddPlayer(player);
        manager.AddGameStatToPlayer(5, new GameStat("Hades", 10, 1500));

        var result = manager.UpdateGameStat(5, "Hades", 25, 5000);

        Assert.True(result);
        Assert.Equal(25, player.GameStats[0].HoursPlayed);
        Assert.Equal(5000, player.GameStats[0].HighScore);
    }

    [Fact]
    public void UpdateGameStat_ShouldReturnFalse_WhenGameStatDoesNotExist()
    {
        var manager = new Manager();
        var player = new Player(6, "frank");

        manager.AddPlayer(player);

        var result = manager.UpdateGameStat(6, "Celeste", 12, 2000);

        Assert.False(result);
    }
}
