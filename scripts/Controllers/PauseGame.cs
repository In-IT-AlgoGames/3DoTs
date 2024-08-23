using Godot;
using System;

public partial class PauseGame : CanvasLayer
{
	[Signal]
	public delegate void PlayEventHandler();
	[Signal]
	public delegate void MenuEventHandler();
	[Signal]
	public delegate void RestartEventHandler();
	[Signal]
	public delegate void GoOutEventHandler();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	private void OnPlayPressedButton()
	{
		EmitSignal("Play");
	}
	private void OnMenuButtonPressed()
	{
		EmitSignal("Menu");
	}
	private void OnRestartButtonPressed()
	{
		EmitSignal("Restart");
	}
	private void OnGoOutButtonPressed()
	{
		EmitSignal("GoOut");
	}
}
