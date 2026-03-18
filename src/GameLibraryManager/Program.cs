using GameLibraryManager.Core;
using GameLibraryManager.Models;
using GameLibraryManager.Services;
using GameLibraryManager.Utilities;

namespace GameLibraryManager;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.Title = "Game Library & Player Stats Manager";
        var manager = new GameLibraryManager.Services.GameLibraryManager();
        var reportService = new ReportService();
        var storageService = new JsonStorageService();
        var logger = TextFileLogger.GetInstance("data/app-log.txt");
        const string dataFilePath = "data/players.json";

        bool isRunning = true;

        while (isRunning)
        {
            TryClearConsole();
            ShowHeader();
            MenuPrinter.ShowMainMenu();

            string? choice = Console.ReadLine();

            if (choice == null)
            {
                Console.WriteLine();
                Console.WriteLine("Input stream closed. Exiting program.");
                logger.LogInfo("Input stream closed. Application exited.");
                break;
            }

            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    AddPlayer(manager, logger);
                    break;
                case "2":
                    AddGameStatToPlayer(manager, logger);
                    break;
                case "3":
                    UpdatePlayerUsername(manager, logger);
                    break;
                case "4":
                    UpdateGameStat(manager, logger);
                    break;
                case "5":
                    ViewAllPlayers(manager);
                    break;
                case "6":
                    SearchPlayerById(manager);
                    break;
                case "7":
                    SearchPlayerByUsername(manager);
                    break;
                case "8":
                    SearchPlayersByGameName(manager);
                    break;
                case "9":
                    SortPlayersByTotalHours(manager);
                    break;
                case "10":
                    SortPlayersByHighestScore(manager);
                    break;
                case "11":
                    ShowMostActivePlayersReport(manager, reportService);
                    break;
                case "12":
                    ShowTopScoringPlayersReport(manager, reportService);
                    break;
                case "13":
                    SaveData(manager, storageService, logger, dataFilePath);
                    break;
                case "14":
                    LoadData(manager, storageService, logger, dataFilePath);
                    break;
                case "15":
                    isRunning = false;
                    Console.WriteLine("Goodbye.");
                    logger.LogInfo("Application closed by user.");
                    break;
                default:
                    Console.WriteLine("Invalid option. Please choose a number from 1 to 15.");
                    logger.LogError($"Invalid menu option entered: {choice}");
                    break;
            }

            if (isRunning)
            {
                Console.WriteLine();
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
            }
        }
    }

    private static void ShowHeader()
    {
        ConsoleHelper.PrintSectionTitle("Welcome to Game Library & Player Stats Manager");
        Console.WriteLine();
    }

    private static void TryClearConsole()
    {
        try
        {
            Console.Clear();
        }
        catch (IOException)
        {
            // Ignore clear errors when the app is run through redirected input/output.
        }
    }

    private static void AddPlayer(GameLibraryManager.Services.GameLibraryManager manager, TextFileLogger logger)
    {
        if (!TryReadNonNegativeInt("Enter player ID: ", out int playerId, logger, "adding player"))
        {
            return;
        }

        if (!TryReadRequiredText("Enter username: ", "Username", out string username, logger, "adding player"))
        {
            return;
        }

        var player = new Player(playerId, username);

        try
        {
            manager.AddPlayer(player);
            Console.WriteLine("Player added successfully.");
            logger.LogInfo($"Added player: ID={player.PlayerId}, Username={player.Username}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            logger.LogError($"Could not add player with ID {player.PlayerId}: {ex.Message}");
        }
    }

    private static void ViewAllPlayers(GameLibraryManager.Services.GameLibraryManager manager)
    {
        List<Player> players = manager.GetAllPlayers();
        DisplayPlayers(players, "Players", "No players found.");
    }

    private static void SearchPlayerById(GameLibraryManager.Services.GameLibraryManager manager)
    {
        if (!TryReadNonNegativeInt("Enter player ID to search: ", out int playerId))
        {
            return;
        }

        Player? player = manager.FindPlayerById(playerId);

        if (player == null)
        {
            Console.WriteLine("Player not found.");
            return;
        }

        ConsoleHelper.PrintSectionTitle("Search Result");
        PrintPlayerDetails(player);
    }

    private static void AddGameStatToPlayer(GameLibraryManager.Services.GameLibraryManager manager, TextFileLogger logger)
    {
        if (!TryReadNonNegativeInt("Enter player ID: ", out int playerId, logger, "adding game stat"))
        {
            return;
        }

        if (!TryReadRequiredText("Enter game name: ", "Game name", out string gameName, logger, "adding game stat"))
        {
            return;
        }

        if (!TryReadNonNegativeInt("Enter hours played: ", out int hoursPlayed, logger, "adding game stat"))
        {
            return;
        }

        if (!TryReadNonNegativeInt("Enter high score: ", out int highScore, logger, "adding game stat"))
        {
            return;
        }

        var gameStat = new GameStat(gameName, hoursPlayed, highScore);
        bool wasAdded;

        try
        {
            wasAdded = manager.AddGameStatToPlayer(playerId, gameStat);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            logger.LogError($"Could not add game stat for player ID {playerId}: {ex.Message}");
            return;
        }

        if (!wasAdded)
        {
            Console.WriteLine("Player not found.");
            logger.LogError($"Could not add game stat because player was not found: ID={playerId}");
            return;
        }

        Console.WriteLine("Game stat added successfully.");
        logger.LogInfo($"Added game stat for player ID {playerId}: Game={gameStat.GameName}, Hours={hoursPlayed}, Score={highScore}");
    }

    private static void UpdatePlayerUsername(GameLibraryManager.Services.GameLibraryManager manager, TextFileLogger logger)
    {
        if (!TryReadNonNegativeInt("Enter player ID: ", out int playerId, logger, "updating username"))
        {
            return;
        }

        if (!TryReadRequiredText("Enter new username: ", "Username", out string username, logger, "updating username"))
        {
            return;
        }

        bool wasUpdated;

        try
        {
            wasUpdated = manager.UpdatePlayerUsername(playerId, username);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            logger.LogError($"Could not update username for player ID {playerId}: {ex.Message}");
            return;
        }

        if (!wasUpdated)
        {
            Console.WriteLine("Player not found.");
            logger.LogError($"Could not update username because player was not found: ID={playerId}");
            return;
        }

        Console.WriteLine("Username updated successfully.");
        logger.LogInfo($"Updated username for player ID {playerId} to {username}");
    }

    private static void UpdateGameStat(GameLibraryManager.Services.GameLibraryManager manager, TextFileLogger logger)
    {
        if (!TryReadNonNegativeInt("Enter player ID: ", out int playerId, logger, "updating game stat"))
        {
            return;
        }

        if (!TryReadRequiredText("Enter game name to update: ", "Game name", out string gameName, logger, "updating game stat"))
        {
            return;
        }

        if (!TryReadNonNegativeInt("Enter new hours played: ", out int hoursPlayed, logger, "updating game stat"))
        {
            return;
        }

        if (!TryReadNonNegativeInt("Enter new high score: ", out int highScore, logger, "updating game stat"))
        {
            return;
        }

        bool wasUpdated;

        try
        {
            wasUpdated = manager.UpdateGameStat(playerId, gameName, hoursPlayed, highScore);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            logger.LogError($"Could not update game stat for player ID {playerId}: {ex.Message}");
            return;
        }

        if (!wasUpdated)
        {
            Console.WriteLine("Player or game stat not found.");
            logger.LogError($"Could not update game stat. Player ID={playerId}, Game={gameName}");
            return;
        }

        Console.WriteLine("Game stat updated successfully.");
        logger.LogInfo($"Updated game stat for player ID {playerId}: Game={gameName}, Hours={hoursPlayed}, Score={highScore}");
    }

    private static void SearchPlayerByUsername(GameLibraryManager.Services.GameLibraryManager manager)
    {
        if (!TryReadRequiredText("Enter username to search: ", "Username", out string username))
        {
            return;
        }

        List<Player> players = manager.FindPlayersByUsername(username);
        DisplayPlayers(players, "Search Results", "No players found.");
    }

    private static void SearchPlayersByGameName(GameLibraryManager.Services.GameLibraryManager manager)
    {
        if (!TryReadRequiredText("Enter game name to search: ", "Game name", out string gameName))
        {
            return;
        }

        List<Player> players = manager.FindPlayersByGameName(gameName);
        DisplayPlayers(players, "Players Matching Game", "No players found for that game.");
    }

    private static void SortPlayersByTotalHours(GameLibraryManager.Services.GameLibraryManager manager)
    {
        List<Player> players = manager.SortPlayersByTotalHoursPlayed();
        DisplayPlayerRanking(players, "Players Sorted by Total Hours Played", "Total Hours", GetTotalHoursPlayed);
    }

    private static void SortPlayersByHighestScore(GameLibraryManager.Services.GameLibraryManager manager)
    {
        List<Player> players = manager.SortPlayersByHighestScore();
        DisplayPlayerRanking(players, "Players Sorted by Highest Score", "Highest Score", GetHighestScore);
    }

    private static void ShowMostActivePlayersReport(
        GameLibraryManager.Services.GameLibraryManager manager,
        ReportService reportService)
    {
        List<Player> players = manager.SortPlayersByTotalHoursPlayed();
        string report = reportService.BuildMostActivePlayersReport(players);
        Console.WriteLine(report);
    }

    private static void ShowTopScoringPlayersReport(
        GameLibraryManager.Services.GameLibraryManager manager,
        ReportService reportService)
    {
        List<Player> players = manager.SortPlayersByHighestScore();
        string report = reportService.BuildTopScoringPlayersReport(players);
        Console.WriteLine(report);
    }

    private static void SaveData(
        GameLibraryManager.Services.GameLibraryManager manager,
        JsonStorageService storageService,
        TextFileLogger logger,
        string filePath)
    {
        bool wasSaved = storageService.SavePlayers(filePath, manager.GetAllPlayers(), out string message);

        Console.WriteLine(message);

        if (wasSaved)
        {
            Console.WriteLine($"Saved file: {filePath}");
            logger.LogInfo($"Saved player data to {filePath}");
        }
        else
        {
            logger.LogError($"Failed to save player data to {filePath}: {message}");
        }
    }

    private static void LoadData(
        GameLibraryManager.Services.GameLibraryManager manager,
        JsonStorageService storageService,
        TextFileLogger logger,
        string filePath)
    {
        bool wasLoaded = storageService.LoadPlayers(filePath, out List<Player> players, out string message);

        Console.WriteLine(message);

        if (!wasLoaded)
        {
            logger.LogError($"Failed to load player data from {filePath}: {message}");
            return;
        }

        manager.ReplaceAllPlayers(players);
        Console.WriteLine($"Loaded file: {filePath}");
        logger.LogInfo($"Loaded player data from {filePath}");
    }

    private static bool TryReadNonNegativeInt(string prompt, out int value, TextFileLogger? logger = null, string? context = null)
    {
        Console.Write(prompt);
        string? input = Console.ReadLine();

        if (!int.TryParse(input, out value))
        {
            Console.WriteLine("Please enter a valid number.");
            logger?.LogError($"Invalid numeric input while {context}: {input}");
            return false;
        }

        if (value < 0)
        {
            Console.WriteLine("Please enter a non-negative number.");
            logger?.LogError($"Negative value entered while {context}: {value}");
            return false;
        }

        return true;
    }

    private static bool TryReadRequiredText(string prompt, string fieldName, out string value, TextFileLogger? logger = null, string? context = null)
    {
        Console.Write(prompt);
        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine($"{fieldName} cannot be empty.");
            logger?.LogError($"Empty text entered for {fieldName} while {context}.");
            value = string.Empty;
            return false;
        }

        value = input.Trim();
        return true;
    }

    private static void PrintPlayerDetails(Player player)
    {
        Console.WriteLine($"Player ID : {player.PlayerId}");
        Console.WriteLine($"Username  : {player.Username}");
        Console.WriteLine("Game Stats:");

        if (player.GameStats.Count == 0)
        {
            Console.WriteLine("  No game stats recorded.");
            Console.WriteLine();
            return;
        }

        for (int i = 0; i < player.GameStats.Count; i++)
        {
            PrintGameStat(player.GameStats[i], i + 1);
        }

        Console.WriteLine();
    }

    private static void PrintGameStat(GameStat gameStat, int number)
    {
        Console.WriteLine($"  {number}. {gameStat.GameName}");
        Console.WriteLine($"     Hours Played : {gameStat.HoursPlayed}");
        Console.WriteLine($"     High Score   : {gameStat.HighScore}");
    }

    private static void DisplayPlayers(List<Player> players, string title, string emptyMessage)
    {
        if (players.Count == 0)
        {
            Console.WriteLine(emptyMessage);
            return;
        }

        ConsoleHelper.PrintSectionTitle(title);

        foreach (Player player in players)
        {
            PrintPlayerDetails(player);
        }
    }

    private static void DisplayPlayerRanking(
        List<Player> players,
        string title,
        string valueLabel,
        Func<Player, int> valueSelector)
    {
        if (players.Count == 0)
        {
            Console.WriteLine("No players found.");
            return;
        }

        ConsoleHelper.PrintSectionTitle(title);

        foreach (Player player in players)
        {
            Console.WriteLine($"{player.Username} - {valueLabel}: {valueSelector(player)}");
        }
    }

    private static int GetTotalHoursPlayed(Player player)
    {
        int totalHours = 0;

        foreach (GameStat gameStat in player.GameStats)
        {
            totalHours += gameStat.HoursPlayed;
        }

        return totalHours;
    }

    private static int GetHighestScore(Player player)
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
