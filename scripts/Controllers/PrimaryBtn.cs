using Godot;
using System;

public partial class PrimaryBtn : TextureButton
{
	Label label;
	public override void _Ready()
	{
		label = GetNode<Label>("Label");
	}
	private void OnMouseEntered()
	{
		Vector2 position = label.Position;
		label.Position = new Vector2(position.X, position.Y + 4);
	}
	private void OnMouseExited()
	{
		Vector2 position = label.Position;
		label.Position = new Vector2(position.X, position.Y - 4);
	
	}
	private void OnButtonDown()
	{
		Vector2 position = label.Position;
		label.Position = new Vector2(position.X, position.Y + 7);
	
	}
	private void OnButtonUp()
	{
		Vector2 position = label.Position;
		label.Position = new Vector2(position.X, position.Y - 7);
		EmitSignal("pressed");
	}
}





