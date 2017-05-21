namespace TestGame.Managers
{
    using lib;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class SpriteList
    {
        /// <summary>
        /// Ширина спрайта
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Высота спрайта
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Точка в которой начинается спрайтлист (левый верхний)
        /// </summary>
        public Point StartPoint { get; set; }

        /// <summary>
        /// Количество спрайтов в строке
        /// </summary>
        public int CountSprites { get; set; }

        public Color Color { get; set; }

        /// <summary>
        /// Анимация приостановлена?
        /// </summary>
        public bool Pause { get; set; } = false;

        /// <summary>
        /// Нужно ли в методе Draw использовать SpriteBatch.Begin() ?
        /// </summary>
        public bool NeedBeginEndSpriteBatchCall = true;

        private string sequence;

        /// <summary>
        /// Последовательность отображения спрайтов по номерам ( прим. 0,1,2,3,4,2,1,5,0)
        /// </summary>
        public string Sequence
        {
            get { return sequence; }
            set
            {
                sequence = value;
                ParseSequence(sequence);
            }
        }

        /// <summary>
        /// Текущий номер спрайта
        /// </summary>
        public int CurrentSprite { get; set; }

        /// <summary>
        /// Дистанция между нарисованными спрайтами в текстуре (пиксели)
        /// </summary>
        public int DistanceBetweenSprites { get; set; } = 10;

        /// <summary>
        /// Период через который меняется спрайт анимации
        /// </summary>
        public int AnimationPeriod { get; set; } = 70;

        /// <summary>
        /// Отражение по горизонтали
        /// </summary>
        public bool FlipHorizontally { get; set; } = false;

        /// <summary>
        /// Использовать позиционирование относительно матрицы отображения
        /// </summary>
        public bool UseViewMatrix { get; set; } = true;

        /// <summary>
        /// Обратить анимацию в обратную сторону?
        /// </summary>
        public bool Reverse { get; set; } = false;

        private SpriteAnimationType animationType = SpriteAnimationType.Loop;

        /// <summary>
        /// Тип анимации (бесконечный цикл, повтор N раз)
        /// </summary>
        public SpriteAnimationType AnimationType
        {
            get { return animationType; }
            set
            {
                animationType = value;
                _currentLoop = 0;
            }
        }

        /// <summary>
        /// Количество повторов последовательности, если выбран не бесконечный цикл повтора последовательности
        /// </summary>
        public int NTimes { get; set; } = 0;

        private bool restartLoop = false;

        /// <summary>
        /// Рестарт анимации
        /// </summary>
        public bool RestartLoop
        {
            private get { return restartLoop; }
            set
            {
                restartLoop = value;
                if (restartLoop)
                    if (Reverse && _sequenceLength > 0)
                        _index = _sequenceLength - 1;
                    else
                        _index = 0;
                _currentLoop = 0;
                restartLoop = false;
            }
        }

        private Texture2D _texture;
        private string _textureName;
        private int _index = 0;
        private int _currentLoop;
        private int _sequenceLength = 0;
        private int _currentTime = 0;

        public SpriteList(string textureName) : this (textureName, new Point(0,0)){ }

        public SpriteList(string textureName, Point startPoint)
        {
            _textureName = textureName;
            if(_textureName.Substring(0,2) == "UI")
                _texture = TextureManager.GetTexture(TextureType.UI, _textureName);
            else
                _texture = TextureManager.GetTexture(TextureType.Sprites, _textureName);

            StartPoint = startPoint;
            Color = Color.White;
        }

        /// <summary>
        /// Обновление спрайта (если указана последовательность)
        /// </summary>
        /// <param name="gameTime">Время</param>
        public void Update(GameTime gameTime)
        {
            if (Pause)
                return;

            if(Sequence != null)
            {
                if(Reverse)
                {
                    if (_index < 0 && _sequenceLength > 0)
                    {
                        _index = _sequenceLength - 1;
                        _currentLoop++;
                    }
                }
                else
                {
                    if (_index == _sequenceLength && _sequenceLength > 0)
                    {
                        _index = 0;
                        _currentLoop++;
                    }
                }
                if (AnimationType == SpriteAnimationType.RepeatNTimes && _currentLoop >= NTimes)
                    return;
                _currentTime += gameTime.ElapsedGameTime.Milliseconds;

                if (_currentTime > AnimationPeriod)
                { 
                    _currentTime -= AnimationPeriod;
                    ParseSequence(Sequence);

                    if (Reverse)
                        _index--;
                    else
                        _index++;
                }
            }
        }

        /// <summary>
        /// Рисуем спрайт
        /// </summary>
        /// <param name="gameTime">Время</param>
        /// <param name="Position">Позиция</param>
        public void Draw (GameTime gameTime,Vector2 Position)
        {
            if(NeedBeginEndSpriteBatchCall)
            {
                if (UseViewMatrix)
                    ScreenManager.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, ScreenManager.viewMatrix);
                else
                    ScreenManager.SpriteBatch.Begin();
            }

            Rectangle tmp = new Rectangle(StartPoint.X + (Width * CurrentSprite) + (DistanceBetweenSprites * CurrentSprite), StartPoint.Y, Width , Height);

            ScreenManager.SpriteBatch.Draw(_texture, Position, tmp, Color, 0, Vector2.Zero, 1f, (FlipHorizontally) ? SpriteEffects.FlipHorizontally : SpriteEffects.None , 0);

            if (NeedBeginEndSpriteBatchCall)
                ScreenManager.SpriteBatch.End();
        }

        /// <summary>
        /// Парсим строку с последовательностью
        /// </summary>
        /// <param name="sequence"></param>
        private void ParseSequence(string sequence)
        {
            int i = 0;
            string[] elements = sequence.Split(new char[] { ',' });

            if (_sequenceLength != elements.Length)
                _sequenceLength = elements.Length;
            if (CountSprites != elements.Length)
                CountSprites = elements.Length;
            if(_index >= 0 && _index < CountSprites)
                if (elements.Length > 0 && elements[_index] != null && elements[_index].Length > 0)
                    int.TryParse(elements[_index], out i);

            CurrentSprite = i;
        }

    }
}
