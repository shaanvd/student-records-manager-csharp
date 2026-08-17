using System;
using System.Collections.Generic;
using System.Text;

namespace StudentRecords.App.Logging;

public interface ILogger
{
    void LogInfo(string message);
    void LogError(string message);
}
