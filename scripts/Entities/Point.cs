using Godot;
using System;

public class Point : Object
{
	private readonly int x;
	private readonly int y;
	private int value;

	// Default constructor
	public Point()
	{
		this.x = 0;
		this.y = 0;
		this.value = 0;
	}

	// Constructor with parameters
	public Point(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	// Getter for value
	public int GetValue()
	{
		return value;
	}

	// Setter for value
	public void SetValue(int val)
	{
		this.value = val;
	}

	// Getter for x
	public int GetX()
	{
		return x;
	}

	// Getter for y
	public int GetY()
	{
		return y;
	}

	// Method to print the point's information
	public void PrintPoint()
	{
		GD.Print($"x: {this.GetX()}, y: {this.GetY()}");
		GD.Print($"value: {this.GetValue()}");
	}
}

