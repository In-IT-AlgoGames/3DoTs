using Godot;
using System;
using System.Collections.Generic;

public class Vertical : Object
{
	private Matrix _matrix;

	public Vertical(Matrix matrix)
	{
		_matrix = matrix;
	}

	public List<Point> CheckThreePointVerticalAlignmentAndKillEnemies(Matrix matrix, Point p, Score playerScore)
	{
		int row = p.GetX();
		int col = p.GetY();
		int target = p.GetValue();
		Point[,] board = matrix.matrix;
		int rowLength = board.GetLength(0);
		Point[] pts = new Point[3];
		List<Point> dead = new List<Point>();

		CheckAlignment(board, p, pts, row, col, target, rowLength, playerScore, dead);
		return dead;
	}

	private void CheckAlignment(Point[,] matrix, Point p, Point[] pts, int row, int col, int target, int rowLength, Score score, List<Point> dead)
	{
		if (row == 0)
		{
			if (matrix[row + 1, col].GetValue() == target && matrix[row + 2, col].GetValue() == target)
			{
				pts[0] = matrix[row, col];
				pts[1] = matrix[row + 1, col];
				pts[2] = matrix[row + 2, col];
			}
		}
		else if (row == rowLength - 1)
		{
			if (matrix[row - 1, col].GetValue() == target && matrix[row - 2, col].GetValue() == target)
			{
				pts[0] = matrix[row - 2, col];
				pts[1] = matrix[row - 1, col];
				pts[2] = matrix[row, col];
			}
		}
		else
		{
			pts[1] = p;

			if (matrix[row - 1, col].GetValue() == target)
			{
				pts[0] = matrix[row - 1, col];
			}

			if (matrix[row + 1, col].GetValue() == target)
			{
				pts[2] = matrix[row + 1, col];
			}
		}

		if (pts[0] != null && pts[1] != null && pts[2] != null)
		{
			KillEnemies(matrix, pts, col, rowLength, score, dead);
		}
	}

	private void KillEnemies(Point[,] matrix, Point[] pts, int col, int rowLength, Score score, List<Point> dead)
	{
		int ally = pts[0].GetValue();

		if (pts[0].GetX() == 0)
		{
			FindAndKillEnemies(matrix, pts[2].GetX(), rowLength, col, 1, ally, score, dead);
		}
		else if (pts[2].GetX() == rowLength - 1)
		{
			FindAndKillEnemies(matrix, pts[0].GetX(), 0, col, -1, ally, score, dead);
		}
		else
		{
			// Downward Kill
			FindAndKillEnemies(matrix, pts[2].GetX(), rowLength, col, 1, ally, score, dead);
			// Upward Kill
			FindAndKillEnemies(matrix, pts[0].GetX(), 0, col, -1, ally, score, dead);
		}
	}

	private void FindAndKillEnemies(Point[,] matrix, int startRow, int endRow, int col, int step, int ally, Score score, List<Point> dead)
	{
		List<Point> enemies = new List<Point>();

		for (int i = startRow + step; (step > 0 ? i < endRow : i >= endRow); i += step)
		{
			if (matrix[i, col].GetValue() == ally)
			{
				startRow = i;
			}
			else
			{
				break;
			}
		}

		for (int i = startRow + step; (step > 0 ? i < endRow : i >= endRow); i += step)
		{
			if (matrix[i, col].GetValue() != ally && matrix[i, col].GetValue() > 0)
			{
				enemies.Add(matrix[i, col]);
			}
			else
			{
				break;
			}
		}

		foreach (Point enemy in enemies)
		{
			enemy.SetValue(enemy.GetValue() * -1);
			score.SetScore(score.GetScore() + 1);
			dead.Add(enemy);
		}
	}
}
