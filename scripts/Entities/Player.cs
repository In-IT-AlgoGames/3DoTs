using Godot;
using System;

public class Player : Object
{
	private int playerId;
	private string name;
	private int pointValue;
	private int coins;
	private int diamonds;

	public Player(string name, int pointValue)
	{
		this.name = name;
		this.pointValue = pointValue;
	}

	public int GetId()
	{
		return playerId;
	}

	public void SetId(int playerId)
	{
		this.playerId = playerId;
	}

	public void SetCoins(int coins)
	{
		this.coins = coins;
	}

	public int GetCoins()
	{
		return coins;
	}

	public void SetDiamonds(int diamonds)
	{
		this.diamonds = diamonds;
	}

	public int GetDiamonds()
	{
		return diamonds;
	}

	public string GetName()
	{
		return name;
	}

	public void SetName(string name)
	{
		this.name = name;
	}

	public int GetPointValue()
	{
		return pointValue;
	}

	public void SetPointValue(int pointValue)
	{
		this.pointValue = pointValue;
	}
}

