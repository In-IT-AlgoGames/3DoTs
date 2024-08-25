using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
public class MCTS : Object
{
	private static int TIME_LIMIT = 5000;
	private Random random = new Random();
	private GameEvaluator gameEval = new GameEvaluator();

	
	public Point BestMove(Matrix matrix, AIPlayer aiPlayer, Player human, Score aiScore, Score humanScore, List<Point> playerPoints, string aiLevel)
	{
		GameNode root = new GameNode(matrix, null, null);
	
		
		long simul = 0;
		int aiInitialScore = aiScore.GetScore();
		int humanInitialScore = humanScore.GetScore();
		double EXPLORATION_PARAM = 0.5;
		double timeLimit;

		if (aiLevel == "easy")
		{
			simul = 200;
			EXPLORATION_PARAM = 2;
		}
		else if (aiLevel == "medium")
		{
			simul = 400;
			EXPLORATION_PARAM = 1.2;
		}
		else if (aiLevel == "hard")
		{
			EXPLORATION_PARAM = 0.9;
		}

		long initialSimul = simul;
		if (aiLevel == "easy" || aiLevel == "medium")
		{
			while (simul > 0)
			{
				aiScore.SetScore(aiInitialScore);
				humanScore.SetScore(humanInitialScore);

				if (simul < initialSimul * 0.5)
				{
					EXPLORATION_PARAM *= 0.5;
				}

				GameNode selectedNode = Select(root, EXPLORATION_PARAM);
				GameNode expandedNode = Expand(selectedNode, playerPoints);
				int result = Simulate(expandedNode, aiPlayer, human, aiScore, humanScore, aiLevel);
				Backpropagate(expandedNode, result);
				simul--;
			}
		}

		if (aiLevel == "hard")
		{
			timeLimit = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + TIME_LIMIT;
			while (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() < timeLimit)
			{
				aiScore.SetScore(aiInitialScore);
				humanScore.SetScore(humanInitialScore);

				if (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() > timeLimit * 0.5)
				{
					EXPLORATION_PARAM *= 0.5;
				}

				GameNode selectedNode = Select(root, EXPLORATION_PARAM);
				GameNode expandedNode = Expand(selectedNode, playerPoints);
				int result = Simulate(expandedNode, aiPlayer, human, aiScore, humanScore, aiLevel);
				Backpropagate(expandedNode, result);
			}
		}

		aiScore.SetScore(aiInitialScore);
		humanScore.SetScore(humanInitialScore);

		return root.GetBestChild(0).move;
	}

	private GameNode Select(GameNode node, double EXPLORATION_PARAM)
	{
		GD.Print("test");
		while (node.children.Any())
		{
			node = node.GetBestChild(EXPLORATION_PARAM);
		}
		return node;
	}

	private GameNode Expand(GameNode node, List<Point> playerPoints)
	{
		int count = 0;
		List<Point> availableMoves = GetAvailableMoves(node.matrix, playerPoints.Last());

		foreach (Point move in availableMoves)
		{
			bool alreadyExists = node.children.Any(child => child.move.Equals(move));
			if (!alreadyExists)
			{
				count++;
				Matrix newMatrix = node.matrix.Copy();
				newMatrix.SetPoint(move);
				GameNode childNode = new GameNode(newMatrix, node, move);
				node.children.Add(childNode);
				Console.WriteLine(count);
			}
			else
			{
				Console.WriteLine("Move already exists: " + move);
			}
		}

		return node;
	}

	private int Simulate(GameNode node, AIPlayer aiPlayer, Player human, Score aiScore, Score humanScore, string aiLevel)
	{
		Matrix tempMatrix = node.matrix.Copy();
		Player currentPlayer = aiPlayer;
		List<Point> aiPlayerPoints = new List<Point>();
		List<Point> humanPlayerPoints = new List<Point>();

		while (tempMatrix.HasZeros())
		{
			Point randomMove = GeneratePriorityMove(tempMatrix);
			randomMove.SetValue(currentPlayer.GetPointValue());
			tempMatrix.SetPoint(randomMove);

			if (currentPlayer.Equals(aiPlayer))
			{
				aiPlayerPoints.Add(randomMove);
				ApplyGameRules(tempMatrix, aiPlayerPoints, humanPlayerPoints, aiScore);
			}
			else
			{
				humanPlayerPoints.Add(randomMove);
				ApplyGameRules(tempMatrix, humanPlayerPoints, aiPlayerPoints, humanScore);
			}

			currentPlayer = currentPlayer == aiPlayer ? human : aiPlayer;
		}

		if (aiLevel == "easy")
			return gameEval.EvaluateBoardEasy(tempMatrix.matrix, aiPlayer.GetPointValue(), aiScore, humanScore);
		else if (aiLevel == "medium")
			return gameEval.EvaluateBoardMedium(tempMatrix.matrix, aiPlayer.GetPointValue(), aiScore, humanScore);
		else
			return gameEval.EvaluateBoardHard(tempMatrix.matrix, aiPlayer.GetPointValue(), aiScore, humanScore);
	}

