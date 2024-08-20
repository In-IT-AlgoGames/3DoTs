using Godot;
using System;

public partial class MainMenu : Node2D
{
	[Signal]
	public delegate void ChangeSceneEventHandler(string nextScene);
	
	private void OnMultiplayerPressed()
	{
		EmitSignal(nameof(ChangeScene), "multiplayer_menu");
	}
}






