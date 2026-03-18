using System.Text;
using GameLibraryManager.Models;

namespace GameLibraryManager.Services;

public class ReportService
{
    public string BuildMostActivePlayersReport(List<Player> players)
    {
        var builder = new StringBuilder();

        builder.AppendLine("Most Active Players Report");
        builder.AppendLine("--------------------------");

        if (players.Count == 0)
        {
            builder.AppendLine("No players found.");
            return builder.ToString();
        }

        for (int i = 0; i < players.Count; i++)
        {
            Player player = players[i];
            builder.AppendLine($"{i + 1}. {player.Username} (ID: {player.PlayerId})");
            builder.AppendLine($"   Total Hours Played: {GetTotalHoursPlayed(player)}");
        }

        return builder.ToString();
    }

    public string BuildTopScoringPlayersReport(List<Player> players)
    {
        var builder = new StringBuilder();

        builder.AppendLine("Top Scoring Players Report");
        builder.AppendLine("--------------------------");

        if (players.Count == 0)
        {
            builder.AppendLine("No players found.");
            return builder.ToString();
        }

        for (int i = 0; i < players.Count; i++)
        {
            Player player = players[i];
            builder.AppendLine($"{i + 1}. {player.Username} (ID: {player.PlayerId})");
            builder.AppendLine($"   Highest Score: {GetHighestScore(player)}");
        }

        return builder.ToString();
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
