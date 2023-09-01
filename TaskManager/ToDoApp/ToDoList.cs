using Drawing;

namespace TaskManager
{
    internal class ToDoList : HeadedTable
    {
        public ToDoList(int width, Rect container, List<ToDo> data) : base(0, container.InnerHeight, container, GetColumns(width), ConsoleColor.Cyan) {
            SetData(data);
            // this.Body.SetActiveRow(0);
        }

        public void SetData(List<ToDo> data)
        {
            this.Body.Clear();
            var rows = data.Select(v => new TableRow(this.Body, new string[] { v.Id.ToString(), v.Name, ToDo.PriorityLabels[v.Priority], ToDo.StateLabels[v.State], v.Expires.ToString("dd-MM-yyyy hh:mm") }));
            this.Body.SetRows(rows.ToArray());
        }

        private static List<TableColumn> GetColumns(int width)
        {
            return new List<TableColumn>()
            {
                new TableColumn("ID", 9, TextAlign.Right),
                new TableColumn("Title", width - 57),
                new TableColumn("Priority", 16, TextAlign.Center),
                new TableColumn("State", 14, TextAlign.Center),
                new TableColumn("Expires", 18, TextAlign.Center)
            };
        }
    }
}
