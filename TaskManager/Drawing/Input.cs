using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utils;

namespace Drawing
{
    public class Input : Rect
    {
        public bool ReanOnly { get; set; } = false;
        public ConsoleColor Color { get; set; }
        public ConsoleColor LabelColor { get; set; }
        public string? Label { get; private set; }
        public virtual string Value { get; set; } = String.Empty;
        public bool Focused { get; private set; } = false;
        public bool Nullable { get; set; } = true;
        public override int Height { get => string.IsNullOrEmpty(this.Label) ? 2 : 3; }
        public bool Empty { get => string.IsNullOrEmpty(this.Value); }
        public new Rect Container { get; set; }
        public string? Format { get; set; } = null;
        public string? Placeholder { get; set; } = null;
        public virtual string ErrorMessage => !this.Nullable && this.Empty ? "Value required" : "Invalid format";
        public virtual bool Valid => this.Nullable && this.Empty || (!this.Empty && (string.IsNullOrEmpty(this.Format) || !this.Empty && Regex.IsMatch(this.Value, this.Format)));
        public Action<string?>? OnChange { get; set; }

        public Input(string? label, string value, Rect container, ConsoleColor background, ConsoleColor color, ConsoleColor labelColor) : base(container.InnerWidth, 1, background)
        {
            this.Label = label;
            this.Value = value;
            this.Container = container;
            this.Color = color;
            this.LabelColor = labelColor;
        }
        public Input(string? label, string value, Rect container) : this(label, value, container, ConsoleColor.Gray, ConsoleColor.Blue, ConsoleColor.Black) { }
        public Input(string label, Rect container, ConsoleColor background, ConsoleColor color, ConsoleColor labelColor) : this(label, "", container, background, color, labelColor) { }
        public Input(string? label, Rect container) : this(label, "", container) { }
        public Input(Rect container) : this(null, container) { }

        public override void Render()
        {
            SetLine(0);

            if (this.Height > 1)
            {
                ConsoleUtils.SetColors(this.Container.Background, this.LabelColor);
                Console.Write(this.Label);
            }
            RenderValue();
        }

        private void RenderValue()
        {
            var hasPlaceholder = this.Empty && this.Placeholder != null;
            ConsoleUtils.SetColors(this.Background, hasPlaceholder ? ConsoleColor.Gray : this.Color);
            SetLine(this.Height - 2);
            var printText = hasPlaceholder ? this.Placeholder : this.Value[^Math.Min(this.Width, this.Value.Length)..];
            Console.Write(printText);
            if (printText?.Length < this.Width) Console.Write(new string(' ', this.Width - printText.Length));
            HandleError();
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
            ConsoleUtils.SetColors(this.Container.Background);
            Console.Write(new string(' ', this.Width));
            if (!this.Valid)
            {
                ConsoleUtils.SetColors(ConsoleColor.Red, ConsoleColor.White);
                Console.CursorLeft = x + this.InnerWidth - this.ErrorMessage.Length;
                Console.WriteLine(this.ErrorMessage);
            }
        }

        private void SetCursor()
        {
            (int x, int y) = SetLine(this.Height - 2);
            var pX = this.Value.Length > this.Width ? this.Width - 1 : Math.Max(this.Value.Length - 1, 0);
            Console.SetCursorPosition(x + pX, y);
            Console.CursorVisible = true;
        }

        public virtual void Type(string value)
        {
            Console.CursorVisible = false;
            this.Value = value;
            this.RenderValue();
            SetCursor();
            Console.CursorVisible = true;
            if (this.Valid) this.OnChange?.Invoke(string.IsNullOrEmpty(this.Value) ? null : this.Value);
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

        public override void Clear()
        {
            this.Value = "";
        }
    }

    public class DateInput : Input
    {
        public DateTime? Date { get; private set; }
        public string _dateFormat;
        public string _value = String.Empty;
        public override string Value {
            get
            {
                return this._value;
            }
            set {
                SetDate(value);
                this._value = value;
            }
        }
        public override bool Valid {
            get {
                if (!base.Valid) return false;
                return this.Empty || this.Date.HasValue;
            }
        }

        public override string ErrorMessage {
            get
            {
                if (!base.Valid) return base.ErrorMessage;
                return "Invalid Date";
            }
        }

        public DateInput(string label, Rect container, ConsoleColor background, ConsoleColor color, ConsoleColor labelColor, string dateFormat) : base(label, container, background, color, labelColor) {
            this._dateFormat = dateFormat;
        }

        public override void Type(string value)
        {
            SetDate(value);
            base.Type(value);
        }

        private void SetDate(string value)
        {
            this._value = value;
            var parsed = DateTime.TryParseExact(value, this._dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);
            if (base.Valid && parsed)
            {
                this.Date = date;
            }
            else if (this.Date != null)
            {
                this.Date = null;
            }
        }
    }
}
