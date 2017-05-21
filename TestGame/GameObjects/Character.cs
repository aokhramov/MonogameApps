namespace TestGame.GameObjects
{
    using Managers;
    using Screens;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using lib;

    public class Character : AliveObject
    {
        private int _jump = 0;
        private int _cnt;

        //листы анимаций
        private SpriteList idle;
        private SpriteList run;
        private SpriteList jump;

        //текущая анимация
        private SpriteList currentAnimationList;

        public Character()
        {
            
            idle = new SpriteList("Character");
            Width = 32;
            Height = 36;
            idle.Sequence = "0,0,0,0,0,0,0,0,1,2,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,2,1";
            idle.Width = Width;
            idle.Height = Height;
            idle.DistanceBetweenSprites = 2;
            idle.NeedBeginEndSpriteBatchCall = false;

            run = new SpriteList("Character",new Point(0,42));
            run.Sequence = "1,2,3,4,5,6,7,8,9,0";
            run.Width = Width;
            run.Height = Height;
            run.DistanceBetweenSprites = 2;
            run.NeedBeginEndSpriteBatchCall = false;

            jump = new SpriteList("Character", new Point(0, 86));
            jump.Sequence = "0,1,2,2,3,3";
            jump.Width = Width;
            jump.Height = Height - 2;
            jump.DistanceBetweenSprites = 2;
            jump.AnimationType = SpriteAnimationType.RepeatNTimes;
            jump.NTimes = 1;
            jump.NeedBeginEndSpriteBatchCall = false;
        }

        public void Update(GameTime gameTime, MapEntry[,] map)
        {
            if (ScreenManager.ActiveScreen != ScreenType.Map)
                return;

            currentAnimationList = idle;
            
            if (InputManager.IsPressed(Keys.Space) &&
                _jump <= 0 && BottomCollide(map) && !TopCollide(map))
            {
                jump.RestartLoop = true;
                _jump = JumpHeight;
            }

            if (InputManager.IsPressed(Keys.A) ||
                InputManager.IsPressed(Keys.Left))
            {
                right = false;
                currentAnimationList = run;
                _cnt = 0;
                while (_cnt < Speed && !LeftCollide(map))
                {
                    Position += new Vector2(-1, 0);
                    _cnt++;
                }
            }

            if (InputManager.IsPressed(Keys.D) ||
                InputManager.IsPressed(Keys.Right))
            {
                right = true;
                currentAnimationList = run;
                _cnt = 0;
                while (_cnt < Speed && !RightCollide(map))
                {
                    Position += new Vector2(1, 0);
                    _cnt++;
                }
            }

            if (_jump > 0)
            {
                _cnt = 0;
                currentAnimationList = jump;
                while (_cnt < FallingSpeed && !TopCollide(map))
                {
                    Position += new Vector2(0, -1);
                    _cnt++;
                }
                _jump-=FallingSpeed;
            }
            if (_jump <= 0)
            {
                _cnt = 0;
                while(_cnt < FallingSpeed && !BottomCollide(map))
                { 
                    Position += new Vector2(0, 1);
                    _cnt++;
                }
            }

            currentAnimationList.Color = Color;
            currentAnimationList.FlipHorizontally = (right) ? false : true;
            currentAnimationList.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            if (ScreenManager.ActiveScreen != ScreenType.Map || currentAnimationList == null)
                return;
            currentAnimationList.Draw(gameTime,Position);
        }

        public override void UpdateRectangles()
        {
            body = new Rectangle((int)Position.X, (int)Position.Y , Width, Height);

            base.UpdateRectangles();
        }
    }
}
