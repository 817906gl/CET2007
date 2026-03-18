namespace GameLibraryManager.Core;

internal static class MenuPrinter
{
    public static void ShowMainMenu()
    {
        Console.WriteLine("Main Menu");
        Console.WriteLine("1. Add Player");
        Console.WriteLine("2. Add Game Stat to Player");
        Console.WriteLine("3. Update Player Username");
        Console.WriteLine("4. Update Game Stat");
        Console.WriteLine("5. View All Players");
        Console.WriteLine("6. Find Player by ID");
        Console.WriteLine("7. Exit");
        Console.WriteLine();
        Console.Write("Select an option: ");
    }
}
