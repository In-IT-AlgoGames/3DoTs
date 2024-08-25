using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


public partial class OneplayerGameBoard : GridContainer
{
	private int boardSize = 20;
	private int cellSize = 25;
	private int currentPlayerId = 1;
	private Node2D currentDot;
	private Vector2 position;
	private int[,] gameBoardSate;

	int playerKillingInRow = 0;
	int aiKillingInRow = 0;
	// AI setup
	AIPlayer aiPlayer ;
	AIPlay aiPlay;
	String aiLevel;
	// Adaptation du code de Mr bray 
	Matrix gameBoardMatrix;
	Player player;
	
	Game game;
	
	Score playerScore;
	Score aiScore;
	Horizontal horizontalCheck;
	Vertical verticalCheck;
	Diagonal diagonalCheck;
	AntiDiagonal antiDiagonalCheck;
	
	Label playerScoreLabel;
	Label aiScoreLabel;
	
	List<Point> playerPoints;
	List<Point> aiPoints;
	 
	// Pour bouger et redimensionner le plateau
	private Vector2 startPosition;
	private Vector2 initialPosition;
	private Vector2 dragOffset = Vector2.Zero;
	private bool isMoving = false;
	private bool isResizing = false;
	private const float DragThreshold = 3.0f;
	private const float EdgeThreshold = 10.0f;
	
	// board elements variables
	[Export] 
	public PackedScene cellScene { get; set; }

	[Export]
	public PackedScene blueDotScene { get; set; }

	[Export]
	public PackedScene redDotScene { get; set; }
	
	[Export]
	public PackedScene deadDotScene { get; set; }
	
	// game ending
	[Signal]
	public delegate void EndGameEventHandler();
	
	// combo signal 
	[Signal]
	public delegate void ComboEventHandler();
	int turnNumber ;
	
	public override void _Ready()
	{ 
		Scale = new Vector2(1.2f, 1.2f); // Initialise le scale à 1,1 pour un affichage normal
	}

	
	public void SetGameOptions(OneplayerGameOptions gameOptions){
		boardSize = gameOptions.boardSize;
			
		game = new Game();
		game.SetId(5); // change this from a global class after	
		player = new Player(LocalPlayer.name, 1);
		player.SetId(1);
		aiPlayer = new AIPlayer("computer", 2);
		aiPlayer.SetId(2);
		aiLevel = gameOptions.gameLevel;
		NewGame();
	}
	
	public void SetScoreLabel(Label player, Label ai){
		playerScoreLabel = player;
		aiScoreLabel = ai;
	}
	
	public void UpdateScoreLabel(){
		playerScoreLabel.Text = playerScore.GetScore().ToString();
		aiScoreLabel.Text = aiScore.GetScore().ToString();
	}

	public void NewGame()
	{
		CreateCells();
		SetCurrentDot();
		Columns = boardSize; // set the gridContainer's column number
		
		// IA part of the game
		aiPlayer = new AIPlayer("Computer", 2);
		aiPlay = new AIPlay();
		 
		
		playerKillingInRow = 0;
		aiKillingInRow = 0;
		
		turnNumber = 0;
		playerPoints = new List<Point>();
		aiPoints = new List<Point>();
		
		playerScore = new Score(player.GetId(), game.GetId());
		aiScore = new Score(aiPlayer.GetId(), game.GetId());
		
		currentPlayerId = player.GetId();
		position = Vector2.Zero;
		gameBoardMatrix = new Matrix(boardSize, boardSize);
		horizontalCheck = new Horizontal(gameBoardMatrix);
		verticalCheck = new Vertical(gameBoardMatrix);
		diagonalCheck = new Diagonal(gameBoardMatrix);
		antiDiagonalCheck = new AntiDiagonal(gameBoardMatrix);
	}
	
	
// this is the part where the IA examine the gameboard and apply it
	public override void _Process(double delta)
	{
		if (position.X == -1) {
			if (currentPlayerId == aiPlayer.GetId())  {
				Point newPoint = aiPlay.BestMove(gameBoardMatrix, player, aiPlayer, aiScore, playerScore, playerPoints, aiLevel);
				int x = newPoint.GetX();
				int y = newPoint.GetY();
				aiPoints.Add(newPoint);
		
				newPoint.SetValue(currentPlayerId);
				gameBoardMatrix.SetPoint(newPoint);
				gameBoardSate[y , x] = currentPlayerId;
				//restart player combo
				playerKillingInRow = 0;

				
				
				float hPosition =  x * cellSize + (cellSize / 2);
				float vPosition =  y * cellSize + (cellSize / 2);
				position = new Vector2(hPosition, vPosition);
				int initialScore = aiScore.GetScore();

				List<Point> deadPoint = CheckAndKill(gameBoardMatrix, horizontalCheck, verticalCheck, diagonalCheck, antiDiagonalCheck, aiPoints, playerPoints, aiScore);

				if (aiScore.GetScore() == initialScore) {
					currentPlayerId = player.GetId();
				} 
				else {
					DrawDeadPoint(deadPoint);
					aiKillingInRow += (aiScore.GetScore() - initialScore);
					CheckCombo();
				}
				
				MoveCurrentDot(position);
				AddChild(currentDot);
				SetCurrentDot();
				UpdateScoreLabel();
				CheckEndGame();
			}
		}
		}

