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

        public TableCell(int x, int width, TableRow row, string value) : base(0, 0, width, 1, row)
        {
            this.Row = row;
            this.Text = new Text(this.Width - 2, 1, value, this);
            this.X = x;
            this.Padding.Left = 1;
            this.Padding.Right = 1;
        }

        public override void Render()
        {
            this.Text.Render(this.Row.Background, this.Row.Foreground);
        }

        public void SetAlign(TextAlign align)
        {
            this.Text.Align = align;
        }
    }
}

