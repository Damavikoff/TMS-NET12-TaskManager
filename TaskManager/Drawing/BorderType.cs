namespace Drawing
{
    public enum BorderStyle
    {
        Solid,
        Double
    }

    public enum BorderElement
    {
        NwCorner,
        NeCorner,
        SeCorner,
        SwCorner,
        HorizontalLine,
        VerticalLine
    }

    public enum BorderSide
    {
        Top,
        Right,
        Bottom,
        Left
    }

    public class BorderType
    {
        private readonly char[] _elements;
        public char Nw { get => GetBorder(BorderElement.NwCorner); }
        public char Ne { get => GetBorder(BorderElement.NeCorner); }
        public char Se { get => GetBorder(BorderElement.SeCorner); }
        public char Sw { get => GetBorder(BorderElement.SwCorner); }
        public char H { get => GetBorder(BorderElement.HorizontalLine); }
        public char V { get => GetBorder(BorderElement.VerticalLine); }
        public ConsoleColor Color { get; set; } = ConsoleColor.White;

        public BorderType(char[] elements, ConsoleColor color) {
            this._elements = elements;
            this.Color = color;
        }
        public BorderType(BorderStyle style, ConsoleColor color) : this(BorderStyles[style], color) { }
        public BorderType(BorderStyle style) : this(BorderStyle.Solid, ConsoleColor.White) { }
        public BorderType() : this(BorderStyle.Solid) { }

        public string GetLine(int length, bool first, bool cornerLeft = true, bool cornerRight = true)
        {
            return (cornerLeft ? (first ? this.Nw : this.Sw) : this.H) +
                   new string(this.H, Math.Max(length - 2, 0)) +
                   (cornerRight ? (first ? this.Ne : this.Se) : this.H);
        }

        private char GetBorder(BorderElement style) => _elements[(int) style];

        private static readonly Dictionary<BorderStyle, char[]> BorderStyles = new()
        {
            { BorderStyle.Solid, new char[] { '┌', '┐', '┘', '└', '─', '│' } },
            { BorderStyle.Double, new char[] { '╔', '╗', '╝', '╚', '═', '║' } }
        };
    }
}
