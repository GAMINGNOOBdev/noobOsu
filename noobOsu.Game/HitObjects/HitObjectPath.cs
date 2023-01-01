using System.Collections.Generic;
using osuTK;

namespace noobOsu.Game.HitObjects
{
    public class HitObjectPath
    {
        private readonly List<Vector2> calculatedCurvePoints = new List<Vector2>();
        private readonly List<PathPoint> pathPoints = new List<PathPoint>();
        private readonly List<SubPath> subPaths = new List<SubPath>();
        private SubPath currentSubPath;
        private bool PathFinished = false;
        private readonly HitObject Parent;
        private Vector2 StartPosition = new Vector2(0);

        public List<PathPoint> CurvePoints { get{ return pathPoints; } }

        public void SetStartPosition(Vector2 pos) => StartPosition = pos;

        public Vector2 GetStartPosition() => StartPosition;

        public HitObjectPath(HitObject parent)
        {
            Parent = parent;
            currentSubPath = new SubPath();
        }

        public void AddAnchorPoint(string p)
        {
            if (PathFinished) return;
            
            Vector2 point = new Vector2(0f);
            string[] pValues = p.Split(':');
            
            point.X = int.Parse(pValues[0]);
            point.Y = int.Parse(pValues[1]);

            if (pathPoints.Count > 0)
            {
                if (pathPoints[pathPoints.Count-1].Position.Equals(point - StartPosition))
                {
                    pathPoints[pathPoints.Count-1].Anchored = true;
                    currentSubPath.FinishPath();
                    subPaths.Add(currentSubPath);
                    currentSubPath = new SubPath();
                    currentSubPath.AddPathPoint(pathPoints[pathPoints.Count-1]);

                    return;
                }
            }

            PathPoint pathPoint = new PathPoint();
            pathPoint.Position = (point - StartPosition);
            
            pathPoints.Add(pathPoint);
            currentSubPath.AddPathPoint(pathPoint);
        }

        public void AddAnchorPoint(Vector2 p)
        {
            if (PathFinished) return;

            if (pathPoints.Count > 0)
            {
                if (pathPoints[pathPoints.Count-1].Position.Equals(p - StartPosition))
                {
                    pathPoints[pathPoints.Count-1].Anchored = true;
                    currentSubPath.FinishPath();
                    subPaths.Add(currentSubPath);
                    currentSubPath = new SubPath();
                    currentSubPath.AddPathPoint(pathPoints[pathPoints.Count-1]);

                    return;
                }
            }

            PathPoint pathPoint = new PathPoint();
            pathPoint.Position = (p - StartPosition);
            
            pathPoints.Add(pathPoint);
            currentSubPath.AddPathPoint(pathPoint);
        }

        public IReadOnlyList<Vector2> GetCurvePoints()
        {
            if (calculatedCurvePoints.Count > 0) return calculatedCurvePoints;

            for (int i = 0; i < subPaths.Count; i++)
            {
                calculatedCurvePoints.AddRange( subPaths[i].GetCurvePoints(Parent.SliderInformation.CurveType) );
            }

            return calculatedCurvePoints;
        }

        public void FinishPath()
        {
            PathFinished = true;
            subPaths.Add(currentSubPath);
        }

        public override string ToString()
        {
            string asString = "HitObjectPath{ " + System.Environment.NewLine;
            for (int i = 0; i < subPaths.Count; i++)
            {
                asString += subPaths[i].ToString() + "," + System.Environment.NewLine;
            }
            asString += "}";
            return asString;
        }

        private List<Vector2> GetUntilNextAnchor(int startIndex)
        {
            List<Vector2> result = new List<Vector2>();

            for (int i = startIndex; i < pathPoints.Count; i++)
            {
                PathPoint p = pathPoints[i];

                result.Add(p.Position);

                if (p.Anchored) break;
            }

            return result;
        }

        private static string ListToString<T>(List<T> list)
        {
            string result = "List{ ";
            foreach (T t in list)
            {
                result += t.ToString() + " | ";
            }
            result += " }";
            return result;
        }
    }
}