using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing
{
    public enum Sort
    {
        Asc,
        Desc
    }

    public class Table : Rect
    {
        public List<TableColumn> Columns { get; private set; } = new List<TableColumn>();
        public ConsoleColor Selection { get; set; } = ConsoleColor.White;
        public ConsoleColor Color { get; set; }
        public int StartIndex { get; set; } = 0;
        public TableRow? ActiveRow { get; set; }
        public List<TableRow> Rows { get; private set; } = new List<TableRow>();

        public Table(int y, int height, Rect container, List<TableColumn> columns, ConsoleColor color) : base(0, y, 0, height, container, container.Background)
        {
            this.Columns = columns;
            this.Color = color;
            this.Width = columns.Select(v => v.Width).Sum();
        }

        public Table(int y, int height, Rect container, List<TableColumn> columns) : this(y, height, container, columns, ConsoleColor.White) { }

        public void SetRows(params TableRow[] rows)
        {
            this.Rows = rows.ToList();
            this.ActiveRow = rows[0];
            this.ActiveRow.Active = true;
        }

        public override void Render()
        {
            this.Fill();
            this.Rows.ForEach(v => v.Render());
        }

        public void SetActiveRow(int i)
        {
            setActiveRow(this.Rows[i]);
        }

        public void setActiveRow(TableRow row)
        {
            if (row == this.ActiveRow) return;
            var oldRow = this.ActiveRow;
            oldRow?.ToggleSelection();
            this.ActiveRow = row;
            row?.ToggleSelection();
            oldRow?.Render();
            row?.Render();

        }
    }

    public class TableColumn
    {
        public string Title { get; set; }
        public int Width { get; private set; }
        public Sort Sort { get; set; }

        public TableColumn(string title, int width, Sort sort)
        {
            this.Title = title; 
            this.Width = width;
        }

        public TableColumn(string title, int width) : this(title, width, Sort.Asc) { }
    }
}
