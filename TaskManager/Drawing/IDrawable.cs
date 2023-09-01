namespace Drawing
{
    public interface IDrawable
    {
        bool Visible { get; set; }
        void Render();
    }
}
