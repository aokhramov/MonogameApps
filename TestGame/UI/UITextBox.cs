namespace TestGame.UI
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Managers;
    using lib;

    public class UITextBox : UIElement
    {
        /// <summary>
        /// Текст 
        /// </summary>
        public UIFont Font { get; set; }
        private Rectangle textBoxArea;

        private Texture2D CornerLT = TextureManager.GetTexture(TextureType.UI, "CornerLT");
        private Texture2D CornerLB = TextureManager.GetTexture(TextureType.UI, "CornerLB");
        private Texture2D CornerRT = TextureManager.GetTexture(TextureType.UI, "CornerRT");
        private Texture2D CornerRB = TextureManager.GetTexture(TextureType.UI, "CornerRB");

        private Texture2D BorderT = TextureManager.GetTexture(TextureType.UI, "BorderT");
        private Texture2D BorderB = TextureManager.GetTexture(TextureType.UI, "BorderB");
        private Texture2D BorderL = TextureManager.GetTexture(TextureType.UI, "BorderL");
        private Texture2D BorderR = TextureManager.GetTexture(TextureType.UI, "BorderR");

        private Texture2D Default = TextureManager.GetTexture(TextureType.UI, "default");

        /// <summary>
        /// Текстбокс в фокусе?
        /// </summary>
        public bool Focused { get; set; } = false;

        public UITextBox() 
            : this(new UIFont(""),new Vector2(0,0), new Color(30, 30, 30)) { }

        public UITextBox(UIFont font) 
            : this(font, new Vector2(0, 0), new Color(30, 30, 30)) { }

        public UITextBox(UIFont font, Vector2 position) 
            : this(font, position, Color.LightPink) { }

        public UITextBox(UIFont font, Vector2 position, Color color)
            : base(position, color)
        {
            Font = font;
            Font.Color = Color.LavenderBlush;
            Width = 200;
            Height = 30;
        }

        protected override void ComputeElement()
        {
            if (Init)
            {
                Alpha = 0.9f;
                if (Width == 0f)
                    Width = Font.Width + BorderL.Width * 2 + BorderR.Width * 2;
            }
            if (Width < Font.Width + BorderL.Width * 2 + BorderR.Width * 2)
                Width = Font.Width + BorderL.Width * 2 + BorderR.Width * 2;
            if (Height < Font.Height + BorderT.Height * 3 + BorderB.Height * 3)
                Height = Font.Height + BorderT.Height * 3 + BorderB.Height * 3;

            Width = (int)Width;
            Height = (int)Height;

            while (Width % Default.Width != 0)
                Width++;
            while (Height % Default.Height != 0)
                Height++;

            Font.Position = FontPosition();

            textBoxArea = new Rectangle((int)Position.X + BorderL.Width, (int)Position.Y + BorderT.Height, (int)Width - BorderL.Width, (int)Height - BorderT.Height);
            base.ComputeElement();
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
            HandleInput();
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            // TODO: привести в более читабельный вид
            ScreenManager.SpriteBatch.Begin();

            //corners
            ScreenManager.SpriteBatch.Draw(CornerLT, Position,
                null, Color * Alpha, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);//lefttop
            ScreenManager.SpriteBatch.Draw(CornerRT, Position + new Vector2(Width - 4, 0),
                null, Color * Alpha, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);//righttop
            ScreenManager.SpriteBatch.Draw(CornerLB, Position + new Vector2(0, Height - 4),
                null, Color * Alpha, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);//leftbot
            ScreenManager.SpriteBatch.Draw(CornerRB, Position + new Vector2(Width - 4, Height - 4),
                null, Color * Alpha, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);//rightbot

            Vector2 pos = Position;

            ScreenManager.SpriteBatch.Draw(Default,
                new Rectangle( (int)pos.X + Default.Width,(int)pos.Y + Default.Height, 
                (int)Width - CornerRT.Width - Default.Width, 
                (int)Height - CornerRB.Height - Default.Height), 
                Color * Alpha);

            pos = Position + new Vector2(CornerLT.Width, 0);
            //left to right

            ScreenManager.SpriteBatch.Draw(BorderT, 
                new Rectangle((int)pos.X, (int)pos.Y , 
                (int)Width - CornerRT.Width - Default.Width, 
                BorderT.Height), Color * Alpha);

            ScreenManager.SpriteBatch.Draw(BorderB, 
                new Rectangle((int)pos.X, (int)(pos.Y + Height - BorderB.Height),
                (int)(Width - CornerRT.Width - Default.Width), 
                BorderB.Height), Color * Alpha);
            //top to bot
            pos = Position + new Vector2(0, CornerLT.Height);

            ScreenManager.SpriteBatch.Draw(BorderL,
                new Rectangle((int)pos.X, (int)pos.Y,
                BorderL.Width,
                (int)(Height - BorderL.Height - Default.Height)), Color * Alpha);

            ScreenManager.SpriteBatch.Draw(BorderR,
                new Rectangle((int)(pos.X + Width - CornerRT.Width), (int)pos.Y,
                BorderR.Width,
                (int)(Height - BorderR.Height - Default.Height)), Color * Alpha);

            ScreenManager.SpriteBatch.End();

            Font.Draw(gameTime);
            base.Draw(gameTime);
        }

        /// <summary>
        /// Определяем нажали ли мышкой в текстбокс - фокус получен или нет
        /// </summary>
        /// <param name="gameTime">Время</param>
        public override void ProcessingOfClicks(GameTime gameTime)
        {
            if (InputManager.MouseStates.LeftButton == ButtonState.Pressed &&
                InputManager.LastMouseStates.LeftButton == ButtonState.Released &&
                InputManager.MouseStates.X >= Position.X &&
                InputManager.MouseStates.Y >= Position.Y &&
                InputManager.MouseStates.X <= (Position.X + textBoxArea.Width) &&
                InputManager.MouseStates.Y <= (Position.Y + textBoxArea.Height))
            {
                System.Console.WriteLine("textbox collide event");
                Focused = true;
            }
            if (Focused && InputManager.MouseStates.LeftButton == ButtonState.Pressed &&
                InputManager.LastMouseStates.LeftButton == ButtonState.Released &&
                (InputManager.MouseStates.X < Position.X ||
                InputManager.MouseStates.Y < Position.Y ||
                InputManager.MouseStates.X > (Position.X + textBoxArea.Width) ||
                InputManager.MouseStates.Y > (Position.Y + textBoxArea.Height)))
            {
                System.Console.WriteLine("textbox UNcollide event");
                Focused = false;
            }
            base.ProcessingOfClicks(gameTime);
        }
        private Vector2 FontPosition()
        {
            // TODO: позиция текста в текстбоксе
            Vector2 pos = Position + new Vector2(CornerLB.Width * 2, CornerLB.Height * 2) ;
            return pos;
        }
        //Обработка нажатий клавиш клавиатуры при активном фокусе - печатаем текст
        private void HandleInput()
        {
            if(Focused)
            {
                var keys = InputManager.KeyboardStates.GetPressedKeys();

                if(keys.Length > 0)
                {
                    for(int i = 0; i < keys.Length; i++)
                    {
                        if (InputManager.LastKeyboardStates.IsKeyUp(keys[i]))
                        {
                            if ((int)keys[i] > 64 && (int)keys[i] < 91)
                                Font.Text += (InputManager.UpperCaseActive)? keys[i].ToString(): keys[i].ToString().ToLower();
                            if ((int)keys[i] > 47 && (int)keys[i] < 58)
                                Font.Text += (keys[i] - 48);
                            if ((int)keys[i] == 8 && Font.Text.Length > 0)// back
                                Font.Text = Font.Text.Substring(0, Font.Text.Length - 1);
                    }
                }
            }
            }
        }
    }
}
