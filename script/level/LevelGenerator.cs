using System;
using System.Collections.Generic;

using Godot;

using INTOnlineCoop.Script.Util;

namespace INTOnlineCoop.Script.Level
{
    /// <summary>
    /// Shape of the terrain
    /// </summary>
    public enum TerrainType
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
        TwoFloatingIslands
    }

    /// <summary>
    /// Generates a level with a given terrain type
    /// </summary>
    public partial class LevelGenerator : RefCounted
    {
        private FastNoiseLite _noiseGenerator;
        private float _noiseThreshold = 20.0f;

        private TerrainType _selectedTerrainType;
        private Stack<(int, int)> _foregroundContourPixels;
        private Stack<(int, int)> _backgroundBorderPixels;

        /// <summary>
        /// Creates a new generator with a TerrainType
        /// </summary>
        public LevelGenerator()
        {
            /*
            _noiseGenerator = new FastNoiseLite()
            {
                NoiseType = FastNoiseLite.NoiseTypeEnum.Simplex,
                Frequency = 0.05f,
                FractalType = FastNoiseLite.FractalTypeEnum.Ridged,
                FractalOctaves = 1
            };
            */
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
        /// <param name="type">TerrainType which the generator should use</param>
        public void SetTerrainType(TerrainType type)
        {
            _selectedTerrainType = type;
        }


        /// <summary>
        /// Generates a terrain with the settings of the generator
        /// </summary>
        public void Generate(int seed)
        {
            GD.Print("Generating terrain for type " + _selectedTerrainType);
            Image templateImage = LoadTerrainTemplate();
            _noiseGenerator.Seed = seed;
            Image terrainImage = GenerateNoiseImage(templateImage);
            _ = terrainImage.SavePng($"res://output/{_selectedTerrainType}/noise.png");

            ImageUtils.TerrainFloodFill(terrainImage,
                new Stack<(int, int)>(new Stack<(int, int)>(_foregroundContourPixels)),
                Colors.Red);
            _ = terrainImage.SavePng($"res://output/{_selectedTerrainType}/fill.png");

            ImageUtils.ReplaceColor(terrainImage, Colors.Yellow, Colors.Black);
            _ = terrainImage.SavePng($"res://output/{_selectedTerrainType}/no_bg.png");
            GD.Print("Terrain generation done!");
        }

        private Image GenerateNoiseImage(Image templateImage)
        {
            GD.Print("Applying perlin noise to template");
            int width = templateImage.GetWidth();
            int height = templateImage.GetHeight();
            Image noiseImage = Image.Create(width, height, false, Image.Format.Rgb8);
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

                    Color terrainPixel = noiseValue > _noiseThreshold ? Colors.Yellow : Colors.Black;
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
                GD.Load<Texture2D>($"res://assets/texture/level/{_selectedTerrainType}.png");
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
    }
}