	private void Backpropagate(GameNode node, int result)
	{
		while (node != null)
		{
			node.visits++;
			node.value += result;
			node = node.parent;
		}
	}

	private Point GeneratePriorityMove(Matrix matrix)
	{
		List<Point> availableMoves = matrix.GetAvailableMoves();
		availableMoves.Sort((move1, move2) => matrix.CalculateCentralityScore(move2).CompareTo(matrix.CalculateCentralityScore(move1)));
		return availableMoves[random.Next(availableMoves.Count)];
	}

	private List<Point> ApplyGameRules(Matrix matrix, List<Point> playerPoints, List<Point> opponentPoints, Score playerScore)
	{
		Horizontal horizontalCheck = new Horizontal(matrix);
		Vertical verticalCheck = new Vertical(matrix);
		Diagonal diagonalCheck = new Diagonal(matrix);
		AntiDiagonal antiDiagonalCheck = new AntiDiagonal(matrix);
		List<Point> allDead = new List<Point>();

		foreach (Point pt in playerPoints)
		{
			List<Point> deadHorizontal = horizontalCheck.CheckThreePointHorizontalAlignmentAndKillEnemies(matrix, pt, playerScore);
			List<Point> deadVertical = verticalCheck.CheckThreePointVerticalAlignmentAndKillEnemies(matrix, pt, playerScore);
			List<Point> deadDiagonal = diagonalCheck.CheckThreePointDiagonalAlignmentAndKillEnemies(matrix, pt, playerScore);
			List<Point> deadAntiDiagonal = antiDiagonalCheck.CheckThreePointAntiDiagonalAlignmentAndKillEnemies(matrix, pt, playerScore);

			allDead.AddRange(deadHorizontal);
			allDead.AddRange(deadVertical);
			allDead.AddRange(deadDiagonal);
			allDead.AddRange(deadAntiDiagonal);
		}

		RemoveKilledPoints(opponentPoints);
		return allDead;
	}

	private void RemoveKilledPoints(List<Point> points)
	{
		points.RemoveAll(point => point.GetValue() < 0);
	}

	private List<Point> GetAvailableMoves(Matrix matrix, Point startPoint)
	{
		Queue<Point> queue = new Queue<Point>();
		HashSet<Point> visited = new HashSet<Point>();
		queue.Enqueue(startPoint);
		visited.Add(startPoint);
		int depth = 0;
		int limit = 10;

		List<Point> availableMoves = new List<Point>();

		while (depth < limit)
		{
			Point current = queue.Dequeue();

			foreach (Point neighbor in GetNeighbors(matrix, current))
			{
				if (!visited.Contains(neighbor))
				{
					visited.Add(neighbor);
					queue.Enqueue(neighbor);
					if (neighbor.GetValue() == 0)
					{
						availableMoves.Add(neighbor);
					}
				}
			}

			if (depth == limit - 3 && !availableMoves.Any() && queue.Any())
			{
				limit += 5;
			}

			depth++;
		}

		foreach (Point pt in availableMoves)
		{
			Console.Write("(" + pt.GetX() + ", " + pt.GetY() + ")");
		}

		return availableMoves;
	}

	private List<Point> GetNeighbors(Matrix matrix, Point point)
	{
		List<Point> neighbors = new List<Point>();
		int x = point.GetX();
		int y = point.GetY();

		int[][] directions = {
			new int[] {1, 0}, new int[] {-1, 0}, new int[] {0, 1}, new int[] {0, -1},
			new int[] {1, 1}, new int[] {1, -1}, new int[] {-1, 1}, new int[] {-1, -1}
		};

		foreach (int[] dir in directions)
		{
			int newX = x + dir[0];
			int newY = y + dir[1];

			if (IsValidPosition(matrix, newX, newY))
			{
				neighbors.Add(matrix.matrix[newX, newY]);
			}
		}

		return neighbors;
	}

	private bool IsValidPosition(Matrix matrix, int x, int y)
	{
		return x >= 0 && x < matrix.matrix.GetLength(0) && y >= 0 && y < matrix.matrix.GetLength(1);
	}
}
