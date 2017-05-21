namespace TestGame.GameObjects
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Managers;
    using Screens;

    public abstract class AliveObject
    {
        protected bool right = true;
        protected Rectangle body;
        protected Rectangle tmpbody;
        protected Rectangle mapcell;

        protected Texture2D Texture;

        /// <summary>
        /// Ширина существа
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Высота существа
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Цвет существа
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Высота прыжка
        /// </summary>
        public int JumpHeight { get; private set; } = 144;

        /// <summary>
        /// Скорость передвижения
        /// </summary>
        public int Speed { get; private set; } = 6;

        /// <summary>
        /// Скорость падения
        /// </summary>
        public int FallingSpeed { get; private set; } = 6;

        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                if (position.X < 0)
                    position.X = 0;
                if (position.X  > (TextureManager.countHorizontalTiles * TextureManager.TileSize) - Width)
                    position.X = (TextureManager.countHorizontalTiles * TextureManager.TileSize) - Width;

                if (position.Y < 0)
                    position.Y = 0;

                if (position.Y > (TextureManager.countVerticalTiles * TextureManager.TileSize) - Height)
                    position.Y = (TextureManager.countVerticalTiles * TextureManager.TileSize) - Height;

                UpdateRectangles();
            }
        }
        public virtual void UpdateRectangles(){}

        // далее обработка столкновений

        protected bool TopCollide(MapEntry[,] map)
        {
            int y = (int)((Position.Y - 1) / TextureManager.TileSize);
            int x1 = (int)(Position.X / TextureManager.TileSize);
            int x2 = (int)((Position.X + Width) / TextureManager.TileSize);

            tmpbody = new Rectangle(body.X, body.Y - 1, body.Width, body.Height);

            for (int i = x1; i <= x2; i++)
            {
                mapcell = new Rectangle((int)map[y, i].Position.X, (int)map[y, i].Position.Y, TextureManager.TileSize, TextureManager.TileSize);

                if (map[y, i].CollidingTexture != null && tmpbody.Intersects(mapcell))
                {
                    return true;
                }
            }
            return false;
        }

        protected bool BottomCollide(MapEntry[,] map)
        {
            int y = (int)((Position.Y + Height + 1) / TextureManager.TileSize);
            if (y == map.GetLength(0))
                return true;
            int x1 = (int)(Position.X / TextureManager.TileSize);
            int x2 = (int)((Position.X + Width) / TextureManager.TileSize);

            tmpbody = new Rectangle(body.X, body.Y + 1, body.Width, body.Height);

            for (int i = x1; i <= x2; i++)
            {
                mapcell = new Rectangle((int)map[y, i].Position.X, (int)map[y, i].Position.Y, TextureManager.TileSize, TextureManager.TileSize);

                if (map[y, i].CollidingTexture != null && tmpbody.Intersects(mapcell))
                {
                    return true;
                }
            }
            return false;
        }

        protected bool LeftCollide(MapEntry[,] map)
        {
            int x = (int)((Position.X - 1) / TextureManager.TileSize);
            int y1 = (int)(Position.Y / TextureManager.TileSize);
            int y2 = (int)((Position.Y + Height) / TextureManager.TileSize);

            tmpbody = new Rectangle(body.X - 1, body.Y, body.Width, body.Height);

            for (int i = y1; i <= y2; i++)
            {
                mapcell = new Rectangle((int)map[i, x].Position.X, (int)map[i, x].Position.Y, TextureManager.TileSize, TextureManager.TileSize);

                if (map[i, x].CollidingTexture != null && tmpbody.Intersects(mapcell))
                {
                    return true;
                }
            }
            return false;
        }

        protected bool RightCollide(MapEntry[,] map)
        {
            int x = (int)((Position.X + Width + 1) / TextureManager.TileSize);
            if (x == map.GetLength(1))
                return true;
            int y1 = (int)(Position.Y / TextureManager.TileSize);
            int y2 = (int)((Position.Y + Height) / TextureManager.TileSize);

            tmpbody = new Rectangle(body.X + 1, body.Y, body.Width, body.Height);

            for (int i = y1; i <= y2; i++)
            {
                mapcell = new Rectangle((int)map[i, x].Position.X, (int)map[i, x].Position.Y, TextureManager.TileSize, TextureManager.TileSize);

                if (map[i, x].CollidingTexture != null && tmpbody.Intersects(mapcell))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
