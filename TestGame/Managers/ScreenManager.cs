namespace TestGame.Managers
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Input;
    using System.Collections.Generic;
    using Screens;
    using System;
    using lib;

    public static class ScreenManager
    {       
        /// <summary>
        /// Инициирован режим выхода из игры
        /// </summary>
        public static bool InitiateGameExitMode = false;

        /// <summary>
        /// Ширина игрового окна в пикселях
        /// </summary>
        public static int Width { get; set; }

        /// <summary>
        /// Высота игрового окна в пикселях
        /// </summary>
        public static int Height { get; set; }

        /// <summary>
        /// Отображаемая область карты. Начальный столбец
        /// </summary>
        public static int Point_X1 { get; set; } = 0;

        /// <summary>
        /// Отображаемая область карты. Конечный столбец
        /// </summary>
        public static int Point_X2 { get; set; } = 0;

        /// <summary>
        /// Отображаемая область карты. Начальная строка
        /// </summary>
        public static int Point_Y1 { get; set; } = 0;

        /// <summary>
        /// Отображаемая область карты. Конечная строка
        /// </summary>
        public static int Point_Y2 { get; set; } = 0;

        public static SpriteBatch SpriteBatch { get; set; }

        /// <summary>
        /// Контент
        /// </summary>
        public static ContentManager Content;

        /// <summary>
        /// Матрица отображения
        /// </summary>
        public static Matrix viewMatrix;

        /// <summary>
        /// Список существующих экранов
        /// </summary>
        public static List<Screen> Screens { get; private set; } = new List<Screen>();

        private static ScreenType activeScreen = ScreenType.Empty;

        /// <summary>
        /// Активный экран
        /// </summary>
        public static ScreenType ActiveScreen
        {
            get { return activeScreen; }
            set
            {
                if(activeScreen != value)
                {
                    LastActiveScreen = activeScreen;
                    activeScreen = value;
                }
            }
        }

        /// <summary>
        /// Предыдущий экран
        /// </summary>
        public static ScreenType LastActiveScreen { get; private set; }

        public static void ProcessingOfClicks(GameTime gameTime, Camera camera)
        {
            foreach (Screen screen in Screens)
            {
                if (screen.ScreenType == ActiveScreen)
                {
                    screen.ProcessingOfClicks(gameTime, camera);
                }
            }
        }

        /// <summary>
        /// Обновляем экраны
        /// </summary>
        /// <param name="gameTime">Время</param>
        /// <param name="camera">Камера с матрицой отображения</param>
        public static void Update(GameTime gameTime, Camera camera)
        {
            ScreenPoints(camera);//пересчитаем отображаемые границы карты

            foreach (Screen screen in Screens)
            {
                if (screen.ScreenType == ActiveScreen)
                { 
                    //screen.ProcessingOfClicks(gameTime, camera);
                    screen.Update(gameTime, camera);
                }
            }
        }

        /// <summary>
        /// Рисуем активный экран
        /// </summary>
        /// <param name="gameTime">Время</param>
        public static void Draw(GameTime gameTime, Camera camera)
        {
            foreach(Screen screen in Screens)
            {
                if (screen.ScreenType == ActiveScreen)
                {
                    //if (ActiveScreen == ScreenType.MapEditorMenu)
                    //{
                    //    foreach (Screen MapScreen in Screens)
                    //        if (MapScreen.ScreenType == ScreenType.Map)
                    //        {
                    //            MapScreen.Draw(gameTime, camera);
                    //        }
                    //}
                    screen.Draw(gameTime, camera);
                }
            }
        }

        /// <summary>
        /// Добавляем экран в список существующих
        /// </summary>
        /// <param name="screen">Экран</param>
        public static void AddScreen (Screen screen)
        {
            Screens.Add(screen);
        }

        /// <summary>
        /// Удаляем экран из списка существующих
        /// </summary>
        /// <param name="screen">Экран</param>
        public static void RemoveScreen(Screen screen)
        {
            Screens.Remove(screen);
        }

        /// <summary>
        /// Пересчитаем отображаемые границы карты относительно центра камеры
        /// </summary>
        /// <param name="camera">Камера</param>
        public static void ScreenPoints(Camera camera)
        {
            int dop_tiles = 4;
            Point_X1 = Math.Max(0, (int)(camera.Position.X / TextureManager.TileSize) - dop_tiles);
            Point_Y1 = Math.Max(0, (int)(camera.Position.Y / TextureManager.TileSize) - dop_tiles);

            Point_X2 = Math.Min(TextureManager.countHorizontalTiles, Point_X1 + TextureManager.countDisplayHorizontalTiles + dop_tiles * 2);
            Point_Y2 = Math.Min(TextureManager.countVerticalTiles, Point_Y1 + TextureManager.countDisplayVerticalTiles + dop_tiles * 2);
        }
    }
}
