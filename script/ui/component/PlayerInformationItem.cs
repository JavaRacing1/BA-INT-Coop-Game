using Godot;

namespace INTOnlineCoop.Script.UI.Component
{
	public partial class PlayerInformationItem : PanelContainer
	{
		[Export] private Label _numberLabel;
		[Export] private Label _nameLabel;
		[Export] private Sprite2D[] _sprites;

		private readonly string[] _playerFigures = { "Athena, Moze, Krieg, Zero", "Axton, Maja, Moze, Amara" };

		private string[] _figureSprites = { "Figure1", "Figure2", "Figure3", "Figure4" };

		private static readonly string[] AvailableFigures =
		{
			"Amara", "Athena", "Axton", "Gaige", "Krieg", "Maja", "Moze", "Nisha", "Salvador", "Whilhelm", "Zero"
		};

		public void SetPlayerNumber(int number)
		{
			if (_numberLabel != null)
			{
				_numberLabel.Text = number.ToString();
			}
		}

		public void SetPlayerName(string name)
		{
			if (_nameLabel != null)
			{
				_nameLabel.Text = name;
			}
		}

		public int GetPlayerNumber()
		{
			return _numberLabel == null ? -1 : int.Parse(_numberLabel.Text);
		}

		private void SettingUpCharacterPanel()
		{
			if (_sprites == null || _sprites.Length == 0)
			{
				GD.PrintErr("Sprites array is not initialized.");
				return;
			}

			for (int i = 0; i < _playerFigures.Length; i++)
			{
				string[] selectedFigures = _playerFigures[i].Split(", ");

				for (int j = 0; j < selectedFigures.Length && j < _sprites.Length; j++)
				{
					string figureName = selectedFigures[j].Trim();

					if (Array.Exists(AvailableFigures, element => element == figureName))
					{
						string texturePath = $"res://assets/sprites/game_figure/{figureName}/head.png";
						Sprite2D sprite = _sprites[j];
						if (sprite != null)
						{
							Texture2D texture = GD.Load<Texture2D>(texturePath);
							if (texture != null)
							{
								sprite.Texture = texture;
							}
							else
							{
								//GD.PrintErr($"Texture not found: {texturePath}");
							}
						}
					}
					else
					{
						GD.PrintErr($"Figure '{figureName}' is not in the available figure list.");
					}
				}
			}
		}
	}
}
