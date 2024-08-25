using System;
using Godot;

public class GameEvaluator : Object
{
	private const int BlockAlignment3Score = 1000;
	private const int Alignment3Score = 2000;
	private const int Alignment2Score = 500;
	private const int KillScore = 50;
	private const int RasanteStrategy = 8000;
	private const int WinStatePoint = 10000;

	public int EvaluateBoardEasy(Point[,] board, int currentPlayerValue, Score aiScore, Score humanScore)
	{
		int score = 0;
		int opponentScore = 0;
		int opponent = (currentPlayerValue == 1) ? 2 : 1;

		score += EvaluateAlignment(board, currentPlayerValue);
		score += aiScore.GetScore() * KillScore;
		opponentScore += EvaluateAlignment(board, opponent);
		opponentScore += humanScore.GetScore() * KillScore;
		score -= opponentScore;

		return score;
	}

	public int EvaluateBoardMedium(Point[,] board, int currentPlayerValue, Score aiScore, Score humanScore)
	{
		int score = 0;
		int opponentScore = 0;
		int opponent = (currentPlayerValue == 1) ? 2 : 1;

		score += EvaluateAlignment(board, currentPlayerValue);
		score += EvaluateBlock(board, currentPlayerValue);
		score += aiScore.GetScore() * KillScore;
		opponentScore += EvaluateAlignment(board, opponent);
		opponentScore += EvaluateBlock(board, opponent);
		opponentScore += humanScore.GetScore() * KillScore;
		score -= opponentScore;

		if (score > 0) score += WinStatePoint;

		return score;
	}

	public int EvaluateBoardHard(Point[,] board, int currentPlayerValue, Score aiScore, Score humanScore)
	{
		int score = 0;
		int opponentScore = 0;
		int opponent = (currentPlayerValue == 1) ? 2 : 1;

		score += EvaluateAlignment(board, currentPlayerValue);
		score += EvaluateBlock(board, currentPlayerValue);
		score += aiScore.GetScore() * KillScore;
		score += RasanteHorizontal(board, currentPlayerValue);
		score += RasanteVertical(board, currentPlayerValue);
		opponentScore += EvaluateAlignment(board, opponent);
		opponentScore += EvaluateBlock(board, opponent);
		opponentScore += humanScore.GetScore() * KillScore;
		opponentScore += RasanteHorizontal(board, opponent);
		opponentScore += RasanteVertical(board, opponent);
		score -= opponentScore;

		if (score > 0) score += WinStatePoint;

		return score;
	}

	private int EvaluateAlignment(Point[,] board, int playerValue)
	{
		int score = 0;

		int rows = board.GetLength(0);
		int cols = board.GetLength(1);

		for (int row = 0; row < rows; row++)
		{
			for (int col = 0; col < cols; col++)
			{
				if (CheckAlignment(board, row, col, playerValue, 3) == "none")
				{
					score += Alignment3Score;
				}
				else if (CheckAlignment(board, row, col, playerValue, 2) == "none")
				{
					score += Alignment2Score;
				}
			}
		}
		return score;
	}

	private string CheckAlignment(Point[,] board, int row, int col, int player, int length)
	{
		if (CheckDirection(board, row, col, player, length, 0, 1)) return "horizontal";
		if (CheckDirection(board, row, col, player, length, 1, 0)) return "vertical";
		if (CheckDirection(board, row, col, player, length, 1, 1)) return "diagonal";
		if (CheckDirection(board, row, col, player, length, 1, -1)) return "anti-diagonal";
		return "none";
	}

	private bool CheckDirection(Point[,] board, int row, int col, int player, int length, int dRow, int dCol)
	{
		int count = 0;
		for (int i = 0; i < length; i++)
		{
			int r = row + i * dRow;
			int c = col + i * dCol;
			if (r >= 0 && r < board.GetLength(0) && c >= 0 && c < board.GetLength(1) && board[r, c].GetValue() == player)
			{
				count++;
			}
		}
		return count == length;
	}

