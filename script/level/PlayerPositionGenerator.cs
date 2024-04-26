using System.Collections.Generic;

using Godot;

using INTOnlineCoop.Script.Util;

namespace INTOnlineCoop.Script.Level
{
    /// <summary>
    /// 
    /// </summary>
    public partial class PlayerPositionGenerator : RefCounted
    {
        /// <summary>
        /// Initializes the position generator
        /// </summary>
        /// <param name="image">Generated terrain image</param>
        /// <param name="terrainName">Name of the terrain type</param>
        /// <param name="debugMode">true if generated images should be saved</param>
        public void Init(Image image, string terrainName, bool debugMode = false)
        {
            List<(int, int)> surfacePoints = ImageUtils.ComputeSurface(image, airPixelAmount: 4);
            surfacePoints = RemoveSecludedSurfacePoints(surfacePoints, image.GetWidth(), image.GetHeight());

            if (debugMode)
            {
                Image debugImage = (Image)image.Duplicate();
                foreach ((int, int) pixel in surfacePoints)
                {
                    debugImage.SetPixel(pixel.Item1, pixel.Item2, Colors.Yellow);
                }

                _ = debugImage.SavePng($"res://output/{terrainName}/8_filtered_surface.png");
            }
        }

        private static List<(int, int)> RemoveSecludedSurfacePoints(List<(int, int)> surfacePoints, int xSize,
            int ySize)
        {
            Bitmap bitmap = new();
            bitmap.Create(new Vector2I(xSize, ySize));
            foreach ((int, int) pixel in surfacePoints)
            {
                bitmap.SetBit(pixel.Item1, pixel.Item2, true);
            }

            List<(int, int)> filteredPixels = new();
            foreach ((int, int) pixel in surfacePoints)
            {
                if (PixelHasSurfaceNeighbors(bitmap, pixel))
                {
                    filteredPixels.Add(pixel);
                }
            }

            return filteredPixels;
        }

        private static bool PixelHasSurfaceNeighbors(Bitmap bitmap, (int, int) pixel)
        {
            int x = pixel.Item1;
            int y = pixel.Item2;

            for (int xOffset = -1; xOffset <= 1; xOffset++)
            {
                for (int yOffset = -1; yOffset <= 1; yOffset++)
                {
                    int combinedX = x + xOffset;
                    int combinedY = y + yOffset;

                    if ((combinedX == x && combinedY == y) || combinedX < 0 || combinedX >= bitmap.GetSize().X ||
                        combinedY < 0 || combinedY >= bitmap.GetSize().Y)
                    {
                        continue;
                    }

                    if (bitmap.GetBit(combinedX, combinedY))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}