using Utils;

namespace Drawing
{
    public class Rect : DrawableShape
    {
        public override bool Visible { get; set; } = true;
        public virtual ConsoleColor Background { get; set; } = ConsoleColor.Black;
        public Padding Padding { get; private set; } = new Padding();
        public Border Border { get; private set; } = new Border();

        public int OutlineWidth { get => this.Padding.Left + this.Padding.Right + (this.Border.Left ? 1 : 0) + (this.Border.Right ? 1 : 0); }
        public int OutlineHeight { get => this.Padding.Top + this.Padding.Bottom + (this.Border.Top ? 1 : 0) + (this.Border.Bottom ? 1 : 0); }
        public override int InnerWidth => this.Width - this.OutlineWidth;
        public override int OuterHeight => this.Height + this.OutlineHeight;
        public override int OutlineLeft => this.Padding.Left + (this.Border.Left ? 1 : 0);
        public override int OutlineTop => this.Padding.Top + (this.Border.Top ? 1 : 0);

        public Rect(int x, int y, int width, int height, ConsoleColor background) : base(x, y, width, height) { this.Background = background; }
        public Rect(int x, int y, int width, int height, Rect container) : this(x, y, width, height, container.Background) { }
        public Rect(int width, int height, Rect container) : this(0, 0, width, height, container) { }
        public Rect(int width, int height, ConsoleColor background) : this(0, 0, width, height, background) { }
        public Rect(int width, int height) : this(0, 0, width, height, Console.BackgroundColor) { }
        public Rect(int width, int height, Rect container, BorderType borderType) : this(width, height, container) {
            this.Border.Type = borderType;
        }

        public (int, int) SetLine(int position)
        {
            (int pX, int pY) = GetAbsolutePosition();
            var y = pY + position;
            Console.SetCursorPosition(pX, y);
            return (pX, y);
        }

        private void DrawBorders()
        {
            if (!this.Border.IsSet) return;

            ConsoleUtils.SetColors(this.Background, this.Border.Type.Color);
            for (int i = 0; i < this.OuterHeight; i++)
            {
                (int x, int y) = SetLine(i);
                if (this.Border.Top && i == 0)
                {
                    Console.Write(this.Border.Type.GetLine(this.OuterWidth, true, this.Border.Left, this.Border.Right));
                    continue;
                }
                if (this.Border.Bottom && i == this.OuterHeight - 1)
                {
                    Console.Write(this.Border.Type.GetLine(this.OuterWidth, false, this.Border.Left, this.Border.Right));
                    continue;
                }
                if (this.Border.Left) Console.Write(this.Border.Type.V);
                if (this.Border.Right)
                {
                    Console.SetCursorPosition(x + this.OuterWidth - 1, y);
                    Console.Write(this.Border.Type.V);
                }
            }
        }

        public void Fill()
        {
            ConsoleUtils.SetColors(Background);
            for (int i = 0; i < this.OuterHeight; i++)
            {
                SetLine(i);
                Console.Write(new string(' ', this.Width));
            }
        }

        public override void Render()
        {
            if (!this.Visible) return;
            Fill();
            DrawBorders();
            RenderChildren();
        }

        public void SetBorderType(BorderStyle style, ConsoleColor color = ConsoleColor.White)
        {
            this.Border.Type = new BorderType(style, color);
        }

        public void FitContainerWidth()
        {
            if (this.Container == null) return;
            this.Width = Math.Max(this.Container.Width - this.OutlineWidth, 0);
        }
    }
}