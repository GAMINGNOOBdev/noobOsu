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
    }
}