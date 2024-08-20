using Godot;
using System;

public class Game : Object
{
	private int gameId;
	private string date;

	public Game()
	{
		this.date = DateTime.Now.ToString("o"); // "o" represents the ISO 8601 format in C#
	}

	public int GetId()
	{
		return gameId;
	}

	public void SetId(int id)
	{
		this.gameId = id;
	}

	public string GetDate()
	{
		return date;
	}

	public void SetDate(string date)
	{
		this.date = date;
	}
}
