namespace TestGame.Screens
{
    using Managers;
    using Microsoft.Xna.Framework;
    using System.Collections.Generic;
    using UI;
    using lib;

    public class MapEditorMenu : Screen
    {
        private List<UIButton> menuEntries = new List<UIButton>();
        private Vector2 positionOffset;
        public Vector2 Position { get; set; }
        private int _width = 400;
        public bool Save { get; set; } = false;

        public MapEditorMenu() : base ()
        {
            menuEntries.Add(new UIButton(new UIFont("Продолжить / Return")));
            menuEntries[menuEntries.Count - 1].Click += Return;

            menuEntries.Add(new UIButton(new UIFont("Сохранить карту / Save Map")));
            menuEntries[menuEntries.Count - 1].Click += SaveMap;

            menuEntries.Add(new UIButton(new UIFont("Выйти в главное меню / Main Menu")));
            menuEntries[menuEntries.Count - 1].Click += CloseMapEditor;

            ScreenType = ScreenType.MapEditorMenu;

            foreach ( UIButton button in menuEntries)
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

        // TODO: доделать 
        private void Return(object sender)
        {
            System.Console.WriteLine("return event");
            IsActive = false;
        }

        private void SaveMap(object sender)
        {
            Save = true;
            System.Console.WriteLine("savemap click event");
        }

        private void CloseMapEditor(object sender)
        {
            ScreenManager.ActiveScreen = ScreenType.MainMenu;
        }
    }
}
