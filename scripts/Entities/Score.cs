using Godot;
using System;

public class Score : Object
{
	private int playerId;
	private int gameId;
	private int score;

	public Score(int playerId, int gameId)
	{
		this.playerId = playerId;
		this.gameId = gameId;
		this.score = 0;
	}

	public int GetScore()
	{
		return score;
	}

	public void SetScore(int score)
	{
		this.score = score;
	}
}
