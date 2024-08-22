using Godot;
using System;

public partial class Computer : Node
{
	private string gameLevel;
	int x, y;
	
	public Computer(string level){
		GD.Print(level);
		gameLevel = level;
		x = 0;
		y = 0;
	}
	
	public Point GetNextMove(){
		Point nextMove = new Point(x,y);
		x ++;
		y ++;
		return nextMove;
	}
}
