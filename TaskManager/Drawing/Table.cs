using System.Collections;

namespace Drawing
{
    public enum Order
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
        public int EndIndex { get => this.StartIndex + this.RenderCount - 1; }
        public TableRow? ActiveRow { get => this.RowsToRender.ElementAtOrDefault(this.CurrentIndex); }
        private int RenderCount { get => this.InnerHeight; }
        public List<TableRow> Rows { get; private set; } = new List<TableRow>();
        public bool Filtered { get; private set; } = false;
        public List<TableRow> FilteredRows { get; private set; } = new List<TableRow>();
        public List<TableRow> Page { get => RowsToRender.Count <= this.RenderCount ? this.RowsToRender : this.RowsToRender.GetRange(this.StartIndex, this.RenderCount); }
        public List<TableRow> RowsToRender => this.Filtered ? this.FilteredRows : this.Rows;
        public TableColumn? SortedBy { get; private set; } = null;
        public int CurrentIndex { get; set; } = 0;
        public int RelativeIndex { get => this.CurrentIndex + this.StartIndex; }

        public Table(int y, int height, Rect container, List<TableColumn> columns, ConsoleColor color) : base(0, y, container.InnerWidth, height, container.Background)
        {
            this.Columns = columns;
            this.Color = color;
            this.Width = columns.Select(v => v.Width).Sum();
        }

        public Table(int height, Rect container, List<TableColumn> columns, ConsoleColor color) : this(0, height, container, columns, color) { }

        public Table(int y, int height, Rect container, List<TableColumn> columns) : this(y, height, container, columns, ConsoleColor.White) { }

        public void SetRows(IEnumerable<TableRow> rows)
        {
            Clear();
            this.Rows.AddRange(rows);
            AddAll(rows);
            SetActiveRow(0);
        }

        public override void Clear()
        {
            base.Clear();
            this.Rows.Clear();
            this.FilteredRows.Clear();
            this.Filtered = false;
        }

        public override void Render()
        {
            this.Fill();
            (int x, int y) = GetAbsolutePosition();
            this.Page.ForEach(v => v.Render());
        }

        public void SetPrev()
        {
            if (this.CurrentIndex == 0) return;
            SetActiveRow(Math.Max(0, this.CurrentIndex - 1));
        }

        public void SetNext()
        {
            if (this.CurrentIndex == this.RowsToRender.Count - 1) return;
            SetActiveRow(Math.Min(this.RowsToRender.Count - 1, this.CurrentIndex + 1));
        }

        public void SetActiveRow(int i)
        {
            if (i == this.CurrentIndex) return;
            if (this.Page.Count == 0) return;
            var toRender = i < this.StartIndex || i > this.EndIndex;
            var current = this.CurrentIndex;
            this.CurrentIndex = i;
            if (toRender)
            {
                this.StartIndex = i < this.StartIndex ? i : i - this.RenderCount + 1;
                this.Render();
                return;
            }
            this.RowsToRender[current].Render();
            this.RowsToRender[this.CurrentIndex].Render();
        }

        public void Sort(int index, Order order = Order.Asc)
        {
            this.SortedBy = this.Columns[index];
            this.SortedBy.Order = order;
            this.SortData(index, order);
        }

        public void Sort()
        {
            if (this.SortedBy == null) return;
            var index = this.Columns.IndexOf(this.SortedBy);
            this.SortData(index, this.SortedBy.Order);
        }

        private void SortData(int index, Order sort)
        {
            this.Rows.Sort((a, b) => (sort == Order.Asc ? a : b).Cells[index].Value.CompareTo((sort == Order.Asc ? b : a).Cells[index].Value));
        }

        public void Reset()
        {
            this.Filtered = false;
            this.CurrentIndex = 0;
            this.FilteredRows.Clear();
        }
    }

    public class HeadedTable : Rect
    {
        private Rect _header;
        public Table Body { get; private set; }
        public ConsoleColor HeaderColor { get; set; }

        public HeadedTable(int y, int height, Rect container, List<TableColumn> columns, ConsoleColor headerColor, ConsoleColor bodyColor) : base(0, y, container.InnerWidth, height, container) {
            this.Body = new Table(2, height - 2, container, columns, bodyColor);
            this.HeaderColor = headerColor;
            SetHeader(out this._header);
            this.Add(this.Body);
        }

        public HeadedTable(int y, int height, Rect container, List<TableColumn> columns, ConsoleColor headerColor) : this(y, height, container, columns, headerColor, ConsoleColor.White) { }
        public HeadedTable(int y, int height, Rect container, List<TableColumn> columns) : this(y, height, container, columns, ConsoleColor.White, ConsoleColor.White) { }

        public override void Render()
        {
            this.Fill();
            this._header.Render();
            this.Body.Render();
        }

        private void SetHeader(out Rect header)
        {
            header = new Rect(this.InnerWidth, 1, this);
            header.Border.Set(false, false, true, false);
            this.SetHeaderCells();
            this.Add(header);
        }

        public void SetHeaderCells()
        {
            this._header.Clear();
            var columns = this.Body.Columns;
            var sortColumn = this.Body.SortedBy;
            var x = 0;
            for (int i = 0; i < columns.Count; i++)
            {
                var column = columns[i];
                var cell = new Rect(x, 0, column.Width, 1, this) { Relative = false };
                var isActive = column == sortColumn;
                var title = new Text(isActive ? column.Width - 2 : column.Width, column.Title, cell, TextAlign.Center, this.HeaderColor);
                cell.Add(title);
                if (isActive)
                {
                    var arrow = new Text(1, 1, SortLabels[column.Order], cell, this.Background, ConsoleColor.Yellow) { X = title.Width - 1 };
                    cell.Add(arrow);
                }
                if (i != columns.Count - 1)
                {
                    var line = new Text(1, 1, this.Body.Border.Type.V.ToString(), cell, this.Background, this.HeaderColor) { X = column.Width - 1 };
                    cell.Add(line);
                }
                this._header.Add(cell);
                x += column.Width;
            }
        }

        public void Sort(int i, Order sort = Order.Asc)
        {
            this.Body.Sort(i, sort);
            this.SetHeaderCells();
        }

        private static readonly Dictionary<Order, string> SortLabels = new()
        {
            { Order.Asc, "▲" },
            { Order.Desc, "▼" }
        };
    }

    public class TableColumn
    {
        public string Title { get; set; }
        public int Width { get; private set; }
        public Order Order { get; set; }
        public TextAlign Align { get; set; }
        public string? Format { get; set; } = null;

        public TableColumn(string title, int width, Order order, TextAlign align)
        {
            this.Title = title; 
            this.Width = width;
            this.Order = order;
            this.Align = align;
        }

        public TableColumn(string title, int width, TextAlign align) : this(title, width, Order.Asc, align) { }

        public TableColumn(string title, int width, Order order) : this(title, width, order, TextAlign.Left) { }

        public TableColumn(string title, int width) : this(title, width, Order.Asc) { }
    }
}
