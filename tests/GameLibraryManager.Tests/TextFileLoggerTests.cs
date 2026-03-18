using GameLibraryManager.Services;
using Xunit;

namespace GameLibraryManager.Tests;

public class TextFileLoggerTests
{
    [Fact]
    public void LogInfo_ShouldWriteMessageToTextFile()
    {
        string filePath = Path.Combine(Path.GetTempPath(), $"log-{Guid.NewGuid()}.txt");
        var logger = new TextFileLogger(filePath);

        logger.LogInfo("Added player alice");

        Assert.True(File.Exists(filePath));

        string content = File.ReadAllText(filePath);
        Assert.Contains("INFO", content);
        Assert.Contains("Added player alice", content);

        File.Delete(filePath);
    }
}
