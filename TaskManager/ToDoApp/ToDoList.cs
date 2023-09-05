using Drawing;

namespace TaskManager
{
    internal class ToDoList : HeadedTable
    {
        public List<ToDo> Data { get; private set; }
        public bool Filtered { get; private set; }

        public ToDoList(int width, Rect container, List<ToDo> data) : base(0, container.InnerHeight, container, GetColumns(width), ConsoleColor.Cyan) {
            this.Data = data;
            SetData(data);
            this.Filtered = false;
            // this.Body.SetActiveRow(0);
        }

        private void SetData(List<ToDo> data)
        {
            var rows = new List<TableRow>();

            foreach (var v in data)
            {
                var values = new List<CellValue>()
                {
                    new CellValue(v.Id),
                    new CellValue(v.Name),
                    new CellValue(ToDo.PriorityLabels[v.Priority]),
                    new CellValue(ToDo.StateLabels[v.State]),
                    new CellValue(v.Expires, v.Expires.ToString("dd-MM-yyyy hh:mm"))
                };
                rows.Add(new TableRow(this.Body, values));
            }
            this.Body.CurrentIndex = 0;
            this.Body.SetRows(rows);
            this.Filtered = true;
        }

        public void Reset()
        {
            if (!this.Filtered) return;
            SetData(this.Data);
            this.Filtered = false;
        }

        public void Filter(Predicate<ToDo> predicate)
        {
            SetData(this.Data.FindAll(predicate));
        }

        private static List<TableColumn> GetColumns(int width)
        {
            return new List<TableColumn>()
            {
                new TableColumn("ID", 9, TextAlign.Right),
                new TableColumn("Title", width - 57),
                new TableColumn("Priority", 16),
                new TableColumn("State", 14),
                new TableColumn("Expires", 18, TextAlign.Center)
            };
        }
    }
}
