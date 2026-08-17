using System;
using System.IO;

namespace StudentRecords.App.Logging;

public class FileLogger : ILogger
{
    private readonly string _logFilePath;

    public FileLogger(string logFilePath)
    {
        _logFilePath = logFilePath;

        // Ensure the directory exists when the logger is created
        string? directory = Path.GetDirectoryName(_logFilePath);
        if (!string.IsNullOrWhiteSpace(directory))
            Directory.CreateDirectory(directory);
    }

    public void LogInfo(string message)
    {
        WriteToFile($"[INFO] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
    }

    public void LogError(string message)
    {
        WriteToFile($"[ERROR] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
    }

    private void WriteToFile(string logEntry)
    {
        // AppendAllText automatically opens the file, adds the new line at the bottom, and closes it
        File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
    }
}