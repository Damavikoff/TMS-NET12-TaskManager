using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing
{
    public class TableCell : Rect
    {
        public Text Text { get; set; }
        private TableRow Row { get; set; }

        public TableCell(int x, int width, TableRow row, string value, TextAlign align) : base(0, 0, width, 1, row)
        {
            this.Row = row;
            this.Text = new Text(this.Width, 1, value, this) { Align = align };
            this.X = x;
            this.Relative = false;
            this.Add(this.Text);
        }

        public TableCell(int x, int width, TableRow row, string value) : this(x, width, row, value, TextAlign.Left) { }

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
            this.Text.Render(this.Row.Background, this.Row.Foreground);
        }

        public void SetAlign(TextAlign align)
        {
            this.Text.Align = align;
        }
    }
}

