using Godot;
using System;
using System.Collections.Generic;

public class GameOptions : Object
{
	public int gameTime { get; set; }
	public int boardSize { get; set; }
	public List<string> playerNames { get; private set; } // Stores the names of players

	public GameOptions()
	{
		// Initialize with default values
		gameTime = 300;
		boardSize = 15;
		playerNames = new List<string>();
	}

	// Method to add a player name
	public void AddPlayerName(string name)
	{
		if (!playerNames.Contains(name))
		{
			playerNames.Add(name);
		}
	}
	
	// Method to remove a player name
	public void RemovePlayerName(string name)
	{
		if (playerNames.Contains(name))
		{
			playerNames.Remove(name);
		}
	}
}
