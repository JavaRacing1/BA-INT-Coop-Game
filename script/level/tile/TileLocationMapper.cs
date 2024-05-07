using System.Collections.Generic;
using System.Collections.Immutable;

using Godot;

namespace INTOnlineCoop.Script.Level.Tile
{
    /// <summary>
    /// Available tile types
    /// </summary>
    public enum LevelTile
    {
        /// <summary>Test tile</summary>
        Test
    }

    /// <summary>
    /// Maps strings or image colors to tile data
    /// </summary>
    public static class TileLocationMapper
    {
        private static readonly ImmutableDictionary<Color, LevelTile> ColorMap = ImmutableDictionary.CreateRange(
            new[] { KeyValuePair.Create(Colors.Red, LevelTile.Test) }
        );

        /// <summary>
        /// Contains colors and its related terrain set id and terrain id
        /// </summary>
        private static readonly ImmutableDictionary<Color, (int, int)> ColorTerrainMap =
            ImmutableDictionary.CreateRange(
                new[] { KeyValuePair.Create(Colors.Red, (0, 0)) });

        private static readonly ImmutableDictionary<LevelTile, TileLocationData> TypeMap =
            ImmutableDictionary.CreateRange(
                new[] { KeyValuePair.Create(LevelTile.Test, new TileLocationData(0, 0, 0)) });

        /// <summary>
        /// Returns the location of the tile by a color
        /// </summary>
        /// <param name="color">Image color</param>
        /// <returns>The location data of the tile</returns>
        public static TileLocationData GetTileByColor(Color color)
        {
            if (!ColorMap.ContainsKey(color))
            {
                GD.PrintErr($"No tile found for color {color}!");
                return null;
            }

            LevelTile levelTile = ColorMap.GetValueOrDefault(color);
            return GetTileByType(levelTile);
        }

        /// <summary>
        /// Returns the location of the tile by a name
        /// </summary>
        /// <param name="levelTile"></param>
        /// <returns>The location data of the tile</returns>
        public static TileLocationData GetTileByType(LevelTile levelTile)
        {
            if (!TypeMap.ContainsKey(levelTile))
            {
                GD.PrintErr($"No tile found for type {levelTile}!");
                return null;
            }

            return TypeMap.GetValueOrDefault(levelTile);
        }

        /// <summary>
        /// Returns terrain data by a color
        /// </summary>
        /// <param name="color">Image color</param>
        /// <returns>Id of the terrain set and terrain id</returns>
        public static (int, int) GetTerrainByColor(Color color)
        {
            if (!ColorTerrainMap.ContainsKey(color))
            {
                GD.PrintErr($"No terrain found for color {color}!");
                return (-1, -1);
            }

            return ColorTerrainMap.GetValueOrDefault(color);
        }
    }
}