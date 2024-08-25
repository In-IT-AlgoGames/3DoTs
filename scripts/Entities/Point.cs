using Godot;
using System;

public class Point : Object
{
	private readonly int x;
	private readonly int y;
	private int value;

	public Point()
	{
		this.x = 0;
		this.y = 0;
		this.value = 0;
	}

	public Point(int x, int y)
	{
		this.x = x;
		this.y = y;
		this.value = 0;
	}

	public Point(int x, int y, int value)
	{
		this.x = x;
		this.y = y;
		this.value = value;
	}

	public int GetValue()
	{
		return value;
	}

	public void SetValue(int val)
	{
		this.value = val;
	}

	public int GetX()
	{
		return x;
	}

	public int GetY()
	{
		return y;
	}

	public void ResetValue()
	{
		this.SetValue(0);
	}

	public void PrintPoint()
	{
		GD.Print("x:" + this.GetX() + ", " + "y:" + this.GetY());
		GD.Print("value:" + this.GetValue());
	}
}
