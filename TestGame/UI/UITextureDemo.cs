namespace TestGame.UI
{
    using lib;
    using Managers;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class UITextureDemo : UIElement
    {
        private Texture2D CurrentTexture;
        public string TextureTypeName { get; set; }
        private Texture2D Default = TextureManager.GetTexture(TextureType.UI, "default");
        private UIFont Font;
        private Rectangle textureDemoArea;

        public UITextureDemo(TextureType type, string mapEntryTextureName) 
            : this(type, mapEntryTextureName, new Vector2(0,0),Color.Wheat) { }

        public UITextureDemo(TextureType type, string mapEntryTextureName, Vector2 position) 
            : this(type, mapEntryTextureName, position, Color.Wheat) { }

        public UITextureDemo(TextureType type, string mapEntryTextureName, Vector2 position, Color color)
            : base(position, color)
        {
            Width = 80;
            Height = 60;
            CurrentTexture = TextureManager.GetTexture(type, mapEntryTextureName);
            TextureTypeName = mapEntryTextureName;
            Font = new UIFont(mapEntryTextureName);
            Font.Size = 10;
        }

        protected override void ComputeElement()
        {

            Font.Position = new Vector2(Position.X + Width/2 - Font.Width/2, Position.Y + Height - 15);

            textureDemoArea = new Rectangle((int)Position.X, (int)Position.Y, (int)Width, (int)Height);
            base.ComputeElement();
        }

        public override void ProcessingOfClicks(GameTime gameTime)
        {
            if (InputManager.MouseStates.LeftButton == ButtonState.Pressed &&
                InputManager.LastMouseStates.LeftButton == ButtonState.Released &&
                InputManager.MouseStates.X >= Position.X &&
                InputManager.MouseStates.Y >= Position.Y &&
                InputManager.MouseStates.X <= (Position.X + textureDemoArea.Width) &&
                InputManager.MouseStates.Y <= (Position.Y + textureDemoArea.Height))
            {
                Clicked = true;
            }
                base.ProcessingOfClicks(gameTime);
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
            Rectangle tmp = new Rectangle(0, 0, 24, 24);

            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(Default, textureDemoArea, Color * Alpha);
            ScreenManager.SpriteBatch.Draw(CurrentTexture, new Vector2((Position.X + Width/2) - TextureManager.TileSize/2 , Position.Y + 10), tmp, Color.White * Alpha, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
            ScreenManager.SpriteBatch.End();

            Font.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
