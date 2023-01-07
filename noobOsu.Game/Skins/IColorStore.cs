using osuTK.Graphics;

namespace noobOsu.Game.Skins
{
    public interface IColorStore
    {
        void AddColor(string color);
        void NextColor();
        void Skip(int amount);
        Color4? GetComboColor();
        
        static Color4 FromString(string s)
        {
            string[] colorValues = s.Split(',');
            
            Color4 col = new Color4();
            col.R = int.Parse(colorValues[0]) / 255f;
            col.G = int.Parse(colorValues[1]) / 255f;
            col.B = int.Parse(colorValues[2]) / 255f;

            if (colorValues.Length > 3)
                col.A = int.Parse(colorValues[3]) / 255f;
            else
                col.A = 1f;

            return col;
        }
    }
}