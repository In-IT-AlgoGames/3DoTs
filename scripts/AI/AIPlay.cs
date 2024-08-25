using Godot;
using System;
using System.Collections.Generic;

public class AIPlay : Object
{
	private MCTS mcts = new MCTS();

	public Point BestMove(Matrix matrix, Player human, AIPlayer aiPlayer, Score aiScore, Score playerScore, List<Point> playerPoints, string aiLevel)
	{
		return mcts.BestMove(matrix, aiPlayer, human, aiScore, playerScore, playerPoints, aiLevel);
	}
}
