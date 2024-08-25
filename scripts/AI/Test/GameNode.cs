using System;
using Godot;
using System.Collections.Generic;


public class GameNode : Object
{
	public Matrix matrix;
	public GameNode parent;
	public Point move;
	public List<GameNode> children;
	public double value;
	public int visits;
	public int depth;

	public GameNode(Matrix matrix, GameNode parent, Point move)
	{
		this.matrix = matrix;
		this.parent = parent;
		this.move = move;
		this.value = 0;
		this.visits = 0;
		this.depth = (parent == null) ? 0 : parent.depth + 1;
		children = new List<GameNode>();
	}



	// this code failed compilations so I replaced it with the one below
	//public GameNode GetBestChild(double EXPLORATION_PARAM)
	//{
		//if (children.Count == 0)
		//{
			//throw new InvalidOperationException("No children available");
		//}
//
		//return children.MaxBy(c =>
			//c.value / c.visits + EXPLORATION_PARAM * Math.Sqrt(Math.Log(visits) / c.visits)
		//);
	//}
	
	
public GameNode GetBestChild(double EXPLORATION_PARAM)
{
	if (children.Count == 0)
	{
		throw new InvalidOperationException("No children available");
	}

	GameNode bestChild = null;
	double bestValue = double.MinValue;

	foreach (var child in children)
	{
		double uctValue = child.value / child.visits +
						  EXPLORATION_PARAM * Math.Sqrt(Math.Log(visits) / child.visits);

		if (uctValue > bestValue)
		{
			bestValue = uctValue;
			bestChild = child;
		}
	}

	return bestChild;
}

}
