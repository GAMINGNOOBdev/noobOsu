using System.IO;

namespace noobOsu.Game.Util
{
    public static class SkinUtil
    {
        private static string[] ImageExtensions = new string[]{
            ".png",
            ".jpg",
            ".jpeg"
        };
        private static string[] AudioExtensions = new string[]{
            ".wav",
            ".ogg",
            ".mp3"
        };

        public static bool ImageExists(string path)
        {
            if (Util.StringUtil.GetExtension(path).Equals(string.Empty))
            {
                bool found = false;
                string newName = string.Empty;
                foreach (string extension in ImageExtensions)
                {
                    newName = path + extension;
                    found = File.Exists(newName);

                    if (found) break;
                }
                return found;
            }

            return File.Exists(path);
        }

        public static bool AudioExists(string path)
        {
            if (Util.StringUtil.GetExtension(path).Equals(string.Empty))
            {
                bool found = false;
                string newName = string.Empty;
                foreach (string extension in AudioExtensions)
                {
                    newName = path + extension;
                    found = File.Exists(newName);

                    if (found) break;
                }
                return found;
            }

            return File.Exists(path);
        }
    }
}