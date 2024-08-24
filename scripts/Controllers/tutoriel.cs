using Godot;
using System;

public partial class tutoriel : Node2D
{
	[Signal]
	public delegate void ChangeSceneEventHandler(string nextScene);
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void _on_back_button_pressed()
{
	EmitSignal(nameof(ChangeScene), "main_menu");
}
}



