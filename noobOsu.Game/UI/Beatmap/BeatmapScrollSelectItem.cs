using osuTK.Graphics;
using noobOsu.Game.Beatmaps;
using noobOsu.Game.UI.Basic;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Containers;

namespace noobOsu.Game.UI.Beatmap
{
    public partial class BeatmapScrollSelectItem : BasicScrollSelectItem<BeatmapSet>
    {
        private BeatmapScrollSelectItemHeader header;
        private SpriteText text;
        private Box box;

        public BeatmapScrollSelectItem(string beatmappath, BeatmapScrollSelect parent) : base(string.Empty, null, parent)
        {
            Width = 100;
            Height = 50;

            Content.Add(header = new BeatmapScrollSelectItemHeader(this));
            Content.Add(
                box = new Box()
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.BlueViolet,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Alpha = 0.5f,
                }
            );
            Content.Add(
                text = new SpriteText(){
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Font = FontUsage.Default.With(size: 20),
                }
            );

            BeatmapSet b = new BeatmapSet();
            b.ReadSetInfo(beatmappath);
            SetValue(b);
            SetName(b.SetName);
            text.Text = b.SetInfo.SongName;
        }

        protected override bool OnClick(ClickEvent e)
        {
            if (e.ControlPressed)
            {
                osu.Framework.Logging.Logger.Log("starting map");
                // todo: implement
            }
            return base.OnClick(e);
        }

        protected override void ItemStateChanged(ScrollSelectItemState newstate)
        {
            switch (newstate)
            {
                case ScrollSelectItemState.Active:
                    box.Alpha = 1f;
                    break;
                
                case ScrollSelectItemState.Inactive:
                    box.Alpha = 0.5f;
                    break;

                default:
                    break;
            }
        }
    }

    public partial class BeatmapScrollSelectItemHeader : CompositeDrawable
    {
        private BeatmapScrollSelectItem ParentItem;

        public BeatmapScrollSelectItemHeader(BeatmapScrollSelectItem parent) : base()
        {
            ParentItem = parent;
        }
    }
}