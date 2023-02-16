using osuTK;
using osuTK.Input;
using noobOsu.Game.UI.Cursor;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.Containers;
using noobOsu.Game.Skins.Properties;

namespace noobOsu.Game.Skins.Drawables
{
    public partial class SkinCursor : CursorAdapter
    {
        private SkinnableSprite cursor, cursorMiddle;
        //private SkinCursorTrail cursorTrail;

        public SkinCursor()
        {
            InternalChildren = new Drawable[]{
                cursor = new SkinnableSprite(){
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(64f),
                    Scale = new Vector2(1f),
                },
                cursorMiddle = new SkinnableSprite(){
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(64f),
                    Scale = new Vector2(1f),
                },
                /*cursorTrail = new SkinCursorTrail(){
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(64f),
                    Scale = new Vector2(1f),
                    Alpha = 0,
                },*/
            };

            AddProperty(
                new SkinnableCursorProperty(cursor, "cursor"){
                    Scaleable = false
                }
            );
            AddProperty(
                new SkinnableCursorProperty(cursorMiddle, "cursormiddle"){
                    Scaleable = false
                }
            );
            //AddProperty(cursorTrail);

            State = CursorState.Normal;
        }

        public override void UpdateCursor()
        {
            base.UpdateCursor();
        }

        public override void ButtonPress(MouseButton button)
        {
            base.ButtonPress(button);
        }

        public override void ButtonRelease(MouseButton button)
        {
            base.ButtonRelease(button);
        }
    }

    public partial class SkinCursorContainer : CursorContainer
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

        public SkinCursorContainer()
        {
            State.Value = Visibility.Hidden;
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

        protected override void Update()
        {
            base.Update();

            cursor?.UpdateCursor();
        }
        
        public void SwitchSkin(ISkin newSkin, TextureStore textures)
        {
            if (newSkin is DefaultSkin)
                return;
            cursor?.ResetResolvedProperties();
            newSkin.ResolveSkinnables(new []{ cursor }, textures);
        }

        protected override Drawable CreateCursor() {
            cursor = new SkinCursor();
            return (Drawable)cursor;
        }
    }
}