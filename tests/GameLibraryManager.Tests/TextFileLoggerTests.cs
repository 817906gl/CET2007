using GameLibraryManager.Services;
using Xunit;

namespace GameLibraryManager.Tests;

public class TextFileLoggerTests
{
    [Fact]
    public void LogInfo_ShouldWriteMessageToTextFile()
    {
        TextFileLogger.ResetForTesting();
        string filePath = Path.Combine(Path.GetTempPath(), $"log-{Guid.NewGuid()}.txt");
        var logger = TextFileLogger.GetInstance(filePath);

        logger.LogInfo("Added player alice");

        Assert.True(File.Exists(filePath));

        string content = File.ReadAllText(filePath);
        Assert.Contains("INFO", content);
        Assert.Contains("Added player alice", content);

        File.Delete(filePath);
    }

    [Fact]
    public void GetInstance_ShouldReturnSameLoggerInstance()
    {
        TextFileLogger.ResetForTesting();
        string filePath = Path.Combine(Path.GetTempPath(), $"singleton-{Guid.NewGuid()}.txt");

        var logger1 = TextFileLogger.GetInstance(filePath);
        var logger2 = TextFileLogger.GetInstance(filePath);

        Assert.Same(logger1, logger2);
    }
}
