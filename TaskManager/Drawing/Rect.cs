using Utils;

namespace Drawing
{
    public class Rect : Shape, IDrawable
    {
        public bool Visible { get; private set; } = true;
        public virtual ConsoleColor Background { get; set; } = ConsoleColor.Black;
        public Padding Padding { get; private set; } = new Padding();
        private BorderType? BorderType { get; set; } = null;

        public int OutlineWidth { get => this.Padding.Left + this.Padding.Right + this.BorderWidth * 2; }
        public int OutlineHeight { get => this.Padding.Top + this.Padding.Bottom + this.BorderWidth * 2; }
        public override int OuterWidth { get => this.Width + this.OutlineWidth; }
        public override int OuterHeight { get => this.Height + this.OutlineHeight; }
        public override int OutlineLeft { get =>this.Padding.Left + this.BorderWidth; }
        public override int OutlineTop { get => this.Padding.Top + this.BorderWidth; }
        private int BorderWidth { get => this.BorderType == null ? 0 : 1; }

        public Rect(int x, int y, int width, int height, Shape? container, ConsoleColor background) : base(x, y, width, height, container) { this.Background = background; }
        public Rect(int x, int y, int width, int height, Rect container) : this(x, y, width, height, container, container.Background) { }
        public Rect(int width, int height, Shape? container, ConsoleColor background) : this(0, 0, width, height, container, background) { }
        public Rect(int width, int height, Shape? container) : this(0, 0, width, height, container, Console.BackgroundColor) { }
        public Rect(int width, int height, ConsoleColor background) : this(0, 0, width, height, null, background) { }
        public Rect(int width, int height) : this(0, 0, width, height, null, Console.BackgroundColor) { }

        public (int, int) SetLine(int position)
        {
            (int pX, int pY) = GetAbsolutePosition();
            var y = pY + position;
            Console.SetCursorPosition(pX, y);
            return (pX, y);
        }

        private void DrawBorders()
        {
            if (this.BorderType == null) return;

            ConsoleUtils.SetColors(this.Background, this.BorderType.Color);
            for (int i = 0; i < this.OuterHeight; i++)
            {
                (int x, int y) = SetLine(i);
                if (i == 0 || i == this.OuterHeight - 1)
                {
                    Console.Write(this.BorderType.GetLine(this.OuterWidth, i == 0));
                    continue;
                }
                Console.Write(this.BorderType.V);
                Console.SetCursorPosition(x + this.OuterWidth - 1, y);
                Console.Write(this.BorderType.V);
            }
        }

        public void Fill()
        {
            ConsoleUtils.SetColors(Background);
            for (int i = 0; i < this.OuterHeight; i++)
            {
                SetLine(i);
                Console.Write(new string(' ', this.OuterWidth));
            }
        }

        public virtual void Render()
        {
            Fill();
            DrawBorders();
            this.RenderChildren();
        }

        public void SetBorders(BorderStyle style, ConsoleColor color = ConsoleColor.White)
        {
            this.BorderType = new BorderType(style, color);
        }

        public void FitContainerWidth()
        {
            if (this.Container == null) return;
            this.Width = Math.Max(this.Container.Width - this.OutlineWidth, 0);
        }

        private void Show()
        {
            if (this.Visible) return;
            this.Visible = true;
            this.Render();
        }

        private void Hide()
        {
            if (!this.Visible) return;
            this.Visible = false;
            if (this.Container != null)
            {
                this.Container.RenderChildren();
            }
            else
            {
                Console.Clear();
            }
        }

        public void Toggle()
        {
            if (this.Visible)
            {
                this.Hide();
            }
            else
            {
                this.Show();
            }
        }
    }
}