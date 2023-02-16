using osuTK;
using System;
using osu.Framework.Graphics;
using System.Collections.Generic;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;

namespace noobOsu.Game.UI.Basic
{
    public partial class BasicSearchableScrollSelect<T> : CompositeDrawable, IScrollSelect<T>
    {
        public static int ITEM_SPACING = 10;

        private static Drawable MakeEmptyDrawable(float y) => new Container(){
            Width=0,
            Height=0,
            Y = y,
        };

        private readonly List<IScrollSelectItem<T>> items = new List<IScrollSelectItem<T>>();
        private readonly BasicScrollSearchContainer<Drawable> SearchContents;
        private IScrollSelectItem<T> currentSelected;
        private readonly BasicTextBox SearchBar;
        private float nextItemY = 0f;

        public IReadOnlyList<IScrollSelectItem<T>> Items { get => items; private set {} }

        public BasicSearchableScrollSelect() : base()
        {
            InternalChildren = new Drawable[] {
                SearchBar = new BasicTextBox(){
                    RelativeSizeAxes = Axes.X,
                    Size = new Vector2(1, 40),
                },
                SearchContents = new BasicScrollSearchContainer<Drawable>(){
                    Margin = new MarginPadding { Top = 40 },
                    Padding = new MarginPadding { Bottom = 40 },
                    RelativeSizeAxes = Axes.Both,
                }
            };
            SearchBar.Current.BindValueChanged( (val) => {
                SearchContents.SearchTerm = val.NewValue;
            } );
        }

        public void InvalidateItems()
        {
            SearchContents.Clear(false);
            nextItemY = 0f;
            foreach (IScrollSelectItem<T> item in items)
            {
                ((Drawable)item).Position = new Vector2(((Drawable)item).X, nextItemY);
                
                if (item.IsShown)
                    nextItemY += item.SizeY + ITEM_SPACING;

                SearchContents.AddItem((Drawable)item);
            }
            FinishAdding();
        }

        protected override void Update()
        {
            InvalidateItems();
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
                ((Drawable)item).Position = new Vector2(((Drawable)item).X, nextItemY);
                nextItemY += item.SizeY + ITEM_SPACING;

                items.Add(item);
                SearchContents.AddItem((Drawable)item);
            }
        }

        // we have to add an empty drawable because if the last element gets bigger by expansion the scroll container still stays the same
        public void FinishAdding() => SearchContents.AddItem(MakeEmptyDrawable(nextItemY));

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
                SearchContents.RemoveItem((Drawable)item, false);
            }
        }

        public void RemoveItem(string name)
        {
            IScrollSelectItem<T> item = GetItem(name);
            if (item != null)
            {
                item.Removed();
                items.Remove(item);
                SearchContents.RemoveItem((Drawable)item, false);
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