using osuTK;
using osuTK.Input;
using noobOsu.Game.Skins;
using osu.Framework.Input.Events;
using osu.Framework.Graphics.Containers;
using noobOsu.Game.Skins.Properties;
using System.Collections.Generic;

namespace noobOsu.Game.UI.Cursor
{
    public partial class CursorAdapter : CompositeDrawable, ICursor
    {
        protected bool[] MouseButtons = new bool[((int)MouseButton.LastButton)+1];
        private const MouseButton DRAG_BUTTON = MouseButton.Left;
        private readonly Skinnable _skinnable = new Skinnable();
        private Vector2 DragStartPos, LastMousePos;
        private CursorState cursorState;

        public CursorState State {
            get => cursorState;
            set
            {
                if (cursorState == value)
                    return;
                
                if (cursorState == CursorState.Dragging && value == CursorState.Normal)
                    DragEnd();
                
                if (cursorState == CursorState.Normal && value == CursorState.Dragging)
                    DragStart(LastMousePos);

                cursorState = value;
            }
        }

        public CursorAdapter()
        {
            State = CursorState.Normal;
        }

        public void SetState(CursorState state)
        {
            State = state;
        }

        public virtual void ButtonPress(MouseButton button)
        {
            MouseButtons[(int)button] = true;
        }

        public virtual void ButtonRelease(MouseButton button)
        {
            MouseButtons[(int)button] = false;
            if (State == CursorState.Dragging && button.Equals(DRAG_BUTTON))
                State = CursorState.Normal;
        }

        public virtual void Moved(MouseMoveEvent movement)
        {
            LastMousePos = movement.MousePosition;

            if (AnyButtonPressed().Equals(DRAG_BUTTON))
                State = CursorState.Dragging;
            
            if (State == CursorState.Dragging)
                DragProgress(movement);
        }

        public virtual void DragStart(Vector2 startPosition)
        {
            DragStartPos = startPosition;
        }

        public virtual void DragProgress(MouseMoveEvent movement)
        {
            //Rotation = (float)Util.VectorUtil.GetAngleBetween(DragStartPos, movement.MousePosition);
        }

        public virtual void DragEnd()
        {
            //Rotation = 0f;
        }

        public virtual void UpdateCursor()
        {

        }

        public MouseButton AnyButtonPressed()
        {
            for (int i = 0; i < MouseButtons.Length; i++)
            {
                if (MouseButtons[i])
                    return (MouseButton)i;
            }
            return (MouseButton)(-1);
        }

        public void AddProperty(ISkinnableProperty property) => _skinnable.AddProperty(property);

        public List<ISkinnableProperty> GetProperties() => _skinnable.GetProperties();

        public void ResetResolvedProperties() => _skinnable.ResetResolvedProperties();
    }
}