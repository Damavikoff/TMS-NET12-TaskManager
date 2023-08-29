using Utils;

namespace Drawing
{
    public class Panel : Rect
    {
        public string? Label { get; private set; }
        public ConsoleColor Color { get; private set; }

        public Panel(int x, int y, int width, int height, Shape? container, string label, ConsoleColor background, ConsoleColor color, BorderStyle borderStyle) : base(x, y, width, height, container, background)
        {
            this.Label = label;
            this.Color = color;
            this.SetBorders(borderStyle, this.Color);
        }
        public Panel(int x, int y, int height, Shape container, string label, ConsoleColor background, ConsoleColor color) : this(x, y, container.Width, height, container, label, background, color, BorderStyle.Double) { }

        public Panel(int x, int y, int height, Rect container, string label, ConsoleColor color) : this(x, y, container.Width - 2, height, container, label, container.Background, color, BorderStyle.Double) { }
        public Panel(int y, int height, Rect container, string label, ConsoleColor color) : this(0, y, container.Width - 2, height, container, label, container.Background, color, BorderStyle.Double) { }
        public Panel(int y, int height, Rect container, string label) : this(y, height, container, label, ConsoleColor.White) { }
        public Panel(int y, int height, Rect container, string label, BorderStyle borderStyle) : this(y, height, container, label, ConsoleColor.White) {
            this.SetBorders(borderStyle, this.Color);
        }

        public override void Render()
        {
            base.Render();
            if (!string.IsNullOrEmpty(this.Label))
            {
                ConsoleUtils.SetColors(this.Background, this.Color);
                (int x, int y) = SetLine(0);
                Console.SetCursorPosition(x + (int)Math.Ceiling((double)this.Width / 2) - (int)Math.Ceiling((double)this.Label.Length / 2), y);
                Console.Write(this.Label);
                ConsoleUtils.SetColors();
            }
        }
    }
}
