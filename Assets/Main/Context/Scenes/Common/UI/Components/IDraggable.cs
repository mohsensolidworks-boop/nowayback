namespace Main.Context.Scenes.Common.UI.Components
{
    public interface IDraggable : ITouchable
    {
        public bool IsDragging { get; }
    }
}
