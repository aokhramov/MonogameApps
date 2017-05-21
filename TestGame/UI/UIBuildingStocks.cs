namespace TestGame.UI
{
    using lib;
    using Managers;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using System;
    using System.Collections.Generic;

    public class UIBuildingStocks : UIElement
    {
        public List<UITextureDemo> TexturesDemo;
        private UIFont Font;
        private Rectangle buildingStocksArea;
        private MapEntryTextureType currentTextureType = MapEntryTextureType.Stone;

        private Texture2D CornerLT = TextureManager.GetTexture(TextureType.UI, "CornerLT");
        private Texture2D CornerLB = TextureManager.GetTexture(TextureType.UI, "CornerLB");
        private Texture2D CornerRT = TextureManager.GetTexture(TextureType.UI, "CornerRT");
        private Texture2D CornerRB = TextureManager.GetTexture(TextureType.UI, "CornerRB");

        private Texture2D BorderT = TextureManager.GetTexture(TextureType.UI, "BorderT");
        private Texture2D BorderB = TextureManager.GetTexture(TextureType.UI, "BorderB");
        private Texture2D BorderL = TextureManager.GetTexture(TextureType.UI, "BorderL");
        private Texture2D BorderR = TextureManager.GetTexture(TextureType.UI, "BorderR");

        private Texture2D Default = TextureManager.GetTexture(TextureType.UI, "default");
        private Texture2D Frame = TextureManager.GetTexture(TextureType.UI, "BuildingStocksFrame");

        public bool IsActive { get; set; } = false;

        public UIBuildingStocks() 
            : this(new Vector2(0,0),Color.BurlyWood) { }

        public UIBuildingStocks(Vector2 position) 
            : this( position, Color.BurlyWood) { }

        public UIBuildingStocks(Vector2 position, Color color)
            : base(position, color)
        {
            Width = 345;
            Height = 180;

            Font = new UIFont("Материалы");
            Font.Size = 12;
            Font.Color = Color.Beige;

            TexturesDemo = new List<UITextureDemo>();
            TexturesDemo.Add(new UITextureDemo(TextureType.UI, "None"));
            TexturesDemo.Add(new UITextureDemo(TextureType.Tilesets, MapEntryTextureType.Stone.ToString()));
            TexturesDemo.Add(new UITextureDemo(TextureType.Tilesets, MapEntryTextureType.Silver.ToString()));
            TexturesDemo.Add(new UITextureDemo(TextureType.Tilesets, MapEntryTextureType.Gold.ToString()));
            TexturesDemo.Add(new UITextureDemo(TextureType.Tilesets, MapEntryTextureType.Dirt.ToString()));
            TexturesDemo.Add(new UITextureDemo(TextureType.Tilesets, MapEntryTextureType.Grass.ToString()));
            TexturesDemo.Add(new UITextureDemo(TextureType.Tilesets, MapEntryTextureType.BackDirt.ToString()));
            TexturesDemo.Add(new UITextureDemo(TextureType.Tilesets, MapEntryTextureType.BackWall.ToString()));
        }

        protected override void ComputeElement()
        {
            if (Position.X + Width > ScreenManager.Width)
                Position = new Vector2(ScreenManager.Width - Width - 1,Position.Y);

            if (Position.Y + Height > ScreenManager.Height)
                Position = new Vector2(Position.X, ScreenManager.Height - Height - 1);

            Font.Position = new Vector2(Position.X + Width / 2 - Font.Width / 2, Position.Y + 10);
            TexturesDemo[0].Position = new Vector2(Position.X + 5, Font.Position.Y + 20);
            TexturesDemo[1].Position = new Vector2(TexturesDemo[0].Position.X + TexturesDemo[0].Width + 5, Font.Position.Y + 20);
            TexturesDemo[2].Position = new Vector2(TexturesDemo[1].Position.X + TexturesDemo[1].Width + 5, Font.Position.Y + 20);
            TexturesDemo[3].Position = new Vector2(TexturesDemo[2].Position.X + TexturesDemo[2].Width + 5, Font.Position.Y + 20);
            TexturesDemo[4].Position = new Vector2(Position.X + 5, TexturesDemo[0].Position.Y + TexturesDemo[0].Height + 10);
            TexturesDemo[5].Position = new Vector2(TexturesDemo[4].Position.X + TexturesDemo[4].Width + 5, TexturesDemo[0].Position.Y + TexturesDemo[0].Height + 10);
            TexturesDemo[6].Position = new Vector2(TexturesDemo[5].Position.X + TexturesDemo[5].Width + 5, TexturesDemo[0].Position.Y + TexturesDemo[0].Height + 10);
            TexturesDemo[7].Position = new Vector2(TexturesDemo[6].Position.X + TexturesDemo[6].Width + 5, TexturesDemo[0].Position.Y + TexturesDemo[0].Height + 10);
            buildingStocksArea = new Rectangle((int)Position.X + BorderL.Width, (int)Position.Y + BorderT.Height, (int)Width - BorderL.Width, (int)Height - BorderT.Height);

            base.ComputeElement();
        }

        public override void ProcessingOfClicks(GameTime gameTime)
        {
            if (!IsActive)
                return;

            if (InputManager.MouseStates.LeftButton == ButtonState.Pressed &&
                InputManager.LastMouseStates.LeftButton == ButtonState.Released &&
                InputManager.MouseStates.X >= Position.X &&
                InputManager.MouseStates.Y >= Position.Y &&
                InputManager.MouseStates.X <= (Position.X + buildingStocksArea.Width) &&
                InputManager.MouseStates.Y <= (Position.Y + buildingStocksArea.Height))
            {
                foreach(UITextureDemo tdemo in TexturesDemo)
                {
                    tdemo.ProcessingOfClicks(gameTime);
                    if (tdemo.Clicked)
                    {
                        Clicked = true;
                        MapEntryTextureType tmp;
                        Enum.TryParse(tdemo.TextureTypeName, out tmp);
                        currentTextureType = tmp;
                    }
                }
            }
            else if (InputManager.IsNewPressedAnyMouseButton())
                IsActive = false;
            base.ProcessingOfClicks(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsActive)
                return;

            if (Compute)
            {
                ComputeElement();
            }

            foreach (UITextureDemo tdemo in TexturesDemo)
                tdemo.Update(gameTime);

            Font.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (!IsActive)
                return;

            ScreenManager.SpriteBatch.Begin();

            //corners
            ScreenManager.SpriteBatch.Draw(CornerLT, Position,
                null, Color * Alpha, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);//lefttop
            ScreenManager.SpriteBatch.Draw(CornerRT, Position + new Vector2(Width - 4, 0),
                null, Color * Alpha, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);//righttop
            ScreenManager.SpriteBatch.Draw(CornerLB, Position + new Vector2(0, Height - 4),
                null, Color * Alpha, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);//leftbot
            ScreenManager.SpriteBatch.Draw(CornerRB, Position + new Vector2(Width - 4, Height - 4),
                null, Color * Alpha, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);//rightbot

            Vector2 pos = Position;

            ScreenManager.SpriteBatch.Draw(Default,
                new Rectangle((int)pos.X + Default.Width, (int)pos.Y + Default.Height,
                (int)Width - CornerRT.Width - Default.Width,
                (int)Height - CornerRB.Height - Default.Height),
                Color * Alpha);

            pos = Position + new Vector2(CornerLT.Width, 0);
            //left to right

            ScreenManager.SpriteBatch.Draw(BorderT,
                new Rectangle((int)pos.X, (int)pos.Y,
                (int)Width - CornerRT.Width - Default.Width,
                BorderT.Height), Color * Alpha);

            ScreenManager.SpriteBatch.Draw(BorderB,
                new Rectangle((int)pos.X, (int)(pos.Y + Height - BorderB.Height),
                (int)(Width - CornerRT.Width - Default.Width),
                BorderB.Height), Color * Alpha);
            //top to bot
            pos = Position + new Vector2(0, CornerLT.Height);

            ScreenManager.SpriteBatch.Draw(BorderL,
                new Rectangle((int)pos.X, (int)pos.Y,
                BorderL.Width,
                (int)(Height - BorderL.Height - Default.Height)), Color * Alpha);

            ScreenManager.SpriteBatch.Draw(BorderR,
                new Rectangle((int)(pos.X + Width - CornerRT.Width), (int)pos.Y,
                BorderR.Width,
                (int)(Height - BorderR.Height - Default.Height)), Color * Alpha);


            ScreenManager.SpriteBatch.End();

            foreach (UITextureDemo tdemo in TexturesDemo)
            { 
                tdemo.Draw(gameTime);
                if(tdemo.TextureTypeName == currentTextureType.ToString())
                {
                    ScreenManager.SpriteBatch.Begin();
                    ScreenManager.SpriteBatch.Draw(Frame, new Vector2(tdemo.Position.X + tdemo.Width / 2 - Frame.Width / 2, tdemo.Position.Y + 6), 
                        null, Color.White * Alpha, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
                    ScreenManager.SpriteBatch.End();
                }
            }

            Font.Draw(gameTime);
            base.Draw(gameTime);
        }


        public MapEntryTextureType CurrentTextureType ()
        {
            return currentTextureType;
        }
    }
}
