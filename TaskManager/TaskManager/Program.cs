using Utils;
using Drawing;
using System.Runtime.Versioning;

namespace TaskManager
{
    class TaskManager
    {
        [SupportedOSPlatform("windows")]
        public static void Main(string[] args)
        {
            ConsoleUtils.CreateWindow(120, 30);
            var rect = new Rect(1, 1, 50, 25, null, ConsoleColor.Yellow);
            rect.Padding.Left = 1;
            rect.Padding.Top = 1;
            rect.Padding.Right = 1;
            // rect.SetBorders(BorderStyle.Double, ConsoleColor.Red);
            //var subRect = new Rect(0, 0, 5, 2, rect, ConsoleColor.Blue);
            //subRect.FitContainerWidth();
            //rect.Children.Add(subRect);
            //Console.SetCursorPosition(0, 14);
            //var text = new Text(rect, 2, "some text to fille the rect which is probably too wide to be inserted, but i hope to see the dots there because we need to render that value.", ConsoleColor.Red);
            //text.Y = 4;
            //rect.Children.Add(text);
            var input = new Input("Title", rect);
            input.Y = 6;
            // rect.Children.Add(input);
            var panel = new Panel(0, 1, rect, "label", ConsoleColor.Red);
            var button = new Button(0, 9, "F8", "Delete", ConsoleKey.F8, rect.Background, ConsoleColor.Blue, rect);
            // rect.Children.Add(panel);
            // rect.Children.Add(button);

            var cList = new List<Rect>()
            {
                new Rect(0, 15, 2, 2, rect, ConsoleColor.Blue),
                new Rect(2, 15, 2, 2, rect, ConsoleColor.Cyan),
                new Rect(4, 15, 2, 2, rect, ConsoleColor.DarkBlue),
                new Rect(6, 15, 2, 2, rect, ConsoleColor.DarkCyan),
                new Rect(8, 15, 2, 2, rect, ConsoleColor.DarkGray),
                new Rect(10, 15, 2, 2, rect, ConsoleColor.DarkGreen),
                new Rect(12, 15, 2, 2, rect, ConsoleColor.DarkMagenta),
                new Rect(14, 15, 2, 2, rect, ConsoleColor.DarkRed),
                new Rect(16, 15, 2, 2, rect, ConsoleColor.DarkYellow),
                new Rect(18, 15, 2, 2, rect, ConsoleColor.Gray),
                new Rect(20, 15, 2, 2, rect, ConsoleColor.Green),
                new Rect(22, 15, 2, 2, rect, ConsoleColor.Magenta),
                new Rect(24, 15, 2, 2, rect, ConsoleColor.Red),
                new Rect(26, 15, 2, 2, rect, ConsoleColor.White),
                new Rect(28, 15, 2, 2, rect, ConsoleColor.Yellow)
            };

            // rect.Children.AddRange(cList);

            var form = new Form(rect.Width, 7, rect);
            var input1 = new Input("Value 1", form);
            var input2 = new Input("Value 2", form);
            input2.Y = 3;
            input2.Format = @"^\d{2}-\d{2}$";
            form.SetInputs(input1, input2);
            form.Y = 9;
            // rect.Children.Add(form);

            var tableColumns = new List<TableColumn>()
            {
                new TableColumn("Column 1", 10),
                new TableColumn("Column 2", 20),
                new TableColumn("Column 3", 15)
            };

            var table = new Table(2, 10, rect, tableColumns, ConsoleColor.White);
            table.Background = ConsoleColor.DarkBlue;
            table.Selection = ConsoleColor.White;
            var row1 = new TableRow(0, table, new string[] { "CellVal1", "CellVal2", "CellVal3asd d asdas dad as dasd adasd" });
            var row2 = new TableRow(1, table, new string[] { "x3", "x4", "Lorem ipsum idt itp" });
            table.SetRows(row1, row2);
            rect.Children.Add(table);
            rect.Render();
            // input.Focus();
            while (true)
            {
                var v = Console.ReadKey(true);
                //if (v.KeyChar == '1') subRect.X += -1;
                //if (v.KeyChar == '2') subRect.X += 1;
                //if (v.KeyChar == '3') subRect.Y += -1;
                //if (v.KeyChar == '4') subRect.Y += 1;
                //rect.Render();
                if (v.KeyChar == '1') table.setActiveRow(row1);
                if (v.KeyChar == '2') table.setActiveRow(row2);
                if (v.Key == ConsoleKey.Escape) Environment.Exit(0);
            }
        }
    }
}