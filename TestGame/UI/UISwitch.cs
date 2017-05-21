namespace TestGame.UI
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using Managers;
    using System;
    using lib;

    public class UISwitch : UIElement
    {
        // TODO глюк переключателя 

        /// <summary>
        /// Текст переключателя
        /// </summary>
        public UIFont Font { get; set; }
        public delegate void EventHandler(object sender);
        /// <summary>
        /// Событие при нажатии на переключатель
        /// </summary>
        public event EventHandler Click;

        private bool change = false;

        private SpriteList _switch;
        //----
        public UISwitch(bool initvalue) 
            : this(initvalue,new UIFont(""),new Vector2(0,0),Color.White) { }

        public UISwitch(bool initvalue, UIFont font) 
            : this(initvalue, font, new Vector2(0, 0), Color.White) { }

        public UISwitch(bool initvalue, UIFont font, Vector2 position) 
            : this(initvalue, font, position, Color.White) { }

        public UISwitch(bool initvalue, UIFont font, Vector2 position, Color color)
            : base(position, color)
        {
            Font = font;
            //анимация переключателя
            _switch = new SpriteList("UISwitch");
            _switch.StartPoint = new Point(0, 0);
            _switch.Color = color;
            _switch.Width = 48;
            _switch.Height = 24;
            _switch.Sequence = "0;1;2;3";
            _switch.DistanceBetweenSprites = 2;
            _switch.AnimationType = SpriteAnimationType.RepeatNTimes;
            _switch.AnimationPeriod = 50;
            _switch.NTimes = 0;
            _switch.UseViewMatrix = false;
            _switch.Reverse = initvalue;

            //TODO определение номера убрать в спрайтлист
            if (_switch.Reverse)
                _switch.CurrentSprite = 3;
            else
                _switch.CurrentSprite = 0;
            
        }
        //----
        protected override void ComputeElement()
        {
            Width = Font.Width + 5 + _switch.Width;
            Height = Math.Max(Font.Height, _switch.Height);
            Font.Position = Position;
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

            if (change)
            {
                if (_switch.NTimes == 0)
                    _switch.NTimes = 1;

                if (_switch.Reverse && _switch.CurrentSprite == 0)
                {
                    _switch.Reverse = false;
                    change = false;
                    _switch.NTimes = 0;
                }
                if (!_switch.Reverse && _switch.CurrentSprite == _switch.CountSprites - 1)
                {
                    _switch.Reverse = true;
                    change = false;
                    _switch.NTimes = 0;
                }
            }

            _switch.Update(gameTime);


            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            _switch.Draw(gameTime, new Vector2(Position.X + Font.Width + 5,Position.Y));
            Font.Draw(gameTime);
            base.Draw(gameTime);
        }

        /// <summary>
        /// Определяем нажали ли на переключатель
        /// </summary>
        /// <param name="gameTime">Время</param>
        public override void ProcessingOfClicks(GameTime gameTime)
        {
            if (InputManager.MouseStates.LeftButton == ButtonState.Pressed &&
                InputManager.LastMouseStates.LeftButton == ButtonState.Released &&
                InputManager.MouseStates.X >= Position.X + Font.Width + 5 &&
                InputManager.MouseStates.Y >= Position.Y &&
                InputManager.MouseStates.X <= (Position.X + Font.Width + 5 + _switch.Width) &&
                InputManager.MouseStates.Y <= (Position.Y + _switch.Height))
            {
                Console.WriteLine("switch collide event");
                change = true;
                _switch.RestartLoop = true;

                if (Click != null)
                    Click(this);
            }
            base.ProcessingOfClicks(gameTime);
        }
    }
}
