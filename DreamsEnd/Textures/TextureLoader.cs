using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RPG.States.AutoElements;
using RPG.States.Characters;

namespace RPG.Textures
{
    public class TextureLoader
    {
        private readonly ContentManager Content;

        public TextureLoader(ContentManager content)
        {
            Content = content;
        }

        public IButton[] MainMenu()
        {
            return new IButton[]
            {
                new Button(Content, "Menu/easy","Menu/easyActive" , 0, -0.5f),
                new Button(Content, "Menu/medium","Menu/mediumActive",0, 0.5f )
            };
        }
        public Minimap Minimap()
        {
            return new Minimap(Content, "Graphics/Maps/WorldMap/Minimap", "Graphics/Misc/Minimap/MinimapCursor");
        }
        public ITilesCollection WorldMapTiles()
        {
            ITile[] tiles = new ITile[]
            {
                new Tile("Graphics/Maps/WorldMap/Overlay/Land/Grass"),
                new Tile("Graphics/Maps/WorldMap/Overlay/Land/Sand"),
                new Tile("Graphics/Maps/WorldMap/Overlay/Water/Shallow"),
                new Tile("Graphics/Maps/WorldMap/Overlay/Water/Ocean"),
                new Tile("Graphics/Maps/WorldMap/Overlay/Water/DeepOcean"),
                new Tile("Graphics/Maps/WorldMap/Overlay/City/CityGround"),
                new Tile("Graphics/Maps/WorldMap/Overlay/City/CityWall"),
                new Tile("Graphics/Maps/WorldMap/Overlay/City/Harbour"),
                new Tile("Graphics/Maps/WorldMap/Overlay/City/House/Roof/RoofStoneUp"),
                new Tile("Graphics/Maps/WorldMap/Overlay/City/House/Roof/RoofStoneDown"),
                new Tile("Graphics/Maps/WorldMap/Overlay/City/House/Roof/RoofStoneRight"),
                new Tile("Graphics/Maps/WorldMap/Overlay/City/House/Roof/RoofStoneLeft"),
                new Tile("Graphics/Maps/WorldMap/Overlay/City/House/Roof/RoofStoneCornerUpRight"),
                new Tile("Graphics/Maps/WorldMap/Overlay/City/House/Roof/RoofMiddleVertical"),
                new Tile("Graphics/Maps/WorldMap/Overlay/City/House/Roof/RoofMiddleHorisontal"),
                new Tile("Graphics/Maps/WorldMap/Overlay/City/House/Roof/RoofMiddleCornerUpRight")
            };
            return new TileCollection(Content, tiles);
        }
        public ITilesCollection Clouds()
        {
            ITile[] tiles = new ITile[]
            {
                new Tile("Graphics/Maps/WorldMap/Overlay/Cloud/cloud1")
            };
            return new TileCollection(Content, tiles);
        }
        public ITilesCollection WorldMaps()
        {
            ITile[] tiles = new ITile[]
            {
                new Tile("Graphics/Maps/WorldMap/BaseMap"),
                new Tile("Graphics/Maps/WorldMap/BaseMap1"),
                new Tile("Graphics/Maps/WorldMap/BaseMap2")
            };
            return new TileCollection(Content, tiles);
        }
        public IWorldCharacter WorldCharacter()
        {
            return new World_Character(Content, new string[] { "Graphics/People/PC/baseChar" });
        }
        public WorldAutoElements WorldAutoElements()
        {
            WorldAutoElements autoElements = new WorldAutoElements();
            autoElements.Init(Content.Load<Texture2D>("Graphics/Items/Transportation/smallBoat"));
            return autoElements;
        }
    }
}
