using osu.Framework.Graphics;

namespace noobOsu.Game.Graphics
{
    public interface IExternalCanAddChildren
    {
        void AddChild(Drawable child);
        void RemoveChild(Drawable child);
    }
}