using osuTK;
using osuTK.Graphics;
using noobOsu.Game.Beatmaps;
using noobOsu.Game.UI.Basic;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using System.Collections.Generic;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Containers;

namespace noobOsu.Game.UI.Beatmap
{
    public partial class BeatmapScrollSelectItem : BasicScrollSelectItem<BeatmapSet>
    {
        private BeatmapScrollSelectItemContent LastItem;
        private BeatmapScrollSelectItemHeader header;
        
        public BeatmapScrollSelectItem(string beatmappath, BeatmapScrollSelect parent) : base(string.Empty, null, parent)
        {
            Width = 100;
            Height = 50;

            SizeY = 50;

            BeatmapSet b = new BeatmapSet();
            b.ReadSetInfo(beatmappath);
            SetValue(b);
            SetName(b.SetName);

            Content.Add(header = new BeatmapScrollSelectItemHeader(this));
        }

        protected override bool OnClick(ClickEvent e)
        {
            return base.OnClick(e);
        }

        protected override bool OnHover(HoverEvent e)
        {
            header.Hovered = true;
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            header.Hovered = false;
            base.OnHoverLost(e);
        }

        public void InsertItemContent(BeatmapScrollSelectItemContent content)
        {
            AddInternal(content);
            SizeY += BasicScrollSelect<object>.ITEM_SPACING + content.SizeY;
            ParentSelect.InvalidateItems();
        }

        public void SetSelectedItem(BeatmapScrollSelectItemContent item)
        {
            if (LastItem != item && LastItem != null)
                LastItem.Deselect();
            
            LastItem = item;
        }

        public void RemoveItemContent(BeatmapScrollSelectItemContent content)
        {
            RemoveInternal(content, false);
            SizeY -= BasicScrollSelect<object>.ITEM_SPACING + content.SizeY;
            ParentSelect.InvalidateItems();
        }

        protected override void ItemStateChanged(ScrollSelectItemState newstate)
        {
            switch (newstate)
            {
                case ScrollSelectItemState.Active:
                    header.Select();
                    break;
                
                case ScrollSelectItemState.Inactive:
                    header.Deselect();
                    break;

                default:
                    break;
            }
        }
    }

    public partial class BeatmapScrollSelectItemHeader : CompositeDrawable
    {
        private List<BeatmapScrollSelectItemContent> ItemContents = new List<BeatmapScrollSelectItemContent>();
        private SpriteText text;
        private bool addedItems;
        private Box box;

        public bool Hovered
        {
            get => false;
            set{
                if (value)
                {
                    box.Colour = Color4.DarkBlue;
                    this.MoveToX(10, 200);
                }
                else
                {
                    box.Colour = Color4.BlueViolet;
                    this.MoveToX(20, 200);
                }
                
                if (box.Alpha == 1.0f)
                    this.MoveToX(0, 200);
            }
        }

        public BeatmapScrollSelectItem ParentItem;

        public BeatmapScrollSelectItemHeader(BeatmapScrollSelectItem parent) : base()
        {
            ParentItem = parent;
            addedItems = false;
            X = 20;

            RelativeSizeAxes = Axes.Both;

            int yOffset = 60;
            foreach(IBeatmapGeneral map in ParentItem.Value.GetBeatmaps())
            {
                ItemContents.Add( new BeatmapScrollSelectItemContent(this, map){ Y = yOffset } );
                yOffset += BasicScrollSelect<object>.ITEM_SPACING + 50;
            }

            box = new Box()
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Color4.BlueViolet,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Alpha = 0.5f,
            };
            text = new SpriteText(){
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Font = FontUsage.Default.With(size: 20),
                Text = ParentItem.Value.SetInfo.SongName,
            };

            AddInternal(box);
            AddInternal(text);
        }

        public void Select()
        {
            box.Alpha = 1.0f;

            if (!addedItems)
            {
                foreach(BeatmapScrollSelectItemContent map in ItemContents)
                    ParentItem.InsertItemContent(map);
            }
            this.MoveToX(0, 200);

            addedItems = true;
        }

        public void Deselect()
        {
            box.Alpha = 0.5f;
            if (addedItems)
            {
                foreach(BeatmapScrollSelectItemContent map in ItemContents)
                {
                    ParentItem.RemoveItemContent(map);
                    map.Deselect();
                }
            }
            this.MoveToX(20, 200);

            addedItems = false;
        }
    }

    public partial class BeatmapScrollSelectItemContent : BasicScrollSelectItem<IBeatmapGeneral>
    {
        private BeatmapScrollSelectItemHeader ParentItemHeader;
        private IBeatmapGeneral Map;
        private SpriteText text;
        private Box box;

        public BeatmapScrollSelectItemContent(BeatmapScrollSelectItemHeader parentHeader, IBeatmapGeneral map) : base(null, map, null)
        {
            Map = map;
            Position = new Vector2(70, 0);
            ParentItemHeader = parentHeader;
            SizeY = ParentItemHeader.ParentItem.SizeY;

            RelativeSizeAxes = Axes.Both;

            box = new Box()
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Color4.Blue,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Alpha = 0.5f,
            };
            text = new SpriteText(){
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Font = FontUsage.Default.With(size: 20),
                Text = Map.SongName + " [" + Map.DifficultyName + "]",
            };

            AddInternal(box);
            AddInternal(text);
        }

        protected override bool OnClick(ClickEvent e)
        {
            if (State == ScrollSelectItemState.Selected)
            {
                Screens.MapSelectScreen.INSTANCE.LoadMap(Value, e.ControlPressed);
            }
            return base.OnClick(e);
        }

        protected override bool OnHover(HoverEvent e)
        {
            this.MoveTo(new Vector2(20, Position.Y), 100);
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            base.OnHoverLost(e);
            this.MoveTo(new Vector2(70, Position.Y), 100);
        }

        protected override void ItemStateChanged(ScrollSelectItemState newstate)
        {
            switch (newstate)
            {
                case ScrollSelectItemState.Active:
                    ParentItemHeader.ParentItem.SetSelectedItem(this);
                    box.Alpha = 1.0f;
                    break;
                
                case ScrollSelectItemState.Inactive:
                    box.Alpha = 0.5f;
                    break;

                default:
                    break;
            }
        }
    }
}