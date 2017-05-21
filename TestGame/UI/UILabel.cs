namespace TestGame.UI
{
    using Managers;
    using Microsoft.Xna.Framework;
    using lib;

    public class UILabel : UIElement
    {
        #region Fields and Properties
        //-------------------------------------------------
        /// <summary>
        /// Текст
        /// </summary>
        public UIFont Font { get; set; }
        private Rectangle labelArea;
        private string lasttext;
        private ContentAlignment textAlignment = ContentAlignment.BottomLeft;
        /// <summary>
        /// Положение текста 
        /// </summary>
        public ContentAlignment TextAlignment
        {
            get { return textAlignment; }
            set
            {
                textAlignment = value;
                Compute = true;
            }
        }

        private int padding = 0;
        /// <summary>
        /// Внутренняя граница
        /// </summary>
        public int Padding
        {
            get { return padding; }
            set
            {
                padding = value;
                if (padding < 0)
                    padding = 0;
                Compute = true;
            }
        }
        //-------------------------------------------------
        #endregion

        #region Constructors
        //-------------------------------------------------
        public UILabel()
            : this(new UIFont(""), new Vector2(0, 0), Color.White) {}

        public UILabel(UIFont font)
            : this(font, new Vector2(0, 0), Color.White) {}

        public UILabel(UIFont font, Vector2 position)
            : this(font, position, Color.White) {}

        public UILabel(UIFont font, Vector2 position,Color color)
            : base(position, color)
        {
            Font = font;
            Texture = TextureManager.GetTexture(TextureType.UI, "default");
        }
        //-------------------------------------------------
        #endregion

        #region Methods
        //-------------------------------------------------
        protected override void ComputeElement()
        {
            if (Init)
            { 
                Width = Font.Width + padding * 2;
                Height = Font.Height + padding * 2;
            }
            if (Width < Font.Width + padding*2)
                Width = Font.Width + padding*2;
            if (Height < Font.Height + padding*2)
                Height = Font.Height + padding*2;

            Font.Position = Position + new Vector2(padding, padding);

            labelArea = new Rectangle((int)Position.X, (int)Position.Y, (int)Width, (int)Height);
            base.ComputeElement();
        }
        public override void Update(GameTime gameTime)
        {
            if (Animation != AnimationType.None)
                Font.Animation = Animation;

            Font.Update(gameTime);

            if ( Compute )
            {
                ComputeElement();
            }
            if(lasttext != Font.Text)
            {
                Compute = true;
                lasttext = Font.Text;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(Texture, labelArea, Color * Alpha);
            ScreenManager.SpriteBatch.End();

            Font.Draw(gameTime);
            base.Draw(gameTime);
        }
        //-------------------------------------------------
        #endregion
    }
}
