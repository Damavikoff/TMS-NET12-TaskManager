namespace TaskManager
{
    public enum ToDoPriority
    {
        P1, P2, P3, P4, P5
    }

    public enum ToDoState
    {
        InProgress,
        Done,
        Failed
    }

    public class ToDo
    {
        private static int _lastId = 0;

        public int Id { get; private set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ToDoPriority Priority { get; set; } = ToDoPriority.P1;
        public ToDoState State { get; set;} = ToDoState.InProgress;
        public DateTime Expires { get; set; } = DateTime.UtcNow;

        public ToDo(int id, string name, string descritpion, ToDoPriority priority, ToDoState state, DateTime expires) {
            this.Id = id;
            this.Name = name;
            this.Description = descritpion;
            this.Priority = priority;
            this.State = state;
            this.Expires = expires;
        }

        public ToDo(string name, string descritpion, ToDoPriority priority, ToDoState state, DateTime expires) : this(0, name, descritpion, priority, state, expires) {
            this.Id = Next();
        }
        public ToDo(string name, string descritpion, DateTime expires) : this(name, descritpion, ToDoPriority.P1, ToDoState.InProgress, expires) { }

        public static readonly Dictionary<ToDoState, string> StateLabels = new()
        {
            { ToDoState.InProgress, "In progress" },
            { ToDoState.Done, "Completed" },
            { ToDoState.Failed, "Failed" }
        };

        public static readonly Dictionary<ToDoPriority, string> PriorityLabels = new()
        {
            { ToDoPriority.P1, "Highest" },
            { ToDoPriority.P2, "High" },
            { ToDoPriority.P3, "Moderate" },
            { ToDoPriority.P4, "Low" },
            { ToDoPriority.P5, "Lowest" }
        };

        private static int Next()
        {
            ToDo._lastId += 1;
            return ToDo._lastId;
        }
    }
}
