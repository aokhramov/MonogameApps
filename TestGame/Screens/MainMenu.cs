namespace TestGame.Screens
{
    using Managers;
    using Microsoft.Xna.Framework;
    using System.Collections.Generic;
    using UI;
    using lib;

    public class MainMenu : Screen
    {
        private List<UIButton> menuEntries = new List<UIButton>();
        private Vector2 positionOffset;
        public Vector2 Position { get; set; }
        
        public MainMenu() : base()
        {
            UIButton newGame = new UIButton(new UIFont("Новая игра"));
            UIButton loadGame = new UIButton(new UIFont("Загрузить игру"));
            UIButton gameOptions = new UIButton(new UIFont("Настройки"));
            UIButton exitGame = new UIButton(new UIFont("Выйти"));
            
            newGame.Click += NewGame;
            loadGame.Click += LoadGame;
            gameOptions.Click += GameOptions;
            exitGame.Click += ExitGame;

            newGame.Font.Color = Color.MediumVioletRed;
            gameOptions.Font.Type = FontType.RegularWithShadow;
            gameOptions.Font.Color = Color.White;
            loadGame.Font.Type = FontType.RegularWithBorder;
            loadGame.Font.Color = Color.SpringGreen;

            newGame.Width = 300;
            loadGame.Width = 300;
            gameOptions.Width = 300;
            exitGame.Width = 300;
            exitGame.Font.Type = FontType.RegularWithShadow;
            

            newGame.Color = Color.Wheat;
            loadGame.Color = Color.Wheat;
            gameOptions.Color = Color.Wheat;
            exitGame.Font.Color = Color.CornflowerBlue;
            menuEntries.Add(newGame);
            menuEntries.Add(loadGame);
            menuEntries.Add(gameOptions);
            menuEntries.Add(exitGame);

            ScreenType = ScreenType.MainMenu;
            Position = new Vector2(500, 200);
            positionOffset = new Vector2(0, 50);
            
            //ScreenManager.AddScreen(this);
        }

        public override void ProcessingOfClicks(GameTime gameTime, Camera camera)
        {
            if (!NeedClicksProcessing || !IsActive)
                return;

            foreach (UIButton button in menuEntries)
            {
                button.ProcessingOfClicks(gameTime);
            }
            base.ProcessingOfClicks(gameTime, camera);
        }

        public override void Update(GameTime gameTime, Camera camera)
        {
            if (!IsActive)
                return;

            foreach (UIButton button in menuEntries)
            {
                button.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime, Camera camera)
        {
            if (!IsActive)
                return;

            Vector2 pos = Position;
            foreach (UIButton mEntry in menuEntries)
            {
                mEntry.Position = pos;
                mEntry.Draw(gameTime);
                pos += positionOffset;
            }
        }

        // TODO: доделать главное меню
        private void NewGame(object sender)
        {
            System.Console.WriteLine("new game click event");
            ScreenManager.ActiveScreen = ScreenType.Map;
        }
        private void LoadGame(object sender)
        {
            System.Console.WriteLine("load game click event");
        }
        private void GameOptions(object sender)
        {
            System.Console.WriteLine("game options click event");
        }
        private void ExitGame(object sender)
        {
            System.Console.WriteLine("exit game click event");
            ScreenManager.InitiateGameExitMode = true;
        }
    }
}
