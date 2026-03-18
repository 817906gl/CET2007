using GameLibraryManager.Core;
using GameLibraryManager.Models;
using GameLibraryManager.Services;

namespace GameLibraryManager;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.Title = "Game Library & Player Stats Manager";
        var manager = new GameLibraryManager.Services.GameLibraryManager();
        var reportService = new ReportService();
        var storageService = new JsonStorageService();
        var logger = new TextFileLogger("data/app-log.txt");
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
        Console.WriteLine("Welcome to Game Library & Player Stats Manager");
        Console.WriteLine("---------------------------------------------");
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
        Console.Write("Enter player ID: ");
        string? idInput = Console.ReadLine();

        if (!int.TryParse(idInput, out int playerId))
        {
            Console.WriteLine("Player ID must be a valid number.");
            logger.LogError($"Invalid player ID entered while adding player: {idInput}");
            return;
        }

        Console.Write("Enter username: ");
        string? username = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(username))
        {
            Console.WriteLine("Username cannot be empty.");
            logger.LogError("Empty username entered while adding player.");
            return;
        }

        var player = new Player(playerId, username.Trim());

        try
        {
            manager.AddPlayer(player);
            Console.WriteLine("Player added successfully.");
            logger.LogInfo($"Added player: ID={player.PlayerId}, Username={player.Username}");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
            logger.LogError($"Could not add player with ID {player.PlayerId}: {ex.Message}");
        }
    }

    private static void ViewAllPlayers(GameLibraryManager.Services.GameLibraryManager manager)
    {
        List<Player> players = manager.GetAllPlayers();

        if (players.Count == 0)
        {
            Console.WriteLine("No players found.");
            return;
        }

        Console.WriteLine("Players");
        Console.WriteLine("-------");

        foreach (Player player in players)
        {
            PrintPlayerDetails(player);
        }
    }

    private static void SearchPlayerById(GameLibraryManager.Services.GameLibraryManager manager)
    {
        Console.Write("Enter player ID to search: ");
        string? idInput = Console.ReadLine();

        if (!int.TryParse(idInput, out int playerId))
        {
            Console.WriteLine("Player ID must be a valid number.");
            return;
        }

        Player? player = manager.FindPlayerById(playerId);

        if (player == null)
        {
            Console.WriteLine("Player not found.");
            return;
        }

        Console.WriteLine("Search Result");
        Console.WriteLine("-------------");
        PrintPlayerDetails(player);
    }

    private static void AddGameStatToPlayer(GameLibraryManager.Services.GameLibraryManager manager, TextFileLogger logger)
    {
        Console.Write("Enter player ID: ");
        string? idInput = Console.ReadLine();

        if (!int.TryParse(idInput, out int playerId))
        {
            Console.WriteLine("Player ID must be a valid number.");
            logger.LogError($"Invalid player ID entered while adding game stat: {idInput}");
            return;
        }

        Console.Write("Enter game name: ");
        string? gameName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(gameName))
        {
            Console.WriteLine("Game name cannot be empty.");
            logger.LogError("Empty game name entered while adding game stat.");
            return;
        }

        Console.Write("Enter hours played: ");
        string? hoursInput = Console.ReadLine();

        if (!int.TryParse(hoursInput, out int hoursPlayed) || hoursPlayed < 0)
        {
            Console.WriteLine("Hours played must be a valid non-negative number.");
            logger.LogError($"Invalid hours played entered while adding game stat: {hoursInput}");
            return;
        }

        Console.Write("Enter high score: ");
        string? scoreInput = Console.ReadLine();

        if (!int.TryParse(scoreInput, out int highScore) || highScore < 0)
        {
            Console.WriteLine("High score must be a valid non-negative number.");
            logger.LogError($"Invalid high score entered while adding game stat: {scoreInput}");
            return;
        }

        var gameStat = new GameStat(gameName.Trim(), hoursPlayed, highScore);
        bool wasAdded = manager.AddGameStatToPlayer(playerId, gameStat);

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
        Console.Write("Enter player ID: ");
        string? idInput = Console.ReadLine();

        if (!int.TryParse(idInput, out int playerId))
        {
            Console.WriteLine("Player ID must be a valid number.");
            logger.LogError($"Invalid player ID entered while updating username: {idInput}");
            return;
        }

        Console.Write("Enter new username: ");
        string? username = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(username))
        {
            Console.WriteLine("Username cannot be empty.");
            logger.LogError("Empty username entered while updating player.");
            return;
        }

        bool wasUpdated = manager.UpdatePlayerUsername(playerId, username.Trim());

        if (!wasUpdated)
        {
            Console.WriteLine("Player not found.");
            logger.LogError($"Could not update username because player was not found: ID={playerId}");
            return;
        }

        Console.WriteLine("Username updated successfully.");
        logger.LogInfo($"Updated username for player ID {playerId} to {username.Trim()}");
    }

    private static void UpdateGameStat(GameLibraryManager.Services.GameLibraryManager manager, TextFileLogger logger)
    {
        Console.Write("Enter player ID: ");
        string? idInput = Console.ReadLine();

        if (!int.TryParse(idInput, out int playerId))
        {
            Console.WriteLine("Player ID must be a valid number.");
            logger.LogError($"Invalid player ID entered while updating game stat: {idInput}");
            return;
        }

        Console.Write("Enter game name to update: ");
        string? gameName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(gameName))
        {
            Console.WriteLine("Game name cannot be empty.");
            logger.LogError("Empty game name entered while updating game stat.");
            return;
        }

        Console.Write("Enter new hours played: ");
        string? hoursInput = Console.ReadLine();

        if (!int.TryParse(hoursInput, out int hoursPlayed) || hoursPlayed < 0)
        {
            Console.WriteLine("Hours played must be a valid non-negative number.");
            logger.LogError($"Invalid hours played entered while updating game stat: {hoursInput}");
            return;
        }

        Console.Write("Enter new high score: ");
        string? scoreInput = Console.ReadLine();

        if (!int.TryParse(scoreInput, out int highScore) || highScore < 0)
        {
            Console.WriteLine("High score must be a valid non-negative number.");
            logger.LogError($"Invalid high score entered while updating game stat: {scoreInput}");
            return;
        }

        bool wasUpdated = manager.UpdateGameStat(playerId, gameName.Trim(), hoursPlayed, highScore);

        if (!wasUpdated)
        {
            Console.WriteLine("Player or game stat not found.");
            logger.LogError($"Could not update game stat. Player ID={playerId}, Game={gameName.Trim()}");
            return;
        }

        Console.WriteLine("Game stat updated successfully.");
        logger.LogInfo($"Updated game stat for player ID {playerId}: Game={gameName.Trim()}, Hours={hoursPlayed}, Score={highScore}");
    }

    private static void SearchPlayerByUsername(GameLibraryManager.Services.GameLibraryManager manager)
    {
        Console.Write("Enter username to search: ");
        string? username = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(username))
        {
            Console.WriteLine("Username cannot be empty.");
            return;
        }

        List<Player> players = manager.FindPlayersByUsername(username.Trim());

        if (players.Count == 0)
        {
            Console.WriteLine("No players found.");
            return;
        }

        Console.WriteLine("Search Results");
        Console.WriteLine("--------------");

        foreach (Player player in players)
        {
            PrintPlayerDetails(player);
        }
    }

    private static void SearchPlayersByGameName(GameLibraryManager.Services.GameLibraryManager manager)
    {
        Console.Write("Enter game name to search: ");
        string? gameName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(gameName))
        {
            Console.WriteLine("Game name cannot be empty.");
            return;
        }

        List<Player> players = manager.FindPlayersByGameName(gameName.Trim());

        if (players.Count == 0)
        {
            Console.WriteLine("No players found for that game.");
            return;
        }

        Console.WriteLine("Players Matching Game");
        Console.WriteLine("---------------------");

        foreach (Player player in players)
        {
            PrintPlayerDetails(player);
        }
    }

    private static void SortPlayersByTotalHours(GameLibraryManager.Services.GameLibraryManager manager)
    {
        List<Player> players = manager.SortPlayersByTotalHoursPlayed();

        if (players.Count == 0)
        {
            Console.WriteLine("No players found.");
            return;
        }

        Console.WriteLine("Players Sorted by Total Hours Played");
        Console.WriteLine("-----------------------------------");

        foreach (Player player in players)
        {
            Console.WriteLine($"{player.Username} - Total Hours: {GetTotalHoursPlayed(player)}");
        }
    }

    private static void SortPlayersByHighestScore(GameLibraryManager.Services.GameLibraryManager manager)
    {
        List<Player> players = manager.SortPlayersByHighestScore();

        if (players.Count == 0)
        {
            Console.WriteLine("No players found.");
            return;
        }

        Console.WriteLine("Players Sorted by Highest Score");
        Console.WriteLine("------------------------------");

        foreach (Player player in players)
        {
            Console.WriteLine($"{player.Username} - Highest Score: {GetHighestScore(player)}");
        }
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
