using Godot;
using System;

public partial class OneplayerGameOptionsController : Node2D
{
	
	[Signal]
	public delegate void StartGameEventHandler(int boardSize, int gameTime, string player1, string player2);
	// By default, it's 5 minutes
	private int gameTime = 300;
	private Label timerLabel;
	private int boardSize = 15;
	private Button playButton;
	private string gameLevel = "easy";
	[Export]
	public ButtonGroup boardSizeButtonGroup { get; set; }
	[Export]
	public ButtonGroup difficultyButtonGroup { get; set; }
	[Export]
	public ButtonGroup dotColorButtonGroup { get; set; }
	
	[Signal]
	public delegate void ChangeSceneEventHandler(string currentScene);
	
	public override void _Ready()
	{	
		GetNode<Label>("PlayButton/Label").Text = "Play";
		Node timerNode = GetNode<Node>("GameTime").GetNode<Node>("Timer");
		timerLabel = timerNode.GetNode<Label>("TimerLabel");
		timerLabel.Text = ConvertSecondsToMinuteSecond(gameTime);
		
		
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
		// Connect each difficulty button to the appropriate method
		foreach (TextureButton button in difficultyButtonGroup.GetButtons()){
			button.Connect("pressed", new Callable(this, nameof(OnDifficultyButtonPressed)));
		}
	
	}

	public void OnBackButtonPressed()
	{
			EmitSignal(nameof(ChangeScene), "main_menu");
	}
	private void _OnPlayButtonPressed()
	{
		EmitSignal(nameof(ChangeScene), "oneplayer_game_board");
	}
	public OneplayerGameOptions GetGameOptions(){
			OneplayerGameOptions gameOptions = new OneplayerGameOptions();
		 
			gameOptions.boardSize = boardSize;
			gameOptions.gameTime = gameTime; 
			gameOptions.gameLevel = gameLevel;
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
		switch (((String) pressedButton.Name).Trim())
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
	
	private void OnDifficultyButtonPressed(){
		TextureButton pressedButton = (TextureButton) difficultyButtonGroup.GetPressedButton();
		gameLevel = ((String) pressedButton.Name).Trim();
	}
	private string ConvertSecondsToMinuteSecond(int totalSeconds)
	{
		int minutes = totalSeconds / 60;
		int seconds = totalSeconds % 60;
		return $"{minutes:D2}:{seconds:D2}";
	}
	
}



