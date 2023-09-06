using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Utils;

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

        public int? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ToDoPriority Priority { get; set; } = ToDoPriority.P1;
        public ToDoState State { get; set;} = ToDoState.InProgress;
        [JsonDateTimeFormat("dd-MM-yyyy")]
        public DateTime Expires { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public IComparable[] Props => new IComparable[] { this.Id ?? 0, this.Name, this.Priority, this.State, this.Expires };
        [JsonIgnore]
        public bool New => this.Id == null;
        [JsonIgnore]
        public static int Next
        {
            get
            {
                ToDo._lastId += 1;
                return ToDo._lastId;
            }
        }

        public ToDo() { }

        public ToDo(int id, string name, string descritpion, ToDoPriority priority, ToDoState state, DateTime expires) {
            this.Id = id;
            this.Name = name;
            this.Description = descritpion;
            this.Priority = priority;
            this.State = state;
            this.Expires = expires;
        }

        public ToDo(string name, string descritpion, ToDoPriority priority, ToDoState state, DateTime expires) : this(0, name, descritpion, priority, state, expires) {
            this.Id = ToDo.Next;
        }
        public ToDo(string name, string descritpion, DateTime expires) : this(name, descritpion, ToDoPriority.P1, ToDoState.InProgress, expires) { }

        public void Fill(ToDo item)
        {
            this.Name = item.Name;
            this.Description = item.Description;
            this.Priority = item.Priority;
            this.State = item.State;
            this.Expires = item.Expires;
        }

        public override string ToString()
        {
            return $"Name: => {this.Name}, Description: => {this.Description}, Priority: => {PriorityLabels[this.Priority]}, State: => {StateLabels[this.State]}, Expires: => {this.Expires:dd-MM-yyyy}";
        }

        public bool Equals(ToDo item)
        {
            return this.Name == item.Name &&
                   this.Description == item.Description &&
                   this.Priority == item.Priority &&
                   this.State == item.State &&
                   this.Expires == item.Expires;
        }

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

        public static void SetLast(int v)
        {
            ToDo._lastId = v;
        }
    }
}
