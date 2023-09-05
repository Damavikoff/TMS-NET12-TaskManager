namespace Drawing
{
    public class TableCell : Rect
    {
        private readonly Text _text;
        private TableRow Row { get; set; }
        public string Label => this._text.Value;
        public IComparable Value { get; private set; }

        public TableCell(int x, int width, TableRow row, string label, IComparable value, TextAlign align ) : base(0, 0, width, 1, row)
        {
            this.Row = row;
            this._text = new Text(this.Width, 1, label, this) { Align = align };
            this.X = x;
            this.Relative = false;
            this.Add(this._text);
            this.Value = value;
        }

        public TableCell(int x, int width, TableRow row, string label, IComparable value) : this(x, width, row, label, value, TextAlign.Left) { }

        public override void Render()
        {
            //var colors = new ConsoleColor[]
            //{
            //    ConsoleColor.DarkGreen,
            //    ConsoleColor.Red,
            //    ConsoleColor.Black,
            //    ConsoleColor.Green,
            //    ConsoleColor.Magenta,
            //    ConsoleColor.Cyan
            //};
            //var rnd = new Random();
            //int i = rnd.Next(0, 5);
            this._text.Render(this.Row.Background, this.Row.Foreground);
        }

        public void SetAlign(TextAlign align)
        {
            this._text.Align = align;
        }
    }

    public class CellValue
    {
        public string Label { get; private set; }
        public IComparable Value { get; private set; }

        public CellValue(IComparable value, string label)
        {
            this.Value = value;
            this.Label = label;
        }

        public CellValue(IComparable value)
        {
            this.Value = value;
            this.Label = value?.ToString() ?? "";
        }
    }
}

