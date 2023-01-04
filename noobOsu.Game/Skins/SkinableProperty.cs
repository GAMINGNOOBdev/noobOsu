namespace noobOsu.Game.Skins
{
    public interface ISkinnableProperty
    {
        Type PropertyType { get; }
        string Name { get; }
        bool Resolved { get; }
        void Resolve(object obj);

        public enum Type
        {
            StaticImage,
            Texture,
            Color,
            Audio,
            Font,
        }
    }

    public partial class SkinnableProperty : ISkinnableProperty
    {
        public ISkinnableProperty.Type PropertyType { get; set; }
        public string Name { get; set; }
        public bool Resolved { get; set; }

        public virtual void Resolve(object obj)
        {
            if (Resolved) return;
            Resolved = true;
        }
    }
}