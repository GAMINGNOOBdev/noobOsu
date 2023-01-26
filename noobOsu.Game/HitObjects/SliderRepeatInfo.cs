using System.Collections.Generic;

namespace noobOsu.Game.HitObjects
{
    public interface ISliderRepeatInfo
    {
        IReadOnlyList<int> GetAllPoints();
        int ForSliderStart();
        int ForSliderEnd();
    }

    public class SliderRepeatInfo : ISliderRepeatInfo
    {
        private List<int> infoPoints = new List<int>();

        public void AddTimingPoint(int time) => infoPoints.Add(time);

        public IReadOnlyList<int> GetAllPoints() => infoPoints;
        
        public int ForSliderStart()
        {
            if (infoPoints.Count < 2) return 0;
            int time = 0;
            for (int i = 1; i < infoPoints.Count; i+=2)
            {
                time = infoPoints[i];
            }
            return time;
        }
        
        public int ForSliderEnd()
        {
            if (infoPoints.Count < 1) return 0;
            int time = 0;
            for (int i = 0; i < infoPoints.Count; i+=2)
            {
                time = infoPoints[i];
            }
            return time;
        }
    }
}