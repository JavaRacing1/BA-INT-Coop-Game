using System;
using System.Collections.Generic;

using Godot;

using INTOnlineCoop.Script.Util;

namespace INTOnlineCoop.Script.Level
{
    /// <summary>
    /// Shape of the terrain
    /// </summary>
    public enum TerrainShape
    {
        /// <summary>One small island</summary>
        OneIslandSmall,

        /// <summary>One medium island</summary>
        OneIslandMedium,

        /// <summary>One big island</summary>
        OneIslandBig,

        /// <summary>Two small islands</summary>
        TwoIslandsSmall,

        /// <summary>Two medium islands</summary>
        TwoIslandsMedium,

        /// <summary>Two big islands</summary>
        TwoIslandsBig,

        /// <summary>One medium island + one floating island</summary>
        OneFloatingIsland,

        /// <summary>One medium island + two floating islands</summary>
        TwoFloatingIslands,

        /// <summary>Multiple floating islands</summary>
        MultipleFloatingIslands
    }

    /// <summary>
    /// Generates a level with a given terrain shape
    /// </summary>
    public partial class LevelGenerator : RefCounted
    {
        private bool _debugModeActive;
        private FastNoiseLite _noiseGenerator;
        private float _noiseThreshold = 20.0f;

        private TerrainShape _selectedTerrainShape;
        private bool _changedTerrainShape;
        private Stack<(int, int)> _foregroundContourPixels;
        private Stack<(int, int)> _backgroundBorderPixels;

        /// <summary>
        /// Creates a new generator with a TerrainShape
        /// </summary>
        public LevelGenerator()
        {
            _noiseGenerator = new FastNoiseLite()
            {
                NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin,
                Frequency = 0.06f,
                FractalType = FastNoiseLite.FractalTypeEnum.None
            };
        }

        /// <summary>
        /// Changes the terrain shape
        /// </summary>
        /// <param name="shape">TerrainShape which the generator should use</param>
        public void SetTerrainShape(TerrainShape shape)
        {
            _selectedTerrainShape = shape;
            _changedTerrainShape = true;
        }

        /// <summary>
        /// Enables the debug mode. When the debug mode is active, all generated images will be saved to
        /// "output/{TerrainShape}/"
        /// </summary>
        public void EnableDebugMode()
        {
            _debugModeActive = true;
        }

        /// <summary>
        /// Generates a terrain with the settings of the generator
        /// </summary>
        /// <returns>An image containing information about the terrain</returns>
        public Image Generate(int seed)
        {
            if (!_changedTerrainShape)
            {
                GD.PrintErr("Terrain shape of LevelGenerator was not set!");
                return null;
            }
            GD.Print("Generating terrain for shape " + _selectedTerrainShape);
            Image templateImage = LoadTerrainTemplate();
            _noiseGenerator.Seed = seed;
            Image terrainImage = GenerateNoiseImage(templateImage);
            SaveImage(terrainImage, "1_noise");

            ImageUtils.TerrainFloodFill(terrainImage,
                new Stack<(int, int)>(new Stack<(int, int)>(_foregroundContourPixels)),
                Colors.Red);
            SaveImage(terrainImage, "2_fill");

            ImageUtils.ReplaceColor(terrainImage, Colors.Yellow, Colors.Transparent);
            SaveImage(terrainImage, "3_no_bg");

            terrainImage = ImageUtils.ApplyMorphologyOperation(terrainImage, MorphologyOperation.Dilation);
            SaveImage(terrainImage, "4_dilation");

            GD.Print("Filling holes");
            ImageUtils.TerrainFloodFill(terrainImage,
                new Stack<(int, int)>(new Stack<(int, int)>(_backgroundBorderPixels)),
                Colors.Yellow);
            ImageUtils.ReplaceColor(terrainImage, Colors.Transparent, Colors.Red);
            ImageUtils.ReplaceColor(terrainImage, Colors.Yellow, Colors.Transparent);
            SaveImage(terrainImage, "5_filled_holes");

            terrainImage = ImageUtils.ApplyMorphologyOperation(terrainImage, MorphologyOperation.Erosion);
            SaveImage(terrainImage, "6_erosion");
            GD.Print("Terrain generation done!");
            return terrainImage;
        }

