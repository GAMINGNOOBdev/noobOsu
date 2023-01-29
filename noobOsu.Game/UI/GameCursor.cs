using osu.Framework.Graphics;
using noobOsu.Game.UI.Cursor;
using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.Containers;

namespace noobOsu.Game.UI
{
    public partial class GameCursor : Container, ICursor
    {
        private Sprite cursorContainer;

        public GameCursor()
        {
            Scale = new osuTK.Vector2(0.2f);
        }

        [BackgroundDependencyLoader]
        protected void load(TextureStore textures)
        {
            InternalChild = cursorContainer = new Sprite(){
                Texture = textures.Get(@"cursor"),
            };
        }
    }

    public partial class GameCursorContainer : CursorContainer
    {
        private ICursor cursor;

        public bool DrawCursor{
            get => State.Value == Visibility.Visible;

            set
            {
                if (value)
                    State.Value = Visibility.Visible;
                else
                    State.Value = Visibility.Hidden;
            }
        }

        public GameCursorContainer()
        {
            State.Value = Visibility.Hidden;
        }

        protected override void Update()
        {
            base.Update();

            if (State.Value != Visibility.Hidden)
                cursor?.Show();
            else
                cursor?.Hide();
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            return base.OnMouseDown(e);
        }
        
        protected override void OnMouseUp(MouseUpEvent e)
        {
            base.OnMouseUp(e);
        }

        protected override Drawable CreateCursor() {
            cursor = new GameCursor();
            return (Drawable)cursor;
        }
    }
}