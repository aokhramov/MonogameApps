namespace TestGame.Screens
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System;
    using lib;
    using Managers;

    public class MapEntry
    {
        /// <summary>
        /// Тип текстуры с обработкой столкновения
        /// </summary>
        public MapEntryTextureType CollideTextureType { get; private set; } = MapEntryTextureType.None;

        /// <summary>
        /// Тип текстуры без обработки столкновения
        /// </summary>
        public MapEntryTextureType NonCollideTextureType { get; private set; } = MapEntryTextureType.None;

        /// <summary>
        /// Тип декоративной текстуры
        /// </summary>
        public MapEntryTextureType DecorateTextureType { get; private set; } = MapEntryTextureType.None;

        /// <summary>
        /// Текстура без столкновения 
        /// </summary>
        public Texture2D NonCollidingTexture { get; set; }

        /// <summary>
        /// Текстура со столкновением
        /// </summary>
        public Texture2D CollidingTexture { get; set; }

        /// <summary>
        /// Декоративная текстура
        /// </summary>
        public Texture2D DecorateTexture { get; set; }

        /// <summary>
        /// Рандом для определения стартовой точки тайла
        /// </summary>
        private static Random rnd = new Random();

        /// <summary>
        /// Стартовая точка тайла
        /// </summary>
        public Point TileStartPoint = new Point(0, 0);
        private int cntTileTypes;

        /// <summary>
        /// Строка с тайлами
        /// </summary>
        public int TilesRow = 0;

        /// <summary>
        /// Просмотривать ли соседние ячейки при обновлении текущей
        /// </summary>
        public bool WatchNearbyCells { get; set; } = false;
        public Vector2 Position;
        public Color Color { get; set; }
        private Color lastwColor;
        /// <summary>
        /// Тень ( 0 - нет, 25 - тень , 225 - объект почти не виден )
        /// </summary>
        public int Shadow { get; set; }
        private int lastShadow;

        public int X;
        public int Y;
        /// <summary>
        /// Обновить ячейку
        /// </summary>
        public bool Refresh { get; set; } = true;

        public Texture2D DamageTexture { get; set; }

        /// <summary>
        /// Изменение типа текстуры
        /// </summary>
        /// <param name="type">Тип текстуры</param>
        /// <param name="countTileTypes">Количество тайлов в один ряд (кол-во столбцов). Для рандомности выбора однотипной текстуры</param>
        public void ChangeType(MapEntryTextureType type, int countTileTypes = 5)
        {
            cntTileTypes = Math.Max(0, countTileTypes);

            switch (type)
            {
                //None (стираем всё)
                case MapEntryTextureType.None:
                default:
                    CollidingTexture = null;
                    CollideTextureType = MapEntryTextureType.None;
                    NonCollidingTexture = null;
                    NonCollideTextureType = MapEntryTextureType.None;
                    DecorateTexture = null;
                    DecorateTextureType = MapEntryTextureType.None;
                    break;

                //CollideTexture
                case MapEntryTextureType.Dirt:
                case MapEntryTextureType.Gold:
                case MapEntryTextureType.Grass:
                case MapEntryTextureType.Silver:
                case MapEntryTextureType.Stone:
                    if(CollideTextureType != type)
                    {
                        CollideTextureType = type;
                        TileStartPoint = new Point(rnd.Next(0, cntTileTypes), 0);
                        TilesRow = 0;
                        WatchNearbyCells = false;
                        CollidingTexture = TextureManager.GetTexture(TextureType.Tilesets, CollideTextureType.ToString());
                    }
                    break;

                //NonCollideTexture
                case MapEntryTextureType.BackDirt:
                case MapEntryTextureType.BackWall:
                    if(NonCollideTextureType != type || CollidingTexture != null)
                    {
                        NonCollideTextureType = type;
                        TileStartPoint = new Point(rnd.Next(0, cntTileTypes), 0);
                        TilesRow = 0;
                        WatchNearbyCells = true;
                        NonCollidingTexture = TextureManager.GetTexture(TextureType.Tilesets, NonCollideTextureType.ToString());
                        CollideTextureType = MapEntryTextureType.None;
                        CollidingTexture = null;
                    }
                    break;
            }
        }
        public void Update(GameTime gameTime)
        {

        }

        public void Draw(GameTime gameTime, Color WorldColor)
        {
            //нет никакой текстуры - нечего и рисовать
            if (CollidingTexture == null && NonCollidingTexture == null && DecorateTexture == null)
                return;

            if(lastwColor != WorldColor || lastShadow != Shadow)
            {
                lastwColor = WorldColor;
                lastShadow = Shadow; 
                CurrentColor(WorldColor);
            }

            Rectangle tmp = new Rectangle(TileStartPoint.X * TextureManager.TileSize, TileStartPoint.Y * TextureManager.TileSize, TextureManager.TileSize, TextureManager.TileSize);
            if (CollidingTexture != null || NonCollidingTexture != null)
                ScreenManager.SpriteBatch.Draw((CollidingTexture != null) ? CollidingTexture : NonCollidingTexture,
                    Position, tmp, Color, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0);

            if (DecorateTexture != null)
                ScreenManager.SpriteBatch.Draw(DecorateTexture, Position, null, Color, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }

        private byte ToByte(int a)
        {
            if (a > 255)
                a = 255;
            if (a < 0)
                a = 0;

            return (byte)a;
        }

        /// <summary>
        /// Вычисляем корректный цвет с учетом тени
        /// </summary>
        /// <param name="WorldColor">Текущий общий оттенок уровня</param>
        public void CurrentColor(Color WorldColor)
        {
            Color =  new Color(ToByte(WorldColor.R - Shadow), ToByte(WorldColor.G - Shadow), ToByte(WorldColor.B - Shadow));
        }
    }
}
