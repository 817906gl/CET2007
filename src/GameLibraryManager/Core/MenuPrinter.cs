namespace GameLibraryManager.Core;

internal static class MenuPrinter
{
    public static void ShowMainMenu()
    {
        Console.WriteLine("Main Menu");
        Console.WriteLine("1. Add Player");
        Console.WriteLine("2. Add Game Stat to Player");
        Console.WriteLine("3. View All Players");
        Console.WriteLine("4. Find Player by ID");
        Console.WriteLine("5. Exit");
        Console.WriteLine();
        Console.Write("Select an option: ");
    }
}
