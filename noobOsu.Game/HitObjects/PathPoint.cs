using osuTK;

namespace noobOsu.Game.HitObjects
{
    public class PathPoint
    {
        public bool Anchored { get; set; } = false;
        public Vector2 Position;

        public override string ToString()
        {
            return "PathPoint{ " + Position + "; Anchored=" + Anchored + " }";
        }
    }

    public enum PathType
    {
        Bezier,
        Catmull,
        Linear,
        PerfrectCircle,
        None,
    }
}