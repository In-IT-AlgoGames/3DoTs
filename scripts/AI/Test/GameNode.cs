using System;
using Godot;
using System.Collections.Generic;
using System.Linq; // Ensure you have this using directive

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

	public GameNode GetBestChild(double EXPLORATION_PARAM)
	{
		if (children.Count == 0)
		{
			throw new InvalidOperationException("No children available");
		}

		return children
			.OrderByDescending(c => c.value / c.visits + EXPLORATION_PARAM * Math.Sqrt(Math.Log(visits) / c.visits))
			.First();
	}
}
