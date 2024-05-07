namespace INTOnlineCoop.Script.Level.Tile
{
    /// <summary>
    /// Information about the atlas position of a tile
    /// </summary>
    public class TileLocationData
    {
        /// <summary>
        /// Creates new location data
        /// </summary>
        /// <param name="tileSetId">ID of the TileSet source</param>
        /// <param name="atlasX">X-coordinate in the atlas</param>
        /// <param name="atlasY">Y-coordinate in the atlas</param>
        /// <param name="alternativeId">ID of the alternative tile</param>
        public TileLocationData(int tileSetId, int atlasX = -1, int atlasY = -1, int alternativeId = -1)
        {
            TileSetId = tileSetId;
            AtlasX = atlasX;
            AtlasY = atlasY;
            AlternativeTileId = alternativeId;
        }

        /// <summary>
        /// Id of the TileSet layer
        /// </summary>
        public int TileSetId { get; }
        /// <summary>
        /// X-coordinate in the atlas
        /// </summary>
        public int AtlasX { get; }
        /// <summary>
        /// Y-coordinate in the atlas
        /// </summary>
        public int AtlasY { get; }
        /// <summary>
        /// Alternative tile id
        /// </summary>
        public int AlternativeTileId { get; }
    }
}
