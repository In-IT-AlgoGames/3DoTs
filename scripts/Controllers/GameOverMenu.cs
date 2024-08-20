using Godot;
using System;

public partial class GameOverMenu : CanvasLayer
{
	[Signal]
	public delegate void MenuEventHandler();
	[Signal]
	public delegate void RestartEventHandler();
	[Signal]
	public delegate void GoOutEventHandler();
	// Called when the node enters the scene tree for the first time.

	private void OnRestartButtonPressed()
	{
		EmitSignal("Restart");
	}
	private void OnGoOutButtonPressed()
	{
		EmitSignal("GoOut");
	}
	private void OnMainButtonPressed()
	{
		EmitSignal("Menu");
	}
	
}


