namespace TestGame
{
    using Managers;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class TestGame : Game
    {
        GraphicsDeviceManager graphics;
        GameManager gameManager;
        
        public TestGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            ScreenManager.Content = Content;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;

            ScreenManager.Width = graphics.PreferredBackBufferWidth;
            ScreenManager.Height = graphics.PreferredBackBufferHeight;

            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            ScreenManager.SpriteBatch = new SpriteBatch(GraphicsDevice);
            gameManager = new GameManager();
            TextureManager.ChangeCountTiles(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            gameManager.Update(gameTime);

            base.Update(gameTime);
            if (ScreenManager.InitiateGameExitMode)
            {
                System.Console.WriteLine("close game event");
                Exit();
            }
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(20, 20, 20));

            gameManager.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
