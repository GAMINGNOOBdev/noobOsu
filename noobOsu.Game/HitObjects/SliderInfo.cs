using System.Collections.Generic;

namespace noobOsu.Game.HitObjects
{
    public interface ISliderInfo
    {
        PathType CurveType { get; }
        int SlideRepeat { get; }
        float Length { get; }
        float TotalSliderSpan { get; }
        ISliderRepeatInfo GetRepeatTimingInfo();
    }

    public class SliderInfo : ISliderInfo
    {
        public PathType CurveType { get; set; } = PathType.None;
        public int SlideRepeat { get; set; } = 0;
        public float Length { get; set; } = 10f;
        public float TotalSliderSpan { get; set; } = 0f;
        public readonly SliderRepeatInfo RepeatTimingInfo = new SliderRepeatInfo();

        public ISliderRepeatInfo GetRepeatTimingInfo() => RepeatTimingInfo;

        public static PathType StringToType(string type)
        {
            if (type.Equals("B")) return PathType.Bezier;
            if (type.Equals("C")) return PathType.Catmull;
            if (type.Equals("L")) return PathType.Linear;
            if (type.Equals("P")) return PathType.PerfrectCircle;
            
            return PathType.None;
        }
    }
}