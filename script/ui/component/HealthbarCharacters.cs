using Godot;

namespace INTOnlineCoop.Script.UI.Component
{
    /// <summary>
    /// 
    /// </summary>
    public partial class HealthbarCharacters : Node
    {
        [Export] private Label _playerNumber;
        [Export] private Label _healthPoints;
        /// <summary>
        /// Healthpoints of any Character
        /// </summary>
        public int Health;
        /// <summary>
        /// 
        /// </summary>
        public override void _Ready()
        {
            //Initialisierung aller Lebensanzeigen für jeden Spieler über den Spielfiguren

            //Setze Farbe von _playerNumber auf rot für Player 1 und blau für Player 2
            //_xx.AddThemeColorOverride("font_color", Color.Color8(255, 0, 0, 255)) = rot;
            //_xx.AddThemeColorOverride("font_color", Color.Color8(0, 0, 255, 255)) = blau;

            //Setze FigurenLeben auf 100 / 100 zu Beginn jedes Spiels
            _healthPoints.Text = Health.ToString();
        }
    }
}
