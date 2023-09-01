using Drawing;

namespace TaskManager
{
    public class App
    {
        public static List<ToDo> Data { get; set; } = new List<ToDo>()
        {
            new ToDo("Task 1", "task1_descr", DateTime.UtcNow),
            new ToDo("Task 2", "task1_descr", DateTime.UtcNow),
            new ToDo("Task 3", "task1_descr", DateTime.UtcNow),
            new ToDo("Task 4", "task1_descr", DateTime.UtcNow),
            new ToDo("Task 5", "task1_descr", DateTime.UtcNow),
            new ToDo("Task 6", "task1_descr", DateTime.UtcNow),
            new ToDo("Task 7", "task1_descr", DateTime.UtcNow),
            new ToDo("Task 8", "task1_descr", DateTime.UtcNow),
            new ToDo("Task 9", "task1_descr", DateTime.UtcNow),
            new ToDo("Task 10", "task1_descr", DateTime.UtcNow),
            new ToDo("Task 11", "task1_descr", DateTime.UtcNow),
            new ToDo("Task 12", "task1_descr", DateTime.UtcNow),
            new ToDo("Task 13", "task1_descr", DateTime.UtcNow),
            new ToDo("Task 14", "task1_descr", DateTime.UtcNow),
            new ToDo("Task 15", "task1_descr", DateTime.UtcNow),
            new ToDo("Task 16", "task1_descr", DateTime.UtcNow),
            new ToDo("Task 17", "task1_descr", DateTime.UtcNow),
            new ToDo("Task 18", "task1_descr", DateTime.UtcNow),
            new ToDo("Task 19", "task1_descr", DateTime.UtcNow),
            new ToDo("Task 20", "task1_descr", DateTime.UtcNow),
            new ToDo("Task 21", "task1_descr", DateTime.UtcNow),
            new ToDo("Task 22", "task1_descr", DateTime.UtcNow),
            new ToDo("Task 23", "task1_descr", DateTime.UtcNow),
            new ToDo("Task 24", "task1_descr", DateTime.UtcNow),
            new ToDo("Task 25", "task1_descr", DateTime.UtcNow),
            new ToDo("Task 26", "task1_descr", DateTime.UtcNow),
            new ToDo("Task 27", "task1_descr", DateTime.UtcNow),
            new ToDo("Task 28", "task1_descr", DateTime.UtcNow),
            new ToDo("Task 29", "task1_descr", DateTime.UtcNow),
            new ToDo("Task 30", "task1_descr", DateTime.UtcNow),
            new ToDo("Task 31", "task1_descr", DateTime.UtcNow),
            new ToDo("Task 32", "task1_descr", DateTime.UtcNow),
            new ToDo("Task 33", "task1_descr", DateTime.UtcNow),
            new ToDo("Task 34", "task1_descr", DateTime.UtcNow),
            new ToDo("Task 35", "task1_descr", DateTime.UtcNow),
            new ToDo("Task 36", "task1_descr", DateTime.UtcNow),
            new ToDo("Task 37", "task1_descr", DateTime.UtcNow),
            new ToDo("Task 38", "task1_descr", DateTime.UtcNow),
            new ToDo("Task 39", "task1_descr", DateTime.UtcNow),
            new ToDo("Task 40", "task1_descr", DateTime.UtcNow),
            new ToDo("Task 41", "task1_descr", DateTime.UtcNow),
            new ToDo("Task 43", "task1_descr", DateTime.UtcNow),
            new ToDo("Task 44", "task1_descr", DateTime.UtcNow)
        };

        private IDrawable? Root { get; set; }

        public App(int widht, int height) {
            this.Setup(widht, height);
        }

        public void Setup(int widht, int height)
        {
            var mainForm = new MainForm(widht, height, App.Data);
            this.Root = mainForm;
        }

        public void Run()
        {
            this.Root?.Render();
        }
    }
}
