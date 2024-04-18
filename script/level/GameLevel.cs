using System;

using Godot;

namespace INTOnlineCoop.Script.Level
{
    public partial class GameLevel : Node2D
    {
        public override void _Ready()
        {
            //Level generator tests
            LevelGenerator generator = new();
            foreach (TerrainType type in Enum.GetValues<TerrainType>())
            {
                generator.SetTerrainType(type);
                generator.Generate();
            }
        }
    }
}

