namespace TestGame.UI
{
    using Managers;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using System.Collections.Generic;
    using System.Text;
    using lib;

    public class charEntry
    {
        public Point point;
        public Vector2 position;

        public charEntry() { }
    }

    public class UIFont : UIElement
    {
        #region Fieds and Properties
        //-------------------------------------------------
        private int charWidth;
        private int charHeight;

        private float spacingBetweenLetters = 1.0f;
        /// <summary>
        /// Растояние между буквами
        /// </summary>
        public float SpacingBetweenLetters
        {
            get { return spacingBetweenLetters; }
            set { spacingBetweenLetters = value; }
        }
        private float spacingBetweenWords = 5.0f;
        /// <summary>
        /// Расстояние между словами
        /// </summary>
        public float SpacingBetweenWords
        {
            get { return spacingBetweenWords; }
            set { spacingBetweenWords = value; }
        }
        private Rectangle tmp;

        private FontType type;
        public FontType Type
        {
            get { return type; }
            set
            {
                type = value;
                Texture = ScreenManager.Content.Load<Texture2D>(string.Format("{0}{1}", "Font/", type.ToString()));
                Compute = true;
            }
        }
        private string text;
        private List<charEntry> font;
        /// <summary>
        /// Отображаемый текст
        /// </summary>
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                Compute = true;
            }
        }
        private float size = 1.0f;
        /// <summary>
        /// Размер текста (по умл. 14)
        /// </summary>
        public int Size
        {
            set
            {
                size = (float) value / 14;
                Compute = true;
            }
        }
        //-------------------------------------------------
        #endregion

        #region Constructors
        //-------------------------------------------------
        public UIFont()
            : this("", new Vector2(0,0), Color.Black) {}

        public UIFont(string text)
            : this(text, new Vector2(0, 0), Color.Black) {}

        public UIFont(string text, Vector2 position)
            : this(text, position, Color.Black) {}

        public UIFont(string text, Vector2 position, Color color)
            : base(position,color)
        {
            Text = text;
            Type = FontType.Regular;
        }
        //-------------------------------------------------
        #endregion
        #region Methods
        //-------------------------------------------------
        //ищем букву на текстуре
        private Point charTexturePoint(byte charnumber)
        {
            string charstr = charnumber.ToString();
            byte corrector = 0;
            int k = 0;
            int m = 2;
            int p = 0;
            if (charnumber < 33)
                return new Point(0,0);
            if (charnumber > 126 && charnumber < 192 && charnumber != 168 && charnumber != 184)
                return new Point(0, 0);

            if (charnumber >= 33 && charnumber < 127)
                corrector = 3;
            else
                corrector = 9;

            switch(Type)
            {
                case FontType.Regular: k = 4; p = 4; break;
                case FontType.RegularWithBorder: k = 2; m = 1; p = 2; break;
                case FontType.RegularWithShadow: k = 2; p = 2; break;
            }

            int i = 0;
            int j = 0;
            int pos = charstr.Length - 1;
            int.TryParse(charstr.Substring(0,pos), out i);
            int.TryParse(charstr.Substring(pos), out j);
            int outi = m + (i - corrector) * charHeight + ((i - corrector) * p);
            int outj = m + j * charWidth + j * k;
            if(charnumber == 168)
            {
                outi = 308;
                outj = 2;
            }
            if (charnumber == 184)
            {
                outi = 308;
                outj = 16;
            }

            return new Point(outj, outi);
        }
        protected override void ComputeElement()
        {
            if(text != null)
            {
                font = null;
                font = new List<charEntry>();

                if (Type == FontType.Regular)
                { 
                    spacingBetweenLetters = 1.0f;
                    charWidth = 10;
                    charHeight = 14;
                }
                if (Type == FontType.RegularWithBorder)
                { 
                    spacingBetweenLetters = 1.0f;
                    charWidth = 12;
                    charHeight = 16;
                }
                if (Type == FontType.RegularWithShadow)
                { 
                    spacingBetweenLetters = 0.0f;
                    charWidth = 12;
                    charHeight = 16;
                }
                byte[] Chars = Encoding.GetEncoding(1251).GetBytes(text);
                Vector2 currentPosition = Position;
                int j = 0;
                Height = 0f;
                for (int i = 0; i < Chars.Length; i++)
                {
                    if (Chars[i] == 32)//space
                        currentPosition += new Vector2(spacingBetweenWords, 0);
                    else
                    {
                        var nfont = new charEntry();
                        font.Add(nfont);
                        font[j].point = charTexturePoint(Chars[i]);
                        font[j].position = new Vector2((int)currentPosition.X, (int)currentPosition.Y);
                        currentPosition += new Vector2(charWidth * size + spacingBetweenLetters, 0);
                        j++;
                    }
                    Height = System.Math.Max(Height, charHeight * size);
                }
                Width = currentPosition.X - Position.X;
            }
            base.ComputeElement();
        }

        public override void Update(GameTime gameTime)
        {
            if (Compute)
            {
                ComputeElement();
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (font == null)
                return;

            foreach (charEntry currentChar in font)
            {
                ScreenManager.SpriteBatch.Begin();
                tmp = new Rectangle(currentChar.point.X, currentChar.point.Y, charWidth, charHeight);

                ScreenManager.SpriteBatch.Draw(Texture,
                        currentChar.position,
                        tmp,
                        Color * Alpha,
                        0,
                        Vector2.Zero,
                        size,
                        SpriteEffects.None,
                        0);
                ScreenManager.SpriteBatch.End();
            }
            base.Draw(gameTime);
        }
        //-------------------------------------------------
        #endregion
    }
}
