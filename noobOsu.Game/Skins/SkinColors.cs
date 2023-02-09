using osuTK.Graphics;

namespace noobOsu.Game.Skins
{
    public interface ISkinColors
    {
        // stores all of the combo colors
        IColorStore SkinComboColors { get; }
        
        // color of input overlay text
        Color4 InputOverlayText { get; }

        // spectrum bar color
        Color4 MenuGlow { get; }

        // slider ball color
        Color4? SliderBall { get; }

        // slider border color
        Color4 SliderBorder { get; }

        // color of the slider track, use combo color if not specified
        Color4 SliderTrackOverride { get; }

        // active song title text color
        Color4 SongSelectActiveText { get; }

        // inactive song title text color
        Color4 SongSelectInactiveText { get; }

        // spinner background color addition
        Color4 SpinnerBackground { get; }

        // color of star2 during breaks
        Color4 StarBreakAdditive { get; }

        // parse info from a string
        void AddColorInfo(string info);
        Color4? GetColorFor(string name);

        // reset the combo colors at each exit/start of a beatmap
        void ResetComboColor();
    }

    public class SkinColors : ISkinColors
    {
        public IColorStore SkinComboColors { get; private set; } = new ColorStore(){ AllowSkip = false };
        public Color4 InputOverlayText { get; set; } = new Color4(0, 0, 0, 1);
        public Color4 MenuGlow { get; set; } = new Color4(0, 78/255, 155/255, 1);
        public Color4? SliderBall { get; set; } = null;
        public Color4 SliderBorder { get; set; } = Color4.White;
        public Color4 SliderTrackOverride { get; set; }
        public Color4 SongSelectActiveText { get; set; } = Color4.Black;
        public Color4 SongSelectInactiveText { get; set; } = Color4.White;
        public Color4 SpinnerBackground { get; set; } = new Color4(100/255, 100/255, 100/255, 1);
        public Color4 StarBreakAdditive { get; set; } = new Color4(1, 182/255, 193/255, 1);

        private Skin Parent;

        public SkinColors(Skin p)
        {
            Parent = p;
        }

        public void ResetComboColor()
        {
            SkinComboColors.RestartColor();
        }

        public void AddColorInfo(string info)
        {
            string[] splitInfo = info.Split(": ");
            
            if (splitInfo.Length < 2) return;

            if (splitInfo[0].StartsWith("Combo"))
            {
                SkinComboColors.AddColor(splitInfo[1]);
            }
            if (splitInfo[0].Equals("InputOverlayText"))
            {
                InputOverlayText = IColorStore.FromString(splitInfo[1]);
            }
            if (splitInfo[0].Equals("MenuGlow"))
            {
                MenuGlow = IColorStore.FromString(splitInfo[1]);
            }
            if (splitInfo[0].Equals("SliderBall"))
            {
                SliderBall = IColorStore.FromString(splitInfo[1]);
            }
            if (splitInfo[0].Equals("SliderBorder"))
            {
                SliderBorder = IColorStore.FromString(splitInfo[1]);
            }
            if (splitInfo[0].Equals("SliderTrackOverride"))
            {
                SliderTrackOverride = IColorStore.FromString(splitInfo[1]);
            }
            if (splitInfo[0].Equals("SongSelectActiveText"))
            {
                SongSelectActiveText = IColorStore.FromString(splitInfo[1]);
            }
            if (splitInfo[0].Equals("SongSelectInactiveText"))
            {
                SongSelectInactiveText = IColorStore.FromString(splitInfo[1]);
            }
            if (splitInfo[0].Equals("SpinnerBackground"))
            {
                SpinnerBackground = IColorStore.FromString(splitInfo[1]);
            }
            if (splitInfo[0].Equals("StarBreakAdditive"))
            {
                StarBreakAdditive = IColorStore.FromString(splitInfo[1]);
            }
        }

        public Color4? GetColorFor(string name)
        {
            if (name.Equals("InputOverlayText"))
            {
                return InputOverlayText;
            }
            if (name.Equals("MenuGlow"))
            {
                return MenuGlow;
            }
            if (name.Equals("SliderBall"))
            {
                if (Parent.General.AllowSliderBallTint)
                    return SliderBall;
                else
                    return Color4.White;
            }
            if (name.Equals("SliderBorder"))
            {
                return SliderBorder;
            }
            if (name.Equals("SliderTrackOverride") || name.Equals("SliderTrack"))
            {
                return SliderTrackOverride;
            }
            if (name.Equals("SongSelectActiveText"))
            {
                return SongSelectActiveText;
            }
            if (name.Equals("SongSelectInactiveText"))
            {
                return SongSelectInactiveText;
            }
            if (name.Equals("SpinnerBackground"))
            {
                return SpinnerBackground;
            }
            if (name.Equals("StarBreakAdditive"))
            {
                return StarBreakAdditive;
            }
            return null;
        }
    }
}