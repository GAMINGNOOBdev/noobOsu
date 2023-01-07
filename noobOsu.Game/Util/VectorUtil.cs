using osuTK;
using System;

namespace noobOsu.Game.Util
{
    public static class VectorUtil
    {
        public static double Magnitude(Vector2 a)
        {
            return Math.Sqrt((double) (a.X*a.X + a.Y*a.Y) );
        }

        public static double GetAngleBetween(Vector2 a, Vector2 b)
        {
            Vector2 d = a - b;

            float angle = 90 + (float)-MathHelper.RadiansToDegrees(Math.Atan2(d.X, d.Y));

            if (float.IsNaN(angle))
                return 0;
            return angle;
        }
    }
}