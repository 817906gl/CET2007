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
}
