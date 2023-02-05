using osuTK;
using System.IO;
using noobOsu.Game.UI.Cursor;
using osu.Framework.Graphics;
using System.Collections.Generic;
using noobOsu.Game.Skins.Drawables;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using noobOsu.Game.HitObjects.Drawables;

namespace noobOsu.Game.Skins
{
    public interface ISkin
    {
        string DirectoryName { get; }
        ISkinGeneral General { get; }
        ISkinColors Colors { get; }
        ISkinFont Font { get; }
        ICursor Cursor { get; }
        ISkinnable SkinnableCursor { get; }

        void LoadSkin(string path);

        SpriteText GetHitobjectNumber(int num, DrawableHitObject hitObject, TextureStore textureStore);

        void ResolveSkinnables(IEnumerable<ISkinnable> skinnables, TextureStore textureStore);

        static ISkinGeneral LoadSkinInfo(string path)
        {
            ISkinGeneral general = new SkinGeneral(null);

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