using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;

namespace noobOsu.Game.UI.Basic
{
    public partial class BasicScrollSearchContainer<T> : SearchContainer<T> where T : Drawable
    {
        private ScrollContainer<T> scrollableContent;

        protected override Container<T> Content => scrollableContent;

        public BasicScrollSearchContainer()
        {
            InternalChild = scrollableContent = new BasicScrollContainer<T>()
            {
                RelativeSizeAxes = Axes.Both,
            };
        }

        public void AddItem(T item)
        {
            Add(item);
        }

        public void RemoveItem(T item, bool disposeImmediately)
        {
            Remove(item, disposeImmediately);
        }
    }
}