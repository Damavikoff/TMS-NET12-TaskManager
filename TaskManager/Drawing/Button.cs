using Utils;

namespace Drawing
{
    public class Button : Rect
    {
        private string Label { get; set; } = "Button";
        public ConsoleKey ConsoleKey { get; private set; }
        private string KeyName { get; set; } = "";
        public override int Width { get => this.KeyName.Length + this.Label.Length + 1; }
        public ConsoleColor TextColor { get; private set; } = ConsoleColor.Blue;

        public Button(int x, int y, string keyName, string label, ConsoleKey key, ConsoleColor background, ConsoleColor foreground, Shape container) : base(x, y, 0, 1, container, background) {
            this.SetBorders(BorderStyle.Solid, ConsoleColor.Black);
            this.KeyName = keyName;
            this.Label = label;
            this.ConsoleKey = key;
            this.TextColor = foreground;
        }
        public Button(int x, int y, string keyName, string label, ConsoleKey key, ConsoleColor foreground, Shape container) : this(x, y, keyName, label, key, ConsoleColor.Gray, foreground, container) { }
        public Button(int x, int y, string keyName, string label, ConsoleKey key, Shape container) : this(x, y, keyName, label, key, ConsoleColor.Gray, ConsoleColor.Black, container) { }
        public Button(int y, string keyName, string label, ConsoleKey key, Shape container) : this(0, y, keyName, label, key, ConsoleColor.Gray, ConsoleColor.Black, container) { }

        public override void Render()
        {
            base.Render();
            (int x, int y) = SetLine(1);
            Console.SetCursorPosition(x + 1, y);
            ConsoleUtils.SetColors(this.Background, ConsoleColor.Black);
            Console.Write(this.KeyName + " ");
            ConsoleUtils.SetColors(this.Background, this.TextColor);
            Console.Write(this.Label);
        }
    }
}
