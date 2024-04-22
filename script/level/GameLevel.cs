using System;

using Godot;

namespace INTOnlineCoop.Script.Level
{
    public partial class GameLevel : Node2D
    {
        private static void OnGenerationButtonPressed()
        {
            //Level generator tests
            LevelGenerator generator = new();
            generator.EnableDebugMode();
            foreach (TerrainType type in Enum.GetValues<TerrainType>())
            {
                generator.SetTerrainType(type);
                int seed = new Random().Next();
                _ = generator.Generate(seed);
            }
        }
    }
}

