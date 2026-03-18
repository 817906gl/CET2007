using GameLibraryManager.Models;
using GameLibraryManager.Services;
using Xunit;

namespace GameLibraryManager.Tests;

public class ReportServiceTests
{
    [Fact]
    public void BuildMostActivePlayersReport_ShouldIncludePlayerNamesAndHours()
    {
        var reportService = new ReportService();
        var players = new List<Player>
        {
            new Player(1, "alice")
            {
                GameStats = new List<GameStat>
                {
                    new GameStat("Minecraft", 12, 5000)
                }
            }
        };

        string report = reportService.BuildMostActivePlayersReport(players);

        Assert.Contains("Most Active Players Report", report);
        Assert.Contains("alice", report);
        Assert.Contains("Total Hours Played: 12", report);
    }

    [Fact]
    public void BuildTopScoringPlayersReport_ShouldIncludePlayerNamesAndScores()
    {
        var reportService = new ReportService();
        var players = new List<Player>
        {
            new Player(2, "bob")
            {
                GameStats = new List<GameStat>
                {
                    new GameStat("Hades", 8, 7000)
                }
            }
        };

        string report = reportService.BuildTopScoringPlayersReport(players);

        Assert.Contains("Top Scoring Players Report", report);
        Assert.Contains("bob", report);
        Assert.Contains("Highest Score: 7000", report);
    }
}
