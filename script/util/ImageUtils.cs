using System.Collections.Generic;

using Godot;

namespace INTOnlineCoop.Script.Util
{
    /// <summary>
    /// Mathematical morphology operations
    /// </summary>
    public enum MorphologyOperation
    {
        /// <summary>Dilation filter</summary>
        Dilation,

        /// <summary>Erosion filter</summary>
        Erosion
    }

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
        public static void TerrainFloodFill(Image image, Stack<(int, int)> stack, Color fillColor,
            int alphaThreshold = 255)
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
        /// Applies a morthology operation to an image
        /// </summary>
        /// <param name="terrainImage">The image</param>
        /// <param name="operation">The used operation</param>
        /// <param name="radius">Size of the dilation</param>
        /// <returns>The processed image</returns>
        public static Image ApplyMorphologyOperation(Image terrainImage, MorphologyOperation operation, int radius = 2)
        {
            GD.Print($"Applying {operation}");
            radius = operation == MorphologyOperation.Erosion ? -radius : radius;
            Bitmap bitmap = new();
            bitmap.CreateFromImageAlpha(terrainImage);

            //Cache lowest 2 bit rows to restore them after erosion
            Bitmap pixelCache = null;
            if (operation == MorphologyOperation.Erosion)
            {
                pixelCache = new Bitmap();
                pixelCache.Create(terrainImage.GetSize());
                for (int x = 0; x < terrainImage.GetWidth(); x++)
                {
                    for (int y = terrainImage.GetHeight() - 2; y < terrainImage.GetHeight(); y++)
                    {
                        pixelCache.SetBit(x, y, bitmap.GetBit(x, y));
                    }
                }
            }

            Rect2I mask = new(Vector2I.Zero, terrainImage.GetSize());
            bitmap.GrowMask(radius, mask);
            if (pixelCache != null)
            {
                for (int x = 0; x < terrainImage.GetWidth(); x++)
                {
                    for (int y = terrainImage.GetHeight() - 2; y < terrainImage.GetHeight(); y++)
                    {
                        bitmap.SetBit(x, y, pixelCache.GetBit(x, y) && !bitmap.GetBit(x, y));
                    }
                }
            }

            return CreateImageFromBitmap(bitmap);
        }

        /// <summary>
        /// Computes the surface of an terrain
        /// </summary>
        /// <param name="image">The image of the terrain</param>
        /// <param name="airPixelAmount">Amount of transparent pixels required to count as surface</param>
        /// <param name="xAirOffset">Amount of air pixels required left or right of a surface point</param>
        /// <returns></returns>
        public static List<(int, int)> ComputeSurface(Image image, int airPixelAmount = 1, int xAirOffset = 0)
        {
            List<(int, int)> surfacePoints = new();
            for (int x = 0; x < image.GetWidth(); x++)
            {
                for (int y = 0; y < image.GetHeight(); y++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    if (pixelColor.ToRgba32() == Colors.Transparent.ToRgba32())
                    {
                        continue;
                    }

                    bool pixelIsValid = HasPixelEnoughAir(image, x, y, airPixelAmount);
                    bool enoughSpaceLeft = HasPixelEnoughAir(image, x, y, airPixelAmount, -xAirOffset);
                    bool enoughSpaceRight = HasPixelEnoughAir(image, x, y, airPixelAmount, xAirOffset);

                    if (pixelIsValid && !enoughSpaceLeft && !enoughSpaceRight)
                    {
                        pixelIsValid = false;
                    }

                    if (pixelIsValid)
                    {
                        surfacePoints.Add((x, y));
                    }
                }
            }

            return surfacePoints;
        }

        /// <summary>
        /// Checks if a pixel has a specific color
        /// </summary>
        /// <param name="image">The image</param>
        /// <param name="x">The x-coordinate of the pixel</param>
        /// <param name="y">The y-coordinate of the pixel</param>
        /// <returns>True if the pixel is not transparent</returns>
        public static bool HasPixelColor(Image image, int x, int y)
        {
            return x >= 0 && x < image.GetWidth() && y >= 0 && y < image.GetHeight() &&
                   image.GetPixel(x, y).A8 != 0;
        }

        /// <summary>
        /// Checks if a pixel has enough air above it
        /// </summary>
        /// <param name="image">The image containing the pixel</param>
        /// <param name="x">The x-coordinate of the pixel</param>
        /// <param name="y">The y-coordinate of the pixel</param>
        /// <param name="airAmount">Needed amount of air</param>
        /// <param name="xOffset">Offset to the x-coordinate</param>
        /// <returns>True if the pixel has enough air above it</returns>
        public static bool HasPixelEnoughAir(Image image, int x, int y, int airAmount, int xOffset = 0)
        {
            for (int yOffset = 1; yOffset <= airAmount; yOffset++)
            {
                if (HasPixelColor(image, x + xOffset, y - yOffset))
                {
                    return false;
                }
            }

            return true;
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