	private int EvaluateBlock(Point[,] board, int player)
	{
		int opponent = (player == 1) ? 2 : 1;
		int score = 0;

		int rows = board.GetLength(0);
		int cols = board.GetLength(1);

		for (int row = 0; row < rows; row++)
		{
			for (int col = 0; col < cols; col++)
			{
				string alignment = CheckAlignment(board, row, col, opponent, 2);

				if (alignment == "horizontal")
				{
					if (col + 2 < cols)
					{
						if (col == 0 && board[row, col + 2].GetValue() == player)
						{
							score += BlockAlignment3Score;
						}
						else if (col - 1 >= 0 &&
									board[row, col - 1].GetValue() == player &&
									board[row, col + 2].GetValue() == player)
						{
							score += BlockAlignment3Score;
						}
						else if (col + 2 == cols - 1 &&
									col - 2 >= 0 &&
									board[row, col - 2].GetValue() == player)
						{
							score += BlockAlignment3Score;
						}
					}
				}
				else if (alignment == "vertical")
				{
					if (row + 2 < rows)
					{
						if (row == 0 && board[row + 2, col].GetValue() == player)
						{
							score += BlockAlignment3Score;
						}
						else if (row - 1 >= 0 &&
									board[row - 1, col].GetValue() == player &&
									board[row + 2, col].GetValue() == player)
						{
							score += BlockAlignment3Score;
						}
						else if (row + 2 == rows - 1 &&
									row - 2 >= 0 &&
									board[row - 2, col].GetValue() == player)
						{
							score += BlockAlignment3Score;
						}
					}
				}
				else if (alignment == "diagonal")
				{
					if (row + 2 < rows && col + 2 < cols)
					{
						if (row == 0 && col == 0 && board[row + 2, col + 2].GetValue() == player)
						{
							score += BlockAlignment3Score;
						}
						else if (row - 1 >= 0 && col - 1 >= 0 &&
									board[row - 1, col - 1].GetValue() == player &&
									board[row + 2, col + 2].GetValue() == player)
						{
							score += BlockAlignment3Score;
						}
						else if (row == rows - 1 && col == cols - 1 &&
									row - 2 >= 0 && col - 2 >= 0 &&
									board[row - 2, col - 2].GetValue() == player)
						{
							score += BlockAlignment3Score;
						}
					}
				}
				else if (alignment == "anti-diagonal")
				{
					if (row + 2 < rows && col - 2 >= 0)
					{
						if (row == 0 && col == cols - 1 && board[row + 2, col - 2].GetValue() == player)
						{
							score += BlockAlignment3Score;
						}
						else if (row - 1 >= 0 && col + 1 < cols &&
								 board[row - 1, col + 1].GetValue() == player &&
								 board[row + 2, col - 2].GetValue() == player)
						{
							score += BlockAlignment3Score;
						}
						else if (row == rows - 1 && col == 0 &&
								 row - 2 >= 0 && col + 2 < cols &&
								 board[row - 2, col + 2].GetValue() == player)
						{
							score += BlockAlignment3Score;
						}
					}
				}
			}
		}
		return score;
	}

	private int RasanteHorizontal(Point[,] board, int player)
	{
		int opponent = (player == 1) ? 2 : 1;
		int score = 0;

		int rows = board.GetLength(0);
		int cols = board.GetLength(1);

		for (int row = 0; row < rows; row++)
		{
			for (int col = 0; col < cols; col++)
			{
				if (col + 4 < cols && row - 2 >= 0 && row + 2 < rows)
				{
					if (RightHorizontalRasanteEvaluationUp(board, row, col, player, opponent * -1)) score += RasanteStrategy;
					if (RightHorizontalRasanteEvaluationDown(board, row, col, player, opponent * -1)) score += RasanteStrategy;
				}
				if (col - 4 >= 0 && row - 2 >= 0 && row + 2 < rows)
				{
					if (LeftHorizontalRasanteEvaluationUp(board, row, col, player, opponent * -1)) score += RasanteStrategy;
					if (LeftHorizontalRasanteEvaluationDown(board, row, col, player, opponent * -1)) score += RasanteStrategy;
				}
			}
		}
		return score;
	}

