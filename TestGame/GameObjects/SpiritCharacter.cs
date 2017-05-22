namespace TestGame.GameObjects
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using Managers;

    public class SpiritCharacter : AliveObject
    {
        public SpiritCharacter(Vector2 position)
        {
            Position = position;
        }

        public void Update(GameTime gameTime)
        {
            //двигаемся влево
            if (InputManager.IsPressed(Keys.A) || InputManager.IsPressed(Keys.Left))
                Position += new Vector2(-Speed, 0);
            //двигаемся вправо
            if (InputManager.IsPressed(Keys.D) || InputManager.IsPressed(Keys.Right))
                Position += new Vector2(Speed, 0);
            //двигаемся вверх
            if (InputManager.IsPressed(Keys.W) || InputManager.IsPressed(Keys.Up))
                Position += new Vector2(0, -Speed);
            //двигаемся вниз
            if (InputManager.IsPressed(Keys.S) || InputManager.IsPressed(Keys.Down))
                Position += new Vector2(0, Speed);
        }

    }
}
