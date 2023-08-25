using Utils;
using Drawing;
using System.Runtime.Versioning;

namespace TaskManager
{
    class TaskManager
    {
        [SupportedOSPlatform("windows")]
        public static void Main(string[] args)
        {
            ConsoleUtils.CreateWindow(120, 30);
            var rect = new Rect(5, 2, 50, 10, null, ConsoleColor.Yellow);
            rect.Padding.Left = 1;
            rect.Padding.Top = 1;
            rect.Padding.Right = 1;
            rect.SetBorders(BorderStyle.Double, ConsoleColor.Red);
            var subRect = new Rect(0, 0, 5, 2, rect, ConsoleColor.Blue);
            subRect.FitContainerWidth();
            rect.Children.Add(subRect);
            Console.SetCursorPosition(0, 14);
            var text = new Text(rect, 2, "some text to fille the rect which is probably too wide to be inserted, but i hope to see the dots there because we need to render that value.", ConsoleColor.Red);
            text.Y = 4;
            rect.Children.Add(text);
            rect.Render();
            while (true)
            {
                var v = Console.ReadKey(true);
                if (v.KeyChar == '1') subRect.X += -1;
                if (v.KeyChar == '2') subRect.X += 1;
                if (v.KeyChar == '3') subRect.Y += -1;
                if (v.KeyChar == '4') subRect.Y += 1;
                rect.Render();
            }
        }
    }
}