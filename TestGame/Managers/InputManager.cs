using Microsoft.Xna.Framework.Input;

namespace TestGame.Managers
{
    public static class InputManager
    {
        /// <summary>
        /// Текущее состояние клавиатуры
        /// </summary>
        public static KeyboardState KeyboardStates;

        /// <summary>
        /// Предыдущее состояние клавиатуры
        /// </summary>
        public static KeyboardState LastKeyboardStates;

        /// <summary>
        /// Текущее состояние мышки
        /// </summary>
        public static MouseState MouseStates;

        /// <summary>
        /// Предыдущее состояние мышки
        /// </summary>
        public static MouseState LastMouseStates;

        /// <summary>
        /// Верхний регистр включен?
        /// </summary>
        public static bool UpperCaseActive = false;

        static InputManager()
        {
            KeyboardStates = new KeyboardState();
            LastKeyboardStates = new KeyboardState();
            MouseStates = new MouseState();
            LastMouseStates = new MouseState();
        }
        public static void Update()
        {
            LastKeyboardStates = KeyboardStates;
            LastMouseStates = MouseStates;
            KeyboardStates = Keyboard.GetState();
            MouseStates = Mouse.GetState();

            if (IsNewPressed(Keys.CapsLock))
                UpperCaseActive = !UpperCaseActive;

            if (IsNewPressed(Keys.RightShift) || IsNewPressed(Keys.LeftShift))
                UpperCaseActive = !UpperCaseActive;

            if (IsKeyUp(Keys.RightShift) || IsKeyUp(Keys.LeftShift))
                UpperCaseActive = !UpperCaseActive;

        }
        /// <summary>
        /// Клавиша клавиатуры нажата?
        /// </summary>
        /// <param name="key">Клавиша</param>
        /// <returns>true, false</returns>
        public static bool IsPressed ( Keys key )
        {
            return (KeyboardStates.IsKeyDown(key));
        }

        /// <summary>
        /// Клавишу клавиатуры отпустили
        /// </summary>
        /// <param name="key">Клавиша</param>
        /// <returns>true, false</returns>
        public static bool IsKeyUp ( Keys key)
        {
            return (KeyboardStates.IsKeyUp(key) && LastKeyboardStates.IsKeyDown(key));
        }

        /// <summary>
        /// Клавиша нажата только что?
        /// </summary>
        /// <param name="key">Клавиша</param>
        /// <returns>true, false</returns>
        public static bool IsNewPressed(Keys key)
        {
            return (KeyboardStates.IsKeyDown(key) && LastKeyboardStates.IsKeyUp(key));
        }

        /// <summary>
        /// Левая кнопка мыши нажата только что?
        /// </summary>
        /// <returns>true, false</returns>
        public static bool IsNewLeftMouseButtonPressed ()
        {
            return (MouseStates.LeftButton == ButtonState.Pressed && LastMouseStates.LeftButton == ButtonState.Released);
        }

        /// <summary>
        /// Средняя кнопка мыши нажата только что?
        /// </summary>
        /// <returns>true, false</returns>
        public static bool IsNewMiddleMouseButtonPressed()
        {
            return (MouseStates.MiddleButton == ButtonState.Pressed && LastMouseStates.MiddleButton == ButtonState.Released);
        }

        /// <summary>
        /// Правая кнопка мыши нажата только что?
        /// </summary>
        /// <returns>true, false</returns>
        public static bool IsNewRightMouseButtonPressed()
        {
            return (MouseStates.RightButton == ButtonState.Pressed && LastMouseStates.RightButton == ButtonState.Released);
        }

        /// <summary>
        /// Какая-нибудь кнопка мышки нажата?
        /// </summary>
        /// <returns></returns>
        public static bool IsPressedAnyMouseButton()
        {
            return (MouseStates.LeftButton == ButtonState.Pressed ||
                MouseStates.MiddleButton == ButtonState.Pressed ||
                MouseStates.RightButton == ButtonState.Pressed);
        }

        /// <summary>
        /// Какая-нибудь кнопка мышки нажата только что?
        /// </summary>
        /// <returns></returns>
        public static bool IsNewPressedAnyMouseButton()
        {
            return ((MouseStates.LeftButton == ButtonState.Pressed && LastMouseStates.LeftButton == ButtonState.Released) ||
                (MouseStates.MiddleButton == ButtonState.Pressed && LastMouseStates.MiddleButton == ButtonState.Released) ||
                (MouseStates.RightButton == ButtonState.Pressed && LastMouseStates.RightButton == ButtonState.Released));
        }
    }
}
