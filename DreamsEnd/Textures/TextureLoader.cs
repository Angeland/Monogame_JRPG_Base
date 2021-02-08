using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using DreamsEnd.States.AutoElements;
using DreamsEnd.States.Characters;
using Microsoft.Xna.Framework;

namespace DreamsEnd.Textures
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
                new Tile("Graphics/Maps/WorldMap/Overlay/Land/Grass", new TileProperties{
                TileColor = new Color(34, 177, 76),
                Walkable = true
                }),
                /*
                new Tile("Graphics/Maps/WorldMap/Overlay/Land/Grass2", new TileProperties{
                tileColor = new Color(34, 177, 76),
                Walkable = true
                }),*/
                new Tile("Graphics/Maps/WorldMap/Overlay/Land/Sand", new TileProperties{
                TileColor = new Color(239, 228, 176),
                Walkable = true
                }),
                new Tile("Graphics/Maps/WorldMap/Overlay/Water/Shallow", new TileProperties{
                TileColor = new Color(153, 217, 234),
                Walkable = true
                }),
                new Tile("Graphics/Maps/WorldMap/Overlay/Water/Ocean", new TileProperties{
                TileColor = new Color(0, 162, 232),
                Sailable = true
                }),
                new Tile("Graphics/Maps/WorldMap/Overlay/Water/DeepOcean", new TileProperties{
                TileColor = new Color(63, 72, 204),
                Sailable = true
                }),
                new Tile("Graphics/Maps/WorldMap/Overlay/City/CityGround", new TileProperties{
                TileColor = new Color(195, 195, 195),
                Walkable = true
                }),
                new Tile("Graphics/Maps/WorldMap/Overlay/City/CityWall", new TileProperties{
                TileColor = new Color(127, 127, 127),
                SubTexturePath = "Graphics/Maps/WorldMap/Overlay/City/CityGround"
                }),
                new Tile("Graphics/Maps/WorldMap/Overlay/City/Harbour", new TileProperties{
                TileColor = new Color(185, 122, 87),
                Walkable = true
                }),
                new Tile("Graphics/Maps/WorldMap/Overlay/City/House/Roof/RoofStoneUp", new TileProperties{
                TileColor = new Color(201, 151, 124)
                }),
                new Tile("Graphics/Maps/WorldMap/Overlay/City/House/Roof/RoofStoneDown", new TileProperties{
                TileColor = new Color(206, 175, 119)
                }),
                new Tile("Graphics/Maps/WorldMap/Overlay/City/House/Roof/RoofStoneRight", new TileProperties{
                TileColor = new Color(216, 179, 158)
                }),
                new Tile("Graphics/Maps/WorldMap/Overlay/City/House/Roof/RoofStoneLeft", new TileProperties{
                TileColor = new Color(218, 188, 156)
                }),
                new Tile("Graphics/Maps/WorldMap/Overlay/City/House/Roof/RoofStoneCornerUpRight", new TileProperties{
                TileColor = new Color(170, 105, 70)
                }),
                new Tile("Graphics/Maps/WorldMap/Overlay/City/House/Roof/RoofMiddleVertical", new TileProperties{
                TileColor = new Color(174, 0, 27)
                }),
                new Tile("Graphics/Maps/WorldMap/Overlay/City/House/Roof/RoofMiddleHorisontal", new TileProperties{
                TileColor = new Color(136, 0, 21)
                }),
                new Tile("Graphics/Maps/WorldMap/Overlay/City/House/Roof/RoofMiddleCornerUpRight", new TileProperties{
                TileColor = new Color(91, 0, 14)
                })
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
