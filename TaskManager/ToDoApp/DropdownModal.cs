using Drawing;
using TaskManager;

namespace ToDoApp
{
    internal class DropdownModal : Form
    {
        private const ConsoleColor COLOR_BACKGROUND = ConsoleColor.DarkCyan;
        private const ConsoleColor COLOR_TITLE = ConsoleColor.Yellow;
        private const ConsoleColor COLOR_BORDER = ConsoleColor.White;
        private const ConsoleColor COLOR_BUTTON = ConsoleColor.Black;
        private const ConsoleColor COLOR_TABLE = ConsoleColor.White;
        private const ConsoleColor COLOR_SELECTION = ConsoleColor.White;

        private const ConsoleKey KEY_SELECT = ConsoleKey.Enter;
        private const ConsoleKey KEY_EXIT = ConsoleKey.Escape;
        private const ConsoleKey KEY_UP = ConsoleKey.UpArrow;
        private const ConsoleKey KEY_DOWN = ConsoleKey.DownArrow;

        private readonly Table _body;
        public Action<IComparable>? Handler { get; set; }
        private readonly string _title;
        private readonly IEnumerable<CellValue> _values;

        public DropdownModal(int width, string title, IEnumerable<CellValue> values) : base(width, 0, COLOR_BACKGROUND)
        {
            this.Visible = false;
            this.Relative = false;
            this._title = title;
            this._values = values;
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
            header.Add(new Text(header, this._title) { Color = COLOR_TITLE, Align = TextAlign.Center });
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
                new TableColumn(this._title, rect.InnerWidth, Order.Asc, TextAlign.Left)
            };
            body = new Table(0, this._values.Count(), rect, columns, COLOR_TABLE);
            rect.Height = body.Height;
            body.Selection = COLOR_SELECTION;
            var rows = this._values.Select(v => new TableRow(this._body, new List<CellValue>() { v })).ToList();
            this._body.SetRows(rows);
            rect.Add(body);
            Add(rect);
        }

        public override void Show()
        {
            this._body.CurrentIndex = 0;
            base.Show();
        }

        public void HandleSelection()
        {
            var index = this._body.CurrentIndex;
            this.Handler?.Invoke(this._body.Rows[index].Cells[0].Value);
            this.Hide();
        }

        private void SetControls()
        {
            var buttons = new Button[]
            {
                new Button(0, "Enter", "Edit", KEY_SELECT, COLOR_BACKGROUND, COLOR_BUTTON) { Action = () => this.HandleSelection() },
                new Button(0, "↑", "Prev", KEY_UP, COLOR_BACKGROUND, COLOR_BUTTON) { Action = () => this._body.SetPrev() },
                new Button(0, "↓", "Next", KEY_DOWN, COLOR_BACKGROUND, COLOR_BUTTON) { Action = () => this._body.SetNext() },
                new Button(0, "Esc", "Exit", KEY_EXIT, COLOR_BACKGROUND, COLOR_BUTTON) { Action = () => this.Hide() }
            };

            this.SetButtons(buttons);
        }
    }
}
