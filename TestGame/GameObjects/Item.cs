namespace TestGame.GameObjects
{
    using Microsoft.Xna.Framework;
    using Managers;
    using lib;

    public class Item
    {
        /// <summary>
        /// Анимация предмета
        /// </summary>
        public SpriteList animation;

        /// <summary>
        /// Позиция предмета
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Тип предмета
        /// </summary>
        public ItemType Type { get; set; }

        public delegate void EventHandler(object sender);
        /// <summary>
        /// Событие при использовании предмета
        /// </summary>
        public event EventHandler Use;
        
        public Item(ItemType itemType)
        {
            Type = itemType;
            animation = SelectItem(itemType);
            Position = new Vector2(0, 0);
            animation.NeedBeginEndSpriteBatchCall = false;
        }
        /// <summary>
        /// Уточняем параметры предмета
        /// </summary>
        /// <param name="itemType"></param>
        /// <returns></returns>
        private SpriteList SelectItem (ItemType itemType)
        {
            SpriteList CurrentSpriteList = new SpriteList(itemType.ToString());
            switch (itemType)
            {
                case ItemType.TestItem:
                    CurrentSpriteList.Sequence = "0,0,1,1,2,2,3,3,4,4,3,3,2,2,1,1";
                    CurrentSpriteList.CountSprites = 5;
                    CurrentSpriteList.Width = 24;
                    CurrentSpriteList.Height = 24;
                    break;
            }
            return CurrentSpriteList;
        }

        public void Update(GameTime gameTime)
        {
            animation.Update(gameTime);
        }

        public void Draw(GameTime gameTime, Color WorldColor)
        {
            animation.Color = WorldColor;
            animation.Draw(gameTime,Position);
        }
        
    }
}
