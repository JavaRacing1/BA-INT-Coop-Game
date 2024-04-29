using Godot;

namespace INTOnlineCoop.Script.Level.Tile
{
    /// <summary>
    /// Manages the tiles of a level
    /// </summary>
    public partial class LevelTileManager : Node2D
    {
        [Export] private TileMap _tileMap;

        /// <summary>
        /// Initializes the tile map
        /// </summary>
        /// <param name="terrainImage">Image containing information about the terrain</param>
        public void InitTileMap(Image terrainImage)
        {
            for (int x = 0; x < terrainImage.GetWidth(); x++)
            {
                for (int y = 0; y < terrainImage.GetHeight(); y++)
                {
                    Color pixelColor = terrainImage.GetPixel(x, y);
                    if (pixelColor.A8 == 0)
                    {
                        continue;
                    }

                    TileLocationData locationData = TileLocationMapper.GetTileByColor(pixelColor);
                    if (locationData == null)
                    {
                        continue;
                    }

                    if (locationData.AlternativeTileId != -1)
                    {
                        _tileMap.SetCell(locationData.LayerId, new Vector2I(x, y), locationData.TileSetId,
                            alternativeTile: locationData.AlternativeTileId);
                    }
                    else
                    {
                        _tileMap.SetCell(locationData.LayerId, new Vector2I(x, y), locationData.TileSetId,
                            new Vector2I(locationData.AtlasX, locationData.AtlasY));
                    }
                }
            }
        }
    }
}