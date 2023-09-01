using Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using TaskManager;

namespace ToDoApp
{
    internal class SortModal : Form
    {
        private const ConsoleColor BACKGROUND_COLOR = ConsoleColor.Gray;
        private const ConsoleColor TITLE_COLOR = ConsoleColor.Blue;
        private const ConsoleColor BORDER_COLOR = ConsoleColor.Black;
        private const ConsoleColor BUTTON_COLOR = ConsoleColor.Black;
        private const ConsoleColor TABLE_COLOR = ConsoleColor.Black;
        private const ConsoleColor SELECTION_COLOR = ConsoleColor.Black;

        private MainForm _mainForm;
        private Table _body;
        private List<TableColumn> _sortColumns;

        public SortModal(int width, MainForm mainForm, List<TableColumn> columns) : base(width, 0, BACKGROUND_COLOR) {
            this._mainForm = mainForm;
            this._sortColumns = columns;
            this.Visible = false;
            this.Relative = false;
            SetHeader();
            SetBody(out this._body);
        }

        private void SetHeader()
        {
            var header = new Rect(this.Width, 1, this, new BorderType());
            header.Padding.Set(0, 1, 0, 1);
            header.Border.Type.Color = BORDER_COLOR;
            header.Border.Set(true, true, true, true);
            var text = new Text(header, "Sort columns") { Color = TITLE_COLOR, Align = TextAlign.Center };
            header.Add(text);
            this.Add(header);
        }

        private void SetBody(out Table body)
        {
            var rect = new Rect(this.InnerWidth, 0, this, new BorderType());
            rect.Border.Type.Color = BORDER_COLOR;
            rect.Border.Set(false, true, true, true);
            rect.Padding.Set(1, 2, 1, 2);
            var columns = new List<TableColumn>();
            columns.Add(new TableColumn("Title", rect.InnerWidth - 5, Sort.Asc, TextAlign.Left));
            columns.Add(new TableColumn("Sort", 5, Sort.Asc, TextAlign.Center));
            body = new Table(this._sortColumns.Count, rect, columns, ConsoleColor.Black);
            rect.Height = body.Height;
            body.Selection = SELECTION_COLOR;
            SetBodyRows();
            rect.Add(body);
            this.Add(rect);
        }

        private void SetBodyRows()
        {
            var rows = new List<TableRow>();
            foreach (var c in this._sortColumns)
            {
                rows.Add(new TableRow(this._body, new string[] { c.Title, c.Sort == Sort.Asc ? "DESC" : "ASC" }));
            }
            this._body.SetRows(rows.ToArray());
        }

        public override void Show()
        {
            this._body.CurrentIndex = 0;
            base.Show();
        }
    }
}
