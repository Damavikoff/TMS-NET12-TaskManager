using Drawing;
using System.Reflection.PortableExecutable;
using ToDoApp;

namespace TaskManager
{
    internal class MainForm : Form
    {

        private const ConsoleColor COLOR_BACKGROUND = ConsoleColor.DarkBlue;
        private const ConsoleColor COLOR_BUTTON = ConsoleColor.Gray;

        private readonly ToDoList _body;
        private List<ToDo> ToDoList { get; set; } = new List<ToDo>();
        private readonly Rect _footer;
        private readonly Form _sortModal;

        public MainForm(int width, int height, List<ToDo> toDoList) : base(width, height, COLOR_BACKGROUND) {
            this.ToDoList = toDoList;
            this.Padding.Set(0, 1, 0, 1);
            SetControls();
            SetHeader(out Rect header);
            SetFooter(out this._footer);
            SetBody(height - header.OuterHeight - this._footer.OuterHeight, out Rect rect, out this._body);
            this.Add(header);
            this.Add(rect);
            this.Add(this._footer);
            SetSortModal(out this._sortModal);
            this._body.SortByIndex(0);
        }

        private void SetHeader(out Rect header)
        {
            header = new Rect(this.Width, 1, this, new BorderType());
            var text = new Text(header, "ToDo List App") { Color = ConsoleColor.Yellow };
            header.Add(text);
            header.Padding.Set(0, 1, 0, 1);
            header.Border.Set(true, true, true, true);
        }

        private void SetBody(int height, out Rect rect, out ToDoList body)
        {
            rect = new Rect(this.InnerWidth, height - 1, this, new BorderType());
            rect.Border.Set(false, true, true, true);
            body = new ToDoList(rect.InnerWidth, rect, this.ToDoList);
            rect.Add(body);
        }
        private void SetFooter(out Rect footer) { 
            footer = new Rect(this.InnerWidth, 3, this, new BorderType());
            footer.AddAll(this.Buttons);
            var x = 0;
            foreach (var b in this.Buttons)
            {
                b.X = x;
                x += b.OuterWidth + 1;
            }
            var escapeButton = this.ButtonKeys[ConsoleKey.Escape];
            escapeButton.X = footer.Width - escapeButton.Width;
        }

        private void SetSortModal(out Form modal)
        {
            modal = new SortModal(50, this, this._body.Body.Columns);
            this.Add(modal);
            modal.Center();
        }

        private void SetControls()
        {
            var buttons = new Button[]
            {
                new Button(0, "F1", "Create", ConsoleKey.F1, COLOR_BACKGROUND, COLOR_BUTTON),
                new Button(0, "Enter", "Edit", ConsoleKey.Enter, COLOR_BACKGROUND, COLOR_BUTTON),
                new Button(0, "Del", "Delete", ConsoleKey.Delete, COLOR_BACKGROUND, COLOR_BUTTON),
                new Button(0, "F2", "Filter", ConsoleKey.F2, COLOR_BACKGROUND, COLOR_BUTTON),
                new Button(0, "F3", "Sort", ConsoleKey.F3, COLOR_BACKGROUND, COLOR_BUTTON),
                new Button(0, "F4", "Reset", ConsoleKey.F4, COLOR_BACKGROUND, COLOR_BUTTON),
                new Button(0, "↑", "Prev", ConsoleKey.UpArrow, COLOR_BACKGROUND, COLOR_BUTTON),
                new Button(0, "↓", "Next", ConsoleKey.DownArrow, COLOR_BACKGROUND, COLOR_BUTTON),
                new Button(0, "Esc", "Exit", ConsoleKey.Escape, COLOR_BACKGROUND, COLOR_BUTTON)
            };

            this.SetButtons(buttons);

            this.ButtonKeys[ConsoleKey.UpArrow].SetAction(ConsoleKey.UpArrow, () => { this._body.Body.SetPrev(); });
            this.ButtonKeys[ConsoleKey.DownArrow].SetAction(ConsoleKey.DownArrow, () => { this._body.Body.SetNext(); });
            this.ButtonKeys[ConsoleKey.Escape].SetAction(ConsoleKey.Escape, () => { Environment.Exit(0); });
            this.ButtonKeys[ConsoleKey.F3].SetAction(ConsoleKey.F3, () => { this._sortModal.Show(); });
        }
    }
}
