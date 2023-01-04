using System.Collections.Generic;

namespace noobOsu.Game.Skins
{
    public interface ISkinnable
    {
        void AddProperty(ISkinnableProperty property);
        List<ISkinnableProperty> GetProperties();
        void ResetResolvedProperties();
    }

    public partial class Skinnable : ISkinnable
    {
        protected readonly List<ISkinnableProperty> Properties = new List<ISkinnableProperty>();

        public void AddProperty(ISkinnableProperty property)
        {
            Properties.Add(property);
        }

        public List<ISkinnableProperty> GetProperties() => Properties;

        public void ResetResolvedProperties()
        {
            foreach (SkinnableProperty p in Properties)
            {
                p.Resolved = false;
            }
        }
    }
}