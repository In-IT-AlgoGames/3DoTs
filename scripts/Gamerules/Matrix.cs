using Godot;
using System;
using System.Collections.Generic;


public class Matrix : Object
{
	public Point[,] matrix;

	public Matrix(int rows, int cols)
	{
		this.matrix = new Point[rows, cols];
		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < cols; j++)
				this.matrix[i, j] = new Point(i, j);
		}
	}

	public bool HasZeros()
	{
		for (int i = 0; i < matrix.GetLength(0); i++)
		{
			for (int j = 0; j < matrix.GetLength(1); j++)
			{
				if (matrix[i, j].GetValue() == 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	public int GetRows()
	{
		return this.matrix.GetLength(0);
	}

	public int GetColumns()
	{
		return this.matrix.GetLength(1);
	}

	public void PrintMatrix()
	{
		// Use a for loop to iterate through each line (row)
	for (int row = 0; row < this.matrix.GetLength(0); row++)
	{
		// Iterate through each column in the current row
		foreach (Point point in this.matrix)
		{
			// Get the value of each point
			GD.Print(" " + point.GetValue());
		}
		GD.Print(" ");
	}
	}

	public void SetPoint(Point p)
	{
		this.matrix[p.GetX(), p.GetY()] = p;
	}

	public int GetPointValue(int x, int y)
	{
		return this.matrix[x, y].GetValue();
	}

	public void ResetCase(int x, int y)
	{
		this.matrix[x, y].SetValue(0);
	}

	public Matrix Copy()
	{
		Matrix newMatrix = new Matrix(this.GetRows(), this.GetColumns());

		for (int i = 0; i < this.GetRows(); i++)
		{
			for (int j = 0; j < this.GetColumns(); j++)
			{
				newMatrix.matrix[i, j] = new Point(this.matrix[i, j].GetX(), this.matrix[i, j].GetY(), this.matrix[i, j].GetValue());
			}
		}

		return newMatrix;
	}

	public List<Point> GetAvailableMoves()
	{
		List<Point> availableMoves = new List<Point>();

		for (int row = 0; row < this.GetRows(); row++)
		{
			for (int col = 0; col < this.GetColumns(); col++)
			{
				if (this.matrix[row, col].GetValue() == 0)
				{
					availableMoves.Add(new Point(row, col, 2));
				}
			}
		}

		return availableMoves;
	}

	public int CalculateCentralityScore(Point point)
	{
		int centerX = matrix.GetLength(0) / 2;
		int centerY = matrix.GetLength(1) / 2;

		// Calculate the Manhattan distance from the center of the matrix
		int distanceFromCenter = Math.Abs(point.GetX() - centerX) + Math.Abs(point.GetY() - centerY);

		// Inverse of the distance: closer to center = higher score
		int maxDistance = centerX + centerY; // Maximum possible distance in the matrix
		return maxDistance - distanceFromCenter;
	}

	public Matrix ExtractSubMatrix(int centerRow, int centerCol, int windowSize)
	{
		int rows = this.GetRows();
		int cols = this.GetColumns();

		// Calculer les indices de début et de fin pour la sous-matrice
		int halfWindowSize = windowSize / 2;
		int startRow = Math.Max(centerRow - halfWindowSize, 0);
		int startCol = Math.Max(centerCol - halfWindowSize, 0);
		int endRow = Math.Min(centerRow + halfWindowSize + 1, rows);
		int endCol = Math.Min(centerCol + halfWindowSize + 1, cols);

		// Ajuster la taille de la sous-matrice si elle dépasse les limites
		int adjustedWindowSizeRow = endRow - startRow;
		int adjustedWindowSizeCol = endCol - startCol;
		int adjustedWindowSize = Math.Min(windowSize, Math.Min(adjustedWindowSizeRow, adjustedWindowSizeCol));

		// Créer la sous-matrice
		Matrix subMatrix = new Matrix(adjustedWindowSize, adjustedWindowSize);

		for (int i = startRow; i < endRow; i++)
		{
			for (int j = startCol; j < endCol; j++)
			{
				subMatrix.SetPoint(new Point(i - startRow, j - startCol, this.GetPointValue(i, j)));
			}
		}

		return subMatrix;
	}
}

