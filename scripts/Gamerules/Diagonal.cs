using Godot;
using System;
using System.Collections.Generic;

public class Diagonal : Object
{
	private Matrix _matrix;

	public Diagonal(Matrix matrix)
	{
		_matrix = matrix;
	}

	public List<Point> CheckThreePointDiagonalAlignmentAndKillEnemies(Matrix matrix, Point p, Score playerScore)
	{
		int row = p.GetX();
		int col = p.GetY();
		int target = p.GetValue();
		Point[,] board = matrix.matrix;
		int rowLength = board.GetLength(0);
		int colLength = board.GetLength(1);
		Point[] pts = new Point[3];
		List<Point> dead = new List<Point>();

		CheckAlignment(board, p, pts, row, col, target, rowLength, colLength, playerScore, dead);
		return dead;
	}

	private void CheckAlignment(Point[,] matrix, Point p, Point[] pts, int row, int col, int target, int rowLength, int colLength, Score score, List<Point> dead)
	{
		// Down-right diagonal
		if (row < rowLength - 2 && col < colLength - 2 &&
			matrix[row + 1, col + 1].GetValue() == target &&
			matrix[row + 2, col + 2].GetValue() == target)
		{
			pts[0] = matrix[row, col];
			pts[1] = matrix[row + 1, col + 1];
			pts[2] = matrix[row + 2, col + 2];
		}
		// Up-left diagonal
		else if (row > 1 && col > 1 &&
				 matrix[row - 1, col - 1].GetValue() == target &&
				 matrix[row - 2, col - 2].GetValue() == target)
		{
			pts[0] = matrix[row - 2, col - 2];
			pts[1] = matrix[row - 1, col - 1];
			pts[2] = matrix[row, col];
		}

		// Additional check for 2 out of 3 alignment
		if (pts[0] == null || pts[1] == null || pts[2] == null)
		{
			pts[1] = p;

			if (row > 0 && col > 0 && matrix[row - 1, col - 1].GetValue() == target)
			{
				pts[0] = matrix[row - 1, col - 1];
			}

			if (row < rowLength - 1 && col < colLength - 1 && matrix[row + 1, col + 1].GetValue() == target)
			{
				pts[2] = matrix[row + 1, col + 1];
			}
		}

		if (pts[0] != null && pts[1] != null && pts[2] != null)
		{
			KillEnemies(matrix, pts, score, dead);
		}
	}

	private void KillEnemies(Point[,] matrix, Point[] pts, Score score, List<Point> dead)
	{
		int ally = pts[0].GetValue();

		// If alignment is on the edge of the matrix
		if (pts[0].GetX() == 0 || pts[0].GetY() == 0)
		{
			FindAndKillEnemies(matrix, pts[2].GetX(), pts[2].GetY(), matrix.GetLength(0), matrix.GetLength(1), 1, ally, score, dead);
		}
		else if (pts[2].GetX() == matrix.GetLength(0) - 1 || pts[2].GetY() == matrix.GetLength(1) - 1)
		{
			FindAndKillEnemies(matrix, pts[0].GetX(), pts[0].GetY(), 0, 0, -1, ally, score, dead);
		}
		else
		{
			// Kill downward-right and upward-left enemies
			FindAndKillEnemies(matrix, pts[2].GetX(), pts[2].GetY(), matrix.GetLength(0), matrix.GetLength(1), 1, ally, score, dead);
			FindAndKillEnemies(matrix, pts[0].GetX(), pts[0].GetY(), 0, 0, -1, ally, score, dead);
		}
	}

	private void FindAndKillEnemies(Point[,] matrix, int startRow, int startCol, int endRow, int endCol, int step, int ally, Score score, List<Point> dead)
	{
		List<Point> enemies = new List<Point>();

		// Find continuous ally points
		for (int i = startRow + step, j = startCol + step;
			 (step > 0 ? i < endRow && j < endCol : i >= endRow && j >= endCol);
			 i += step, j += step)
		{
			if (matrix[i, j].GetValue() == ally)
			{
				startRow = i;
				startCol = j;
			}
			else
			{
				break;
			}
		}

		// Collect enemy points
		for (int i = startRow + step, j = startCol + step;
			 (step > 0 ? i < endRow && j < endCol : i >= endRow && j >= endCol);
			 i += step, j += step)
		{
			if (matrix[i, j].GetValue() != ally && matrix[i, j].GetValue() > 0)
			{
				enemies.Add(matrix[i, j]);
			}
			else
			{
				break;
			}
		}

		// Kill enemies and update the score
		foreach (Point enemy in enemies)
		{
			enemy.SetValue(enemy.GetValue() * -1);  // Mark enemy as killed
			score.SetScore(score.GetScore() + 1);    // Increase player score
			dead.Add(enemy);                         // Add to dead enemies list
		}
	}
}
