using GameLibraryManager.Models;
using GameLibraryManager.Services;
using Xunit;

namespace GameLibraryManager.Tests;

public class JsonStorageServiceTests
{
    [Fact]
    public void SavePlayersAndLoadPlayers_ShouldPreservePlayerData()
    {
        var storageService = new JsonStorageService();
        string filePath = Path.Combine(Path.GetTempPath(), $"players-{Guid.NewGuid()}.json");
        var players = new List<Player>
        {
            new Player(1, "alice")
            {
                GameStats = new List<GameStat>
                {
                    new GameStat("Minecraft", 10, 4000)
                }
            }
        };

        bool saveResult = storageService.SavePlayers(filePath, players, out string saveMessage);
        bool loadResult = storageService.LoadPlayers(filePath, out List<Player> loadedPlayers, out string loadMessage);

        Assert.True(saveResult, saveMessage);
        Assert.True(loadResult, loadMessage);
        Assert.Single(loadedPlayers);
        Assert.Equal("alice", loadedPlayers[0].Username);
        Assert.Single(loadedPlayers[0].GameStats);
        Assert.Equal("Minecraft", loadedPlayers[0].GameStats[0].GameName);

        File.Delete(filePath);
    }

    [Fact]
    public void LoadPlayers_ShouldReturnFalse_WhenFileDoesNotExist()
    {
        var storageService = new JsonStorageService();
        string filePath = Path.Combine(Path.GetTempPath(), $"missing-{Guid.NewGuid()}.json");

        bool result = storageService.LoadPlayers(filePath, out List<Player> players, out string message);

        Assert.False(result);
        Assert.Empty(players);
        Assert.Contains("not found", message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void LoadPlayers_ShouldReturnFalse_WhenJsonIsMalformed()
    {
        var storageService = new JsonStorageService();
        string filePath = Path.Combine(Path.GetTempPath(), $"bad-{Guid.NewGuid()}.json");

        File.WriteAllText(filePath, "{ this is not valid json");

        bool result = storageService.LoadPlayers(filePath, out List<Player> players, out string message);

        Assert.False(result);
        Assert.Empty(players);
        Assert.Contains("valid format", message, StringComparison.OrdinalIgnoreCase);

        File.Delete(filePath);
    }

    [Fact]
    public void LoadPlayers_ShouldReturnFalse_WhenDuplicatePlayerIdsExist()
    {
        var storageService = new JsonStorageService();
        string filePath = Path.Combine(Path.GetTempPath(), $"duplicate-{Guid.NewGuid()}.json");

        string json = """
        [
          { "PlayerId": 1, "Username": "alice", "GameStats": [] },
          { "PlayerId": 1, "Username": "bob", "GameStats": [] }
        ]
        """;

        File.WriteAllText(filePath, json);

        bool result = storageService.LoadPlayers(filePath, out List<Player> players, out string message);

        Assert.False(result);
        Assert.Empty(players);
        Assert.Contains("duplicate", message, StringComparison.OrdinalIgnoreCase);

        File.Delete(filePath);
    }
}