	private int RasanteVertical(Point[,] board, int player)
	{
		int opponent = (player == 1) ? 2 : 1;
		int score = 0;

		int rows = board.GetLength(0);
		int cols = board.GetLength(1);

		for (int row = 0; row < rows; row++)
		{
			for (int col = 0; col < cols; col++)
			{
				if (row - 4 >= 0 && col - 2 >= 0 && col + 2 < cols)
				{
					if (TopVerticalRasanteEvaluationLeft(board, row, col, player, opponent * -1)) score += RasanteStrategy;
					if (TopVerticalRasanteEvaluationRight(board, row, col, player, opponent * -1)) score += RasanteStrategy;
				}
				if (row + 4 < rows && col - 2 >= 0 && col + 2 < cols)
				{
					if (BottomVerticalRasanteEvaluationLeft(board, row, col, player, opponent * -1)) score += RasanteStrategy;
					if (BottomVerticalRasanteEvaluationRight(board, row, col, player, opponent * -1)) score += RasanteStrategy;
				}
			}
		}
		return score;
	}

	private bool RightHorizontalRasanteEvaluationUp(Point[,] board, int row, int col, int player, int opponent)
	{
		return board[row, col + 1].GetValue() == opponent && 
			   board[row, col + 2].GetValue() == opponent &&
			   board[row, col + 3].GetValue() == opponent &&
			   board[row - 1, col + 4].GetValue() == player;
	}

	private bool RightHorizontalRasanteEvaluationDown(Point[,] board, int row, int col, int player, int opponent)
	{
		return board[row, col + 1].GetValue() == opponent && 
			   board[row, col + 2].GetValue() == opponent &&
			   board[row, col + 3].GetValue() == opponent &&
			   board[row + 1, col + 4].GetValue() == player;
	}

	private bool LeftHorizontalRasanteEvaluationUp(Point[,] board, int row, int col, int player, int opponent)
	{
		return board[row, col - 1].GetValue() == opponent &&
			   board[row, col - 2].GetValue() == opponent &&
			   board[row, col - 3].GetValue() == opponent &&
			   board[row - 1, col - 4].GetValue() == player;
	}

	private bool LeftHorizontalRasanteEvaluationDown(Point[,] board, int row, int col, int player, int opponent)
	{
		return board[row, col - 1].GetValue() == opponent &&
			   board[row, col - 2].GetValue() == opponent &&
			   board[row, col - 3].GetValue() == opponent &&
			   board[row + 1, col - 4].GetValue() == player;
	}

	private bool TopVerticalRasanteEvaluationLeft(Point[,] board, int row, int col, int player, int opponent)
	{
		return board[row - 1, col].GetValue() == opponent &&
			   board[row - 2, col].GetValue() == opponent &&
			   board[row - 3, col].GetValue() == opponent &&
			   board[row - 4, col - 1].GetValue() == player;
	}

	private bool TopVerticalRasanteEvaluationRight(Point[,] board, int row, int col, int player, int opponent)
	{
		return board[row - 1, col].GetValue() == opponent &&
			   board[row - 2, col].GetValue() == opponent &&
			   board[row - 3, col].GetValue() == opponent &&
			   board[row - 4, col + 1].GetValue() == player;
	}

	private bool BottomVerticalRasanteEvaluationLeft(Point[,] board, int row, int col, int player, int opponent)
	{
		return board[row + 1, col].GetValue() == opponent &&
			   board[row + 2, col].GetValue() == opponent &&
			   board[row + 3, col].GetValue() == opponent &&
			   board[row + 4, col - 1].GetValue() == player;
	}

	private bool BottomVerticalRasanteEvaluationRight(Point[,] board, int row, int col, int player, int opponent)
	{
		return board[row + 1, col].GetValue() == opponent &&
			   board[row + 2, col].GetValue() == opponent &&
			   board[row + 3, col].GetValue() == opponent &&
			   board[row + 4, col + 1].GetValue() == player;
	}
}
