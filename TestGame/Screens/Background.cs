namespace TestGame.Screens
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Managers;
    using lib;
    using System;

    public class Background : Screen
    {
        //TODO Background
        public Texture2D bgTexture1 = TextureManager.GetTexture(TextureType.Images, "Background1");
        public Texture2D bgTexture2 = TextureManager.GetTexture(TextureType.Images, "Background2");
        public Texture2D bgTexture3 = TextureManager.GetTexture(TextureType.Images, "Background3");

        private Rectangle r_BG1_1;
        private Rectangle r_BG1_2;
        private Rectangle r_BG2_1;
        private Rectangle r_BG2_2;
        private Rectangle r_BG3_1;
        private Rectangle r_BG3_2;

        private float speed1 = 0.3f;
        private float speed2 = 0.2f;
        private float speed3 = 0.1f;

        public Color Color = Color.White;
        public int Width = 0;
        public int Height = 0;
        public Background(){}

        public override void Update(GameTime gameTime, Camera camera)
        {
            if (!IsActive)
                return;

            float xs = (int)Math.Max(0, camera.BindedObject.Position.X);
            float xe = xs + (ScreenManager.Width / camera.CameraMultiplier);
            xe = Math.Min(xe, Width);

            float dx = xs * speed3;
            while (dx > bgTexture3.Width)
                dx -= bgTexture3.Width;

            float dex = Math.Min(bgTexture3.Width - dx,  xe - xs );

            //TODO Позиционирование прямоугольников.
            r_BG3_1 = new Rectangle((int)dx, 0, (int)dex, (bgTexture3.Height));
            //r_BG3_2 = new Rectangle(xp , 0, xe - xp, (bgTexture3.Height));

            //r_BG1_1 = new Rectangle((int)(x * speed1), 0, (int)(bgTexture1.Width - x * speed1), (bgTexture1.Height));

            //r_BG2_1 = new Rectangle((int)(x * speed2), 0, (int)(bgTexture2.Width - x * speed2), (bgTexture2.Height));

            base.Update(gameTime, camera);
        }

        public override void Draw(GameTime gameTime,Camera camera)
        {
            if (!IsActive)
                return;

            float x = Math.Max(0, camera.Position.X);

            ScreenManager.SpriteBatch.Draw(bgTexture3, new Vector2(x , Height - bgTexture3.Height - 0), r_BG3_1, Color, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            //ScreenManager.SpriteBatch.Draw(bgTexture3, new Vector2(x , Height - bgTexture3.Height - 0), r_BG3_2, Color, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);

            //ScreenManager.SpriteBatch.Draw(bgTexture2, new Vector2(x * speed2, Height - bgTexture2.Height - 300), null, Color, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            //ScreenManager.SpriteBatch.Draw(bgTexture1, new Vector2(x * speed1, Height - bgTexture1.Height), null, Color, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);

            base.Draw(gameTime,camera);
        }
    }
}
