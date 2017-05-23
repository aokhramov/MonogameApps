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
        private int _width = 400;

        public MainMenu() : base()
        {
            menuEntries.Add(new UIButton(new UIFont("Загрузить карту / Load Map")));
            menuEntries[menuEntries.Count - 1].Click += LoadMap;

            menuEntries.Add(new UIButton(new UIFont("Редактор карт / Map Editor")));
            menuEntries[menuEntries.Count - 1].Click += MapEditor;

            menuEntries.Add(new UIButton(new UIFont("Настройки / Options")));
            menuEntries[menuEntries.Count - 1].Click += GameOptions;

            menuEntries.Add(new UIButton(new UIFont("Выход / Exit")));
            menuEntries[menuEntries.Count - 1].Click += ExitGame;


            ScreenType = ScreenType.MainMenu;

            foreach (UIButton button in menuEntries)
            {
                button.Width = _width;
                button.Color = Color.BlueViolet;
                button.Font.Color = Color.AntiqueWhite;
                button.Font.Type = FontType.RegularWithShadow;
            }
            positionOffset = new Vector2(0, 50);
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

            Position = new Vector2(ScreenManager.Width / 2 - _width / 2, ScreenManager.Height / 2 - menuEntries.Count * 30);
            Vector2 pos = Position;
            foreach (UIButton button in menuEntries)
            {
                button.Position = pos;
                button.Update(gameTime);
                pos += positionOffset;
            }
        }

        public override void Draw(GameTime gameTime, Camera camera)
        {
            if (!IsActive)
                return;

            foreach (UIButton button in menuEntries)
            {
                button.Draw(gameTime);
            }
        }

        // TODO: доделать главное меню
        private void LoadMap(object sender)
        {
            ScreenManager.ActiveScreen = ScreenType.Map;
        }

        private void MapEditor(object sender)
        {
            ScreenManager.ActiveScreen = ScreenType.MapEditor;
        }

        private void GameOptions(object sender)
        {
            System.Console.WriteLine("TODO game options");
        }

        private void ExitGame(object sender)
        {
            ScreenManager.InitiateGameExitMode = true;
        }
    }
}
