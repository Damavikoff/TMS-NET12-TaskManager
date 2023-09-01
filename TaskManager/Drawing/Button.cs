using System;
using Utils;

namespace Drawing
{
    public class Button : Rect
    {
        private string Title { get; set; } = "Button";
        private string Label { get; set; } = "";
        public override int InnerWidth { get => this.Label.Length + this.Title.Length + 1; }
        public override int Width { get => this.InnerWidth + this.OutlineWidth; }
        public ConsoleColor TextColor { get; private set; } = ConsoleColor.Blue;
        public ConsoleKey Key { get; set; }
        public Action? Action { get; set; }

        public Button(int x, int y, string label, string title, ConsoleColor background, ConsoleColor foreground, ConsoleKey key) : base(x, y, 0, 1, background) {
            this.SetBorderType(BorderStyle.Solid, foreground);
            this.Padding.Set(0, 1, 0, 1);
            this.Border.Set(true, true, true, true);
            this.Label = label;
            this.Title = title;
            this.TextColor = foreground;
            this.Key = key;
            this.Relative = false;
        }
        public Button(int x, int y, string label, string title, ConsoleColor foreground, ConsoleKey key, Rect container) : this(x, y, label, title, container.Background, foreground, key) { }
        public Button(int x, int y, string label, string title, ConsoleKey key, Rect container) : this(x, y, label, title, ConsoleColor.White, key, container) { }
        public Button(int y, string label, string title, ConsoleKey key, Rect container) : this(0, y, label, title, key, container) { }
        public Button(int y, string label, string title, ConsoleKey key, ConsoleColor background, ConsoleColor foreground) : this(0, y, label, title, background, foreground, key) { }

        public override void Render()
        {
            base.Render();
            (int x, int y) = SetLine(1);
            Console.SetCursorPosition(x + this.OutlineLeft, y);
            ConsoleUtils.SetColors(this.Background, ConsoleColor.Red);
            Console.Write(this.Label + " ");
            ConsoleUtils.SetColors(this.Background, this.TextColor);
            Console.Write(this.Title);
        }

        public void Click()
        {
            if (!this.Visible || this.Action is null) return;
            this?.Action();
        }

        public void SetAction(ConsoleKey key, Action action)
        {
            this.Key = key;
            this.Action = action;
        }

        public void SetAction(Action action)
        {
            this.Action = action;
        }
    }

    public class KeyAction
    {
        public ConsoleKey Key {  get; private set; }
        public Action? Action { get; set; }
        public KeyAction(ConsoleKey key, Action? action) {
            this.Key = key;
            this.Action = action;
        }

        public KeyAction(ConsoleKey key) : this(key, null) { }
    }
}
