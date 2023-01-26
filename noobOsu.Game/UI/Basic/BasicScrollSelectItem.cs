using osuTK;
using osuTK.Graphics;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;

namespace noobOsu.Game.UI.Basic
{
    public enum ScrollSelectItemState
    {
        Selected,
        Active = Selected,
        Inactive,
    }

    public interface IScrollSelectItem<T> : IDrawable
    {
        // value of this item
        T Value { get; }

        // name of this item
        string ItemName { get; }

        // state of this item
        ScrollSelectItemState State { get; }

        // called to (de-)select this item
        void Select();
        void Deselect();

        // called when this item is removed from it's parent, kind of like a dispose method
        void Removed();
    }

    public partial class BasicScrollSelectItem<T> : CompositeDrawable, IScrollSelectItem<T>
    {
        protected IScrollSelect<T> ParentSelect { get; private set; }
        private ScrollSelectItemState state = ScrollSelectItemState.Inactive;
        private T itemValue;

        protected readonly DrawSizePreservingFillContainer Content = new DrawSizePreservingFillContainer(){
            TargetDrawSize = new Vector2(350, 50),
            BorderColour = new Color4(1f, 1f, 1f, 1f),
            CornerRadius = 5,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
        };

        public T Value {
            get => itemValue;
            private set
            {
                itemValue = value;
            }
        }
        public string ItemName { get; private set; }
        public ScrollSelectItemState State {
            get => state;
            private set{
                if (state == value)
                    return;
                
                state = value;
                ItemStateChanged(state);
            }
        }

        public BasicScrollSelectItem(string name, T value, IScrollSelect<T> parent)
        {
            RelativeSizeAxes = Axes.X;
            ItemName = name;
            itemValue = value;
            ParentSelect = parent;

            InternalChild = Content;
        }

        public void Select() 
        {
            if (ParentSelect != null)
                ParentSelect.Select(this);
            State = ScrollSelectItemState.Active;
        }

        public void Deselect() => State = ScrollSelectItemState.Inactive;

        public virtual void Removed() {
            Dispose(true);
        }

        protected virtual void ItemStateChanged(ScrollSelectItemState newstate) => throw new System.NotImplementedException();

        protected override bool OnClick(ClickEvent e)
        {
            Select();
            return base.OnClick(e);
        }

        protected void SetValue(T val) {
            this.itemValue = val;
        }

        protected void SetName(string name)
        {
            this.ItemName = name;
        }
    }
}