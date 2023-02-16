using System.IO;
using noobOsu.Game.Stores;
using noobOsu.Game.UI.Cursor;
using osu.Framework.IO.Stores;
using System.Collections.Generic;
using noobOsu.Game.Skins.Drawables;
using noobOsu.Game.Skins.Properties;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using noobOsu.Game.HitObjects.Drawables;

namespace noobOsu.Game.Skins
{
    public class DefaultSkin : ISkin
    {
        private ResourceStore<byte[]> resources;
        private SkinCursor cursor;

        public string DirectoryName => "Default";
        public ISkinGeneral General { get; private set; }
        public ISkinColors Colors { get; private set; }
        public ISkinFont Font { get; private set; }
        public ICursor Cursor
        {
            get => cursor;
            set
            {
                throw new System.NotSupportedException("cursor is changed automatically so no need for a manual set");
            }
        }
        public ISkinnable SkinnableCursor
        {
            get => cursor;
            set
            {
                throw new System.NotSupportedException("cursor is changed automatically so no need for a manual set");
            }
        }

        public DefaultSkin(ResourceStore<byte[]> gameResources)
        {
            General = new SkinGeneral(this);
            Colors = new SkinColors(this);
            Font = new SkinFont(this);
            cursor = new SkinCursor();
            resources = gameResources;
        }

        public SpriteText GetHitobjectNumber(int num, DrawableHitObject hitObject, TextureStore textureStore) {
            string numAsString = num.ToString();

            SpriteText numberTextures = new SpriteText(){
                Font = GetHitcircleFont(hitObject.Radius / (double)SkinFontStore.INSTANCE?.GetCharacterScale( numAsString[0] )),
                Text = numAsString,
            };
            return numberTextures;
        }

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
                            if (property.Name.Contains("@2x"))
                                property.SetScale(2);

                            property.Resolve(textureStore.Get("default/" + property.Name));
                            break;
                        
                        case ISkinnableProperty.Type.Color:
                            property.Resolve( Colors.GetColorFor(property.Name) );
                            break;
                        
                        case ISkinnableProperty.Type.Audio:
                            //property.Resolve(noobOsuAudioManager.INSTANCE.GetTrackStore().Get("Skins/" + DirectoryName + "/" + property.Name));
                            break;
                        
                        case ISkinnableProperty.Type.Font:
                            //property.Resolve(new object[] {this, textureStore});
                            break;
                        
                        case ISkinnableProperty.Type.Bool:
                            property.Resolve(General.GetInfoFor(property.Name));
                            break;
                    }
                }
            }
        }

        public void LoadSkin(string path)
        {
            StreamReader file = new StreamReader( resources.GetStream("default/skin.ini") );

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

            cursor.ResetResolvedProperties();
        }

        private FontUsage GetHitcircleFont(double size)
        {
            return FontUsage.Default.With(size: (float)size);
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