using System;

using Godot;

namespace INTOnlineCoop.Script.Level
{
    /// <summary>
    /// Manager class of the level
    /// </summary>
    public partial class GameLevel : Node2D
    {
        /// <summary>
        /// Initializes the level instance
        /// </summary>
        /// <param name="terrainImage">Image containing the shape of the terrain</param>
        public void Init(Image terrainImage)
        {
            //Placeholder to prevent errors -> Remove when implementing GameLevel
            _ = terrainImage;

            GD.Print("GameLevel initialized!");
        }

        private static void OnGenerationButtonPressed()
        {
            //Level generator tests
            LevelGenerator generator = new();
            PlayerPositionGenerator positionGenerator = new();
            generator.EnableDebugMode();
            foreach (TerrainShape type in Enum.GetValues<TerrainShape>())
            {
                generator.SetTerrainShape(type);
                int seed = new Random().Next();
                Image image = generator.Generate(seed);
                positionGenerator.Init(image, type.ToString(), true);

                double positionSeed = new Random().NextDouble();
                for (int i = 0; i < 5; i++)
                {
                    (double, double) pos = positionGenerator.GetSpawnPosition(positionSeed);
                    GD.Print(pos);
                }
            }
        }
    }
}

