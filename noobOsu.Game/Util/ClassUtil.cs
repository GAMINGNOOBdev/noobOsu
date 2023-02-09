using System;

namespace noobOsu.Game.Util
{
    public static class ClassUtil
    {
        public static Action GetAction(object Object, string methodName)
        {
            if (Object.GetType().GetMethod(methodName) == null)
                return null;
            
            return Object.GetType().GetMethod(methodName).CreateDelegate<Action>();
        }
    }
}