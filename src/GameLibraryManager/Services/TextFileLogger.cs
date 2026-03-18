namespace GameLibraryManager.Services;

public class TextFileLogger
{
    private readonly string _filePath;

    public TextFileLogger(string filePath)
    {
        _filePath = filePath;
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
