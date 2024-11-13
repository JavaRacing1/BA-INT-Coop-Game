using Godot;
using System;

namespace INTOnlineCoop.Script.UI.Screen
{
	
	/// <summary>
	/// The start scene of the game
	/// </summary>
	public partial class CharacterSelection : Control
	{
		[Export] private TextureButton _confirmCharacterSelection;
		[Export] private Lable _fontClorAmara;
		[Export] private Lable _fontClorAthena;
		[Export] private Lable _fontClorAxton;
		[Export] private Lable _fontClorGaige;
		[Export] private Lable _fontClorKrieg;
		[Export] private Lable _fontClorMaja;
		[Export] private Lable _fontClorMoze;
		[Export] private Lable _fontClorNisha;
		[Export] private Lable _fontClorSalvador;
		[Export] private Lable _fontClorWhilhelm;
		[Export] private Lable _fontClorZero;
	
		private int _selectedPlayerCharacterNumber;
		private bool _activateCharacterConfirmButton;
		
		/// <summary>
		/// Receives node references and loads the credit file
		/// </summary>
		public override void _Ready()
		{
			_selectedPlayerCharacterNumber = 0;
			_activateCharacterConfirmButton = flase;
			_confirmCharacterSelection.visible = _activateCharacterConfirmButton;
			_confirmCharacterSelection.disabled != _activateCharacterConfirmButton;
		}
		
		private void OnAmaraHeadPressed()
		{
			_fontClorAmara;
		}
	}
}
