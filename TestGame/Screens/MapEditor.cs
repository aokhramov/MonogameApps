namespace TestGame.Screens
{
    using lib;
    using GameObjects;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Managers;
    using Microsoft.Xna.Framework.Input;
    using System.Xml;
    using System.IO;
    using System.Reflection;

    public class MapEditor : Map
    {
        /// <summary>
        /// Меню конструктора карты
        /// </summary>
        private MapEditorMenu mapEditorMenu { get; set; }

        public MapEditor (string mapName)
            : base (mapName)
        {
            ScreenType = ScreenType.MapEditor;

            mapEditorMenu = new MapEditorMenu();
            mapEditorMenu.IsActive = false;
        }

        /// <summary>
        /// Сохранение карты
        /// </summary>
        public void SaveMap()
        {
            string Location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = string.Format("{0}{1}{2}.xml", Location, "\\Content\\Maps\\", MapName);
            string str = "";
            string a, b, c;

            using (XmlWriter xmlWriter = XmlWriter.Create(path))
            {
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("Map");
                xmlWriter.WriteElementString("Width", Width.ToString());
                xmlWriter.WriteElementString("Height", Height.ToString());
                for (int i = 0; i < height; i++)
                {
                    xmlWriter.WriteStartElement("Row");

                    for (int j = 0; j < width; j++)
                    {
                        a = map[i, j].CollideTextureType.ToString().Replace("None", "");
                        b = map[i, j].NonCollideTextureType.ToString().Replace("None", "");
                        c = map[i, j].DecorateTextureType.ToString().Replace("None", "");

                        str = string.Format("{0};{1};{2}", a, b, c);
                        xmlWriter.WriteElementString("Cell", str);
                    }

                    xmlWriter.WriteEndElement();
                }

                foreach (Item item in Items)
                {
                    xmlWriter.WriteStartElement("Item");
                    xmlWriter.WriteElementString("ItemType", item.Type.ToString());
                    xmlWriter.WriteElementString("Item_X", item.Position.X.ToString());
                    xmlWriter.WriteElementString("Item_Y", item.Position.Y.ToString());
                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
            }
        }

        public override void ProcessingOfClicks(GameTime gameTime, Camera camera)
        {
            if (!IsActive)
                return;

            UInterface.ProcessingOfClicks(gameTime, camera);

            if (UInterface.ClickDetected)
                return;

            if (InputManager.IsNewPressed(Keys.Escape))
            {
                mapEditorMenu.IsActive = true;
            }

            mapEditorMenu.ProcessingOfClicks(gameTime, camera);

            if (mapEditorMenu.Save)
            {
                mapEditorMenu.Save = false;
                SaveMap();
            }

            //тыкнули за границы окна - выводим меню
            if (!ScreenManager.SpriteBatch.GraphicsDevice.Viewport.Bounds.Contains(new Point(InputManager.MouseStates.Position.X, InputManager.MouseStates.Position.Y))
                && InputManager.IsPressedAnyMouseButton())
                mapEditorMenu.IsActive = true;

            if (InputManager.MouseStates.LeftButton == ButtonState.Pressed)
            {
                int y = (int)((camera.Position.Y + InputManager.MouseStates.Y / camera.CameraMultiplier) / TextureManager.TileSize);
                int x = (int)((camera.Position.X + InputManager.MouseStates.X / camera.CameraMultiplier) / TextureManager.TileSize);
                Vector2 pos = CellPosition(x, y);
                y = (int)pos.Y / TextureManager.TileSize;
                x = (int)pos.X / TextureManager.TileSize;

                SetTexture(UInterface.BuildingStocks.CurrentTextureType(), x, y);
                ClickDetected = true;
            }

            base.ProcessingOfClicks(gameTime, camera);
        }

        public override void Update(GameTime gameTime, Camera camera)
        {
            if (!IsActive)
                return;
            camera.DisableTracking();

            if (mapEditorMenu.IsActive)
            {
                mapEditorMenu.Update(gameTime, camera);
                return;
            }

            Sky = new Rectangle(0, 0, Width * TextureManager.TileSize, Height * TextureManager.TileSize);
            background.Update(gameTime, camera);
            background.Color = WorldColor;
            background.Width = Width * TextureManager.TileSize;
            background.Height = Height * TextureManager.TileSize;
            UInterface.Update(gameTime, camera);

            for (int i = ScreenManager.Point_Y1; i < ScreenManager.Point_Y2; i++)
            {
                for (int j = ScreenManager.Point_X1; j < ScreenManager.Point_X2; j++)
                {
                    if (map[i, j].Refresh)
                    {
                        map[i, j].Refresh = false;
                        UpdateTile(i, j);
                        UpdateTile(i - 1, j);
                        UpdateTile(i, j - 1);
                        UpdateTile(i + 1, j);
                        UpdateTile(i, j + 1);
                    }
                }
            }
            foreach (Item item in Items)
                item.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, Camera camera)
        {
            if (!IsActive)
                return;

            ScreenManager.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, ScreenManager.viewMatrix);

            ScreenManager.SpriteBatch.Draw(Default, Sky, WorldColor);
            background.Draw(gameTime, camera);


            for (int i = ScreenManager.Point_Y1; i < ScreenManager.Point_Y2; i++)
            {
                for (int j = ScreenManager.Point_X1; j < ScreenManager.Point_X2; j++)
                {

                    map[i, j].Draw(gameTime, WorldColor);
                }
            }

            foreach (Item item in Items)
                item.Draw(gameTime, WorldColor);

            ScreenManager.SpriteBatch.End();

            UInterface.Draw(gameTime, camera);

            mapEditorMenu.Draw(gameTime, camera);
        }
    }
}
