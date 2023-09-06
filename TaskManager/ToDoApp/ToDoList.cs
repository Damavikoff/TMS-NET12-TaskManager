using Drawing;

namespace TaskManager
{
    internal class ToDoList : HeadedTable
    {
        public List<ToDo> Data { get; private set; }
        public List<ToDo> FilteredData { get; private set; } = new List<ToDo>();
        public bool Filtered { get; private set; }
        public ToDo? Current => this.DataList.ElementAtOrDefault(this.Body.CurrentIndex);
        public List<ToDo> DataList => this.Filtered ? this.FilteredData : this.Data;

        public ToDoList(int width, Rect container, List<ToDo> data) : base(0, container.InnerHeight, container, GetColumns(width), ConsoleColor.Cyan) {
            this.Data = data;
            SetFilteredData(data);
        }

        public void SetData(List<ToDo> data)
        {
            this.Data = data;
            SetFilteredData(this.Data);
            SortData();
        }

        private void SetFilteredData(List<ToDo> data)
        {
            var rows = new List<TableRow>();

            foreach (var v in data)
            {
                var values = new List<CellValue>()
                {
                    new CellValue(v.Id ?? 0),
                    new CellValue(v.Name),
                    new CellValue(ToDo.PriorityLabels[v.Priority]),
                    new CellValue(ToDo.StateLabels[v.State]),
                    new CellValue(v.Expires, v.Expires.ToString("dd-MM-yyyy"))
                };
                rows.Add(new TableRow(this.Body, values));
            }
            this.Filtered = data.Count < this.Data.Count;
            if (this.Filtered) {
                this.FilteredData = data;
            }
            else
            {
                this.FilteredData.Clear();
            }
            this.Body.CurrentIndex = 0;
            this.Body.SetRows(rows);
        }

        public void Reset()
        {
            if (!this.Filtered) return;
            SetFilteredData(this.Data);
        }

        public void Filter(Predicate<ToDo> predicate)
        {
            SetFilteredData(this.Data.FindAll(predicate));
        }

        public void SortData(int index, Order order)
        {
            this.DataList.Sort((a, b) =>
            {
                var aV = a.Props[index];
                var bV = b.Props[index];
                return order == Order.Asc ? aV.CompareTo(bV) : bV.CompareTo(aV);
            });
            this.Body.Clear();
            Sort(index, order);
            SetFilteredData(this.DataList);
        }

        public void SortData()
        {
            if (this.Body.SortedBy == null) return;
            var index = this.Body.Columns.IndexOf(this.Body.SortedBy);
            this.SortData(index, this.Body.SortedBy.Order);
        }

        public void Save(ToDo item)
        {
            if (item.New)
            {
                item.Id = ToDo.Next;
                this.Data.Add(item);
            }
            this.SortData();
        }

        private static List<TableColumn> GetColumns(int width)
        {
            return new List<TableColumn>()
            {
                new TableColumn("ID", 9, TextAlign.Right),
                new TableColumn("Title", width - 53),
                new TableColumn("Priority", 16),
                new TableColumn("State", 14),
                new TableColumn("Expires", 14, TextAlign.Center)
            };
        }
    }
}
