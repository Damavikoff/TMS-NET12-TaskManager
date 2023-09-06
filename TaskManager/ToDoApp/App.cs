using Drawing;
using Utils;

namespace TaskManager
{
    public class App
    {
        public static List<ToDo> Data { get; set; } = new List<ToDo>();
        //{
        //new ToDo("Task 1", "task1_descr", DateTime.UtcNow),
        //new ToDo("Task 2", "task1_descr", DateTime.UtcNow),
        //new ToDo("Task 3", "task1_descr", DateTime.UtcNow),
        //new ToDo("Task 4", "task1_descr", DateTime.UtcNow),
        //new ToDo("Task 5", "task1_descr", DateTime.UtcNow),
        //new ToDo("Task 6", "task1_descr", DateTime.UtcNow),
        //new ToDo("Task 7", "task1_descr", DateTime.UtcNow),
        //new ToDo("Task 8", "task1_descr", DateTime.UtcNow),
        //new ToDo("Task 9", "task1_descr", DateTime.UtcNow),
        //new ToDo("Task 10", "task1_descr", DateTime.UtcNow),
        //new ToDo("Task 11", "task1_descr", DateTime.UtcNow),
        //new ToDo("Task 12", "task1_descr", DateTime.UtcNow),
        //new ToDo("Task 13", "task1_descr", DateTime.UtcNow),
        //new ToDo("Task 14", "task1_descr", ToDoPriority.P4, ToDoState.Failed, DateTime.UtcNow),
        //new ToDo("Task 15", "task1_descr", DateTime.UtcNow),
        //new ToDo("Task 16", "task1_descr", DateTime.UtcNow),
        //new ToDo("Task 17", "task1_descr", DateTime.UtcNow),
        //new ToDo("Task 18", "task1_descr", DateTime.UtcNow),
        //new ToDo("Task 19", "task1_descr", DateTime.UtcNow),
        //new ToDo("Task 20", "task1_descr", DateTime.UtcNow),
        //new ToDo("Task 21", "task1_descr", DateTime.UtcNow),
        //new ToDo("Task 22", "task1_descr", DateTime.UtcNow),
        //new ToDo("Task 23", "task1_descr", DateTime.UtcNow),
        //new ToDo("Task 24", "task1_descr", DateTime.UtcNow),
        //new ToDo("Task 25", "task1_descr", DateTime.UtcNow),
        //new ToDo("Task 26", "task1_descr", DateTime.UtcNow),
        //new ToDo("Task 27", "task1_descr", DateTime.UtcNow),
        //new ToDo("Task 28", "task1_descr", DateTime.UtcNow),
        //new ToDo("Task 29", "task1_descr", DateTime.UtcNow),
        //new ToDo("Task 30", "task1_descr", DateTime.UtcNow),
        //new ToDo("Task 31", "task1_descr", DateTime.UtcNow),
        //new ToDo("Task 32", "task1_descr", DateTime.UtcNow),
        //new ToDo("Task 33", "task1_descr", DateTime.UtcNow),
        //new ToDo("Task 34", "task1_descr", DateTime.UtcNow),
        //new ToDo("Task 35", "task1_descr", DateTime.UtcNow),
        //new ToDo("Task 36", "task1_descr", DateTime.UtcNow),
        //new ToDo("Task 37", "task1_descr", DateTime.UtcNow),
        //new ToDo("Task 38", "task1_descr", DateTime.UtcNow),
        //new ToDo("Task 39", "task1_descr", DateTime.UtcNow),
        //new ToDo("Task 40", "task1_descr", DateTime.UtcNow),
        //new ToDo("Task 41", "task1_descr", DateTime.UtcNow),
        //new ToDo("Task 43", "task1_descr", DateTime.UtcNow),
        //new ToDo("Task 44", "task1_descr", DateTime.UtcNow)
        //};

        private Logger? _logger;
        private FileHandler? Storage { get; set; }

        private MainForm _form;

        public App(int widht, int height) {
            Setup(widht, height, out this._form);
        }

        private void Setup(int widht, int height, out MainForm form)
        {
            if (this.Storage != null) this.Read();
            form = new MainForm(widht, height, App.Data, this);
        }

        public void Run()
        {
            this._form.Render();
        }

        public void SetLog(string path)
        {
            this._logger = new Logger(path);
        }

        public void SetStorage(string path)
        {
            this.Storage = new FileHandler(path);
            this.Read();
        }

        public void Log(string message)
        {
            if (this._logger == null) return;
            this._logger.Log(message);
        }

        public void Write(IEnumerable<ToDo> list)
        {
            if (this.Storage == null) return;
            this.Storage.Write(JsonHandler.Serialize(list));
        }

        public void Read()
        {
            if (this.Storage == null) return;
            var list = JsonHandler.Deserialize<List<ToDo>>(this.Storage.Read());
            if (list == null) return;
            App.Data.Clear();
            App.Data.AddRange(list);
            if (list.Count > 0)
            {
                var last = list[^1];
                ToDo.SetLast(Math.Max(last.Id ?? 0, list.Count));
            }
            this._form.SetData(App.Data);
        }
    }
}
