using GameLibraryManager.Models;
using GameLibraryManager.Services;
using Xunit;

namespace GameLibraryManager.Tests;

public class CourseworkCoreTests
{
    [Fact]
    public void AddPlayer_ShouldAddPlayerToManager()
    {
        var manager = new GameLibraryManager.Services.GameLibraryManager();
        var player = new Player(101, "alice");

        manager.AddPlayer(player);
        var players = manager.GetAllPlayers();

        Assert.Single(players);
        Assert.Equal("alice", players[0].Username);
    }

    [Fact]
    public void AddPlayer_ShouldPreventDuplicatePlayerIds()
    {
        var manager = new GameLibraryManager.Services.GameLibraryManager();

        manager.AddPlayer(new Player(102, "bob"));

        Assert.Throws<InvalidOperationException>(() =>
            manager.AddPlayer(new Player(102, "bobby")));
    }

    [Fact]
    public void FindPlayerById_ShouldReturnCorrectPlayer()
    {
        var manager = new GameLibraryManager.Services.GameLibraryManager();
        manager.AddPlayer(new Player(103, "charlie"));

        Player? result = manager.FindPlayerById(103);

        Assert.NotNull(result);
        Assert.Equal("charlie", result!.Username);
    }

    [Fact]
    public void AddGameStatToPlayer_ShouldStoreGameStatForPlayer()
    {
        var manager = new GameLibraryManager.Services.GameLibraryManager();
        manager.AddPlayer(new Player(104, "diana"));

        bool result = manager.AddGameStatToPlayer(104, new GameStat("Minecraft", 15, 5000));
        Player? player = manager.FindPlayerById(104);

        Assert.True(result);
        Assert.NotNull(player);
        Assert.Single(player!.GameStats);
        Assert.Equal("Minecraft", player.GameStats[0].GameName);
    }

    [Fact]
    public void SaveAndLoad_ShouldKeepPlayerData()
    {
        var storageService = new JsonStorageService();
        string filePath = Path.Combine(Path.GetTempPath(), $"coursework-{Guid.NewGuid()}.json");
        var players = new List<Player>
        {
            new Player(105, "ella")
            {
                GameStats = new List<GameStat>
                {
                    new GameStat("Hades", 9, 3200)
                }
            }
        };

        bool saveResult = storageService.SavePlayers(filePath, players, out string saveMessage);
        bool loadResult = storageService.LoadPlayers(filePath, out List<Player> loadedPlayers, out string loadMessage);

        Assert.True(saveResult, saveMessage);
        Assert.True(loadResult, loadMessage);
        Assert.Single(loadedPlayers);
        Assert.Equal("ella", loadedPlayers[0].Username);
        Assert.Single(loadedPlayers[0].GameStats);
        Assert.Equal(3200, loadedPlayers[0].GameStats[0].HighScore);

        File.Delete(filePath);
    }
}
