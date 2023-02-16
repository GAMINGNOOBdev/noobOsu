using System;
using System.IO;
using System.Threading;
using noobOsu.Game.Util;
using System.Diagnostics;
using osu.Framework.Text;
using noobOsu.Game.Skins;
using System.Threading.Tasks;
using osu.Framework.IO.Stores;
using System.Collections.Generic;
using osu.Framework.Graphics.Textures;

namespace noobOsu.Game.Stores
{
    public partial class SkinFontStore : IResourceStore<TextureUpload>, IGlyphStore
    {
        public static SkinFontStore INSTANCE { get; private set; } = null;

        public static void SetCurrentSkin(ISkin skin)
        {
            if (INSTANCE != null)
            {
                INSTANCE.CurrentSkin = skin;
                INSTANCE.LoadAllChars();
            }
        }

        private readonly List<Stream> filestreams = new List<Stream>();
        private readonly List<string> filenames = new List<string>();
        private readonly List<TextureUpload> textures = new List<TextureUpload>();
        private readonly List<(CharacterGlyph,int)> characterInfo = new List<(CharacterGlyph, int)>();
        private readonly List<char> characters = new List<char>();
        private ISkin skin;

        public string FontName {
            get
            {
                if (skin != null)
                    return skin.DirectoryName;
                
                return "__SKIN__FONT__RESERVED__";
            }
            set => throw new System.NotSupportedException("setting the fontname is not supported");
        }

        public float? Baseline => 0;

        public ISkin CurrentSkin
        {
            get => skin;
            set
            {
                if (!ISkin.Equals(skin, value))
                    ClearCached();
                skin = value;
            }
        }

        public SkinFontStore() {
            INSTANCE = this;
        }

        public void Dispose()
        {
            foreach (TextureUpload texture in textures)
            {
                texture.Dispose();
            }
            foreach (Stream file in filestreams)
            {
                file.Dispose();
                file.Close();
            }
        }

        public TextureUpload Get(string name)
        {
            if (!Char.IsDigit(GetChar(name)))
                return null;

            string fullName = GetPath(name);
            fullName = TextureUtil.HasBiggerResolution(fullName);

            if (filenames.Contains(fullName))
            {
                return textures[ filenames.IndexOf(fullName) ];
            }
            
            TextureUpload texture = new TextureUpload(GetStream(fullName));
            textures.Add(texture);

            return texture;
        }

        public CharacterGlyph Get(char character)
        {
            if (characters.Contains(character))
                return characterInfo[characters.IndexOf(character)].Item1;

            TextureUpload chr = Get($"{character}");
            CharacterGlyph glyph = new CharacterGlyph(character, 0, 0, chr.Width, 0, this);

            characters.Add(character);
            characterInfo.Add( (glyph, GetScaleOf($"{character}")) );

            return glyph;
        }

        public int GetCharacterScale(char c)
        {
            if (characters.Contains(c))
                return characterInfo[characters.IndexOf(c)].Item2;
            else
            {
                // just try to load the new character
                Get(c);

                // re-check if the character got added into the list
                if (characters.Contains(c))
                    return characterInfo[characters.IndexOf(c)].Item2;
            }
            
            return 1;
        }

        public IReadOnlyList<char> GetChars() => characters;

        public System.Threading.Tasks.Task<TextureUpload> GetAsync(string name, CancellationToken cancellationToken = default) => throw new System.NotImplementedException();

        public IEnumerable<string> GetAvailableResources() => filenames;

        public int GetKerning(char left, char right)
        {
            if (skin == null)
                return 0;
            
            return skin.Font.HitCircleOverlap;
        }

        public Stream GetStream(string name)
        {
            if (filenames.Contains(name))
            {
                return filestreams[filenames.IndexOf(name)];
            }
            
            Stream file = File.OpenRead(name);
            
            filenames.Add(name);
            filestreams.Add(file);
            
            return file;
        }

        // only numbers are supported
        public bool HasGlyph(char c) => Char.IsDigit(c) && characters.Contains(c) && skin != null;

        public Task LoadFontAsync() => null;

        CharacterGlyph IResourceStore<CharacterGlyph>.Get(string name) => Get(name[0]);

        Task<CharacterGlyph> IResourceStore<CharacterGlyph>.GetAsync(string name, CancellationToken cancellationToken) => throw new System.NotImplementedException();

        
        private void LoadAllChars()
        {
            if (skin == null)
                return;

            for (char c = '0'; c <= '9'; c++)
            {
                Get(c);
            }
        }

        private string GetPath(string name)
        {
            return "Skins/" + skin.DirectoryName + "/" + skin.Font.HitCirclePrefix + "-" + GetChar(name) + ".png";
        }

        private int GetScaleOf(string name)
        {
            if (!Char.IsDigit(GetChar(name)))
                return 1;

            string fullName = GetPath(name);
            if (TextureUtil.HasBiggerResolution(fullName).Equals(fullName))
                return 1;
            
            return 2;
        }

        private char GetChar(string str)
        {
            if (str.StartsWith(FontName + "/"))
                return (str.Substring(FontName.Length + 1))[0];
            else
                return str[0];
        }

        private void ClearCached()
        {
            Dispose();
            filestreams.Clear();
            filenames.Clear();
            textures.Clear();
            characters.Clear();
            characterInfo.Clear();
        }
    }
}