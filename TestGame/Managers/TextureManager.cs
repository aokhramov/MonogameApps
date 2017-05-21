namespace TestGame.Managers
{
    using Microsoft.Xna.Framework.Graphics;
    using lib;

    public static class TextureManager
    {
        /// <summary>
        /// Общее количество тайлов в строке
        /// </summary>
        public static int countHorizontalTiles = 0;

        /// <summary>
        /// Количество отображаемых тайлов в строке
        /// </summary>
        public static int countDisplayHorizontalTiles = 0;

        /// <summary>
        /// Общее количетсво тайлов в столбце
        /// </summary>
        public static int countVerticalTiles = 0;

        /// <summary>
        /// Количество отображаемых тайлов в столбце
        /// </summary>
        public static int countDisplayVerticalTiles = 0;

        /// <summary>
        /// Размер тайла в пикселях (квадрат)
        /// </summary>
        public static int TileSize = 24;

        /// <summary>
        /// Пересчет количества тайлов при изменение расширения игрового окна
        /// </summary>
        /// <param name="width">Ширина окна</param>
        /// <param name="height">Высота окна</param>
        public static void ChangeCountTiles(int width,int height)
        {
            countDisplayHorizontalTiles = width / TileSize;
            if (countDisplayHorizontalTiles * TileSize < width)
                countDisplayHorizontalTiles++;

            countDisplayVerticalTiles = height / TileSize;
            if (countDisplayVerticalTiles * TileSize < height)
                countDisplayVerticalTiles++;
        }

        /// <summary>
        /// Возвращаем текстуру по типу и наименованию
        /// </summary>
        /// <param name="type">Тип текстуры</param>
        /// <param name="name">Наименование текстуры</param>
        /// <returns></returns>
        public static Texture2D GetTexture(TextureType type,string name)
        {
            Texture2D _tmp;
            _tmp = ScreenManager.Content.Load<Texture2D>(string.Format("{0}/{1}", type, name));
            return _tmp;
        }
    }
}
