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
        private int _width = 400;

        public GameMenu() : base ()
        {
            menuEntries.Add(new UIButton(new UIFont("Продолжить / Return")));
            menuEntries[menuEntries.Count - 1].Click += ReturnGame;

            //menuEntries.Add(new UIButton(new UIFont("Сохранить игру")));
            //menuEntries[menuEntries.Count - 1].Click += SaveGame;

            //menuEntries.Add(new UIButton(new UIFont("Загрузить игру")));
            //menuEntries[menuEntries.Count - 1].Click += LoadGame;

            menuEntries.Add(new UIButton(new UIFont("Настройки / Options")));
            menuEntries[menuEntries.Count - 1].Click += GameOptions;

            menuEntries.Add(new UIButton(new UIFont("Выйти в главное меню / Main Menu")));
            menuEntries[menuEntries.Count - 1].Click += Exit;

            ScreenType = ScreenType.GameMenu;

            foreach (UIButton button in menuEntries)
            {
                button.Width = _width;
                button.Color = Color.BlueViolet;
                button.Font.Color = Color.AntiqueWhite;
                button.Font.Type = FontType.RegularWithShadow;
            }
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

            Position = new Vector2(ScreenManager.Width / 2 - _width / 2, ScreenManager.Height /2 - menuEntries.Count * 30);
            Vector2 pos = Position;
            foreach (UIButton button in menuEntries)
            {
                button.Position = pos;
                button.Update(gameTime);
                pos += positionOffset;
            }
            base.Update(gameTime, camera);
        }

        public override void Draw(GameTime gameTime, Camera camera)
        {
            if (!IsActive)
                return;

            foreach (UIButton button in menuEntries)
            {
                button.Draw(gameTime);
            }
            base.Draw(gameTime, camera);
        }

        // TODO: доделать игровое меню
        private void ReturnGame(object sender)
        {
            System.Console.WriteLine("return game click event");
            IsActive = false;
        }

        private void GameOptions(object sender)
        {
            System.Console.WriteLine("TODO gameoptions");
        }

        private void Exit(object sender)
        {
            ScreenManager.ActiveScreen = ScreenType.MainMenu;
        }
    }
}
