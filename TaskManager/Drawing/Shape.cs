using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing
{
    public abstract class Shape
    {
        public int X { get; set; }
        public int Y { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public virtual int OuterWidth { get => this.Width; }
        public virtual int OuterHeight { get => this.Height; }
        public virtual int InnerX { get => this.X; }
        public virtual int InnerY { get => this.Y; }

        public Shape? Container { get; private set; }
        public List<IDrawable> Children { get; set; } = new List<IDrawable>();

        public Shape(int x, int y, int width, int height, Shape? container)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
            this.Container = container;
        }

        public Shape(int width, int height, Shape container) : this(0, 0, width, height, container) { }
        public Shape(int width, int height) : this(0, 0, width, height, null) { }

        public (int, int) GetAbsolutePosition()
        {
            if (this.Container == null) return (this.X, this.Y);
            return (this.Container.InnerX + this.X, this.Container.InnerY + this.Y);
        }

        public void RenderChildren()
        {
            foreach (var v in this.Children)
            {
                if (v.IsVisible) v.Render();
            }
        }
    }
}
