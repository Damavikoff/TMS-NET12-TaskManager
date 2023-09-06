using System.Text.RegularExpressions;

namespace Utils
{
    public enum LogLevel
    {
        Severe,
        Warn,
        Info
    }
    public class Logger
    {
        private const string DATE_FORMAT = "dd-MM-yyyy HH:mm";
        private readonly FileHandler _fileHandler;
        public Logger(string path)
        {
            this._fileHandler = new FileHandler(path);
        }

        public void Log(LogLevel level, string message)
        {
            this._fileHandler.Append($"[{DateTime.UtcNow.ToString(DATE_FORMAT)}] {LevelLabels[level]} - {message}\n");
        }

        public void Log(string message)
        {
            Log(LogLevel.Info, message);
        }

        private static Dictionary<LogLevel, string> LevelLabels = new() {
            { LogLevel.Severe, "SEVERE" },
            { LogLevel.Warn, "WARN" },
            { LogLevel.Info, "INFO" }
        };
    }
}
