using System.Collections.Generic;

namespace noobOsu.Game.Util
{
    public static class StoreageUtil
    {
        public static List<T> ToList<T>(IEnumerable<T> e)
        {
            List<T> elements = new List<T>();
            foreach (T element in e)
                elements.Add(element);
            
            return elements;
        }
    }
}