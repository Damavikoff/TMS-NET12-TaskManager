namespace Drawing
{
    public class Form : Rect
    {
        public List<Input> Inputs {  get; private set; } = new List<Input>();
        public List<Button> Buttons { get; private set; } = new List<Button>();
        private Input? ActiveInput { get; set; } = null;
        private List<ConsoleKey> ButtonKeys { get => this.Buttons.Select(v => v.ConsoleKey).ToList(); }

        public Form(int width, int height, Rect container) : base(width, height, container, container.Background) { }

        public override void Render()
        {
            this.Fill();
            this.Inputs.ForEach(x => x.Render());
            this.Buttons.ForEach(x => x.Render());
            SwitchInput();
            SetKeys();
        }

        private void SetKeys()
        {
            var keysToListen = this.Buttons.Select(v => v.ConsoleKey).ToList();
            while (this.Visible)
            {
                var v = Console.ReadKey(true);
                if (v.Key == ConsoleKey.Tab)
                {
                    SwitchInput();
                }
                else if (this.ButtonKeys.Contains(v.Key))
                {
                    Environment.Exit(0);
                }
            }
        }

        private void SwitchInput()
        {
            if (this.Inputs.Count == 0) return;
            if (this.ActiveInput == null)
            {
                this.ActiveInput = this.Inputs.First();
                this.ActiveInput.Focus();
                return;
            }
            var index = this.Inputs.FindIndex(v => this.ActiveInput != null && v == this.ActiveInput);
            this.ActiveInput.Blur();
            this.ActiveInput = this.Inputs[(index + 1) % this.Inputs.Count];
            this.ActiveInput.Focus();
        }

        public void SetInputs(params Input[] inputs)
        {
            this.Inputs = inputs.ToList();
        }

        public void SetButtons(params Button[] buttons)
        {
            this.Buttons = buttons.ToList();
        }
    }
}
