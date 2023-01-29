using osuTK;
using System;
using noobOsu.Game.Util;
using osu.Framework.Utils;
using System.Collections.Generic;

namespace noobOsu.Game.HitObjects
{
    public class HitObjectPath
    {
        private readonly List<Vector2> calculatedCurvePoints = new List<Vector2>();
        private readonly List<double> calculatedCurvePointLengths = new List<double>();
        private readonly List<PathPoint> pathPoints = new List<PathPoint>();
        private readonly List<SubPath> subPaths = new List<SubPath>();
        private SubPath currentSubPath;
        private bool PathFinished = false;
        private readonly HitObject Parent;
        private Vector2 StartPosition = new Vector2(0);

        public double CalculatedLength{ get; private set; }

        public List<PathPoint> CurvePoints { get{ return pathPoints; } }

        public void SetStartPosition(Vector2 pos) => StartPosition = pos;

        public Vector2 GetStartPosition() => StartPosition;

        public Vector2 GetFirstPoint() => pathPoints[0].Position;
        public Vector2 GetLastPoint() => pathPoints[pathPoints.Count-1].Position;

        public HitObjectPath(HitObject parent)
        {
            Parent = parent;
            currentSubPath = new SubPath();
            calculatedCurvePointLengths.Add(0);
        }

        
        public double GetLastAngle()
        {
            if (pathPoints.Count < 2)
            {
                return 0;
            }
            return VectorUtil.GetAngleBetween(pathPoints[pathPoints.Count-2].Position, GetLastPoint());
        }

        public double GetFirstAngle()
        {
            if (pathPoints.Count < 2)
            {
                return 0;
            }
            return VectorUtil.GetAngleBetween(pathPoints[1].Position, pathPoints[0].Position);
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

        public Vector2 GetProgressPoint(double progress)
        {
            double p = Math.Clamp(progress, 0, 1) * Parent.Path.GetLength();
            return interpolate(IndexOfLength(p), p);
        }

        public IReadOnlyList<Vector2> GetCurvePoints() => calculatedCurvePoints;

        public void FinishPath()
        {
            PathFinished = true;
            subPaths.Add(currentSubPath);

            // calculate all vertices when finishing the path

            // calculate the curve vertices
            for (int i = 0; i < subPaths.Count; i++)
            {
                calculatedCurvePoints.AddRange( subPaths[i].GetCurvePoints(Parent.SliderInformation.CurveType) );
            }

            // also calculate the distances between each vertex and the origin
            for (int i = 0; i < calculatedCurvePoints.Count-1; i++)
            {
                CalculatedLength += (calculatedCurvePoints[i+1] - calculatedCurvePoints[i]).Length;
                calculatedCurvePointLengths.Add(CalculatedLength);
            }
        }

        public double GetLength() => calculatedCurvePointLengths[^1];

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

        private int IndexOfLength(double len)
        {
            int idx = calculatedCurvePointLengths.BinarySearch(len);
            return idx < 0 ? ~idx : idx;
        }

        private Vector2 interpolate(int idx, double val)
        {
            if (idx <= 0)
                return calculatedCurvePoints[0];
            if (idx >= calculatedCurvePoints.Count)
                return calculatedCurvePoints[calculatedCurvePoints.Count-1];
            
            Vector2 p, q;
            p = calculatedCurvePoints[idx-1];
            q = calculatedCurvePoints[idx];

            double pDistance = calculatedCurvePointLengths[idx-1];
            double qDistance = calculatedCurvePointLengths[idx];

            if (Precision.AlmostEquals(pDistance, qDistance))
                return p;
            
            double x = (val - pDistance) / (qDistance - pDistance);
            return p + (q - p) * (float)x;
        }
    }
}