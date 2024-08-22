using Godot;
using System;

public partial class JokerMenu : Node2D
{
	[Signal] 
	public delegate void ChangeSceneEventHandler(string nextScene);
	
	private void OnBackButtonPressed()
	{
		 EmitSignal("ChangeScene", "main_menu");
	}
}

