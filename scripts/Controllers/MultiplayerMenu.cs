using Godot;
using System;

public partial class MultiplayerMenu : Node2D
{
	[Signal]
	public delegate void ChangeSceneEventHandler(string nextScene);
	
	
	private void OnLocalButtonPressed()
	{
		EmitSignal(nameof(ChangeScene), "game_options");
	}
	private void OnBackButtonPressed()
	{
		EmitSignal(nameof(ChangeScene), "main_menu");		
	}
}






