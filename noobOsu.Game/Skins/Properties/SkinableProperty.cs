namespace noobOsu.Game.Skins.Properties
{
    public interface ISkinnableProperty
    {
        Type PropertyType { get; }
        string Name { get; }
        bool Resolved { get; }
        float Scale { get; }
        void Resolve(object obj);
        void SetScale(float scale);

        public enum Type
        {
            // loadable assembly types
            StaticImage,
            Texture,
            Color,
            Audio,
            Font,

            // primitive types
            Double,
            Bool,
            Int,
        }
    }

    public partial class SkinnableProperty : ISkinnableProperty
    {
        public ISkinnableProperty.Type PropertyType { get; set; }
        public string Name { get; set; }
        public bool Resolved { get; set; }
        public float Scale { get; set; }

        public virtual void Resolve(object obj)
        {
            if (Resolved) return;
            Resolved = true;
        }

        public virtual void SetScale(float scale) => Scale = scale;
    }
}