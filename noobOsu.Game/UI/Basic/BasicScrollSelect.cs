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

        void Select(IScrollSelectItem<T> item);
        void SelectRandom();
        IScrollSelectItem<T> GetSelected();

        void AddItem(IScrollSelectItem<T> item);
        IScrollSelectItem<T> GetItem(string name);
        void RemoveItem(IScrollSelectItem<T> item);
        void RemoveItem(string name);
    }

    public partial class BasicScrollSelect<T> : CompositeDrawable, IScrollSelect<T>
    {
        private const int ITEM_SPACING = 20;

        private readonly List<IScrollSelectItem<T>> items = new List<IScrollSelectItem<T>>();
        private Vector2 nextItemPosition = new Vector2(0);
        private readonly Container<Drawable> Contents;
        private IScrollSelectItem<T> currentSelected;

        public IReadOnlyList<IScrollSelectItem<T>> Items { get => items; private set {} }

        public BasicScrollSelect() : base()
        {
            RelativeSizeAxes = Axes.X;
            Width = 200;
            Height = 700;

            InternalChild = Contents = Contents = new Container<Drawable>{
                RelativeSizeAxes = Axes.Both
            };
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
                nextItemPosition += ((Drawable)item).Size;
                nextItemPosition.Y += ITEM_SPACING;
                nextItemPosition.X = 0;

                items.Add(item);
                Contents.Add((Drawable)item);
            }
        }

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