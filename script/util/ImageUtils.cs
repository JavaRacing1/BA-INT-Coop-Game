using System.Collections.Generic;

using Godot;

namespace INTOnlineCoop.Script.Util
{
    /// <summary>
    /// Utilities for editing images
    /// </summary>
    public static class ImageUtils
    {
        /// <summary>
        /// Applies the flood fill algorithm to an image.
        /// <br/><br/>
        /// The flood fill algorithm starts with one pixel and jumps to the leftmost pixel with the same color in the current row.
        /// After that, it will replace all pixels to the rightmost point with the same color.
        /// During the filling process, it searches for spans below/above the current row and adds them to the working stack.
        /// </summary>
        /// <param name="image">The image</param>
        /// <param name="stack">Stack containing all target pixels</param>
        /// <param name="fillColor">The color applied to all found pixels</param>
        /// <param name="colorThreshold">The red color threshold for pixel comparison</param>
        public static void TerrainFloodFill(Image image, Stack<(int, int)> stack, Color fillColor, int colorThreshold = 255)
        {
            GD.Print("Filling terrain");
            int width = image.GetWidth();
            int height = image.GetHeight();

            while (stack.Count > 0)
            {
                (int, int) pixel = stack.Pop();
                int x = pixel.Item1;
                int y = pixel.Item2;
                while (x >= 0 && image.GetPixel(x, y).R8 < colorThreshold)
                {
                    x--;
                }
                x++;
                bool spanAboveAdded = false;
                bool spanBelowAdded = false;
                while (x < width && image.GetPixel(x, y).R8 < colorThreshold)
                {
                    image.SetPixel(x, y, fillColor);
                    if (!spanAboveAdded && y > 0 && image.GetPixel(x, y - 1).R8 < colorThreshold)
                    {
                        stack.Push((x, y - 1));
                        spanAboveAdded = true;
                    }
                    else if (spanAboveAdded && y > 0 && image.GetPixel(x, y - 1).R8 >= colorThreshold)
                    {
                        spanAboveAdded = false;
                    }

                    if (!spanBelowAdded && y < height - 1 &&
                        image.GetPixel(x, y + 1).R8 < colorThreshold)
                    {
                        stack.Push((x, y + 1));
                        spanBelowAdded = true;
                    }
                    else if (spanBelowAdded && y < height - 1 && image.GetPixel(x, y + 1).R8 >= colorThreshold)
                    {
                        spanBelowAdded = false;
                    }

                    x++;
                }
            }
            GD.Print("Terrain filled!");
        }
    }
}
