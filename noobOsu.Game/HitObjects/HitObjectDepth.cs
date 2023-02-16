using osuTK;
using noobOsu.Game.Beatmaps;
using System.Collections.Generic;

namespace noobOsu.Game.HitObjects
{
    public interface IHitObjectDepth
    {
        int GetDepth(IHitObject obj);
    }

    public class HitObjectDepth : IHitObjectDepth
    {
        private int CurrentDepth = 0;

        public IBeatmap ParentMap;

        public HitObjectDepth(IBeatmap parent)
        {
            ParentMap = parent;
        }

        public int GetDepth(IHitObject obj)
        {
            if (ParentMap == null)
                return 0;

            IHitObject prev = GetPreviousOf(obj, ParentMap.HitObjects);

            if (prev == null)
                return 0;
            
            if ((prev.EndTime + 200) < (obj.Time - obj.ObjectTiming.TotalVisibleTime))
                return CurrentDepth;

            return CurrentDepth++;
        }

        public static IHitObject GetPreviousOf(IHitObject obj, IReadOnlyList<IHitObject> objects)
        {
            List<IHitObject> hitObjects = new List<IHitObject>(objects);
            if (!hitObjects.Contains(obj))
                return obj;

            int objectIndex = hitObjects.IndexOf(obj);
            
            if (objectIndex == 0)
                return obj;

            return objects[objectIndex-1];
        }
    }
}