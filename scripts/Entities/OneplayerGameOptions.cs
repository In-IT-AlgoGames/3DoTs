using Godot;
using System;
using System.Collections.Generic;

public class OneplayerGameOptions : Object
{
	public int gameTime { get; set; }
	public int boardSize { get; set; }
	public string gameLevel { get; set; }
	public OneplayerGameOptions()
	{
		// Initialize with default values
		gameTime = 300;
		boardSize = 15;
		gameLevel = "easy";
	}

	
}
