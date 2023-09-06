using Drawing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using TaskManager;

namespace ToDoApp
{
    internal class SortModal : Form
    {
        private const ConsoleColor COLOR_BACKGROUND = ConsoleColor.Gray;
        private const ConsoleColor COLOR_TITLE = ConsoleColor.Blue;
        private const ConsoleColor COLOR_BORDER = ConsoleColor.Black;
        private const ConsoleColor COLOR_BUTTON = ConsoleColor.Black;
        private const ConsoleColor COLOR_TABLE = ConsoleColor.Black;
        private const ConsoleColor COLOR_SELECTION = ConsoleColor.Black;

        private readonly MainForm _mainForm;
        private readonly Table _body;
        private readonly List<TableColumn> _sortColumns;
        private Table DataList => this._mainForm.Body.Body;

        public SortModal(int width, MainForm mainForm) : base(width, 0, COLOR_BACKGROUND) {
            this._mainForm = mainForm;
            this._sortColumns = this.DataList.Columns;
            this.Visible = false;
            this.Relative = false;
            SetHeader();
            SetBody(out this._body);
            SetControls(); 
        }

        private void SetHeader()
        {
            var header = new Rect(this.Width, 1, this, new BorderType());
            header.Padding.Set(0, 1, 0, 1);
            header.Border.Type.Color = COLOR_BORDER;
            header.Border.Set(true, true, true, true);
            var text = new Text(header, "Sort columns") { Color = COLOR_TITLE, Align = TextAlign.Center };
            header.Add(text);
            this.Add(header);
        }

        private void SetBody(out Table body)
        {
            var rect = new Rect(this.InnerWidth, 0, this, new BorderType());
            rect.Border.Type.Color = COLOR_BORDER;
            rect.Border.Set(false, true, true, true);
            rect.Padding.Set(1, 2, 1, 2);
            var columns = new List<TableColumn>
            {
                new TableColumn("Title", rect.InnerWidth - 5, Order.Asc, TextAlign.Left),
                new TableColumn("Sort", 5, Order.Asc, TextAlign.Center)
            };
            body = new Table(this._sortColumns.Count, rect, columns, COLOR_TABLE);
            rect.Height = body.Height;
            body.Selection = COLOR_SELECTION;
            SetBodyRows();
            rect.Add(body);
            this.Add(rect);
        }

        private void SetBodyRows()
        {
            var rows = new List<TableRow>();
            foreach (var c in this._sortColumns)
            {
                var label = c == this.DataList.SortedBy && c.Order == Order.Asc ? "DESC" : "ASC";
                var values = new List<CellValue>()
                {
                    new CellValue(c.Title),
                    new CellValue(label)
                };
                rows.Add(new TableRow(this._body, values));
            }
            this._body.SetRows(rows.ToArray());
        }

        public override void Show()
        {
            this._body.CurrentIndex = 0;
            this.SetBodyRows();
            base.Show();
        }

        public void SetSort()
        {
            var index = this._body.CurrentIndex;
            var column = this._sortColumns[index];
            var sort = column == this.DataList.SortedBy && column.Order == Order.Asc ? Order.Desc : Order.Asc;
            this._mainForm.Body.SortData(index, sort);
            this.Hide();
        }

        private void SetControls()
        {
            var buttons = new Button[]
            {
                new Button(0, "Enter", "Edit", ConsoleKey.Enter, COLOR_BACKGROUND, COLOR_BUTTON),
                new Button(0, "↑", "Prev", ConsoleKey.UpArrow, COLOR_BACKGROUND, COLOR_BUTTON),
                new Button(0, "↓", "Next", ConsoleKey.DownArrow, COLOR_BACKGROUND, COLOR_BUTTON),
                new Button(0, "Esc", "Exit", ConsoleKey.Escape, COLOR_BACKGROUND, COLOR_BUTTON)
            };

            this.SetButtons(buttons);

            this.ButtonKeys[ConsoleKey.UpArrow].SetAction(ConsoleKey.UpArrow, () => this._body.SetPrev());
            this.ButtonKeys[ConsoleKey.DownArrow].SetAction(ConsoleKey.DownArrow, () => this._body.SetNext());
            this.ButtonKeys[ConsoleKey.Enter].SetAction(ConsoleKey.Enter, () => this.SetSort());
            this.ButtonKeys[ConsoleKey.Escape].SetAction(ConsoleKey.Escape, () => this.Hide());
        }
    }
}
