using System;

using Godot;

namespace INTOnlineCoop.Script.Level
{
    /// <summary>
    /// Manager class of the level
    /// </summary>
    public partial class GameLevel : Node2D
    {
        private static void OnGenerationButtonPressed()
        {
            //Level generator tests
            LevelGenerator generator = new();
            PlayerPositionGenerator positionGenerator = new();
            generator.EnableDebugMode();
            foreach (TerrainType type in Enum.GetValues<TerrainType>())
            {
                generator.SetTerrainType(type);
                int seed = new Random().Next();
                Image image = generator.Generate(seed);
                positionGenerator.Init(image, type.ToString(), true);

                double positionSeed = new Random().NextDouble();
                for (int i = 0; i < 24; i++)
                {
                    (double, double) pos = positionGenerator.GetSpawnPosition(positionSeed);
                    GD.Print(pos);
                }
            }
        }
    }
}

