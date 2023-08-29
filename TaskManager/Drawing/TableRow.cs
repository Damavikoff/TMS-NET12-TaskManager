using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing
{
    public class TableRow : Rect
    {
        public bool Active { get; set; }
        public List<TableCell> Cells { get; private set; }
        public override ConsoleColor Background { get => this.Active ? this.Table.Selection : this.Table.Background; }
        public ConsoleColor Foreground { get => this.Active ? this.Table.Background : this.Table.Color; }
        public Table Table { get; private set; }

        public TableRow(int y, Table table, string[] values) : base(0, y, table.Width, 1, table) {
            this.Table = table;
            this.Cells = new List<TableCell>();
            var x = 0;
            for (int i = 0; i < values.Length; i++)
            {
                var w = table.Columns[i].Width;
                this.Cells.Add(new TableCell(x, w, this, values[i]));
                x += w;
            }
        }

        public override void Render()
        {
            this.Fill();
            this.Cells.ForEach(cell => cell.Render());
        }

        public void ToggleSelection()
        {
            this.Active = !this.Active;
        }
    }
}
