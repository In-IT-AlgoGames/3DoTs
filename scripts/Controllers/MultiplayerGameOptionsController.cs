using Godot;
using System;

public partial class MultiplayerGameOptionsController : Node2D
{
	
	[Signal]
	public delegate void StartGameEventHandler(int boardSize, int gameTime, string player1, string player2);
	// By default, it's 5 minutes
	private int gameTime = 300;
	private Label timerLabel;
	
	private LineEdit player1Edit;
	private LineEdit player2Edit;
	
	private int boardSize = 15;
	private Button playButton;
	[Export]
	public ButtonGroup boardSizeButtonGroup { get; set; }
	 
	[Signal]
	public delegate void ChangeSceneEventHandler(string currentScene);
	
	
	public override void _Ready()
	{
		
		GetNode<Label>("PlayButton/Label").Text = "Play";
		Node timerNode = GetNode<Node>("GameTime").GetNode<Node>("Timer");
		timerLabel = timerNode.GetNode<Label>("TimerLabel");
		timerLabel.Text = ConvertSecondsToMinuteSecond(gameTime);
		
		player1Edit = GetNode<Node>("Player1Input").GetNode<LineEdit>("PlayerName");
		player2Edit = GetNode<Node>("Player2Input").GetNode<LineEdit>("PlayerName");
		
		player1Edit.Text = "Player 1";
		player1Edit.PlaceholderText = "Player 1";
		player2Edit.Text = "Player 2";
		player2Edit.PlaceholderText = "Player 2";
		
		
		var increaseButton = timerNode.GetNode<TextureButton>("IncreaseButton");
		var decreaseButton = timerNode.GetNode<TextureButton>("DecreaseButton");

		// Connect the pressed signals to the parent's methods using Callables
		increaseButton.Connect("pressed", new Callable(this, nameof(OnIncreaseButtonPressed)));
		decreaseButton.Connect("pressed", new Callable(this, nameof(OnDecreaseButtonPressed)));
	
		  // Connect each button in the ButtonGroup to the method
		foreach (TextureButton button in boardSizeButtonGroup.GetButtons())
		{
			button.Connect("pressed", new Callable(this, nameof(OnBoardSizeButtonPressed)));
		}
	
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void OnBackButtonPressed()
	{
			EmitSignal(nameof(ChangeScene), "multiplayer_menu");
	}
	private void _OnPlayButtonPressed()
	{
		EmitSignal(nameof(ChangeScene), "game_board");
	}
	public MultiplayerGameOptions GetGameOptions(){
			MultiplayerGameOptions gameOptions = new MultiplayerGameOptions();
			string player1Name = (player1Edit.Text.Trim() != "")? player1Edit.Text.Trim() : "Player 1";
			string player2Name = (player2Edit.Text.Trim() != "")? player2Edit.Text.Trim() : "Player 2";
			
			gameOptions.boardSize = boardSize;
			gameOptions.gameTime = gameTime;
			gameOptions.AddPlayerName(player1Name);
			gameOptions.AddPlayerName(player2Name);
			
			return gameOptions;			
		}
	 private void OnIncreaseButtonPressed()
	{
		gameTime += 30;
		if (gameTime > 900)
		{
			gameTime = 180;
		}
		UpdateTimerLabel();
	}

	private void OnDecreaseButtonPressed()
	{
		gameTime -= 30;
		if (gameTime < 180)
		{
			gameTime = 900;
		}
		UpdateTimerLabel();
	}

	private void UpdateTimerLabel()
	{
		timerLabel.Text = ConvertSecondsToMinuteSecond(gameTime);
	}
	
	
	private void OnBoardSizeButtonPressed()
	{
		// Get the button that was pressed
		TextureButton pressedButton = (TextureButton)boardSizeButtonGroup.GetPressedButton();
		// Update boardSize based on the text of the pressed button
		switch (((String)pressedButton.Name).Trim())
		{
			case "15x15":
				boardSize = 15;
				break;
			case "20x20":
				boardSize = 20;
				break;
			case "25x25":
				boardSize = 25;
				break;
			case "30x30":
				boardSize = 30;
				break;
			default:
				GD.Print("Unknown board size");
				break;
		}

	}
	private string ConvertSecondsToMinuteSecond(int totalSeconds)
	{
		int minutes = totalSeconds / 60;
		int seconds = totalSeconds % 60;
		return $"{minutes:D2}:{seconds:D2}";
	}
	
}
