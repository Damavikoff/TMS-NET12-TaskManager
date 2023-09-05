namespace Drawing
{
    public class Form : Rect
    {
        public List<Input> Inputs {  get; private set; } = new List<Input>();
        public List<Button> Buttons { get; private set; } = new List<Button>();
        private Input? ActiveInput { get; set; } = null;
        private List<Input> WritableInputs { get => this.Inputs.FindAll(v => !v.ReanOnly).ToList(); }
        public Dictionary<ConsoleKey, Button> ButtonKeys { get; private set; } = new Dictionary<ConsoleKey, Button>();
        public override int Height => this.ContentHeight;

        public Form(int width, int height) : base(width, height) { }
        public Form(int width, int height, ConsoleColor background) : base(width, height, background) { }
        public Form(int width, int height, Rect container) : base(width, height, container.Background) { }

        public override void Render()
        {
            if (!this.Visible) return;
            base.Render();
            //SwitchInput();
            SetKeys();
        }

        private void SetKeys()
        {
            while (this.Visible)
            {
                var v = Console.ReadKey(true);
                if (this.Inputs.Count > 0 && v.Key == ConsoleKey.Tab)
                {
                    SwitchInput();
                }
                else if (this.ButtonKeys.ContainsKey(v.Key))
                {
                    this.ButtonKeys[v.Key].Click();
                }
            }
        }

        private void SwitchInput()
        {
            var inputs = this.WritableInputs;
            if (inputs.Count == 0) return;
            if (this.ActiveInput == null)
            {
                this.ActiveInput = inputs.First();
                this.ActiveInput.Focus();
                return;
            }
            var index = inputs.FindIndex(v => this.ActiveInput != null && v == this.ActiveInput);
            this.ActiveInput.Blur();
            this.ActiveInput = inputs[(index + 1) % inputs.Count];
            this.ActiveInput.Focus();
        }

        public void SetInputs(params Input[] inputs)
        {
            this.Inputs = inputs.ToList();
        }

        public void SetButtons(params Button[] buttons)
        {
            this.Buttons = buttons.ToList();
            this.ButtonKeys = this.Buttons.ToDictionary(v => v.Key, v => v);
        }

        public void Reset()
        {
            this.ActiveInput = null;
        }
    }
}
