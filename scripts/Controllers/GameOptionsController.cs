using Godot;
using System;

public partial class GameOptionsController : Node2D
{
	// Called when the node enters the scene tree for the first time.
	[Signal]
	public delegate void ChangeSceneEventHandler(string currentScene);
	GameOptionsChoice signalEmitter;
	public override void _Ready()
	{
		signalEmitter = GetNode<GameOptionsChoice>("GameOptionsChoice");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void OnBackButtonPressed()
	{
			EmitSignal(nameof(ChangeScene), "main_menu");
	}
	private void _OnPlayButtonPressed()
	{
		EmitSignal(nameof(ChangeScene), "game_board");
	}
	public GameOptions GetGameOptions(){
		return signalEmitter.GetGameOptions();
	}
	
}






