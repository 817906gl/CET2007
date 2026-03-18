namespace GameLibraryManager.Services;

public class TextFileLogger
{
    private static TextFileLogger? _instance;
    private static readonly object _lock = new object();
    private readonly string _filePath;

    private TextFileLogger(string filePath)
    {
        _filePath = filePath;
    }

    public static TextFileLogger GetInstance(string filePath)
    {
        if (_instance != null)
        {
            return _instance;
        }

        lock (_lock)
        {
            // Create the logger only once so the same instance is reused.
            _instance ??= new TextFileLogger(filePath);
        }

        return _instance;
    }

    public static void ResetForTesting()
    {
        lock (_lock)
        {
            _instance = null;
        }
    }

    public void LogInfo(string message)
    {
        WriteLog("INFO", message);
    }

    public void LogError(string message)
    {
        WriteLog("ERROR", message);
    }

    private void WriteLog(string level, string message)
    {
        try
        {
            string? directory = Path.GetDirectoryName(_filePath);

            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}{Environment.NewLine}";
            File.AppendAllText(_filePath, logEntry);
        }
        catch
        {
            // Logging should not stop the application.
        }
    }
}
