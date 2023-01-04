using System.Collections.Generic;

namespace noobOsu.Game.Skins
{
    public interface ISkinGeneral
    {
        // Name of the skin
        string Name { get; }

        // Name of the creator of this skin
        string Author { get; }

        // version of this skin (-1 if latest)
        float Version { get; }

        // animation framerate, -1 if all frames should be displayed in one second
        int AnimationFramerate { get; }

        // determines if the slider ball has the same color as the combo
        bool AllowSliderBallTint { get; }

        // specifies if combo bursts are shown in a random order
        bool ComboBurstRandom { get; }

        // true = center, false = top left
        bool CursorCentre { get; }

        // true = expand on click, false = don't expand
        bool CursorExpand { get; }

        // true = rotate cursor constantly
        bool CursorRotate { get; }

        // true = rotate cursor trail constantly
        bool CursorTrailRotate { get; }

        // specifies on which combo the bursts should dislpay
        List<int> ComboBurstSounds { get; }

        // hitcircle over number or not
        bool HitCircleOverlayAboveNumber { get; }

        // true = always play normal hitsounds
        bool LayeredHitSounds { get; }

        // true = flip sliderball on reverse
        bool SliderBallFlip { get; }

        // true = fade playfield during a spinner
        bool SpinnerFadePlayfield { get; }

        // true = pitch spinner sounds the longer you spin
        bool SpinnerFrequencyModulate { get; }
        
        // idk
        bool SpinnerNoBlink { get; }

        // parse info from a string
        void AddGeneralInfo(string info);
    }

    public class SkinGeneral : ISkinGeneral
    {
        public string Name { get; private set; } = "";
        public string Author { get; private set; } = "";
        public float Version { get; private set; } = -1;
        public int AnimationFramerate { get; private set; } = -1;
        public bool AllowSliderBallTint { get; private set; } = false;
        public bool ComboBurstRandom { get; private set; } = false;
        public bool CursorCentre { get; private set; } = true;
        public bool CursorExpand { get; private set; } = true;
        public bool CursorRotate { get; private set; } = true;
        public bool CursorTrailRotate { get; private set; } = true;
        public List<int> ComboBurstSounds { get; private set; } = new List<int>();
        public bool HitCircleOverlayAboveNumber { get; private set; } = true;
        public bool LayeredHitSounds { get; private set; } = true;
        public bool SliderBallFlip { get; private set; } = true;
        public bool SpinnerFadePlayfield { get; private set; } = false;
        public bool SpinnerFrequencyModulate { get; private set; } = true;
        public bool SpinnerNoBlink { get; private set; } = false;

        public void AddGeneralInfo(string info)
        {
            string[] splitInfo = info.Split(": ");
            
            if (splitInfo.Length < 2) return;

            if (splitInfo[0].Equals("Name"))
            {
                Name = splitInfo[1];
            }
            if (splitInfo[0].Equals("Author"))
            {
                Author = splitInfo[1];
            }
            if (splitInfo[0].Equals("Version"))
            {
                if (splitInfo[1].Equals("latest")) return;
                Version = float.Parse(splitInfo[1]);
            }
            if (splitInfo[0].Equals("AnimationFramerate"))
            {
                AnimationFramerate = int.Parse(splitInfo[1]);
            }
            if (splitInfo[0].Equals("AllowSliderBallTint"))
            {
                AllowSliderBallTint = int.Parse(splitInfo[1]) > 0;
            }
            if (splitInfo[0].Equals("ComboBurstRandom"))
            {
                ComboBurstRandom = int.Parse(splitInfo[1]) > 0;
            }
            if (splitInfo[0].Equals("CursorCentre"))
            {
                CursorCentre = int.Parse(splitInfo[1]) > 0;
            }
            if (splitInfo[0].Equals("CursorExpand"))
            {
                CursorExpand = int.Parse(splitInfo[1]) > 0;
            }
            if (splitInfo[0].Equals("CursorRotate"))
            {
                CursorRotate = int.Parse(splitInfo[1]) > 0;
            }
            if (splitInfo[0].Equals("CursorTrailRotate"))
            {
                CursorTrailRotate = int.Parse(splitInfo[1]) > 0;
            }
            if (splitInfo[0].Equals("ComboBurstSounds"))
            {
                ///TODO: -- implement --
            }
            if (splitInfo[0].Equals("HitCircleOverlayAboveNumber"))
            {
                HitCircleOverlayAboveNumber = int.Parse(splitInfo[1]) > 0;
            }
            if (splitInfo[0].Equals("LayeredHitSounds"))
            {
                LayeredHitSounds = int.Parse(splitInfo[1]) > 0;
            }
            if (splitInfo[0].Equals("SliderBallFlip"))
            {
                SliderBallFlip = int.Parse(splitInfo[1]) > 0;
            }
            if (splitInfo[0].Equals("SpinnerFadePlayfield"))
            {
                SpinnerFadePlayfield = int.Parse(splitInfo[1]) > 0;
            }
            if (splitInfo[0].Equals("SpinnerFrequencyModulate"))
            {
                SpinnerFrequencyModulate = int.Parse(splitInfo[1]) > 0;
            }
            if (splitInfo[0].Equals("SpinnerNoBlink"))
            {
                SpinnerNoBlink = int.Parse(splitInfo[1]) > 0;
            }
        }
    }
}