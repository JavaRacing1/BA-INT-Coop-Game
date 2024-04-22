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
        /// <param name="alphaThreshold">The alpha color threshold for pixel comparison</param>
        public static void TerrainFloodFill(Image image, Stack<(int, int)> stack, Color fillColor, int alphaThreshold = 255)
        {
            GD.Print("Filling terrain");
            int width = image.GetWidth();
            int height = image.GetHeight();

            while (stack.Count > 0)
            {
                (int, int) pixel = stack.Pop();
                int x = pixel.Item1;
                int y = pixel.Item2;
                while (x >= 0 && image.GetPixel(x, y).A8 < alphaThreshold)
                {
                    x--;
                }
                x++;
                bool spanAboveAdded = false;
                bool spanBelowAdded = false;
                while (x < width && image.GetPixel(x, y).A8 < alphaThreshold)
                {
                    image.SetPixel(x, y, fillColor);
                    if (!spanAboveAdded && y > 0 && image.GetPixel(x, y - 1).A8 < alphaThreshold)
                    {
                        stack.Push((x, y - 1));
                        spanAboveAdded = true;
                    }
                    else if (spanAboveAdded && y > 0 && image.GetPixel(x, y - 1).A8 >= alphaThreshold)
                    {
                        spanAboveAdded = false;
                    }

                    if (!spanBelowAdded && y < height - 1 &&
                        image.GetPixel(x, y + 1).A8 < alphaThreshold)
                    {
                        stack.Push((x, y + 1));
                        spanBelowAdded = true;
                    }
                    else if (spanBelowAdded && y < height - 1 && image.GetPixel(x, y + 1).A8 >= alphaThreshold)
                    {
                        spanBelowAdded = false;
                    }

                    x++;
                }
            }
            GD.Print("Terrain filled!");
        }

        /// <summary>
        /// Replaces a color in an image
        /// </summary>
        /// <param name="image">The image</param>
        /// <param name="oldColor">Color which should be replaced</param>
        /// <param name="newColor">Color used for replacement</param>
        public static void ReplaceColor(Image image, Color oldColor, Color newColor)
        {
            GD.Print($"Replacing color {oldColor} with {newColor}");
            int width = image.GetWidth();
            int height = image.GetHeight();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    if (pixelColor.ToRgba32() == oldColor.ToRgba32())
                    {
                        image.SetPixel(x, y, newColor);
                    }
                }
            }
        }

        /// <summary>
        /// Applies dilation to an image
        /// </summary>
        /// <param name="terrainImage">The image</param>
        /// <param name="radius">Size of the dilation</param>
        /// <returns></returns>
        public static Image ApplyDilation(Image terrainImage, int radius = 2)
        {
            GD.Print("Applying dilation");
            Bitmap bitmap = new();
            bitmap.CreateFromImageAlpha(terrainImage);
            Rect2I mask = new(Vector2I.Zero, terrainImage.GetSize());
            bitmap.GrowMask(radius, mask);
            return CreateImageFromBitmap(bitmap);
        }

        private static Image CreateImageFromBitmap(Bitmap bitmap)
        {
            Image image = bitmap.ConvertToImage();
            image.Convert(Image.Format.Rgba8);
            for (int x = 0; x < image.GetWidth(); x++)
            {
                for (int y = 0; y < image.GetHeight(); y++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    if (pixelColor.ToRgba32() == Colors.White.ToRgba32())
                    {
                        image.SetPixel(x, y, Colors.Red);
                    }
                    else if (pixelColor.ToRgba32() == Colors.Black.ToRgba32())
                    {
                        image.SetPixel(x, y, Colors.Transparent);
                    }
                }
            }

            return image;
        }
    }
}