	public void CreateCells()
	{
		int totalCell = boardSize * boardSize;
		gameBoardSate = new int[boardSize, boardSize];

		for (int i = 0; i < totalCell; i++)
		{
			TextureRect cell = (TextureRect)cellScene.Instantiate();
			AddChild(cell);
		}

		for (int i = 0; i < boardSize; i++)
		{
			for (int j = 0; j < boardSize; j++)
			{
				gameBoardSate[i, j] = 0;
			}
		}
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (inputEvent is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.ButtonIndex == MouseButton.Left)
			{
				if (mouseEvent.Pressed)
				{
					startPosition = mouseEvent.Position;
					initialPosition = GlobalPosition;

					if (IsEdge(mouseEvent.Position))
					{
						isResizing = true;
					}
					else
					{
						isMoving = true;
					}
				}
				else
				{
					// Terminer le mouvement ou redimensionnement et vérifier si un point doit être ajouté
					if (isMoving && startPosition.DistanceTo(mouseEvent.Position) <= DragThreshold)
					{
						AddPoint(mouseEvent);
					}

					isMoving = false;
					isResizing = false;
					dragOffset = Vector2.Zero;
				}
			}
		}
		else if (inputEvent is InputEventMouseMotion mouseMotionEvent)
		{
			if (isResizing)
			{
				// Calculer le changement de scale en fonction du déplacement de la souris
				Vector2 newScale = Scale + new Vector2(dragOffset.X / Size.X, dragOffset.Y / Size.Y);
				newScale = new Vector2(Mathf.Max(0.1f, newScale.X), Mathf.Max(0.1f, newScale.Y)); // Empêcher le scale de devenir trop petit

				// Calculer l'ajustement pour maintenir le centre fixe
				Vector2 centerOffset = (Size * (newScale - Scale) / 2);
				Scale = newScale;
				GlobalPosition -= centerOffset;

				// Mettre à jour le décalage pour le prochain événement de souris
				dragOffset = mouseMotionEvent.Position - startPosition;
				startPosition = mouseMotionEvent.Position;
			}
			else if (isMoving)
			{
				// Déplace le GridContainer en fonction de la souris
				dragOffset = mouseMotionEvent.Position - startPosition;
				if (dragOffset.Length() > DragThreshold)
				{
					GlobalPosition = initialPosition + dragOffset;
				}
			}
		}
	}


	public bool IsEdge(Vector2 mousePosition)
	{
		float leftEdge = Position.X;
		float topEdge = Position.Y;
		float rightEdge = leftEdge + Size.X * Scale.X;
		float bottomEdge = topEdge + Size.Y * Scale.Y;

		return (Mathf.Abs(mousePosition.X - leftEdge) < EdgeThreshold || 
				Mathf.Abs(mousePosition.Y - topEdge) < EdgeThreshold || 
				Mathf.Abs(rightEdge - mousePosition.X) < EdgeThreshold || 
				Mathf.Abs(bottomEdge - mousePosition.Y) < EdgeThreshold);
	}

	private void AddPoint(InputEventMouseButton mouseEvent)
	{
		Vector2 clickPosition = mouseEvent.Position;
		// Calculer la position réelle sur la grille
		int x = (int)((clickPosition.X - GlobalPosition.X) / (cellSize * Scale.X));
		int y = (int)((clickPosition.Y - GlobalPosition.Y) / (cellSize * Scale.Y));

		if (x >= 0 && x < boardSize && y >= 0 && y < boardSize && gameBoardSate[y, x] == 0)
		{
			position = new Vector2(x * cellSize + (cellSize / 2), y * cellSize + (cellSize / 2));
			//	gameBoardSate[y, x] = currentPlayerId;
			Point newPoint = new Point(x,y);
			newPoint.SetValue(currentPlayerId);
			gameBoardMatrix.SetPoint(newPoint);
			gameBoardSate[y, x] = currentPlayerId;
			if (currentPlayerId == player.GetId()) {
				 playerPoints.Add(newPoint);
				aiKillingInRow = 0;
				
				int initialScore = playerScore.GetScore();
				
				List<Point> deadPoint = CheckAndKill(gameBoardMatrix, horizontalCheck, verticalCheck, diagonalCheck, antiDiagonalCheck, playerPoints, aiPoints, playerScore);
				
				if (playerScore.GetScore() == initialScore) {
					currentPlayerId = aiPlayer.GetId();
				} 
				else {
					DrawDeadPoint(deadPoint);		
					playerKillingInRow += (playerScore.GetScore() - initialScore);
					CheckCombo();
				}
				
			MoveCurrentDot(position);
			AddChild(currentDot);
			SetCurrentDot();
			position = new Vector2(-1,-1);
			UpdateScoreLabel();
			CheckEndGame();
			}
		}
	}
	
	public void CheckCombo(){
		if (playerKillingInRow >= 3) EmitSignal("Combo");	
	}
	
	private void SetCurrentDot()
	{
		currentDot = currentPlayerId == 1 ? (Node2D)blueDotScene.Instantiate() : (Node2D)redDotScene.Instantiate();
	}

	private void MoveCurrentDot(Vector2 position)
	{
		currentDot.Position = position;
		
	}
	
	
	private List<Point> RemoveKilledPoints(List<Point> points)
	{
		//points.RemoveAll(point => point.GetValue() < 0);
	  // Find all points that should be removed
		List<Point> removedPoints = points.Where(point => point.GetValue() < 0).ToList();

		// Remove these points from the original list
		points.RemoveAll(point => point.GetValue() < 0);

		// Return the list of removed points
		return removedPoints;
	}

	private List<Point> CheckAndKill(
		Matrix matrix,
		Horizontal horizontalCheck,
		Vertical verticalCheck,
		Diagonal diagonalCheck,
		AntiDiagonal antiDiagonalCheck,
		List<Point> pointListToCheck,
		List<Point> opponentPointListToCheck,
		Score playerScore)
	{
		foreach (Point pt in pointListToCheck)
		{
			horizontalCheck.CheckThreePointHorizontalAlignmentAndKillEnemies(matrix, pt, playerScore);
			verticalCheck.CheckThreePointVerticalAlignmentAndKillEnemies(matrix, pt, playerScore);
			diagonalCheck.CheckThreePointDiagonalAlignmentAndKillEnemies(matrix, pt, playerScore);
			antiDiagonalCheck.CheckThreePointAntiDiagonalAlignmentAndKillEnemies(matrix, pt, playerScore);
		}

		return RemoveKilledPoints(opponentPointListToCheck);
	}
	public void DrawDeadPoint(List<Point> deadPoint){
		foreach(Point point in deadPoint){
			int x = point.GetX();
			int y = point.GetY();
			Vector2 deadPosition = new Vector2(x * cellSize + (cellSize / 2), y * cellSize + (cellSize / 2));
			Node2D deadPointNode = (Node2D) deadDotScene.Instantiate();
			deadPointNode.Position = deadPosition;
			AddChild(deadPointNode);
		}
	}
	
	// endgame checking
	private void CheckEndGame(){
		turnNumber++;
		if (turnNumber == (boardSize * boardSize)) EmitSignal("EndGame");
	}
	public int[] GetScore(){
		return new int[]{playerScore.GetScore(), aiScore.GetScore()};
	}
}
