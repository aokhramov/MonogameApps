namespace TestGame.Managers
{
    using lib;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Screens;

    public class GameManager
    {
        //TODO убрать нагромаждения отсюда в item, aliveobject и screenmanager
        public MainMenu mainMenu;

        public Map level;
        public Camera camera;

        public Texture2D frame;
       
        int i = 0;
        int j = 0;
        string state;

        public GameManager()
        {
            //ScreenManager.ActiveScreen = Screens.ScreenType.MainMenu;

            mainMenu = new MainMenu();
            
            //отображаем карту сразу
            ScreenManager.ActiveScreen = ScreenType.Map;
            //Главное меню пока отображает чушь

            level = new Map("TestMap");
            camera = new Camera(ScreenManager.Width,ScreenManager.Height);
            
            camera.Tracking(level.Player);

            level.Player.Position = new Vector2(300, 2340);//996

            frame = TextureManager.GetTexture(TextureType.Images, "frame");
        }

        public void Update(GameTime gameTime)
        {
            InputManager.Update();

            camera.Update();
            //level.Update(gameTime, camera);
            i++;

            if (i > 100 && i < 5000)
            {
                state = "Темнеет";
                j++;
                if(j == 100)
                { 
                    level.WorldColor = new Color(level.WorldColor.R - 5, level.WorldColor.G - 5, level.WorldColor.B - 5);
                    j = 0;
                }
            }

            if (i > 5000)
            {
                state = "Светлеет";
                j++;
                if (j == 100)
                {
                    level.WorldColor = new Color(level.WorldColor.R + 5, level.WorldColor.G + 5, level.WorldColor.B + 5);
                    j = 0;
                }
            }
            if (i > 10000)
                i = 0;
            level.UInterface.State = state;

            ScreenManager.Update(gameTime, camera);
            ScreenManager.ProcessingOfClicks(gameTime, camera);
        }
        public void Draw(GameTime gameTime)
        {
            ScreenManager.Draw(gameTime, camera);

            ScreenManager.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, ScreenManager.viewMatrix);
            ScreenManager.SpriteBatch.Draw(frame,
                level.CellPosition ((int)((camera.Position.X + InputManager.MouseStates.X/camera.CameraMultiplier) / TextureManager.TileSize),
               (int)((camera.Position.Y + InputManager.MouseStates.Y/camera.CameraMultiplier) / TextureManager.TileSize)),
               null, Color.Aquamarine, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);

            ScreenManager.SpriteBatch.End();
        }
    }
}
