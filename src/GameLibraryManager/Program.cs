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
                break;
            }

            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    AddPlayer(manager);
                    break;
                case "2":
                    AddGameStatToPlayer(manager);
                    break;
                case "3":
                    UpdatePlayerUsername(manager);
                    break;
                case "4":
                    UpdateGameStat(manager);
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
                    SaveData(manager, storageService, dataFilePath);
                    break;
                case "14":
                    LoadData(manager, storageService, dataFilePath);
                    break;
                case "15":
                    isRunning = false;
                    Console.WriteLine("Goodbye.");
                    break;
                default:
                    Console.WriteLine("Invalid option. Please choose a number from 1 to 15.");
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

    private static void AddPlayer(GameLibraryManager.Services.GameLibraryManager manager)
    {
        Console.Write("Enter player ID: ");
        string? idInput = Console.ReadLine();

        if (!int.TryParse(idInput, out int playerId))
        {
            Console.WriteLine("Player ID must be a valid number.");
            return;
        }

        Console.Write("Enter username: ");
        string? username = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(username))
        {
            Console.WriteLine("Username cannot be empty.");
            return;
        }

        var player = new Player(playerId, username.Trim());

        try
        {
            manager.AddPlayer(player);
            Console.WriteLine("Player added successfully.");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
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

    private static void AddGameStatToPlayer(GameLibraryManager.Services.GameLibraryManager manager)
    {
        Console.Write("Enter player ID: ");
        string? idInput = Console.ReadLine();

        if (!int.TryParse(idInput, out int playerId))
        {
            Console.WriteLine("Player ID must be a valid number.");
            return;
        }

        Console.Write("Enter game name: ");
        string? gameName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(gameName))
        {
            Console.WriteLine("Game name cannot be empty.");
            return;
        }

        Console.Write("Enter hours played: ");
        string? hoursInput = Console.ReadLine();

        if (!int.TryParse(hoursInput, out int hoursPlayed) || hoursPlayed < 0)
        {
            Console.WriteLine("Hours played must be a valid non-negative number.");
            return;
        }

        Console.Write("Enter high score: ");
        string? scoreInput = Console.ReadLine();

        if (!int.TryParse(scoreInput, out int highScore) || highScore < 0)
        {
            Console.WriteLine("High score must be a valid non-negative number.");
            return;
        }

        var gameStat = new GameStat(gameName.Trim(), hoursPlayed, highScore);
        bool wasAdded = manager.AddGameStatToPlayer(playerId, gameStat);

        if (!wasAdded)
        {
            Console.WriteLine("Player not found.");
            return;
        }

        Console.WriteLine("Game stat added successfully.");
    }

    private static void UpdatePlayerUsername(GameLibraryManager.Services.GameLibraryManager manager)
    {
        Console.Write("Enter player ID: ");
        string? idInput = Console.ReadLine();

        if (!int.TryParse(idInput, out int playerId))
        {
            Console.WriteLine("Player ID must be a valid number.");
            return;
        }

        Console.Write("Enter new username: ");
        string? username = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(username))
        {
            Console.WriteLine("Username cannot be empty.");
            return;
        }

        bool wasUpdated = manager.UpdatePlayerUsername(playerId, username.Trim());

        if (!wasUpdated)
        {
            Console.WriteLine("Player not found.");
            return;
        }

        Console.WriteLine("Username updated successfully.");
    }

    private static void UpdateGameStat(GameLibraryManager.Services.GameLibraryManager manager)
    {
        Console.Write("Enter player ID: ");
        string? idInput = Console.ReadLine();

        if (!int.TryParse(idInput, out int playerId))
        {
            Console.WriteLine("Player ID must be a valid number.");
            return;
        }

        Console.Write("Enter game name to update: ");
        string? gameName = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(gameName))
        {
            Console.WriteLine("Game name cannot be empty.");
            return;
        }

        Console.Write("Enter new hours played: ");
        string? hoursInput = Console.ReadLine();

        if (!int.TryParse(hoursInput, out int hoursPlayed) || hoursPlayed < 0)
        {
            Console.WriteLine("Hours played must be a valid non-negative number.");
            return;
        }

        Console.Write("Enter new high score: ");
        string? scoreInput = Console.ReadLine();

        if (!int.TryParse(scoreInput, out int highScore) || highScore < 0)
        {
            Console.WriteLine("High score must be a valid non-negative number.");
            return;
        }

        bool wasUpdated = manager.UpdateGameStat(playerId, gameName.Trim(), hoursPlayed, highScore);

        if (!wasUpdated)
        {
            Console.WriteLine("Player or game stat not found.");
            return;
        }

        Console.WriteLine("Game stat updated successfully.");
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
        string filePath)
    {
        bool wasSaved = storageService.SavePlayers(filePath, manager.GetAllPlayers(), out string message);

        Console.WriteLine(message);

        if (wasSaved)
        {
            Console.WriteLine($"Saved file: {filePath}");
        }
    }

    private static void LoadData(
        GameLibraryManager.Services.GameLibraryManager manager,
        JsonStorageService storageService,
        string filePath)
    {
        bool wasLoaded = storageService.LoadPlayers(filePath, out List<Player> players, out string message);

        Console.WriteLine(message);

        if (!wasLoaded)
        {
            return;
        }

        manager.ReplaceAllPlayers(players);
        Console.WriteLine($"Loaded file: {filePath}");
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
