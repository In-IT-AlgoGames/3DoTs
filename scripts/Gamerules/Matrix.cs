using Godot;
using System;

public class Matrix : Object
{
	public Point[,] matrix;

	// Constructeur pour initialiser la matrice avec des Points
	public Matrix(int rows, int cols)
	{
		this.matrix = new Point[rows, cols];
		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < cols; j++)
			{
				this.matrix[i, j] = new Point();
			}
		}
	}

	// Vérifie si la matrice contient des zéros
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

	// Affiche la matrice
	public void PrintMatrix()
	{
		for (int i = 0; i < matrix.GetLength(0); i++)
		{
			for (int j = 0; j < matrix.GetLength(1); j++)
			{
				GD.Print($"{matrix[i, j].GetValue()} ");
			}
			GD.Print("\n");
		}
	}

	// Définit un point dans la matrice
	public void SetPoint(Point p)
	{
		this.matrix[p.GetX(), p.GetY()] = p;
	}

	// Transpose la matrice
	public void TransposeMatrix()
	{
		int rows = this.matrix.GetLength(0);
		int cols = this.matrix.GetLength(1);
		Point[,] transposedMatrix = new Point[cols, rows];

		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < cols; j++)
			{
				transposedMatrix[j, i] = this.matrix[i, j];
			}
		}
		this.matrix = transposedMatrix;
	}

	// Retourne le nombre de lignes
	public int GetRows()
	{
		return this.matrix.GetLength(0);
	}

	// Retourne le nombre de colonnes
	public int GetColumns()
	{
		return this.matrix.GetLength(1);
	}

	// Retourne la valeur d'un point spécifique
	public int GetPointValue(int x, int y)
	{
		return this.matrix[x, y].GetValue();
	}
}

