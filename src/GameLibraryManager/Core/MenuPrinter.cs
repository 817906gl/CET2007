namespace GameLibraryManager.Core;

internal static class MenuPrinter
{
    public static void ShowMainMenu()
    {
        Console.WriteLine("Main Menu");
        Console.WriteLine("1. Add Player");
        Console.WriteLine("2. View All Players");
        Console.WriteLine("3. Find Player by ID");
        Console.WriteLine("4. Exit");
        Console.WriteLine();
        Console.Write("Select an option: ");
    }
}
