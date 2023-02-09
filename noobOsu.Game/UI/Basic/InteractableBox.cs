using System;
using osu.Framework.Input.Events;
using osu.Framework.Graphics.Shapes;

namespace noobOsu.Game.UI.Basic
{
    public partial class InteractableBox : Box
    {
        public Action<ClickEvent> ClickAction { get; set; } = null!;
        public Action<DoubleClickEvent> DoubleClickAction { get; set; } = null!;
        public Action<KeyDownEvent> KeyDownAction { get; set; } = null!;
        public Action<KeyUpEvent> KeyUpAction { get; set; } = null!;
        public Action<MouseDownEvent> MouseDownAction { get; set; } = null!;
        public Action<MouseUpEvent> MouseUpAction { get; set; } = null!;
        public Action<HoverEvent> HoverAction { get; set; } = null!;
        public Action<HoverLostEvent> HoverLostAction { get; set; } = null!;
        public Action<ScrollEvent> ScrollAction { get; set; } = null!;

        protected override bool OnClick(ClickEvent e)
        {
            ClickAction?.Invoke(e);
            return true;
        }

        protected override bool OnDoubleClick(DoubleClickEvent e)
        {
            DoubleClickAction?.Invoke(e);
            return true;
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            KeyDownAction?.Invoke(e);
            return true;
        }

        protected override void OnKeyUp(KeyUpEvent e)
        {
            KeyUpAction?.Invoke(e);
            base.OnKeyUp(e);
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            MouseDownAction?.Invoke(e);
            return true;
        }

        protected override void OnMouseUp(MouseUpEvent e)
        {
            MouseUpAction?.Invoke(e);
            base.OnMouseUp(e);
        }

        protected override bool OnHover(HoverEvent e)
        {
            HoverAction?.Invoke(e);
            return true;
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            HoverLostAction?.Invoke(e);
            base.OnHoverLost(e);
        }

        protected override bool OnScroll(ScrollEvent e)
        {
            ScrollAction?.Invoke(e);
            return true;
        }
    }
}