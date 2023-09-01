namespace Drawing
{
    public abstract class Sided<T>
    {
        public T Left {  get; set; }
        public T Right { get; set; }
        public T Top { get; set; }
        public T Bottom { get; set; }

        public Sided(T top, T right, T bottom, T left) { 
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
            this.Left = left;
        }

        public void Set(T top, T right, T bottom, T left)
        {
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
            this.Left = left;
        }
    }

    public class Padding : Sided<int>
    {
        public Padding(int top, int right, int bottom, int left) : base(top, right, bottom, left) { }
        public Padding() : this(0, 0, 0, 0) { }
    }

    public class Border : Sided<bool>
    {
        public bool IsSet { get => this.Top || this.Right || this.Bottom || this.Left; }

        public BorderType Type { get; set; } = new BorderType();
        public Border(bool top, bool right, bool bottom, bool left) : base(top, right, bottom, left) { }
        public Border() : this(false, false, false, false) { }
        public Border(BorderType borderType) : this() {
            this.Type = borderType;
        }
    }
}
