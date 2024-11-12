using Godot;

namespace INTOnlineCoop.Script.Util
{
    public enum ChoosableCharacter
    {
        Axton,
        Zane,
        Zero,
        Krieg,
        Whilhelm,
        Maja,
        Nisha,
        Gaige,
        Athena
    }

    /// <summary>
    /// Class for storing player data over network
    /// </summary>
    public class PlayerData : RefCounted
    {
        public string Name { get; private set; }

        public PlayerData(string playerName)
        {
            Name = playerName;
        }
    }
}