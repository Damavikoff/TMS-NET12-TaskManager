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

    public class Text : Rect
    {

        public ConsoleColor Color { get; set; }
        public TextAlign Align { get; set; } = TextAlign.Left;
        public string Value { get; set; } = "";

        public Text(int width, int height, string value, Shape container, ConsoleColor background, ConsoleColor color) : base(width, height, container, background)
        {
            this.Color = color;
            this.Value = value;
        }
        public Text(int width, int height, string value, Shape container, ConsoleColor background) : this(width, height, value, container, background, Console.ForegroundColor) { }
        public Text(int width, int height, string value, Rect container) : this(width, height, value, container, container.Background, Console.ForegroundColor) { }
        public Text(Rect container, int height, string value, ConsoleColor color) : this(container.Width, height, value, container, container.Background, color) { }
        public Text(Rect container, int height, string value) : this(container.Width, height, value, container, container.Background) { }

        public override void Render()
        {
            var lines = GetLines();
            ConsoleUtils.SetColors(this.Background, this.Color);
            for (int i = 0; i < lines.Count; i++)
            {
                SetLine(i);
                Console.Write(lines[i]);
            }
            ConsoleUtils.SetColors();
        }

        public List<string> GetLines()
        {
            var lines = new List<string>();
            if (string.IsNullOrEmpty(this.Value)) return lines;

            var words = Regex.Split(this.Value, @"\s");
            var count = 0;
            var builder = new StringBuilder();
            foreach (var w in words)
            {
                if (lines.Count >= this.Height) break;
                if (builder.Length + w.Length <= this.Width) {
                    builder.Append(count == 0 ? w : $" {w}");
                    count++;
                    continue;
                }
                var isLast = lines.Count == this.Height - 1;
                if (lines.Count == this.Height - 1)
                {
                    var value = builder.Length + 4 >= this.Width ? "..." : $" {w.Substring(0, this.Width - builder.Length - 4)}...";
                    builder.Append(value);
                }
                lines.Add(builder.ToString());
                if (isLast) break;
                builder.Clear();
                builder.Append(w);
            }
            return lines;
        }
    }
}
