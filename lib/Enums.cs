namespace lib
{
    /// <summary>
    /// Тип текстуры (наименование) в ячейке
    /// </summary>
    public enum MapEntryTextureType
    {
        None,
        Stone,
        Gold,
        Silver,
        Dirt,
        Grass,
        BackDirt,
        BackWall
    }

    /// <summary>
    /// Тип повторяемости анимации спрайта
    /// </summary>
    public enum SpriteAnimationType
    {
        Loop,
        RepeatNTimes
    }

    /// <summary>
    /// Тип текстуры
    /// </summary>
    public enum TextureType
    {
        Font,
        UI,
        Images,
        Sprites,
        Tilesets
    }

    /// <summary>
    /// Тип экрана
    /// </summary>
    public enum ScreenType
    {
        Empty,
        Logo,
        Map,
        MapEditor,
        UserInterface,
        MainMenu,
        GameMenu,
        MapEditorMenu
    }


    /// <summary>
    /// Тип анимации для элемента интерфейса
    /// </summary>
    public enum AnimationType
    {
        None,
        Open,
        Close,
        ShortFlashing,
        EndlessFlashing
    }

    /// <summary>
    /// Расположение контента внутри элемента интерфейса
    /// </summary>
    public enum ContentAlignment
    {
        BottomCenter,
        BottomLeft,
        BottomRight,
        MiddleCenter,
        MiddleLeft,
        MiddleRight,
        TopCenter,
        TopLeft,
        TopRight
    }

    /// <summary>
    /// Тип шрифта
    /// </summary>
    public enum FontType
    {
        Regular,
        RegularWithBorder,
        RegularWithShadow
    }

    /// <summary>
    /// Тип уровня
    /// </summary>
    public enum LevelType
    {
        Solo,
        MultiPlayer,
        MapEditor
    }

    /// <summary>
    /// Тип предмета
    /// </summary>
    public enum ItemType
    {
        TestItem
    }

}

