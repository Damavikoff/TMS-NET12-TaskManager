using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing
{
    public class TableRow : Rect
    {
        public List<TableCell> Cells { get; private set; }
        public override ConsoleColor Background { get => this.Active ? this.Table.Selection : this.Table.Background; }
        public ConsoleColor Foreground { get => this.Active ? this.Table.Background : this.Table.Color; }
        public Table? Table { get; private set; }
        public override int Y { get => this.Table.Page.IndexOf(this); }
        public bool Active { get => this.Table.ActiveRow == this; }

        public TableRow(Table table, List<CellValue> values) : base(0, 0, table.Width, 1, table) {
            this.Table = table;
            this.Cells = new List<TableCell>();
            var x = 0;
            var count = table.Columns.Count;
            for (int i = 0; i < count; i++)
            {
                var column = table.Columns[i];
                var w = column.Width;
                var v = values[i];
                var cell = new TableCell(x, w, this, v.Label, v.Value, column.Align);
                //cell.Padding.Left = i > 0 ? 1 : 0;
                cell.Padding.Right = i == count - 1 ? 0 : 1;
                this.Cells.Add(cell);
                x += w;
            }
            this.AddAll(this.Cells);
        }

        public override void Render()
        {
            this.Fill();
            this.Cells.ForEach(cell => cell.Render());
        }

        public override void Clear()
        {
            this.Table = default;
            base.Clear();
        }
    }
}
