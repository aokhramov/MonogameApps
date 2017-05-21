namespace TestGame.Screens
{
    using UI;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using lib;
    using Managers;

    public class UserInterface : Screen
    {
        public UILabel label { get; set; }
        public UILabel label2 { get; set; }
        public UILabel label3 { get; set; }
        //public UISwitch uiswitch { get; set; }
        //public UITextBox textBox { get; set; }
        public string State { get; set; }
        public bool ShadowsEnable { get; set; } = true;

        public UIBuildingStocks BuildingStocks { get; set; }
        int _fps = 0;

        public UserInterface()
        {
            label = new UILabel();
            label.Position = new Vector2(10, 10);
            label.Color = Color.Transparent;
            label.Font.Color = Color.Wheat;
            label.Font.Type = FontType.RegularWithShadow;
            label.Font.Size = 10;

            label2 = new UILabel();
            label2.Position = new Vector2(10, 30);
            label2.Color = Color.Transparent;
            label2.Font.Color = Color.BlueViolet;
            label2.Font.Type = FontType.RegularWithShadow;
            label2.Font.Size = 10;

            label3 = new UILabel();
            label3.Position = new Vector2(10, 50);
            label3.Color = Color.Transparent;
            label3.Font.Color = Color.Coral;
            label3.Font.Type = FontType.RegularWithShadow;
            label3.Font.Size = 10;

            //textBox = new UITextBox(new UIFont("Test textbox"), new Vector2(10, 200));

            //uiswitch = new UISwitch(ShadowsEnable, new UIFont("Тени"), new Vector2(10, 100), Color.Turquoise);
            //uiswitch.Font.Color = Color.Turquoise;
            BuildingStocks = new UIBuildingStocks();
            ScreenType = ScreenType.UserInterface;
        }
        public override void ProcessingOfClicks(GameTime gameTime, Camera camera)
        {
            if (!IsActive)
                return;

            label.ProcessingOfClicks(gameTime);
            label2.ProcessingOfClicks(gameTime);
            label3.ProcessingOfClicks(gameTime);
            //textBox.ProcessingOfClicks(gameTime);
            //uiswitch.ProcessingOfClicks(gameTime);

            BuildingStocks.ProcessingOfClicks(gameTime);
            if (BuildingStocks.Clicked)
                ClickDetected = true;

            base.ProcessingOfClicks(gameTime, camera);
        }
        public override void Update (GameTime gameTime, Camera camera)
        {
            if (!IsActive)
                return;

            _fps = (int)(1 / (float)gameTime.ElapsedGameTime.TotalSeconds);

            label.Font.Text = string.Format("Левая кнопка мыши - рисуем ( по одному). + Левый шифт - быстрое рисование");
            label.Update(gameTime);

            label2.Font.Text = string.Format("Правая кнопка мыши - МАТЕРИАЛЫ. Камера {0} / {1}", camera.Position.X, camera.Position.Y);
            label2.Update(gameTime);

            label3.Font.Text = string.Format("ФПС = {0} {1}", _fps, State);
            label3.Update(gameTime);

            if (InputManager.MouseStates.RightButton == ButtonState.Pressed)
            {
                BuildingStocks.IsActive = true;
                BuildingStocks.Position = new Vector2(InputManager.MouseStates.Position.X + 10, InputManager.MouseStates.Position.Y);
            }
            BuildingStocks.Update(gameTime);
            //textBox.Update(gameTime);
            //uiswitch.Update(gameTime);
            base.Update(gameTime, camera);
        }
        public override void Draw(GameTime gameTime, Camera camera)
        {
            if (!IsActive)
                return;

            label.Draw(gameTime);
            label2.Draw(gameTime);
            label3.Draw(gameTime);

            //textBox.Draw(gameTime);
            //uiswitch.Draw(gameTime);
            BuildingStocks.Draw(gameTime);
            base.Draw(gameTime, camera);
        }
    }
}
