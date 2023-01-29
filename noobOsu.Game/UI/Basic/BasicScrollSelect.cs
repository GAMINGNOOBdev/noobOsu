using osuTK;
using System;
using osuTK.Graphics;
using osu.Framework.Graphics;
using System.Collections.Generic;
using osu.Framework.Input.Events;
using osu.Framework.Graphics.Containers;

namespace noobOsu.Game.UI.Basic
{
    public interface IScrollSelect<T> : IComparable<IScrollSelect<T>>
    {
        IReadOnlyList<IScrollSelectItem<T>> Items { get; }

        void InvalidateItems();

        void Select(IScrollSelectItem<T> item);
        void SelectRandom();
        IScrollSelectItem<T> GetSelected();

        void AddItem(IScrollSelectItem<T> item);
        void FinishAdding();
        IScrollSelectItem<T> GetItem(string name);
        void RemoveItem(IScrollSelectItem<T> item);
        void RemoveItem(string name);
    }

    public partial class BasicScrollSelect<T> : CompositeDrawable, IScrollSelect<T>
    {
        public static int ITEM_SPACING = 10;

        private static Drawable MakeEmptyDrawable(float y) => new Container(){
            Width=0,
            Height=0,
            Y = y,
        };

        private readonly List<IScrollSelectItem<T>> items = new List<IScrollSelectItem<T>>();
        private Vector2 nextItemPosition = new Vector2(0);
        private readonly BasicScrollContainer<Drawable> Contents;
        private IScrollSelectItem<T> currentSelected;

        public IReadOnlyList<IScrollSelectItem<T>> Items { get => items; private set {} }

        public BasicScrollSelect() : base()
        {
            InternalChild = Contents = Contents = new BasicScrollContainer<Drawable>(Direction.Vertical){
                RelativeSizeAxes = Axes.Both,
            };
        }

        public void InvalidateItems()
        {
            Contents.Clear(false);
            nextItemPosition = new Vector2(0);
            foreach (IScrollSelectItem<T> item in items)
            {
                ((Drawable)item).Position = nextItemPosition;
                nextItemPosition.Y += item.SizeY + ITEM_SPACING;

                Contents.Add((Drawable)item);
            }
            FinishAdding();
        }

        public void Select(IScrollSelectItem<T> item)
        {
            if (currentSelected != null)
                currentSelected.Deselect();
            
            currentSelected = item;
        }

        public void SelectRandom()
        {
            if (items.Count < 1)
                return;
            
            int index = Random.Shared.Next(0, items.Count);
            currentSelected = items[index];
            currentSelected.Select();
        }

        public IScrollSelectItem<T> GetSelected() => currentSelected;

        public void AddItem(IScrollSelectItem<T> item)
        {
            if (item == null)
                return;
            
            if (GetItem(item.ItemName) == null)
            {
                ((Drawable)item).Position = nextItemPosition;
                nextItemPosition.Y += item.SizeY + ITEM_SPACING;

                items.Add(item);
                Contents.Add((Drawable)item);
            }
        }

        // we have to add an empty drawable because if the last element gets bigger by expansion the scroll container still stays the same
        public void FinishAdding() => Contents.Add(MakeEmptyDrawable(nextItemPosition.Y));

        public IScrollSelectItem<T> GetItem(string name)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
                return null;

            foreach (IScrollSelectItem<T> item in items)
            {
                if (item.ItemName.Equals(name))
                    return item;
            }
            return null;
        }

        public void RemoveItem(IScrollSelectItem<T> item) {
            if (item == null)
                return;

            if (GetItem(item.ItemName) != null)
            {
                item.Removed();
                items.Remove(item);
                Contents.Remove((Drawable)item, false);
            }
        }

        public void RemoveItem(string name)
        {
            IScrollSelectItem<T> item = GetItem(name);
            if (item != null)
            {
                item.Removed();
                items.Remove(item);
                Contents.Remove((Drawable)item, false);
            }
        }

        public int CompareTo(IScrollSelect<T> other)
        {
            if (other == null)
                return -1;

            if (other.Items.Count < Items.Count)
                return 1;
            if (other.Items.Count > Items.Count)
                return -1;
            
            if (other.Items.Equals(Items))
                return 0;
            
            return -1;
        }
    }
}