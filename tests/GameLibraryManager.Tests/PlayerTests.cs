using GameLibraryManager.Models;
using Xunit;

namespace GameLibraryManager.Tests;

public class PlayerTests
{
    [Fact]
    public void PlayerConstructor_ShouldSetBasicValues()
    {
        var player = new Player(1, "alice");

        Assert.Equal(1, player.PlayerId);
        Assert.Equal("alice", player.Username);
        Assert.NotNull(player.GameStats);
        Assert.Empty(player.GameStats);
    }

    [Fact]
    public void Player_ShouldAllowGameStatToBeAdded()
    {
        var player = new Player(2, "bob");
        var stat = new GameStat("Stardew Valley", 25, 9000);

        player.GameStats.Add(stat);

        Assert.Single(player.GameStats);
        Assert.Equal("Stardew Valley", player.GameStats[0].GameName);
    }
}
