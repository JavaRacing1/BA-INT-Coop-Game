using System;

using Godot;

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
            Generate();
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
        public void Generate()
        {
            _noiseGenerator.Seed = new Random().Next();
            Texture2D templateTexture =
                GD.Load<Texture2D>($"res://assets/texture/level/{_selectedTerrainType}.png");
            Image templateImage = templateTexture.GetImage();
            Image noiseImage = GenerateNoiseImage(templateImage);
            _ = noiseImage.SavePng($"res://output/{_selectedTerrainType}/noise.png");
        }

        private Image GenerateNoiseImage(Image templateImage)
        {
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
                    Color terrainPixel = noiseValue > _noiseThreshold ? Colors.Green : Colors.Black;
                    noiseImage.SetPixel(x, y, terrainPixel);
                }
            }

            return noiseImage;
        }
    }
}
