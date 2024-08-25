using Godot;
using System;

public partial class JokerCard : TextureRect
{
	Label jokerNumberLabel;
	int joker = 0;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		jokerNumberLabel =  GetNode<Label>("ValueLabel");
		jokerNumberLabel.Text = joker.ToString();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void OnIncreaseButtonPressed()
	{
		if (joker < 10 )
		joker ++;
		jokerNumberLabel.Text = joker.ToString();
	}
	private void OnDecreaseButtonPressed()
	{
		if (joker > 0 )
		joker --;
		jokerNumberLabel.Text = joker.ToString();
	
	}
}





