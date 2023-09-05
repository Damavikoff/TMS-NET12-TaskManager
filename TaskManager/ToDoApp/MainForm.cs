﻿using Drawing;
using System.Reflection.PortableExecutable;
using ToDoApp;

namespace TaskManager
{
    internal class MainForm : Form
    {

        private const ConsoleColor COLOR_BACKGROUND = ConsoleColor.DarkBlue;
        private const ConsoleColor COLOR_BUTTON = ConsoleColor.Gray;
        private const ConsoleColor COLOR_TITLE = ConsoleColor.Yellow;

        private const ConsoleKey KEY_EXIT = ConsoleKey.Escape;
        private const ConsoleKey KEY_CREATE = ConsoleKey.F1;
        private const ConsoleKey KEY_EDIT = ConsoleKey.Enter;
        private const ConsoleKey KEY_DELETE = ConsoleKey.Delete;
        private const ConsoleKey KEY_SEARCH = ConsoleKey.F2;
        private const ConsoleKey KEY_SORT = ConsoleKey.F3;
        private const ConsoleKey KEY_RESET = ConsoleKey.F4;
        private const ConsoleKey KEY_UP = ConsoleKey.UpArrow;
        private const ConsoleKey KEY_DOWN = ConsoleKey.DownArrow;

        private readonly ToDoList _body;
        private readonly Rect _footer;
        private readonly Form _sortModal;
        private readonly Form _searchModal;
        private List<ToDo> ToDoList { get; set; } = new List<ToDo>();
        public ToDoList Body => _body;

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
            this._body.Sort(0);
            this.SetSortModal(out this._sortModal);
            this.SetSearchModal(out this._searchModal);
        }

        private void SetHeader(out Rect header)
        {
            header = new Rect(this.Width, 1, this, new BorderType());
            var text = new Text(header, "ToDo List App") { Color = COLOR_TITLE };
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
            var escapeButton = this.ButtonKeys[KEY_EXIT];
            escapeButton.X = footer.Width - escapeButton.Width;
        }

        private void UpdateFooterButtons()
        {
            var hasRows = this._body.Body.Rows.Count > 0;
            this.ButtonKeys[KEY_RESET].Toggle(this._body.Filtered);
            this.ButtonKeys[KEY_SORT].Toggle(hasRows);
            this.ButtonKeys[KEY_EDIT].Toggle(hasRows);
            this.ButtonKeys[KEY_DELETE].Toggle(hasRows);

            var x = 0;
            foreach (var b in this.Buttons.FindAll(v => v.Visible && v != this.ButtonKeys[KEY_EXIT]))
            {
                b.X = x;
                x += b.OuterWidth;
            }
        }

        private void SetSortModal(out Form modal)
        {
            modal = new SortModal(50, this);
            this.Add(modal);
            modal.Center();
        }

        private void SetSearchModal(out Form modal)
        {
            modal = new SearchModal(60, this);
            this.Add(modal);
            modal.Center();
        }

        private void SetControls()
        {
            var buttons = new Button[]
            {
                new Button(0, "F1", "Create", KEY_CREATE, COLOR_BACKGROUND, COLOR_BUTTON),
                new Button(0, "Enter", "Edit", KEY_EDIT, COLOR_BACKGROUND, COLOR_BUTTON),
                new Button(0, "Del", "Delete", KEY_DELETE, COLOR_BACKGROUND, COLOR_BUTTON),
                new Button(0, "F2", "Search", KEY_SEARCH, COLOR_BACKGROUND, COLOR_BUTTON) { Action = () => this._searchModal.Show() },
                new Button(0, "F3", "Sort", KEY_SORT, COLOR_BACKGROUND, COLOR_BUTTON) { Action = () => this._sortModal.Show() },
                new Button(0, "F4", "Reset", KEY_RESET, COLOR_BACKGROUND, COLOR_BUTTON) { Action = () => Reset() },
                new Button(0, "↑", "Prev", KEY_UP, COLOR_BACKGROUND, COLOR_BUTTON) { Action = () => this._body.Body.SetPrev(), Visible = false },
                new Button(0, "↓", "Next", KEY_DOWN, COLOR_BACKGROUND, COLOR_BUTTON) { Action = () => this._body.Body.SetNext(), Visible = false },
                new Button(0, "Esc", "Exit", KEY_EXIT, COLOR_BACKGROUND, COLOR_BUTTON) { Action = () => Environment.Exit(0) }
            };
            this.SetButtons(buttons);
        }

        private void Reset()
        {
            this._body.Reset();
            this.RenderChildren();
        }

        public override void RenderChildren()
        {
            UpdateFooterButtons();
            base.RenderChildren();
        }
    }
}
