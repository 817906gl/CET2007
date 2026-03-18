namespace GameLibraryManager.Utilities;

public static class ConsoleHelper
{
    public static void PrintSectionTitle(string title)
    {
        Console.WriteLine(title);
        Console.WriteLine(new string('-', title.Length));
    }
}
