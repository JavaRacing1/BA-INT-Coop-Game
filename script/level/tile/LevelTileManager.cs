using System.Collections.Generic;

using Godot;
using Godot.Collections;

namespace INTOnlineCoop.Script.Level.Tile
{
    /// <summary>
    /// Manages the tiles of a level
    /// </summary>
    public partial class LevelTileManager : Node2D
    {
        [Export] private TileMapLayer _tileMap;

        /// <summary>
        /// Initializes the tile map
        /// </summary>
        /// <param name="terrainImage">Image containing information about the terrain</param>
        /// <param name="useTerrains">If terrains should be used instead of tiles</param>
        public void InitTileMap(Image terrainImage, bool useTerrains = false)
        {
            if (terrainImage == null)
            {
                return;
            }
            System.Collections.Generic.Dictionary<Color, Array<Vector2I>> pixelCache = new();
            for (int x = 0; x < terrainImage.GetWidth(); x++)
            {
                for (int y = 0; y < terrainImage.GetHeight(); y++)
                {
                    Color pixelColor = terrainImage.GetPixel(x, y);
                    if (pixelColor.A8 == 0)
                    {
                        continue;
                    }

                    if (useTerrains)
                    {
                        if (!pixelCache.ContainsKey(pixelColor))
                        {
                            pixelCache.Add(pixelColor, new Array<Vector2I>());
                        }
                        pixelCache.GetValueOrDefault(pixelColor).Add(new Vector2I(x, y));
                    }
                    else
                    {
                        TileLocationData locationData = TileLocationMapper.GetTileByColor(pixelColor);
                        if (locationData == null)
                        {
                            continue;
                        }

                        if (locationData.AlternativeTileId != -1)
                        {
                            _tileMap.SetCell(new Vector2I(x, y), locationData.TileSetId,
                                alternativeTile: locationData.AlternativeTileId);
                        }
                        else
                        {
                            _tileMap.SetCell(new Vector2I(x, y), locationData.TileSetId,
                                new Vector2I(locationData.AtlasX, locationData.AtlasY));
                        }
                    }

                }
            }

            if (useTerrains)
            {
                foreach (Color color in pixelCache.Keys)
                {
                    (int, int) terrainInformation = TileLocationMapper.GetTerrainByColor(color);
                    if (terrainInformation == (-1, -1))
                    {
                        continue;
                    }
                    Array<Vector2I> cachedCells = pixelCache.GetValueOrDefault(color);
                    _tileMap.SetCellsTerrainConnect(cachedCells, terrainInformation.Item1, terrainInformation.Item2);
                }
            }
        }

        /// <summary>
        /// Returns the tile size of the TileMap
        /// </summary>
        /// <returns>Current tile size</returns>
        public Vector2I GetTileSize()
        {
            return _tileMap == null ? Vector2I.Zero : _tileMap.TileSet.TileSize;
        }
    }
}
