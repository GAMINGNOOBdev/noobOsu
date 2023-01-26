using System;
using osuTK.Graphics;

namespace noobOsu.Game.Util
{
    public static class ColorUtil
    {
        public static Color4 Interpolate(Color4 a, Color4 b, float portion)
        {
            if (portion <= 0)
                return a;
            if (portion >= 1)
                return b;
            
            Color4 delta = Multiply(Subtract(b, a), portion);

            return Add(a, delta);
        }

        public static Color4 Lighten(Color4 col, float portion)
        {
            portion *= 0.5f;
            Color4 newColor = new Color4(
                col.R * (1 + portion/2),
                col.G * (1 + portion/2),
                col.B * (1 + portion/2),
                col.A
            );

            return Normalized(AddRGB(newColor, portion));
        }

        public static Color4 Add(Color4 start, float amount)
        {
            return new Color4(start.R + amount, start.G + amount, start.B + amount, start.A + amount);
        }

        public static Color4 Add(Color4 start, Color4 end)
        {
            return new Color4(start.R + end.R, start.G + end.G, start.B + end.B, start.A + end.A);
        }

        public static Color4 AddRGB(Color4 start, float amount)
        {
            return new Color4(start.R + amount, start.G + amount, start.B + amount, start.A);
        }

        public static Color4 AddRGB(Color4 start, Color4 end)
        {
            return new Color4(start.R + end.R, start.G + end.G, start.B + end.B, start.A);
        }

        public static Color4 Subtract(Color4 start, float amount)
        {
            return new Color4(start.R - amount, start.G - amount, start.B - amount, start.A - amount);
        }

        public static Color4 Subtract(Color4 start, Color4 end)
        {
            return new Color4(start.R - end.R, start.G - end.G, start.B - end.B, start.A - end.A);
        }

        public static Color4 Multiply(Color4 start, float amount)
        {
            return new Color4(start.R * amount, start.G * amount, start.B * amount, start.A * amount);
        }

        public static Color4 Multiply(Color4 start, Color4 amount)
        {
            return new Color4(start.R * amount.R, start.G * amount.G, start.B * amount.B, start.A * amount.A);
        }

        public static Color4 Divide(Color4 start, float amount)
        {
            return new Color4(start.R / amount, start.G / amount, start.B / amount, start.A / amount);
        }

        public static Color4 Divide(Color4 start, Color4 amount)
        {
            return new Color4(start.R / amount.R, start.G / amount.G, start.B / amount.B, start.A / amount.A);
        }

        public static Color4 Normalized(Color4 color)
        {
            return new Color4(
                Math.Min(1, color.R),
                Math.Min(1, color.G),
                Math.Min(1, color.B),
                Math.Min(1, color.A)
            );
        }
    }
}