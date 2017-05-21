namespace TestGame.UI
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Managers;
    using lib;

    public class UIButton : UIElement
    {
        #region Fields and Properties
        //-------------------------------------------------
        /// <summary>
        /// Текст кнопки
        /// </summary>
        public UIFont Font { get; set; }

        private Rectangle buttonArea;
        // TODO: привести в порядок после того как доделаешь TextureManager
        private Texture2D CornerLT = TextureManager.GetTexture(TextureType.UI, "CornerLT");
        private Texture2D CornerLB = TextureManager.GetTexture(TextureType.UI, "CornerLB");
        private Texture2D CornerRT = TextureManager.GetTexture(TextureType.UI, "CornerRT");
        private Texture2D CornerRB = TextureManager.GetTexture(TextureType.UI, "CornerRB");

        private Texture2D BorderT = TextureManager.GetTexture(TextureType.UI, "BorderT");
        private Texture2D BorderB = TextureManager.GetTexture(TextureType.UI, "BorderB");
        private Texture2D BorderL = TextureManager.GetTexture(TextureType.UI, "BorderL");
        private Texture2D BorderR = TextureManager.GetTexture(TextureType.UI, "BorderR");

        private Texture2D Default = TextureManager.GetTexture(TextureType.UI, "default");

        public delegate void EventHandler(object sender);
        /// <summary>
        /// Событие по клику на кнопку
        /// </summary>
        public event EventHandler Click;

        public ContentAlignment TextAlignment { get; set; } = ContentAlignment.MiddleCenter;
        //-------------------------------------------------
        #endregion

        #region Constructors
        //-------------------------------------------------
        public UIButton () 
            : this(new UIFont(""),new Vector2(0,0),Color.White) {}

        public UIButton(UIFont font) 
            : this(font, new Vector2(0, 0), Color.White) {}

        public UIButton(UIFont font, Vector2 position) 
            : this(font, position, Color.White) {}

        public UIButton(UIFont font, Vector2 position, Color color)
            : base(position, color)
        {
            Font = font;
        }
        //-------------------------------------------------
        #endregion

        #region Methods
        //-------------------------------------------------
        protected override void ComputeElement()
        {
            if (Init)
            {
                if ( Width == 0f)
                    Width = Font.Width + BorderL.Width*2 + BorderR.Width*2;
                if ( Height == 0f)
                    Height = Font.Height + BorderT.Height*3 + BorderB.Height*3;
            }
            if (Width < Font.Width + BorderL.Width * 2 + BorderR.Width * 2)
                Width = Font.Width + BorderL.Width * 2 + BorderR.Width * 2;
            if (Height < Font.Height + BorderT.Height*3  + BorderB.Height*3 )
                Height = Font.Height + BorderT.Height*3  + BorderB.Height*3 ;

            Width = (int)Width;
            Height = (int)Height;

            while (Width % Default.Width != 0)
                Width++;
            while (Height % Default.Height != 0)
                Height++;

            Font.Position = FontPosition();

            buttonArea = new Rectangle((int)Position.X, (int)Position.Y,(int)Width, (int)Height);

            base.ComputeElement();
        }

        public override void Update(GameTime gameTime)
        {
            if (Animation != Font.Animation)
                Font.Animation = Animation;

            Font.Update(gameTime);

            if (Compute)
            {
                ComputeElement();
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            // TODO: привести в более читабельный вид
            ScreenManager.SpriteBatch.Begin();

            //corners
            ScreenManager.SpriteBatch.Draw(CornerLT, Position,
                null, Color * Alpha, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);//lefttop
            ScreenManager.SpriteBatch.Draw(CornerRT, Position + new Vector2(Width-4,0),
                null, Color * Alpha, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);//righttop
            ScreenManager.SpriteBatch.Draw(CornerLB, Position + new Vector2(0, Height-4),
                null, Color * Alpha, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);//leftbot
            ScreenManager.SpriteBatch.Draw(CornerRB, Position + new Vector2(Width-4, Height-4),
                null, Color * Alpha, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);//rightbot

            Vector2 pos = Position;

            ScreenManager.SpriteBatch.Draw(Default,
                new Rectangle((int)pos.X + Default.Width, (int)pos.Y + Default.Height,
                (int)Width - CornerRT.Width - Default.Width,
                (int)Height - CornerRB.Height - Default.Height),
                Color * Alpha);

            pos = Position+ new Vector2(CornerLT.Width,0);
            //left to right
            ScreenManager.SpriteBatch.Draw(BorderT,
                new Rectangle((int)pos.X, (int)pos.Y,
                (int)Width - CornerRT.Width - Default.Width,
                BorderT.Height), Color * Alpha);

            ScreenManager.SpriteBatch.Draw(BorderB,
                new Rectangle((int)pos.X, (int)(pos.Y + Height - BorderB.Height),
                (int)(Width - CornerRT.Width - Default.Width),
                BorderB.Height), Color * Alpha);
            //top to bot
            pos = Position + new Vector2(0, CornerLT.Height);

            ScreenManager.SpriteBatch.Draw(BorderL,
                new Rectangle((int)pos.X, (int)pos.Y,
                BorderL.Width,
                (int)(Height - BorderL.Height - Default.Height)), Color * Alpha);

            ScreenManager.SpriteBatch.Draw(BorderR,
                new Rectangle((int)(pos.X + Width - CornerRT.Width), (int)pos.Y,
                BorderR.Width,
                (int)(Height - BorderR.Height - Default.Height)), Color * Alpha);

            ScreenManager.SpriteBatch.End();

            Font.Draw(gameTime);
            base.Draw(gameTime);
        }

        /// <summary>
        /// Определяем нажата ли кнопка
        /// </summary>
        /// <param name="gameTime"></param>
        public override void ProcessingOfClicks(GameTime gameTime)
        {
            if (InputManager.MouseStates.LeftButton == ButtonState.Pressed &&
                InputManager.LastMouseStates.LeftButton == ButtonState.Released &&
                InputManager.MouseStates.X >= Position.X &&
                InputManager.MouseStates.Y >= Position.Y &&
                InputManager.MouseStates.X <= (Position.X + buttonArea.Width) &&
                InputManager.MouseStates.Y <= (Position.Y + buttonArea.Height))
            {
                System.Console.WriteLine("collide event");
                if (Click != null)
                    Click(this);
                // TODO: убрать! (для теста анимации)
                if(Animation != AnimationType.ShortFlashing)
                    Animation = AnimationType.ShortFlashing;
            }
            base.ProcessingOfClicks(gameTime);
        }

        /// <summary>
        /// Позиция текста относительно кнопки
        /// </summary>
        /// <returns>Позиция</returns>
        private Vector2 FontPosition()
        {
            // TODO: позиция текста в кнопке
            Vector2 pos = Position + new Vector2(CornerLB.Width * 2, (Height / 2) - (Font.Height / 2)); ;
            switch(TextAlignment)
            {
                case ContentAlignment.BottomCenter:
                    break;
                case ContentAlignment.BottomLeft:
                    break;
                case ContentAlignment.BottomRight:
                    break;
                case ContentAlignment.MiddleCenter:
                    pos = Position + new Vector2((Width / 2) - (Font.Width / 2), (Height / 2) - (Font.Height / 2));
                    break;
                case ContentAlignment.MiddleLeft:
                    break;
                case ContentAlignment.MiddleRight:
                    break;
                case ContentAlignment.TopCenter:
                    break;
                case ContentAlignment.TopLeft:
                    break;
                case ContentAlignment.TopRight:
                    break;
            }
            return pos;
        }
        //-------------------------------------------------
        #endregion
    }
}
