using System.Collections.Generic;
using osuTK;
using osu.Framework.Graphics.Colour;

namespace noobOsu.Game.Beatmaps
{
    public class BeatmapColors
    {
        private readonly ColourInfo DefaultColor;
        private readonly List<ColourInfo> Colors = new List<ColourInfo>();
        private int CurrentColorIndex = 0;

        public BeatmapColors()
        {
            Vector4 col = new Vector4(1f);

            DefaultColor = new ColourInfo{
                TopLeft = SRGBColour.FromVector(col),
                BottomLeft = SRGBColour.FromVector(col),
                TopRight = SRGBColour.FromVector(col),
                BottomRight = SRGBColour.FromVector(col),
            };
        }

        public void AddColor(string color)
        {
            string[] colorValues = color.Split(',');
            
            Vector4 col = new Vector4(1f);
            col.X = int.Parse(colorValues[0]) / 255f;
            col.Y = int.Parse(colorValues[1]) / 255f;
            col.Z = int.Parse(colorValues[2]) / 255f;

            ColourInfo info = new ColourInfo{
                TopLeft = SRGBColour.FromVector(col),
                BottomLeft = SRGBColour.FromVector(col),
                TopRight = SRGBColour.FromVector(col),
                BottomRight = SRGBColour.FromVector(col),
            };

            Colors.Add(info);
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

        public ColourInfo GetComboColor()
        {
            if (Colors.Count == 0) return DefaultColor;
            return Colors[CurrentColorIndex];
        }
    }
}