namespace TestGame.Screens
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Xml;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using lib;
    using Managers;
    using System.Collections.Generic;
    using GameObjects;

    public class Map : Screen
    {
        /// <summary>
        /// Наименование карты
        /// </summary>
        public string MapName { get; set; }

        //дефолтная текстура для рисования
        private Texture2D Default = TextureManager.GetTexture(TextureType.UI, "default");
        /// <summary>
        /// Отрисовка неба (цвет)
        /// </summary>
        Rectangle Sky;
        /// <summary>
        /// Задние фоны
        /// </summary>
        Background background;
        /// <summary>
        /// Включить тени
        /// </summary>
        public bool ShadowsEnable { get; private set; } = true;

        private int width;
        /// <summary>
        /// Ширина карты - количество столбцов
        /// </summary>
        public int Width
        {
            get { return width; }
        }
        private int height;
        /// <summary>
        /// Высота карты - количество строк
        /// </summary>
        public int Height
        {
            get { return height; }
        }
        /// <summary>
        /// Карта уровня
        /// </summary>
        public MapEntry[,] map;

        /// <summary>
        /// Текущий общий оттенок уровня
        /// </summary>
        public Color WorldColor { get; set; } = new Color(255,225,225);

        /// <summary>
        /// Персонаж пользователя
        /// </summary>
        public Character Player { get; set; }
        /// <summary>
        /// Пользовательский интерфейс
        /// </summary>
        public UserInterface UInterface { get; private set; }

        /// <summary>
        /// Предметы на уровне
        /// </summary>
        public List<Item> Items { get; set; }

        /// <summary>
        /// Игровое меню
        /// </summary>
        private GameMenu gameMenu { get; set; }

        /// <summary>
        /// Меню конструктора карты
        /// </summary>
        private MapEditorMenu mapEditorMenu { get; set; }


        private bool widthInitiated = false;
        private bool heightInitiated = false;

        public Map (string mapName)
        {
            ScreenType = ScreenType.Map;
            MapName = mapName;
            background = new Background();
            Player = new Character();
            UInterface = new UserInterface();
            //UInterface.uiswitch.Click += ChangeShadowsState;

            gameMenu = new GameMenu();
            gameMenu.IsActive = false;
            mapEditorMenu = new MapEditorMenu();
            mapEditorMenu.IsActive = false;

            Items = new List<Item>();

            LoadMap(MapName);
        }
        private void ReCreateArray()
        {
            map = null;
            map = new MapEntry[ height, width ];

            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    map[i, j] = new MapEntry();
                    map[i, j].X = j;
                    map[i, j].Y = i;
                    map[i, j].Position = new Vector2(0, 0) + new Vector2(TextureManager.TileSize * j, TextureManager.TileSize * i);
                }

            widthInitiated = false;
            heightInitiated = false;
            TextureManager.countHorizontalTiles = width;
            TextureManager.countVerticalTiles = height;
        }
        /// <summary>
        /// Загрузка карты из xml файла
        /// </summary>
        /// <param name="mapName">Название xml файла с уровнем, .xml указывать не нужно</param>
        public void LoadMap(string mapName)
        {
            int i , j;
            string Location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = string.Format("{0}{1}{2}.xml", Location , "\\Content\\Maps\\" ,mapName );
            string attr;

            using (XmlReader reader = XmlReader.Create(path))
            {
                reader.MoveToContent();
                i = 0;
                j = 0;
                while (reader.Read())
                {
                    
                    switch(reader.Name)
                    {
                        case "Width":
                            if (reader.IsStartElement())
                            { 
                                reader.Read();
                                if (int.TryParse(reader.Value, out width))
                                    widthInitiated = true;
                            }
                            break;
                        case "Height":
                            if (reader.IsStartElement())
                            { 
                                reader.Read();
                                if (int.TryParse(reader.Value, out height))
                                    heightInitiated = true;
                            }
                            break;
                        case "Row":
                            if (!reader.IsStartElement())
                            { 
                                i++;
                                j = 0;
                            }
                            else
                            {
                                attr = reader.GetAttribute("id");
                                if (attr != null)
                                    int.TryParse(attr, out i);
                            }
                            break;
                        case "Cell":
                            if (!reader.IsStartElement())
                                j++;
                            else
                            {
                                attr = reader.GetAttribute("id");
                                if (attr != null)
                                    int.TryParse(attr, out j);

                                reader.Read();
                                ParseCell(map[i, j], reader.Value);
                                
                            }
                            break;
                        case "ItemType":
                            if(reader.IsStartElement())
                            {
                                reader.Read();
                                ItemType tmp;
                                Enum.TryParse(reader.Value, out tmp);
                                Items.Add(new Item(tmp));
                            }
                            break;
                        case "Item_X":
                            if (reader.IsStartElement())
                            {
                                reader.Read();
                                float x;
                                if (float.TryParse(reader.Value, out x))
                                    Items[Items.Count - 1].Position = new Vector2(x, Items[Items.Count - 1].Position.Y);
                            }
                            break;
                        case "Item_Y":
                            if (reader.IsStartElement())
                            {
                                reader.Read();
                                float y;
                                if (float.TryParse(reader.Value, out y))
                                    Items[Items.Count - 1].Position = new Vector2(Items[Items.Count - 1].Position.X, y);
                            }
                            break;
                    }
                    if (widthInitiated && heightInitiated)
                        ReCreateArray();
                }
            }
        }
        /// <summary>
        /// Разбираем строку <Cell>...</Cell> из файла и укладываем в текущую ячейку карты уровня
        /// </summary>
        /// <param name="mapEntry">Ячейка карты</param>
        /// <param name="cell">Строка со значением ячейки из файла</param>
        private void ParseCell(MapEntry mapEntry, string cell)
        {
            MapEntryTextureType mapEntryTextureType;
            string[] elements = cell.Split(new char[] { ';' });

            //NonCollideTexture (первой грузим чтобы не перекрыла CollideTexture)
            if (elements.Length > 0 && elements[1] != null && elements[1].Length > 0)
            {
                mapEntryTextureType = MapEntryTextureType.None;
                if (Enum.TryParse(elements[1], out mapEntryTextureType))
                    SetTexture(mapEntryTextureType, mapEntry.X, mapEntry.Y);
            }

            //CollideTexture
            if (elements.Length > 0 && elements[0] != null && elements[0].Length > 0)
            {
                mapEntryTextureType = MapEntryTextureType.None;
                if (Enum.TryParse(elements[0], out mapEntryTextureType))
                    SetTexture(mapEntryTextureType, mapEntry.X, mapEntry.Y);
            }

            //DecorateTexture
            if (elements.Length > 0 && elements[2] != null && elements[2].Length > 0)
            {
                mapEntryTextureType = MapEntryTextureType.None;
                if (Enum.TryParse(elements[2], out mapEntryTextureType))
                    SetTexture(mapEntryTextureType, mapEntry.X, mapEntry.Y);
            }

        }

        /// <summary>
        /// Сохранение карты
        /// </summary>
        public void SaveMap ()
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
                        a = map[i, j].CollideTextureType.ToString().Replace("None","");
                        b = map[i, j].NonCollideTextureType.ToString().Replace("None", "");
                        c = map[i, j].DecorateTextureType.ToString().Replace("None", "");

                        str = string.Format("{0};{1};{2}", a, b, c);
                        xmlWriter.WriteElementString("Cell", str);
                    }

                    xmlWriter.WriteEndElement();
                }

                foreach(Item item in Items)
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
            if(mapEditorMenu.Save)
            {
                mapEditorMenu.Save = false;
                SaveMap();
            }

            //тыкнули за границы окна - выводим меню
            if (!ScreenManager.SpriteBatch.GraphicsDevice.Viewport.Bounds.Contains(new Point(InputManager.MouseStates.Position.X, InputManager.MouseStates.Position.Y))
                && InputManager.IsPressedAnyMouseButton())
                mapEditorMenu.IsActive = true;

            if (InputManager.IsNewLeftMouseButtonPressed() || 
                (InputManager.MouseStates.LeftButton == ButtonState.Pressed && InputManager.IsPressed(Keys.LeftShift)))
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

        /// <summary>
        /// Обновляем карту. Ищем ячейки, которые нужно обновить и обновляем, соседние ячейки так же обновляются
        /// </summary>
        /// <param name="gameTime">Время</param>
        public override void Update(GameTime gameTime, Camera camera)
        {
            if (!IsActive)
                return;

            if(mapEditorMenu.IsActive)
            {
                mapEditorMenu.Update(gameTime, camera);
                return;
            }
            if(gameMenu.IsActive)
            { 
                gameMenu.Update(gameTime, camera);
                return;
            }

            Sky = new Rectangle(0, 0, Width * TextureManager.TileSize, Height * TextureManager.TileSize);
            Player.Update(gameTime, map);
            Player.Color = WorldColor;
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
            
            base.Update(gameTime, camera);
        }
        /// <summary>
        /// Устанавливаем текстуру в ячейку карты
        /// </summary>
        /// <param name="mapEntryTexture">Тип текстуры</param>
        /// <param name="x">Адрес ячейки. Столбец</param>
        /// <param name="y">Адрес ячейки. Строка</param>
        public void SetTexture (MapEntryTextureType mapEntryTexture, int x, int y)
        {
            if (ScreenManager.ActiveScreen != ScreenType.Map)
                return;

            map[y, x].ChangeType(mapEntryTexture);
            map[y, x].Refresh = true;
        }
        /// <summary>
        /// Проверяем ячейку на наличие текстуры
        /// </summary>
        /// <param name="x">Адрес ячейки. Столбец</param>
        /// <param name="y">Адрес ячейки. Строка</param>
        /// <returns></returns>
        private bool CheckCell(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return true;

            if (map[y, x].CollidingTexture != null || map[y, x].NonCollidingTexture != null)
                return true;
            else
                return false;
        }
        /// <summary>
        /// Обновляем ячейку карты. Соседние ячейки берутся в расчет
        /// </summary>
        /// <param name="x">Адрес ячейки. Столбец</param>
        /// <param name="y">Адрес ячейки. Строка</param>
        private void UpdateTileUseNearbyCells (int x , int y)
        {
            bool left = false, up = false, right = false, down = false;
            left = CheckCell(x - 1, y);
            up = CheckCell(x, y - 1);
            right = CheckCell(x + 1, y);
            down = CheckCell(x, y + 1);

            
            if (map[y, x].CollidingTexture == null && map[y, x].NonCollidingTexture == null)
                return;

            map[y, x].TilesRow = 0; // окружен со всех сторон
            if (!down && !left && !right && !up)
            {
                map[y, x].TileStartPoint = new Point(map[y, x].TileStartPoint.X, 0);
                return;
            }

            if (down && !left && !right && !up)
                map[y, x].TilesRow = 1;

            if (down && left && right && !up)
                map[y, x].TilesRow = 2;

            if (down && left && !right && !up)
                map[y, x].TilesRow = 3;

            if (!down && left && !right && !up)
                map[y, x].TilesRow = 4;

            if (down && left && !right && up)
                map[y, x].TilesRow = 5;

            if (!down && left && !right && up)
                map[y, x].TilesRow = 6;

            if (!down && !left && !right && up)
                map[y, x].TilesRow = 7;

            if (!down && left && right && up)
                map[y, x].TilesRow = 8;

            if (!down && !left && right && up)
                map[y, x].TilesRow = 9;

            if (!down && !left && right && !up)
                map[y, x].TilesRow = 10;

            if (down && !left && right && up)
                map[y, x].TilesRow = 11;

            if (down && !left && right && !up)
                map[y, x].TilesRow = 12;

            if (!down && left && right && !up)
                map[y, x].TilesRow = 13;

            if (down && !left && !right && up)
                map[y, x].TilesRow = 14;

            map[y, x].TileStartPoint = new Point(map[y, x].TileStartPoint.X, map[y, x].TilesRow);
        }
        /// <summary>
        /// Обновляем тайл в ячейке
        /// </summary>
        /// <param name="y">Адрес ячейки. Строка</param>
        /// <param name="x">Адрес ячейки. Столбец</param>
        private void UpdateTile(int y, int x)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return;

            if (map[y, x].WatchNearbyCells)
                UpdateTileUseNearbyCells(x, y);

            CalcShadow(x, y);
        }
        /// <summary>
        /// Расчет тени в ячейке карты
        /// </summary>
        /// <param name="x">Адрес ячейки. Столбец</param>
        /// <param name="y">Адрес ячейки. Строка</param>
        private void CalcShadow(int x, int y)
        {
            map[y, x].Shadow = 0;
            if (!ShadowsEnable || x < 0 || x >= Width || y < 0 || y >= Height)
                return;

            bool left = false, leftOut = false;
            bool up = false, upOut = false;
            bool right = false, rightOut = false;
            bool down = false, downOut = false;

            if (x - 1 < 0)
            {
                leftOut = true;
                left = true;
            }
            else if (map[y,x-1].CollidingTexture != null)
                left = true;

            if (y - 1 < 0)
            {
                upOut = true;
                up = true;
            }
            else if (map[y - 1, x].CollidingTexture != null)
                up = true;

            if (x + 1 >= Width)
            {
                rightOut = true;
                right = true;
            }
            else if (map[y, x + 1].CollidingTexture != null)
                right = true;

            if (y + 1 >= Height)
            {
                downOut = true;
                down = true;
            }
            else if (map[y + 1, x].CollidingTexture != null)
                down = true;


            if ((map[y, x].CollidingTexture == null && map[y, x].NonCollidingTexture != null) && ((left && !leftOut) || (up && !upOut) || (right && !rightOut) || (down && !downOut)))
                map[y, x].Shadow = 25;

            if (map[y, x].CollidingTexture != null && left && up && right && down)
                map[y, x].Shadow = 225;
        }
        /// <summary>
        /// Рисуем карту (только видимую часть)
        /// </summary>
        /// <param name="gameTime">Время</param>
        /// <param name="camera">Камера с матрицой отображения</param>
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

            Player.Draw(gameTime);
            ScreenManager.SpriteBatch.End();

            UInterface.Draw(gameTime, camera);

            mapEditorMenu.Draw(gameTime, camera);
            gameMenu.Draw(gameTime, camera);

            base.Draw(gameTime, camera);
        }
        /// <summary>
        /// Корректная позиция ячейки. Запрещает рисовать за пределами карты
        /// </summary>
        /// <param name="x">Адрес ячейки. Столбец</param>
        /// <param name="y">Адрес ячейки. Строка</param>
        /// <returns>Корректный адрес ячейки</returns>
        public Vector2 CellPosition( int x , int y)
        {
            return map[Math.Min(Math.Max(y,0),Height-1) , Math.Min(Math.Max(x,0),Width-1)].Position;
        }
        /// <summary>
        /// Тени - вкл, выкл
        /// </summary>
        /// <param name="sender"></param>
        public void ChangeShadowsState(object sender)
        {
            ShadowsEnable = !ShadowsEnable;
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    map[i, j].Refresh = true;
            UInterface.ShadowsEnable = ShadowsEnable;
        }
    }
}
