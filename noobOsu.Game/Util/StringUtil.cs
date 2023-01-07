using System.Collections.Generic;

namespace noobOsu.Game.Util
{
    public static class StringUtil
    {
        public static string RemoveComments(string src)
        {
            string newString = src.Trim();
            if (newString.Contains("//"))
            {
                newString = newString.Substring(0, newString.IndexOf("//"));
            }

            return newString;
        }

        public static string ListToString<T>(IReadOnlyList<T> list)
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