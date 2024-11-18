using Godot;

using INTOnlineCoop.Script.Player;


namespace INTOnlineCoop.Script.UI.Component
{
    /// <summary>
    /// 
    /// </summary>
    public partial class PlayerInformationItem : PanelContainer
    {
        [Export] private Label _numberLabel;
        [Export] private Label _nameLabel;
        [Export] private Sprite2D[] _sprites;

        /// <summary>
        /// Changes the player number
        /// </summary>
        /// <param name="number">New player number</param>
        public void SetPlayerNumber(int number)
        {
            if (_numberLabel != null)
            {
                _numberLabel.Text = number.ToString();
            }
        }

        /// <summary>
        /// Sets the player name
        /// </summary>
        /// <param name="name">New player name</param>
        public void SetPlayerName(string name)
        {
            if (_nameLabel != null)
            {
                _nameLabel.Text = name;
            }
        }

        /// <summary>
        /// Returns the current player number
        /// </summary>
        /// <returns>Player number</returns>
        public int GetPlayerNumber()
        {
            return _numberLabel == null ? -1 : int.Parse(_numberLabel.Text);
        }

        /// <summary>
        /// Set the character textures
        /// </summary>
        /// <param name="characterTypes"></param>
        public void SetCharacters(CharacterType[] characterTypes)
        {
            if (_sprites is not { Length: 4 } || characterTypes.Length != 4)
            {
                GD.PrintErr("Sprites array is not initialized.");
                return;
            }

            for (int i = 0; i < 4; i++)
            {
                Sprite2D sprite = _sprites[i];
                CharacterType character = characterTypes[i];
                if (character != CharacterType.None)
                {
                    sprite.Texture = character.HeadTexture;
                }
            }
        }
    }
}