        private Image GenerateNoiseImage(Image templateImage)
        {
            GD.Print("Applying perlin noise to template");
            int width = templateImage.GetWidth();
            int height = templateImage.GetHeight();
            Image noiseImage = Image.Create(width, height, false, Image.Format.Rgba8);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Color pixel = templateImage.GetPixel(x, y);

                    //Green pixel -> land must be generated
                    if (pixel.G8 == 255)
                    {
                        noiseImage.SetPixel(x, y, Colors.Red);
                        continue;
                    }

                    //Generate first layer with a bigger resolution
                    _noiseGenerator.Frequency = 0.07f;
                    float noiseValue = Math.Abs(_noiseGenerator.GetNoise2D(x, y));
                    noiseValue = Math.Max(0, (25 - (noiseValue * 256)) * 8);

                    //Generate second layer with a lower resolution to prevent large bumps in the black zone
                    if (pixel.B8 == 0)
                    {
                        _noiseGenerator.Frequency = 0.1f;
                        float noiseValue2 = Math.Abs(_noiseGenerator.GetNoise2D(width - x - 1, y));
                        noiseValue2 = Math.Max(0, (25 - (noiseValue2 * 256)) * 8);
                        noiseValue = (noiseValue + noiseValue2) / 2.0f;
                    }

                    Color terrainPixel = noiseValue > _noiseThreshold ? Colors.Yellow : Colors.Transparent;
                    noiseImage.SetPixel(x, y, terrainPixel);
                }
            }
            GD.Print("Noise applied!");
            return noiseImage;
        }

        private Image LoadTerrainTemplate()
        {
            GD.Print("Loading terrain template");
            Texture2D templateTexture =
                GD.Load<Texture2D>($"res://assets/texture/level/{_selectedTerrainShape}.png");
            Image templateImage = templateTexture.GetImage();
            CalculateTemplateEdges(templateImage);
            GD.Print("Loaded!");
            return templateImage;
        }

        private void CalculateTemplateEdges(Image template, int colorThreshold = 100)
        {
            _foregroundContourPixels = new Stack<(int, int)>();
            _backgroundBorderPixels = new Stack<(int, int)>();
            int width = template.GetWidth();
            int height = template.GetHeight();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Color pixel = template.GetPixel(x, y);
                    if (pixel.G8 < colorThreshold)
                    {
                        if (template.GetPixel(Math.Max(0, x - 1), y).G8 > colorThreshold ||
                            template.GetPixel(Math.Min(width - 1, x + 1), y).G8 > colorThreshold ||
                            template.GetPixel(x, Math.Max(0, y - 1)).G8 > colorThreshold ||
                            template.GetPixel(x, Math.Min(height - 1, y + 1)).G8 > colorThreshold)
                        {
                            _foregroundContourPixels.Push((x, y));
                        }
                        else if (pixel.B8 < colorThreshold && (x == 0 || y == 0 || x == width - 1 || y == height - 1))
                        {
                            _backgroundBorderPixels.Push((x, y));
                        }
                    }
                }
            }
        }

        private void SaveImage(Image image, String name)
        {
            if (_debugModeActive)
            {
                string path = $"res://output/{_selectedTerrainShape}/";
                using DirAccess dirAccess = DirAccess.Open("res://");
                if (dirAccess != null)
                {
                    if (!dirAccess.DirExists("output") || !dirAccess.DirExists($"output/{_selectedTerrainShape}"))
                    {
                        _ = dirAccess.MakeDirRecursive($"output/{_selectedTerrainShape}");
                    }
                    _ = image.SavePng($"{path}/{name}.png");
                }
            }
        }
    }
}