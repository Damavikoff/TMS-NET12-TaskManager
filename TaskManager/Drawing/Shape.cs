using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing
{
    public abstract class Shape
    {
        public virtual int X { get; set; }
        public virtual int Y { get; set; }
        public virtual int Height { get; set; }
        public virtual int Width { get; set; }

        public Shape(int x, int y, int width, int height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public Shape(int width, int height) : this(0, 0, width, height) { }
    }

    public abstract class DrawableShape : Shape, IDrawable
    {
        private int _y;
        private int _actualWidth;
        public override int Y
        {
            get => this.Relative && this.Container is not null ? this.GetRelativePosition() : this._y;
            set => this._y = value;
        }
        public override int Width
        {
            get => GetInnerMeasures().Item1;
            set { _actualWidth = value; }
        }

        public virtual int InnerWidth { get => this.Width; }
        public virtual int InnerHeight { get => this.Height; }
        public virtual int OuterWidth { get => this.Width; }
        public virtual int OuterHeight { get => this.Height; }
        public virtual int OutlineLeft { get => 0; }
        public virtual int OutlineTop { get => 0; }
        public virtual bool Visible { get; set; } = true;
        public bool Relative { get; set; } = true;
        public virtual int ContentHeight => this.Children.FindAll(v => v.Visible).Sum(x => x.OuterHeight);

        public virtual List<DrawableShape> Children { get; set; } = new List<DrawableShape>();
        public virtual DrawableShape? Container { get; set; }

        public DrawableShape(int x, int y, int width, int height) : base(x, y, width, height)
        {
        }
        public DrawableShape(int width, int height) : this(0, 0, width, height) { }

        public (int, int) GetAbsolutePosition()
        {
            if (this.Container == null) return (this.X, this.Y);
            (int x, int y) = this.Container.GetAbsolutePosition();
            return (x + this.Container.OutlineLeft + this.X, y + this.Container.OutlineTop + this.Y);
        }

        private (int, int) GetInnerMeasures()
        {
            if (this.Container == null) return (this._actualWidth, this.Height);
            return (Math.Min(this.Container.InnerWidth, this._actualWidth), Math.Min(this.Container.InnerHeight, this.Height));
        }

        public abstract void Render();
        public virtual void RenderChildren()
        {
            this.Children.ForEach(v => { if (v.Visible) v.Render(); });
        }

        public int GetRelativePosition()
        {
            if (this.Container == null) return 0;
            var i = this.Container.Children.IndexOf(this);
            return this.Container.Children.GetRange(0, this.Container.Children.IndexOf(this)).Sum(v => v.OuterHeight);
        }

        public void Add(DrawableShape item)
        {
            this.Children.Add(item);
            item.Container = this;
        }

        public void AddAll(IEnumerable<DrawableShape> list)
        {
            foreach (var item in list)
            {
                Add(item);
            }
        }

        public virtual void Clear()
        {
            this.Children.ForEach(v => { v.Container = null; });
            this.Children.Clear();
        }

        public virtual void Show()
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

        public void Center ()
        {
            if (this.Container == null) return;
            var width = this.Container.OuterWidth;
            var height = this.Container.OuterHeight;
            this.X = width / 2 - this.OuterWidth / 2;
            this.Y = height / 2 - this.OuterHeight / 2;
        }
    }
}
