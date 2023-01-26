using osu.Framework.Logging;
using osu.Framework.Graphics;
using System.Collections.Generic;
using osu.Framework.Graphics.Containers;

namespace noobOsu.Game.Skins.Drawables
{
    public partial class FlexableDrawable : CompositeDrawable
    {
        private List<Drawable> children = new List<Drawable>();
        public void AddInternalElement(Drawable d) {
            AddInternal(d);
            children.Add(d);
        }

        public IReadOnlyList<Drawable> Children() => children;

        public new void Dispose()
        {
            foreach(Drawable d in children)
            {
                d.Dispose();
            }
            base.Dispose();
        }

        public void DebugInfo()
        {
            string message = "childrenPos: ";

            foreach (Drawable d in children)
            {
                message += d.Position.ToString() + " | ";
            }

            Logger.Log(message);
        }
    }
}