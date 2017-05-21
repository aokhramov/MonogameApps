namespace TestGame.UI
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using lib;

    // TODO: добавить анимаций
    
    public abstract class UIElement
    {
        #region Fields and Properties
        //-------------------------------------------------
        /// <summary>
        /// Создан только что?
        /// </summary>
        public bool Init { get; private set; } = true;
        /// <summary>
        /// Отображается?
        /// </summary>
        public bool Visible { get; set; } = false;
        /// <summary>
        /// Нужно пересчитать параметры элемента?
        /// </summary>
        public bool Compute { get; set; } = true;

        /// <summary>
        /// По элементу кликнули?
        /// </summary>
        public bool Clicked { get; set; }

        private float width = 0f;
        /// <summary>
        /// Ширина элемента
        /// </summary>
        public float Width
        {
            get { return width; }
            set
            {
                width = value;
                Compute = true;
            }
        }
        private float height = 0f;
        /// <summary>
        /// Высота элемента
        /// </summary>
        public float Height
        {
            get { return height; }
            set
            {
                height = value;
                Compute = true;
            }
        }

        private Vector2 position;
        /// <summary>
        /// Позиция элемента
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                Compute = true;
            }
        }
        private Color color;
        /// <summary>
        /// Цвет элемента
        /// </summary>
        public Color Color
        {
            get { return color; }
            set
            {
                color = value;
                Compute = true;
            }
        }
        private Texture2D texture;
        /// <summary>
        /// Текстура элемента
        /// </summary>
        public Texture2D Texture
        {
            get { return texture; }
            set
            {
                texture = value;
                Compute = true;
            }
        }
        /// <summary>
        /// Тип анимации элемента
        /// </summary>
        public AnimationType Animation { get; set; } = AnimationType.None;
        protected float Alpha = 1f;
        private byte? animationCounter = null;
        private byte? AnimationCounter
        {
            get { return animationCounter; }
            set
            {
                animationCounter = value;
                if (value != null)
                    Alpha = (float)value / 255;
            }
        }
        private byte animationSpeed = 5;
        private int steps = 0;
        private bool addAlpha = true;
        //-------------------------------------------------
        #endregion

        #region Constructors
        //-------------------------------------------------
        public UIElement (Vector2 position, Color color)
        {
            Position = position;
            Color = color;
        }
        //-------------------------------------------------
        #endregion

        #region Methods
        //-------------------------------------------------
        /// <summary>
        /// Пересчитываем параметры элемента
        /// </summary>
        protected virtual void ComputeElement()
        {
            Compute = false;
            Init = false;
        }
        /// <summary>
        /// Обработка кликов по элементу
        /// </summary>
        /// <param name="gameTime">Время</param>
        public virtual void ProcessingOfClicks(GameTime gameTime) { }


        /// <summary>
        /// Обновление элемента
        /// </summary>
        /// <param name="gameTime">Время</param>
        public virtual void Update(GameTime gameTime)
        {
            Clicked = false;

            if( Animation == AnimationType.Open )
            {
                if (AnimationCounter == null)
                    AnimationCounter = 0;
                AnimationOpen();
            }
            if( Animation == AnimationType.Close)
            {
                if (AnimationCounter == null)
                    AnimationCounter = 255;
                AnimationClose();
            }
            if( Animation == AnimationType.ShortFlashing)
            {
                if (AnimationCounter == null)
                { 
                    AnimationCounter = 0;
                    steps = 0;
                    addAlpha = true;
                }
                AnimationShortFlashing();
            }
            if (Animation == AnimationType.EndlessFlashing)
            {
                if (AnimationCounter == null)
                { 
                    AnimationCounter = 0;
                    addAlpha = true;
                }
                AnimationEndlessFlashing();
            }
        }


        /// <summary>
        /// Рисуем элемент
        /// </summary>
        /// <param name="gameTime">Время</param>
        public virtual void Draw(GameTime gameTime) { }

        private void AnimationOpen()
        {
            if( AnimationCounter == 255 )
            {
                Animation = AnimationType.None;
                AnimationCounter = null;
                Alpha = 1f;
            }
            else
            {
                AnimationCounter += animationSpeed;
            }
        }
        private void AnimationClose()
        {
            if ( AnimationCounter == 0 )
            { 
                Animation = AnimationType.None;
                AnimationCounter = null;
                Alpha = 0f;
            }
            else
            {
                AnimationCounter -= animationSpeed;
            }
        }
        private void AnimationShortFlashing()
        {
            if(AnimationCounter == 0)
            {
                addAlpha = true;
            }
            if (AnimationCounter == 255)
            {
                addAlpha = false;
                steps++;
            }
            if (addAlpha)
                AnimationCounter += animationSpeed;
            else
                AnimationCounter -= animationSpeed;
            if (steps == 3)
            { 
                Animation = AnimationType.None;
                AnimationCounter = null;
                Alpha = 1f;
            }
        }
        private void AnimationEndlessFlashing()
        {
            if (AnimationCounter == 0)
            {
                addAlpha = true;
            }
            if (AnimationCounter == 255)
            {
                addAlpha = false;
            }
            if (addAlpha)
                AnimationCounter += animationSpeed;
            else
                AnimationCounter -= animationSpeed;
        }
        //-------------------------------------------------
        #endregion
    }
}
