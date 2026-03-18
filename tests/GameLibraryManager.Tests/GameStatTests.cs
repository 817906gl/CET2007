using GameLibraryManager.Models;
using Xunit;

namespace GameLibraryManager.Tests;

public class GameStatTests
{
    [Fact]
    public void GameStatConstructor_ShouldSetValues()
    {
        var gameStat = new GameStat("Hades", 12, 4500);

        Assert.Equal("Hades", gameStat.GameName);
        Assert.Equal(12, gameStat.HoursPlayed);
        Assert.Equal(4500, gameStat.HighScore);
    }
}
