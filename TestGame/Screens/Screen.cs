namespace TestGame.Screens
{
    using Managers;
    using Microsoft.Xna.Framework;
    using lib;

    public abstract class Screen
    {
        // TODO переделать?
        /// <summary>
        /// Экран активен?
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Обрабатывать клики по экрану в текущем цикле? 
        /// </summary>
        public bool NeedClicksProcessing { get; set; } = true;

        /// <summary>
        /// В текущем цикле на данном экране клик обработан?
        /// </summary>
        public bool ClickDetected { get; set; }

        /// <summary>
        /// Тип экрана
        /// </summary>
        public ScreenType ScreenType { get; set; }

        /// <summary>
        /// Обработка кликов в текущем экране
        /// </summary>
        /// <param name="gameTime">Время</param>
        /// <param name="camera">Камера</param>
        public virtual void ProcessingOfClicks(GameTime gameTime, Camera camera) { }

        /// <summary>
        /// Обновление текущего экрана
        /// </summary>
        /// <param name="gameTime">Время</param>
        /// <param name="camera">Камера</param>
        public virtual void Update(GameTime gameTime,Camera camera)
        {
            if (NeedClicksProcessing == false && InputManager.IsNewPressedAnyMouseButton())
                NeedClicksProcessing = true;
            ClickDetected = false;
        }

        /// <summary>
        /// Рисуем текущий экран
        /// </summary>
        /// <param name="gameTime">Время</param>
        /// <param name="camera">Камера</param>
        public virtual void Draw(GameTime gameTime,Camera camera) { }

        public Screen()
        {
            ScreenManager.AddScreen(this);
        }
        public void Close()
        {
            ScreenManager.RemoveScreen(this);
        }
    }
}
