using Godot;
using System;
using System.Collections.Generic;

public class Horizontal : Object
{
	private Matrix matrix;

	public Horizontal(Matrix matrix)
	{
		this.matrix = matrix;
	}

	public void CheckThreePointHorizontalAlignmentAndKillEnemies(Matrix matrix, Point p, Score score)
	{
		int row = p.GetX();
		int col = p.GetY();
		int target = p.GetValue();
		Point[,] board = matrix.matrix;
		int colLength = board.GetLength(1);
		Point[] pts = new Point[3];

		CheckAlignment(board, p, pts, row, col, target, colLength, score);
	}

	private void CheckAlignment(Point[,] matrix, Point p, Point[] pts, int row, int col, int target, int colLength, Score score)
	{
		if (col == 0)
		{
			if (matrix[row, col + 1].GetValue() == target && matrix[row, col + 2].GetValue() == target)
			{
				pts[0] = matrix[row, col];
				pts[1] = matrix[row, col + 1];
				pts[2] = matrix[row, col + 2];
			}
		}
		else if (col == colLength - 1)
		{
			if (matrix[row, col - 1].GetValue() == target && matrix[row, col - 2].GetValue() == target)
			{
				pts[0] = matrix[row, col - 2];
				pts[1] = matrix[row, col - 1];
				pts[2] = matrix[row, col];
			}
		}
		else
		{
			pts[1] = p;

			if (matrix[row, col - 1].GetValue() == target)
			{
				pts[0] = matrix[row, col - 1];
			}

			if (matrix[row, col + 1].GetValue() == target)
			{
				pts[2] = matrix[row, col + 1];
			}
		}

		if (pts[0] != null && pts[1] != null && pts[2] != null)
		{
			KillEnemies(matrix, pts, row, colLength, score);
		}
	}

	private void KillEnemies(Point[,] matrix, Point[] pts, int row, int colLength, Score score)
	{
		int ally = pts[0].GetValue();

		if (pts[0].GetY() == 0)
		{
			FindAndKillEnemies(matrix, row, pts[2].GetY(), colLength, 1, ally, score);
		}
		else if (pts[2].GetY() == colLength - 1)
		{
			FindAndKillEnemies(matrix, row, pts[0].GetY(), 0, -1, ally, score);
		}
		else
		{
			// Right Kill
			FindAndKillEnemies(matrix, row, pts[2].GetY(), colLength, 1, ally, score);
			// Left Kill
			FindAndKillEnemies(matrix, row, pts[0].GetY(), 0, -1, ally, score);
		}
	}

	private void FindAndKillEnemies(Point[,] matrix, int row, int startCol, int endCol, int step, int ally, Score score)
	{
		List<Point> enemies = new List<Point>();

		for (int i = startCol + step; (step > 0 ? i < endCol : i >= endCol); i += step)
		{
			if (matrix[row, i].GetValue() == ally)
			{
				startCol = i;
			}
			else
			{
				break;
			}
		}

		for (int i = startCol + step; (step > 0 ? i < endCol : i >= endCol); i += step)
		{
			if (matrix[row, i].GetValue() != ally && matrix[row, i].GetValue() > 0)
			{
				enemies.Add(matrix[row, i]);
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
			GD.Print($"({enemy.GetX()}, {enemy.GetY()}) value: {enemy.GetValue()}");
		}
	}
}
