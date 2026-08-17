using StudentRecords.App.Logging;

namespace StudentRecords.Tests;

public class DummyLogger : ILogger
{
    public void LogInfo(string message) { }
    public void LogError(string message) { }
}