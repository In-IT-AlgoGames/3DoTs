using Godot;
using System;

public partial class MainMenu : Node2D
{
	[Signal]
	public delegate void ChangeSceneEventHandler(string nextScene);
	
	Label coinsLabel;
	Label diamondsLabel;
	
	public override void _Ready()
	{ 
		coinsLabel = GetNode<Label>("Coins/Value");
		diamondsLabel = GetNode<Label>("Diamonds/Value");
		
		coinsLabel.Text = LocalPlayer.coins.ToString();
		diamondsLabel.Text = LocalPlayer.diamonds.ToString();
	}
	private void OnMultiplayerPressed()
	{
		EmitSignal(nameof(ChangeScene), "multiplayer_menu");
	}
	private void OnSoloButtonPressed()
	{
		EmitSignal(nameof(ChangeScene), "oneplayer_options");
	}
	

	private void OnJokerpressed()
	{
		EmitSignal(nameof(ChangeScene), "joker_menu");
	}
}


