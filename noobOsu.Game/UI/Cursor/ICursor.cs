using osuTK;
using osuTK.Input;
using noobOsu.Game.Skins;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;

namespace noobOsu.Game.UI.Cursor
{
    public interface ICursor : IDrawable, ISkinnable
    {
        CursorState State { get; }

        void ButtonPress(MouseButton button);
        void ButtonRelease(MouseButton button);
        void Moved(MouseMoveEvent movement);
        void DragStart(Vector2 startPosition);
        void DragProgress(MouseMoveEvent movement);
        void DragEnd();
        void UpdateCursor();

        void SetState(CursorState state);

        MouseButton AnyButtonPressed();
    }

    public enum CursorState
    {
        Normal,
        Dragging,
        Hidden,
    }
}