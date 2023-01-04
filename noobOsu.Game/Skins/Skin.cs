using System.IO;
using System.Collections.Generic;
using osu.Framework.Graphics.Textures;

namespace noobOsu.Game.Skins
{
    public class Skin : ISkin
    {
        public ISkinGeneral General { get; private set; } = new SkinGeneral();
        public ISkinColors Colors { get; private set; } = new SkinColors();
        public ISkinFont Font { get; private set; } = new SkinFont();

        public void ResolveSkinnables(IEnumerable<ISkinnable> skinnables, TextureStore textureStore)
        {
            foreach (ISkinnable obj in skinnables)
            {
                foreach (ISkinnableProperty property in obj.GetProperties())
                {
                    switch(property.PropertyType)
                    {
                        case ISkinnableProperty.Type.Texture:
                        case ISkinnableProperty.Type.StaticImage:
                            if (File.Exists("Skins/" + General.Name + "/" + property.Name))
                                property.Resolve(textureStore.Get("Skins/" + General.Name + "/" + property.Name));
                            else
                                property.Resolve(textureStore.Get("Skins/default/" + property.Name));
                            break;
                        
                        case ISkinnableProperty.Type.Color:
                            property.Resolve( Colors.GetColorFor(property.Name) );
                            break;
                        
                        case ISkinnableProperty.Type.Audio:
                            property.Resolve(noobOsuAudioManager.INSTANCE.GetTrackStore().Get("Skins/" + General.Name + "/" + property.Name));
                            break;
                        
                        case ISkinnableProperty.Type.Font:
                            ///TODO: -- implement --
                            break;
                    }
                }
            }
        }

        public void LoadSkin(string path)
        {
            StreamReader file = new StreamReader(path);

            SkinLoadingSection section = SkinLoadingSection.None;

            string line;
            while ((line = file.ReadLine()) != null)
            {
                if (line == "") continue;
                line = line.TrimStart();
                if (line.StartsWith("//")) continue;
                line = Util.StringUtil.RemoveComments(line);

                if (line.Equals("[General]"))
                {
                    section = SkinLoadingSection.General;
                    continue;
                }
                if (line.Equals("[Colours]"))
                {
                    section = SkinLoadingSection.Colors;
                    continue;
                }
                if (line.Equals("[Fonts]"))
                {
                    section = SkinLoadingSection.Font;
                    continue;
                }
                if (line.Equals("[CatchTheBeat]"))
                {
                    section = SkinLoadingSection.None;
                    continue;
                }
                if (line.Equals("[Mania]"))
                {
                    section = SkinLoadingSection.None;
                    continue;
                }

                if (section == SkinLoadingSection.General)
                {
                    General.AddGeneralInfo(line);
                }
                if (section == SkinLoadingSection.Colors)
                {
                    Colors.AddColorInfo(line);
                }
                if (section == SkinLoadingSection.Font)
                {
                    Font.AddFontInfo(line);
                }
            }
            file.Close();
        }

        private enum SkinLoadingSection
        {
            General,
            Colors,
            Font,
            None,
        }
    }
}