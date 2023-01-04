using System.Collections.Generic;
using osuTK;
using osuTK.Graphics;

namespace noobOsu.Game.Skins
{
    public class ColorStore : IColorStore
    {
        private readonly List<Color4> Colors = new List<Color4>();
        private int CurrentColorIndex = 0;

        public void AddColor(string color)
        {
            Colors.Add(IColorStore.FromString(color));
        }

        public void NextColor()
        {
            if (Colors.Count == 0) return;
            CurrentColorIndex++;
            CurrentColorIndex %= Colors.Count;
        }

        public void Skip(int amount)
        {
            if (Colors.Count == 0) return;
            CurrentColorIndex += amount;
            CurrentColorIndex %= Colors.Count;
        }

        public Color4 GetComboColor()
        {
            return Colors[CurrentColorIndex];
        }
    }
}