using Drawing;
using System.Globalization;
using TaskManager;

namespace ToDoApp
{
    internal class EditModal : Form
    {
        private const ConsoleColor COLOR_BACKGROUND = ConsoleColor.Gray;
        private const ConsoleColor COLOR_TITLE = ConsoleColor.Blue;
        private const ConsoleColor COLOR_BORDER = ConsoleColor.Black;
        private const ConsoleColor COLOR_BUTTON = ConsoleColor.Black;
        private const ConsoleColor COLOR_TABLE = ConsoleColor.Black;
        private const ConsoleColor COLOR_TEXT = ConsoleColor.Black;
        private const ConsoleColor COLOR_INPUT = ConsoleColor.White;

        private const ConsoleKey KEY_SAVE = ConsoleKey.Enter;
        private const ConsoleKey KEY_PRIORITY = ConsoleKey.P;
        private const ConsoleKey KEY_STATE = ConsoleKey.S;
        private const ConsoleKey KEY_EXIT = ConsoleKey.Escape;

        private const string FORMAT_DATE = "dd-MM-yyyy";

        private readonly MainForm _mainForm;
        private readonly Rect _body;
        private readonly ToDo _copy = new();
        private ToDo _target;

        private readonly Text _title;
        private Input _nameInput;
        private Input _descriptionInput;
        private Input _priorityInput;
        private Input _stateInput;
        private DateInput _expireInput;
        private readonly DropdownModal _priorityModal;
        private readonly DropdownModal _stateModal;

        public EditModal(int width, MainForm mainForm) : base(width, 0, COLOR_BACKGROUND)
        {
            this._mainForm = mainForm;
            this.Visible = false;
            this.Relative = false;
            SetHeader(out this._title);
            SetForm(out _body);
            SetControls();
            SetFooter();
            SetPriorityModal(out this._priorityModal);
            SetStateModal(out this._stateModal);
        }

        private void SetHeader(out Text title)
        {
            var header = new Rect(this.Width, 1, this, new BorderType());
            header.Padding.Set(0, 1, 0, 1);
            header.Border.Type.Color = COLOR_BORDER;
            header.Border.Set(true, true, true, true);
            title = new Text(header, "Edit Data") { Color = COLOR_TITLE, Align = TextAlign.Center };
            header.Add(title);
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
            AddInputs(rect);
            rect.AddAll(this.Inputs);
            rect.FitHeight();
            Add(rect);
        }

        private void AddInputs(Rect rect)
        {
            this._nameInput = new Input("Title", rect, COLOR_INPUT, COLOR_TEXT, COLOR_TITLE) { Nullable = false, OnChange = (v) => this._copy.Name = v ?? string.Empty };
            this._descriptionInput = new Input("Description", rect, COLOR_INPUT, COLOR_TEXT, COLOR_TITLE) { OnChange = (v) => this._copy.Description = v };
            this._priorityInput = new Input("Priority", rect, COLOR_INPUT, COLOR_TEXT, COLOR_TITLE) { ReanOnly = true };
            this._stateInput = new Input("State", rect, COLOR_INPUT, COLOR_TEXT, COLOR_TITLE) { ReanOnly = true };
            this._expireInput = new DateInput("Expires", rect, COLOR_INPUT, COLOR_TEXT, COLOR_TITLE, FORMAT_DATE) {
                Nullable = false,
                Placeholder = FORMAT_DATE,
                Format = @"^\d{2}-\d{2}-\d{4}$",
                OnChange = (v) => SetExpireDate()
            };
            SetInputs(new List<Input>() { this._nameInput, this._descriptionInput, this._priorityInput, this._stateInput, this._expireInput });
        }

        private void SetExpireDate()
        {
            if (this._expireInput.Date == null) return;
            this._copy.Expires = this._expireInput.Date ?? DateTime.UtcNow;
        }

        private void SetFooter()
        {
            var footer = new Rect(this.InnerWidth, 3, this, new BorderType());
            var b1 = this.ButtonKeys[KEY_SAVE];
            b1.X = footer.Width - b1.Width;
            footer.Add(b1);
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
                new Button(0, "Enter", "Save", KEY_SAVE, COLOR_BACKGROUND, COLOR_BUTTON) { Action = () => Save() },
                new Button(0, "P", "Priority", KEY_PRIORITY, COLOR_BACKGROUND, COLOR_BUTTON) { Action = () => this._priorityModal.Show() },
                new Button(0, "S", "State", KEY_STATE, COLOR_BACKGROUND, COLOR_BUTTON) { Action = () => this._stateModal.Show() },
                new Button(0, "Esc", "Exit", KEY_EXIT, COLOR_BACKGROUND, COLOR_BUTTON) { Action = () => Hide(), Visible = false },
            };
            SetButtons(buttons);
        }

        private void SetPriorityModal(out DropdownModal modal)
        {
            var list = ((ToDoPriority[])Enum.GetValues(typeof(ToDoPriority))).Select(v => new CellValue(v, ToDo.PriorityLabels[v]));
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
            var v = (ToDoPriority)value;
            this._copy.Priority = v;
            this._priorityInput.Value = ToDo.PriorityLabels[v];
        }

        private void SetState(IComparable value)
        {
            var v = (ToDoState) value;
            this._copy.State = v;
            this._stateInput.Value = ToDo.StateLabels[v];
        }

        public override void Show()
        {
            this.Show(new ToDo());
        }

        public void Show(ToDo? target)
        {
            if (target == null) return;
            this._target = target;
            this._copy.Fill(target);
            this._title.Value = target.New ? "Create ToDo" : "Edit ToDo";
            this.Inputs.ForEach(x => x.Clear());
            this.FillInputs();
            this.Reset();
            base.Show();
        }

        private void FillInputs()
        {
            if (!this._target.New)
            {
                this._nameInput.Value = this._copy.Name;
                this._descriptionInput.Value = this._copy.Description ?? String.Empty;
                this._expireInput.Value = this._copy.Expires.ToString(FORMAT_DATE);
            }
            this._priorityInput.Value = ToDo.PriorityLabels[this._copy.Priority];
            this._stateInput.Value = ToDo.StateLabels[this._copy.State];
        }

        private void Save()
        {
            if (!this.Valid) return;
            if (!this._target.New && this._copy.Equals(this._target)) this.Hide();
            var message = GetLogMessage();
            this._target.Fill(this._copy);
            this._mainForm.Body.Save(this._target);
            this._mainForm.App.Log(message);
            this._mainForm.Save();
            this.Hide();
        }

        private string GetLogMessage()
        {
            var message = string.Empty;
            if (this._target.New)
            {
                message += $"New task was created: {this._copy}";
            }
            else
            {
                message += $"Task {this._target.Id} was updated: ";
                var c = this._copy;
                var t = this._target;
                if (c.Name != t.Name) message += $"Name: {t.Name} => {c.Name};";
                if (c.Description != t.Description) message += $"Description: {t.Description} => {c.Description};";
                if (c.Priority != t.Priority) message += $"Priority: {ToDo.PriorityLabels[t.Priority]} => {ToDo.PriorityLabels[c.Priority]};";
                if (c.State != t.State) message += $"State {ToDo.StateLabels[t.State]} => {ToDo.StateLabels[c.State]};";
                if (!c.Expires.Equals(t.Expires)) message += $"ExpireDate: {t.Expires.ToString(FORMAT_DATE)} => {c.Expires.ToString(FORMAT_DATE)};";
            }
            return message;
        }
    }
}