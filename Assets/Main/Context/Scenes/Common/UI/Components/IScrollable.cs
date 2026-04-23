namespace Main.Context.Scenes.Common.UI.Components
{
    public interface IScrollable : IDraggable
    {
        public Direction ScrollDirection { get; }
    }

    public enum Direction
    {
        Horizontal,
        Vertical
    }
}
