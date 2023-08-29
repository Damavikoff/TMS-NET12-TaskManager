using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utils;

namespace Drawing
{
    public enum TextAlign
    {
        Left,
        Center,
        Right
    }

    public partial class Text : Rect
    {

        public ConsoleColor Color { get; set; }
        public TextAlign Align { get; set; } = TextAlign.Left;
        public string Value { get; set; } = "";

        public Text(int width, int height, string value, Shape container, ConsoleColor background, ConsoleColor color) : base(width, height, container, background)
        {
            Color = color;
            Value = value;
        }
        public Text(int width, int height, string value, Shape container, ConsoleColor background) : this(width, height, value, container, background, Console.ForegroundColor) { }
        public Text(int width, int height, string value, Rect container) : this(width, height, value, container, container.Background, Console.ForegroundColor) { }
        public Text(Rect container, int height, string value, ConsoleColor color) : this(container.Width, height, value, container, container.Background, color) { }
        public Text(Rect container, int height, string value) : this(container.Width, height, value, container, container.Background) { }

        public override void Render()
        {
            Render(Background, Color);
        }

        public void Render(ConsoleColor background, ConsoleColor foreground)
        {
            var lines = GetLines();
            ConsoleUtils.SetColors(background, foreground);
            for (int i = 0; i < lines.Count; i++)
            {
                (int x, int y) = SetLine(i);
                Console.Write(lines[i]);
            }
        }

        private List<string> GetLines()
        {
            var lines = new List<string>();
            if (string.IsNullOrEmpty(Value)) return lines;

            var words = WordSplitRegex().Split(Value);
            var count = 0;
            var builder = new StringBuilder();
            foreach (var w in words)
            {
                if (lines.Count >= Height) break;
                if (builder.Length + w.Length <= Width) {
                    builder.Append(count == 0 ? w : $" {w}");
                    count++;
                    continue;
                }
                var isLast = lines.Count == Height - 1;
                if (isLast)
                {
                    builder.Append($" {w}");
                    lines.Add(GetLine(builder.ToString().Substring(0, Width - 3) + "..."));
                    return lines;
                }
                lines.Add(GetLine(builder.ToString()));
                builder.Clear();
                builder.Append(w);
            }
            lines.Add(GetLine(builder.ToString()));
            return lines;
        }

        private string GetLine(string text)
        {
            if (text.Length < Width)
            {
                var diff = this.Width - text.Length;
                if (this.Align == TextAlign.Left) return text + new string(' ', Width - text.Length);
                if (this.Align == TextAlign.Right) return new string(' ', Width - text.Length) + text;
                var half = (int)(diff / 2);
                return new string(' ', half) + text + new string(' ', diff - half);
            }
            return text;
        }

        [GeneratedRegex("\\s")]
        private static partial Regex WordSplitRegex();
    }
}
