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

        public bool Save { get; set; } = false;

        public MapEditorMenu() : base ()
        {
            menuEntries.Add(new UIButton(new UIFont("Продолжить")));
            menuEntries[0].Click += Return;

            menuEntries.Add(new UIButton(new UIFont("Сохранить карту")));
            menuEntries[1].Click += SaveMap;

            menuEntries.Add(new UIButton(new UIFont("Выход")));
            menuEntries[2].Click += CloseMapEditor;

            ScreenType = ScreenType.MapEditorMenu;
            Position = new Vector2(500, 200);
            positionOffset = new Vector2(0, 50);
            foreach ( UIButton button in menuEntries)
            {
                button.Width = 300;
                button.Color = Color.BlueViolet;
                button.Font.Color = Color.AntiqueWhite;
                button.Font.Type = FontType.RegularWithShadow;
            }
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
            System.Console.WriteLine("close mapeditor event");
            ScreenManager.InitiateGameExitMode = true;
        }
    }
}
