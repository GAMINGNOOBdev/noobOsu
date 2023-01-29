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
            newString = newString.Trim();

            return newString;
        }

        public static string RemoveQuotes(string src)
        {
            string newString = src;
            newString = src.Replace("'", string.Empty);
            newString = newString.Replace("\"", string.Empty);
            newString = newString.Replace("''", string.Empty);
            return newString;
        }

        public static string RemoveWhitespaces(string src)
        {
            string newString = src;
            newString = src.Replace(" ", string.Empty);
            newString = newString.Replace("\t", string.Empty);
            newString = newString.Replace(System.Environment.NewLine, string.Empty);
            return newString;
        }

        public static string ArrayToString<T>(T[] array)
        {
            string result = "Array[ ";
            foreach (T t in array)
            {
                result += t.ToString() + " | ";
            }
            result = result.Substring(0, result.Length-3);
            result += " ]";
            return result;
        }

        public static string ListToString<T>(IReadOnlyList<T> list)
        {
            string result = "List{ ";
            foreach (T t in list)
            {
                result += t.ToString() + " | ";
            }
            result = result.Substring(0, result.Length-3);
            result += " }";
            return result;
        }
    }
}