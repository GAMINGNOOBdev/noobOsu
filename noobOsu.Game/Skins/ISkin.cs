using System.IO;
using System.Collections.Generic;
using osu.Framework.Graphics.Textures;

namespace noobOsu.Game.Skins
{
    public interface ISkin
    {
        ISkinGeneral General { get; }
        ISkinColors Colors { get; }
        ISkinFont Font { get; }

        void LoadSkin(string path);

        void ResolveSkinnables(IEnumerable<ISkinnable> skinnables, TextureStore textureStore);

        static ISkinGeneral LoadSkinInfo(string path)
        {
            ISkinGeneral general = new SkinGeneral();

            StreamReader file = new StreamReader(path);
            string line;
            while ((line = file.ReadLine()) != null)
            {
                if (line == "") continue;
                line = line.TrimStart();
                if (line.StartsWith("//")) continue;

                if (line.Equals("[General]"))
                {
                    while ((line = file.ReadLine()) != null)
                    {
                        if (line.StartsWith('[')) break;
                        if (line == "") continue;
                        line = line.TrimStart();
                        if (line.StartsWith("//")) continue;

                        general.AddGeneralInfo(line);
                    }
                    break;
                }
            }
            file.Close();

            return general;
        }

        static List<string> GetAvailableSkinFolders()
        {
            List<string> result = new List<string>();

            foreach (string s in Directory.EnumerateDirectories("Skins/", "*", SearchOption.TopDirectoryOnly))
            {
                result.Add(s.Substring("Skins/".Length));
            }

            return result;
        }
    }
}