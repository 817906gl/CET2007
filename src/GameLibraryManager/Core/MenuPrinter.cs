namespace GameLibraryManager.Core;

internal static class MenuPrinter
{
    public static void ShowMainMenu()
    {
        Console.WriteLine("Main Menu");
        Console.WriteLine("1. Add Player");
        Console.WriteLine("2. Update Player Stats");
        Console.WriteLine("3. Search Players");
        Console.WriteLine("4. Sort Players");
        Console.WriteLine("5. Save Data");
        Console.WriteLine("6. Load Data");
        Console.WriteLine("0. Exit");
        Console.WriteLine();
        Console.WriteLine("Select an option: [placeholder]");
    }
}
