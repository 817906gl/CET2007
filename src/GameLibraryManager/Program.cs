using GameLibraryManager.Core;

namespace GameLibraryManager;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.Title = "Game Library & Player Stats Manager";

        Console.WriteLine("Welcome to Game Library & Player Stats Manager");
        Console.WriteLine("---------------------------------------------");
        Console.WriteLine();

        MenuPrinter.ShowMainMenu();

        Console.WriteLine();
        Console.WriteLine("This is the initial project scaffold.");
        Console.WriteLine("Business logic will be added in later steps.");
    }
}
