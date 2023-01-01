using System.Collections.Generic;
using osu.Framework.Utils;
using osuTK;

namespace noobOsu.Game.HitObjects
{
    public class SubPath
    {
        private readonly List<PathPoint> pathPoints = new List<PathPoint>();
        private bool PathFinished = false;

        public List<PathPoint> CurvePoints { get{ return pathPoints; } }

        public void AddPathPoint(PathPoint p)
        {
            if (PathFinished) return;
            
            pathPoints.Add(p);
        }

        public IReadOnlyList<Vector2> GetCurvePoints(PathType type)
        {
            List<Vector2> tempCurvePoints = new List<Vector2>();
            for (int i = 0; i < pathPoints.Count; i++)
            {
                tempCurvePoints.Add(pathPoints[i].Position);
            }

            switch (type)
            {
                case PathType.Bezier:
                    return PathApproximator.ApproximateBezier(tempCurvePoints.ToArray());

                case PathType.Catmull:
                    return PathApproximator.ApproximateCatmull(tempCurvePoints.ToArray());

                case PathType.Linear:
                    return PathApproximator.ApproximateLinear(tempCurvePoints.ToArray());

                case PathType.PerfrectCircle:
                    if (tempCurvePoints.Count != 3) break;
                    return PathApproximator.ApproximateCircularArc(tempCurvePoints.ToArray());
                
                case PathType.None:
                default:
                    return tempCurvePoints;
            }

            return PathApproximator.ApproximateBezier(tempCurvePoints.ToArray());
        }

        public void FinishPath()
        {
            PathFinished = true;
        }

        public override string ToString()
        {
            string asString = "SubPath{ ";
            for (int i = 0; i < pathPoints.Count; i++)
            {
                asString += pathPoints[i].ToString() + " , ";
            }
            asString += "}";
            return asString;
        }
    }
}