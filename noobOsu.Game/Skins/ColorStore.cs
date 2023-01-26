using System.Collections.Generic;
using osuTK;
using osuTK.Graphics;

namespace noobOsu.Game.Skins
{
    public class ColorStore : IColorStore
    {
        private readonly List<Color4> Colors = new List<Color4>();
        private int CurrentColorIndex = 0;
        private int ComboNumber = 0;

        public void AddColor(string color)
        {
            Colors.Add(IColorStore.FromString(color));
        }

        public void NextColor()
        {
            if (Colors.Count == 0) return;

            ComboNumber = 0;

            CurrentColorIndex++;
            CurrentColorIndex %= Colors.Count;
        }

        public void Skip(int amount)
        {
            if (Colors.Count == 0) return;
            CurrentColorIndex += amount;
            CurrentColorIndex %= Colors.Count;
        }

        public Color4? GetColor()
        {
            if (Colors.Count == 0)
                return Color4.White;
            
            ComboNumber++;
            
            return Colors[CurrentColorIndex];
        }

        public void RestartColor()
        {
            CurrentColorIndex = 0;
            ComboNumber = 0;
        }

        public int GetComboNumber() => ComboNumber;

        public bool IsEmpty() => Colors.Count == 0;
    }
}