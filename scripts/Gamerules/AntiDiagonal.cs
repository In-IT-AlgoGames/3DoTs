using Godot;
using System;
using System.Collections.Generic;


public class AntiDiagonal : Object
{
	private Matrix matrix;

	public AntiDiagonal(Matrix matrix)
	{
		this.matrix = matrix;
	}

	public void CheckThreePointAntiDiagonalAlignmentAndKillEnemies(Matrix matrix, Point p, Score score)
	{
		int row = p.GetX();
		int col = p.GetY();
		int target = p.GetValue();
		Point[,] board = matrix.matrix;
		int rowLength = board.GetLength(0);
		int colLength = board.GetLength(1);
		Point[] pts = new Point[3];

		CheckAlignment(board, p, pts, row, col, target, rowLength, colLength, score);
	}

	private void CheckAlignment(Point[,] matrix, Point p, Point[] pts, int row, int col, int target, int rowLength, int colLength, Score score)
	{
		if (row <= rowLength - 3 && col >= 2)
		{
			if (matrix[row + 1, col - 1].GetValue() == target && matrix[row + 2, col - 2].GetValue() == target)
			{
				pts[0] = matrix[row, col];
				pts[1] = matrix[row + 1, col - 1];
				pts[2] = matrix[row + 2, col - 2];
			}
		}

		if (row >= 2 && col <= colLength - 3)
		{
			if (matrix[row - 1, col + 1].GetValue() == target && matrix[row - 2, col + 2].GetValue() == target)
			{
				pts[0] = matrix[row - 2, col + 2];
				pts[1] = matrix[row - 1, col + 1];
				pts[2] = matrix[row, col];
			}
		}

		if (pts[0] == null || pts[1] == null || pts[2] == null)
		{
			pts[1] = p;

			if (row > 0 && col < colLength - 1 && matrix[row - 1, col + 1].GetValue() == target)
			{
				pts[0] = matrix[row - 1, col + 1];
			}

			if (row < rowLength - 1 && col > 0 && matrix[row + 1, col - 1].GetValue() == target)
			{
				pts[2] = matrix[row + 1, col - 1];
			}
		}

		if (pts[0] != null && pts[1] != null && pts[2] != null)
		{
			KillEnemies(matrix, pts, rowLength, colLength, score);
		}
	}

	private void KillEnemies(Point[,] matrix, Point[] pts, int rowLength, int colLength, Score score)
	{
		int ally = pts[0].GetValue();

		// Kill downward-left
		if (pts[0].GetX() == 0 || pts[0].GetY() == colLength - 1)
		{
			FindAndKillEnemies(matrix, pts[2].GetX(), pts[2].GetY(), rowLength, 0, 1, -1, ally, score);
		}
		// Kill upward-right
		else if (pts[2].GetX() == rowLength - 1 || pts[2].GetY() == 0)
		{
			FindAndKillEnemies(matrix, pts[0].GetX(), pts[0].GetY(), 0, colLength, -1, 1, ally, score);
		}
		else
		{
			// Kill downward-left
			FindAndKillEnemies(matrix, pts[2].GetX(), pts[2].GetY(), rowLength, 0, 1, -1, ally, score);
			// Kill upward-right
			FindAndKillEnemies(matrix, pts[0].GetX(), pts[0].GetY(), 0, colLength, -1, 1, ally, score);
		}
	}

	private void FindAndKillEnemies(Point[,] matrix, int startRow, int startCol, int endRow, int endCol, int rowStep, int colStep, int ally, Score score)
	{
		List<Point> enemies = new List<Point>();

		for (int i = startRow + rowStep, j = startCol + colStep; (rowStep > 0 ? i < endRow : i >= endRow) && (colStep > 0 ? j < endCol : j >= endCol); i += rowStep, j += colStep)
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

		for (int i = startRow + rowStep, j = startCol + colStep; (rowStep > 0 ? i < endRow : i >= endRow) && (colStep > 0 ? j < endCol : j >= endCol); i += rowStep, j += colStep)
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

		foreach (Point enemy in enemies)
		{
			enemy.SetValue(enemy.GetValue() * -1);
			score.SetScore(score.GetScore() + 1);
			GD.Print($"({enemy.GetX()}, {enemy.GetY()}) value: {enemy.GetValue()}");
		}
	}
}

