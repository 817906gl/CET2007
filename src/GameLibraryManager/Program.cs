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
                    ViewAllPlayers(manager);
                    break;
                case "4":
                    FindPlayerById(manager);
                    break;
                case "5":
                    isRunning = false;
                    Console.WriteLine("Goodbye.");
                    break;
                default:
                    Console.WriteLine("Invalid option. Please choose 1, 2, 3, 4, or 5.");
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

    private static void FindPlayerById(GameLibraryManager.Services.GameLibraryManager manager)
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

        Console.WriteLine($"Player found: ID = {player.PlayerId}, Username = {player.Username}");
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
}
