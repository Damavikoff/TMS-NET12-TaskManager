using Drawing;
using TaskManager;

namespace ToDoApp
{
    internal class SearchModal : Form
    {
        private const ConsoleColor COLOR_BACKGROUND = ConsoleColor.Gray;
        private const ConsoleColor COLOR_TITLE = ConsoleColor.Blue;
        private const ConsoleColor COLOR_BORDER = ConsoleColor.Black;
        private const ConsoleColor COLOR_BUTTON = ConsoleColor.Black;
        private const ConsoleColor COLOR_TABLE = ConsoleColor.Black;
        private const ConsoleColor COLOR_TEXT = ConsoleColor.Black;
        private const ConsoleColor COLOR_INPUT = ConsoleColor.White;

        private const ConsoleKey KEY_SEARCH = ConsoleKey.Enter;
        private const ConsoleKey KEY_RESET = ConsoleKey.F1;
        private const ConsoleKey KEY_PRIORITY = ConsoleKey.P;
        private const ConsoleKey KEY_STATE = ConsoleKey.S;
        private const ConsoleKey KEY_EXIT = ConsoleKey.Escape;

        private readonly MainForm _mainForm;
        private readonly Rect _body;
        private int? _id;
        private string? _name;
        private ToDoPriority? _priority;
        private ToDoState? _state;
        private string? _expires;
        private Input _priorityInput;
        private Input _stateInput;
        private DropdownModal _priorityModal;
        private DropdownModal _stateModal;

        public SearchModal(int width, MainForm mainForm) : base(width, 0, COLOR_BACKGROUND)
        {
            this._mainForm = mainForm;
            this.Visible = false;
            this.Relative = false;
            SetHeader();
            SetForm(out _body);
            SetControls();
            SetFooter();
            SetPriorityModal(out this._priorityModal);
            SetStateModal(out this._stateModal);
        }

        private void SetHeader()
        {
            var header = new Rect(this.Width, 1, this, new BorderType());
            header.Padding.Set(0, 1, 0, 1);
            header.Border.Type.Color = COLOR_BORDER;
            header.Border.Set(true, true, true, true);
            var text = new Text(header, "Search Data") { Color = COLOR_TITLE, Align = TextAlign.Center };
            header.Add(text);
            this.Add(header);
        }

        private void SetForm(out Rect rect)
        {
            rect = new Rect(this.InnerWidth, 0, this, new BorderType());
            rect.Border.Type.Color = COLOR_BORDER;
            rect.Border.Set(false, true, true, true);
            rect.Padding.Set(1, 1, 1, 1);
            this._priorityInput = new Input("Priority", rect, COLOR_INPUT, COLOR_TEXT, COLOR_TITLE) { ReanOnly = true };
            this._stateInput = new Input("State", rect, COLOR_INPUT, COLOR_TEXT, COLOR_TITLE) { ReanOnly = true };
            var inputs = new List<Input>()
            {
                new Input("Id", rect, COLOR_INPUT, COLOR_TEXT, COLOR_TITLE) { Format = @"^\d+$", OnChange = (v) => this._id = v == null ? null : int.Parse(v) },
                new Input("Title", rect, COLOR_INPUT, COLOR_TEXT, COLOR_TITLE) { OnChange = (v) => this._name = v },
                this._priorityInput,
                this._stateInput,
                new Input("Expires", rect, COLOR_INPUT, COLOR_TEXT, COLOR_TITLE) { Placeholder = "dd-mm-yyyy", Format = @"^\d{2}-\d{2}-\d{4}$", OnChange = (v) => this._expires = v }
            };
            rect.AddAll(inputs);
            rect.FitHeight();
            Add(rect);
            SetInputs(inputs.ToArray());
        }

        private void SetFooter()
        {
            var footer = new Rect(this.InnerWidth, 3, this, new BorderType());
            var b1 = this.ButtonKeys[KEY_SEARCH];
            b1.X = footer.Width - b1.Width;
            var b2 = this.ButtonKeys[KEY_RESET];
            b2.X = b1.X - b2.OuterWidth;
            footer.Add(b1);
            footer.Add(b2);
            var b3 = this.ButtonKeys[KEY_PRIORITY];
            footer.Add(b3);
            var b4 = this.ButtonKeys[KEY_STATE];
            b4.X = b3.OuterWidth;
            footer.Add(b4);
            Add(footer);
        }

        private void SetControls()
        {
            var buttons = new Button[]
            {
                new Button(0, "Enter", "Search", KEY_SEARCH, COLOR_BACKGROUND, COLOR_BUTTON) { Action = () => this.DoSearch() },
                new Button(0, "P", "Priority", KEY_PRIORITY, COLOR_BACKGROUND, COLOR_BUTTON) { Action = () => this._priorityModal.Show() },
                new Button(0, "S", "State", KEY_STATE, COLOR_BACKGROUND, COLOR_BUTTON) { Action = () => this._stateModal.Show() },
                new Button(0, "F1", "Reset", KEY_RESET, COLOR_BACKGROUND, COLOR_BUTTON) { Action = () => ClearBody() },
                new Button(0, "Esc", "Exit", KEY_EXIT, COLOR_BACKGROUND, COLOR_BUTTON) { Action = () => Hide(), Visible = false },
            };
            SetButtons(buttons);
        }

        private void SetPriorityModal(out DropdownModal modal)
        {
            var list = ((ToDoPriority[]) Enum.GetValues(typeof(ToDoPriority))).Select(v => new CellValue(v, ToDo.PriorityLabels[v]));
            modal = new DropdownModal(40, "Priority", list) { Handler = SetPriority };
            Add(modal);
            modal.Center();
        }

        private void SetStateModal(out DropdownModal modal)
        {
            var list = ((ToDoState[])Enum.GetValues(typeof(ToDoState))).Select(v => new CellValue(v, ToDo.StateLabels[v]));
            modal = new DropdownModal(40, "State", list) { Handler = SetState };
            Add(modal);
            modal.Center();
        }

        private void SetPriority(IComparable value)
        {
            var v = (ToDoPriority) value;
            this._priority = v;
            this._priorityInput.Value = ToDo.PriorityLabels[v];
        }

        private void SetState(IComparable value)
        {
            var v = (ToDoState) value;
            this._state = v;
            this._stateInput.Value = ToDo.StateLabels[v];
        }

        public override void Render()
        {
            ClearData();
            base.Render();
        }

        private void ClearData()
        {
            this._id = null;
            this._name = null;
            this._priority = null;
            this._expires = null;
            this.Inputs.ForEach(x => x.Clear());
            this.Reset();
        }

        private void ClearBody()
        {
            ClearData();
            this._body.Render();
        }

        private void DoSearch()
        {
            if (this._id == null && this._name == null && this._priority == null && this._state == null && this._expires == null)
            {
                this._mainForm.Body.Reset();
            }
            else
            {
                Predicate<ToDo> predicate = (v) => {
                    return (this._id == null || this._id == v.Id) &&
                    (this._name == null || v.Name.ToUpper().Contains(this._name.ToUpper())) &&
                    (this._priority == null || this._priority == v.Priority) &&
                    (this._state == null || this._state == v.State) &&
                    (this._expires == null || this._expires == v.Expires.ToString("dd-MM-yyyy"));
                };
                this._mainForm.Body.Filter(predicate);
            }
            this.Hide();
        }
    }
}
