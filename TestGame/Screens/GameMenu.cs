namespace TestGame.Screens
{
    using Managers;
    using Microsoft.Xna.Framework;
    using System.Collections.Generic;
    using UI;
    using lib;

    public class GameMenu : Screen
    {
        private List<UIButton> menuEntries = new List<UIButton>();
        private Vector2 positionOffset;
        public Vector2 Position { get; set; }

        public GameMenu() : base ()
        {
            UIButton returnGame = new UIButton(new UIFont("Продолжить"));
            UIButton saveGame = new UIButton(new UIFont("Сохранить игру"));
            UIButton loadGame = new UIButton(new UIFont("Загрузить игру"));
            UIButton gameOptions = new UIButton(new UIFont("Настройки"));
            UIButton exitGame = new UIButton(new UIFont("Выйти"));

            returnGame.Click += ReturnGame;
            saveGame.Click += SaveGame;
            loadGame.Click += LoadGame;
            gameOptions.Click += GameOptions;
            exitGame.Click += ExitGame;
            returnGame.Font.Color = Color.Wheat;
            returnGame.Font.Type = FontType.RegularWithBorder;

            returnGame.Width = 300;
            saveGame.Width = 300;
            loadGame.Width = 300;
            gameOptions.Width = 300;
            exitGame.Width = 300;

            menuEntries.Add(returnGame);
            menuEntries.Add(saveGame);
            menuEntries.Add(loadGame);
            menuEntries.Add(gameOptions);
            menuEntries.Add(exitGame);

            ScreenType = ScreenType.GameMenu;
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
            base.Update(gameTime, camera);
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
            base.Draw(gameTime, camera);
        }

        // TODO: доделать игровое меню
        private void ReturnGame(object sender)
        {
            System.Console.WriteLine("return game click event");
            IsActive = false;
        }
        private void SaveGame(object sender)
        {
            System.Console.WriteLine("save game click event");
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
