using osu.Framework.Graphics;
using noobOsu.Game.UI.Cursor;
using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.Containers;
using osuTK.Input;

namespace noobOsu.Game.UI
{
    public partial class GameCursor : CursorAdapter
    {
        private Sprite cursorContainer, cursorOverlayContainer;

        public GameCursor()
        {
            Scale = new osuTK.Vector2(0.2f);
        }

        [BackgroundDependencyLoader]
        protected void load(TextureStore textures)
        {
            InternalChildren = new Drawable[]
            {
                cursorContainer = new Sprite(){
                    Texture = textures.Get(@"cursor"),
                },
                cursorOverlayContainer = new Sprite(){
                    Texture = textures.Get(@"cursor_overlay"),
                    Alpha = 0f,
                },
            };
        }

        public override void ButtonPress(MouseButton button)
        {
            base.ButtonPress(button);
            cursorOverlayContainer.FadeInFromZero(100);
        }

        public override void ButtonRelease(MouseButton button)
        {
            base.ButtonRelease(button);
            cursorOverlayContainer.FadeOutFromOne(100);
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

        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            cursor?.Moved(e);
            return base.OnMouseMove(e);
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            cursor?.ButtonPress(e.Button);
            return base.OnMouseDown(e);
        }
        
        protected override void OnMouseUp(MouseUpEvent e)
        {
            cursor?.ButtonRelease(e.Button);
            base.OnMouseUp(e);
        }

        protected override Drawable CreateCursor() {
            cursor = new GameCursor();
            return (Drawable)cursor;
        }
    }
}