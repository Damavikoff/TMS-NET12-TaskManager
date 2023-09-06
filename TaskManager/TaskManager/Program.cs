using Utils;
using System.Runtime.Versioning;

namespace TaskManager
{
    class TaskManager
    {
        [SupportedOSPlatform("windows")]
        public static void Main(string[] args)
        {
            ConsoleUtils.CreateWindow(100, 30);
            var app = new App(Console.WindowWidth, Console.WindowHeight);
            app.SetLog(@"./todo_log.log");
            app.SetStorage(@"./todo_storage.json");
            app.Run();
        }
    }
}