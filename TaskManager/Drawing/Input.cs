using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utils;

namespace Drawing
{
    public class Input : Rect
    {
        public ConsoleColor Color { get; set; }
        public string? Label { get; private set; }
        public string Value { get; set; } = "";
        public bool Focused { get; private set; } = false;
        public new int Height { get => string.IsNullOrEmpty(this.Label) ? 2 : 3; }
        public bool Empty { get => string.IsNullOrEmpty(this.Value); }
        public new Rect Container { get; set; }
        public string? Format { get; set; } = null;
        public string ErrorMessage { get; set; } = "Invalid Formatting!";
        private bool Valid { get => string.IsNullOrEmpty(this.Format) || Regex.IsMatch(this.Value, this.Format); }

        public Input(string? label, string value, Rect container, ConsoleColor background, ConsoleColor color) : base(container.Width, 1, container, background)
        {
            this.Label = label;
            this.Value = value;
            this.Container = container;
            this.Width = container.Width;
            this.Color = color;
        }
        public Input(string? label, string value, Rect container) : this(label, value, container, ConsoleColor.Gray, ConsoleColor.Blue) { }
        public Input(string? label, Rect container) : this(label, "", container) { }
        public Input(Rect container) : this(null, container) { }

        public override void Render()
        {
            (int x, int y) = SetLine(0);

            if (this.Height > 1)
            {
                ConsoleUtils.SetColors(this.Container.Background, this.Color);
                Console.Write(this.Label);
            }
            RenderValue();
        }

        private void RenderValue()
        {
            ConsoleUtils.SetColors(this.Background, this.Color);
            SetLine(this.Height - 2);
            var printText = this.Value[^Math.Min(this.Width, this.Value.Length)..];
            Console.Write(printText);
            if (printText.Length < this.Width) Console.Write(new string(' ', this.Width - printText.Length));
            if (!string.IsNullOrEmpty(this.Format)) HandleError();
        }

        public void Focus()
        {
            this.Focused = true;
            SetCursor();
            while (this.Focused)
            {
                var v = Console.ReadKey(true);
                if (EscapeKeys.Contains(v.Key))
                {
                    this.Blur();
                    return;
                }
                if (Regex.IsMatch(v.KeyChar.ToString(), @"\P{C}"))
                {
                    Type(this.Value + v.KeyChar);
                }
                else if (v.Key == ConsoleKey.Backspace && !this.Empty)
                {
                    Type(this.Value[..^1]);
                }
            }
        }

        private void HandleError()
        {
            (int x, int y) = SetLine(this.Height - 1);
            Console.SetCursorPosition(x, y);
            if (this.Valid)
            {
                ConsoleUtils.SetColors(this.Container.Background);
                Console.WriteLine(new string(' ', this.Width));
            }
            else
            {
                ConsoleUtils.SetColors(ConsoleColor.Red, ConsoleColor.White);
                Console.WriteLine(this.ErrorMessage);
            }
        }

        private void SetCursor()
        {
            Console.CursorVisible = false;
            (int x, int y) = SetLine(this.Height - 2);
            var pX = this.Value.Length > this.Width ? this.Width - 1 : Math.Max(this.Value.Length - 1, 0);
            Console.SetCursorPosition(x + pX, y);
            Console.CursorVisible = true;
        }

        private void Type(string value)
        {
            this.Value = value;
            this.RenderValue();
            SetCursor();
        }

        public void Blur()
        {
            this.Focused = false;
            Console.CursorVisible = false;
        }

        private static readonly ConsoleKey[] EscapeKeys =
        {
            ConsoleKey.Enter, ConsoleKey.Escape, ConsoleKey.Tab
        };
    }
}
