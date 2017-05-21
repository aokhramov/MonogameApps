namespace TestGame.Managers
{
    using GameObjects;
    using Microsoft.Xna.Framework;

    public class Camera
    {
        /// <summary>
        /// "Живой" объект к которому привязывается камера
        /// </summary>
        public AliveObject BindedObject { get; private set; }

        private Vector2 screenCenter;
        private Vector2 lastBindedObjectPosition;
        private float lastCameraMultiply;
        /// <summary>
        /// Зум камеры
        /// </summary>
        public float CameraMultiplier { get; set; } = 1f;

        public Vector2 Position { get; set; }

        private int _width;
        private int _height;

        // TODO: доделать. Изменение объекта за которым движется камера. плавность переключения
        public Camera(int width, int height)
        {
            ChangeScreenSize(width, height);
        }

        /// <summary>
        /// Обновляем камеру
        /// </summary>
        public void Update()
        {
            if (_width != ScreenManager.Width || _height != ScreenManager.Height)
                ChangeScreenSize(ScreenManager.Width, ScreenManager.Height);

            if(BindedObject != null && (lastBindedObjectPosition != BindedObject.Position || lastCameraMultiply != CameraMultiplier))
            {
                if (lastCameraMultiply != CameraMultiplier)
                    lastCameraMultiply = CameraMultiplier;

                if (lastBindedObjectPosition != BindedObject.Position)
                    lastBindedObjectPosition = BindedObject.Position;

                UpdateViewMatrix();
            }

            if (InputManager.MouseStates.ScrollWheelValue != InputManager.LastMouseStates.ScrollWheelValue)
            {
                if(InputManager.MouseStates.ScrollWheelValue > InputManager.LastMouseStates.ScrollWheelValue &&
                    CameraMultiplier < 2.5f)
                    CameraMultiplier += 0.1f;

                if (InputManager.MouseStates.ScrollWheelValue < InputManager.LastMouseStates.ScrollWheelValue &&
                    CameraMultiplier > 0.7f)
                    CameraMultiplier -= 0.1f;

                float width = ScreenManager.Width / CameraMultiplier;
                float height = ScreenManager.Height / CameraMultiplier;

                TextureManager.ChangeCountTiles((int)width, (int)height);
            }
        }
        /// <summary>
        /// Обновляем матрицу отображения камеры при изменении расширения окна
        /// </summary>
        /// <param name="width">Ширина окна</param>
        /// <param name="height">Высота окна</param>
        public void ChangeScreenSize(int width, int height)
        {
            _width = width;
            _height = height;
            screenCenter.X = width / 2;
            screenCenter.Y = height / 2;
            UpdateViewMatrix();
        }
        /// <summary>
        /// Привязываем камеру к "живому" объекту
        /// </summary>
        /// <param name="obj">Объект</param>
        public void Tracking(AliveObject obj)
        {
            BindedObject = obj;
        }
        /// <summary>
        /// Отвязываем камеру от объекта
        /// </summary>
        public void DisableTracking()
        {
            BindedObject = null;
        }
        /// <summary>
        /// Обновляем матрицу отображения
        /// </summary>
        private void UpdateViewMatrix()
        {
            if (BindedObject != null)
            { 
                ScreenManager.viewMatrix = Matrix.CreateTranslation(new Vector3(screenCenter.X/CameraMultiplier - BindedObject.Position.X, 
                    screenCenter.Y/CameraMultiplier - BindedObject.Position.Y, 0.0f)) *
                    Matrix.CreateRotationZ(0f) *
                    Matrix.CreateScale(CameraMultiplier, CameraMultiplier, 1.0f) *
                    Matrix.CreateTranslation(new Vector3(Vector2.Zero, 0.0f));

                Position = new Vector2( BindedObject.Position.X - (_width/CameraMultiplier/2),  BindedObject.Position.Y - (_height/CameraMultiplier/2));
            }
            else
                ScreenManager.viewMatrix = Matrix.CreateTranslation(0.0f, 0.0f, 0.0f);
        }
    }
}
