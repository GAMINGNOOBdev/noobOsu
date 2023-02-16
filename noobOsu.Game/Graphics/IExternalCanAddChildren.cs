using osu.Framework.Graphics;

namespace noobOsu.Game.Graphics
{
    public interface IExternalCanAddChildren
    {
        void AddChild(Drawable child);
        void AddChild(Drawable child, bool dynamicSize);
        void RemoveChild(Drawable child);
        void RemoveChild(Drawable child, bool dynamicSize);
    }
}