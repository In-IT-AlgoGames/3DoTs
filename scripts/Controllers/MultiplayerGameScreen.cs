using Godot;
using System;

public partial class MultiplayerGameScreen : Node2D
{
	[Signal]
	public delegate void ChangeSceneEventHandler(string currentScene);
	
	MultiplayerGameOptions gameOptions = new MultiplayerGameOptions();
	Label player1;
	Label player2;
	Label player1Score;
	Label player2Score;
	Label timeLeftLabel;
	Timer timer;
	int timeLeft;
	int gameTime;
	// Game paused menu
	
	int hint = 0;
	int moveDot = 0;
	int additionalTurn = 0;
	int sniperNumber = 0;
	CanvasLayer gamePausedMenu;
	CanvasLayer gameOverMenu;
	MultiplayerGameBoard gameBoardSection;
	
	// managing Joker Label
	Label hintP1, moveDotP1, addMoveP1, sniperP1;
	Label hintP2, moveDotP2, addMoveP2, sniperP2;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		hintP1 = GetNode<Label>("JokerPlayer1/Hint/Number");
		hintP2 = GetNode<Label>("JokerPlayer2/Hint/Number");
		
		moveDotP1 = GetNode<Label>("JokerPlayer1/Move/Number");
		moveDotP2 = GetNode<Label>("JokerPlayer2/Move/Number");
		
		addMoveP1 = GetNode<Label>("JokerPlayer1/Additional/Number");
		addMoveP2 = GetNode<Label>("JokerPlayer2/Additional/Number");
		
		sniperP1 = GetNode<Label>("JokerPlayer1/SniperMode/Number");
		sniperP2 = GetNode<Label>("JokerPlayer2/SniperMode/Number");
		
		hintP1.Text = gameOptions.hint.ToString();
		moveDotP1.Text = gameOptions.moveDot.ToString();
		addMoveP1.Text = gameOptions.additionalTurn.ToString();
		sniperP1.Text = gameOptions.sniperNumber.ToString();

		hintP2.Text = gameOptions.hint.ToString();
		moveDotP2.Text = gameOptions.moveDot.ToString();
		addMoveP2.Text = gameOptions.additionalTurn.ToString();
		sniperP2.Text = gameOptions.sniperNumber.ToString();

		
		
		player1 = GetNode<Label>("Player1Name");
		player2 = GetNode<Label>("Player2Name");
		timeLeftLabel = GetNode<Label>("TimeLeft");
		timer = GetNode<Timer>("Timer");
		// Pause game menu
		gamePausedMenu = GetNode<CanvasLayer>("PauseGame");
		gamePausedMenu.Hide();
		
		// game over menu
		gameOverMenu = GetNode<CanvasLayer>("GameOverMenu");
		gameOverMenu.Hide();
		// Connect the Timer's timeout signal to a method
		timer.Timeout += OnTimerTimeout;
		timer.WaitTime = 1.0f;
		player1Score = GetNode<Label>("Player1Score");
		player2Score = GetNode<Label>("Player2Score");
		
		gameBoardSection.SetScoreLabel(player1Score, player2Score);
		
		SetUpScreen();
		}

	public void SetGameOptions(MultiplayerGameOptions gameOptions)
	{
		gameBoardSection = GetNode<MultiplayerGameBoard>("GameBoardSection");
		gameBoardSection.SetGameOptions(gameOptions);
		
		
		this.gameOptions = gameOptions;
		 hint = gameOptions.hint;
		 moveDot = gameOptions.moveDot;
		 additionalTurn = gameOptions.additionalTurn;
		 sniperNumber = gameOptions.sniperNumber;
		timeLeft = gameOptions.gameTime;
		gameTime = timeLeft;
	}

	private void SetUpScreen()
	{
		timer.Start();
		player1.Text = this.gameOptions.playerNames[0];
		player2.Text = this.gameOptions.playerNames[1];
		UpdateTimeLeftLabel();
	}

	// Method called every time the Timer times out (every second)
	private void OnTimerTimeout()
	{
		if (timeLeft > 0)
		{
			timeLeft--;
			UpdateTimeLeftLabel();
		}
		else
		{
			timer.Stop();
			OnEndGame();
		}
	}

	private void UpdateTimeLeftLabel()
	{
		timeLeftLabel.Text = ConvertSecondsToMinuteSecond(timeLeft);
	}

	private string ConvertSecondsToMinuteSecond(int totalSeconds)
	{
		int minutes = totalSeconds / 60;
		int seconds = totalSeconds % 60;
		return $"{minutes:D2}:{seconds:D2}";
	}
	private void OnRestartButton()
	{
		timeLeft = gameTime;
		GD.Print("test");
		GetTree().Paused = false;
		GetTree().CallGroup("BlueDot", "queue_free");
		GetTree().CallGroup("RedDot", "queue_free");
		GetTree().CallGroup("DeadIcon", "queue_free");
		GetTree().CallGroup("Cell", "queue_free");	
		player1Score.Text = "0";
		player2Score.Text = "0";
		gameBoardSection.NewGame();
		gamePausedMenu.Hide();
	}
	private void OnEndGame()
	{
		int[] score = gameBoardSection.GetScore();
		Label winnerLabel = gameOverMenu.GetNode<Label>("WinnerLabel");
		if (score[0] > score[1]) {
			winnerLabel.Text = player1.Text + " Wins !!!";
		}
		else if (score[0] < score[1]){
			winnerLabel.Text = player2.Text + " Wins !!!";			
		}
		else {
			winnerLabel.Text = "it's draw !!!";
		}
		
		gameOverMenu.Show();
		GetTree().Paused = true;
	}
	
	
	// Button Event 
	private void _on_pause_button_pressed()
	{
		GetTree().Paused = true;
		gamePausedMenu.Show();
	}
	
	private void OnPlayButton()
	{
		GetTree().Paused = false;
		gamePausedMenu.Hide();
	}
	private void OnPauseGameMenu()
	{
		GetTree().Paused = false;		
		EmitSignal(nameof(ChangeScene), "main_menu");
	}
	private void OnGoOut()
	{
		GetTree().Paused = false;
		EmitSignal(nameof(ChangeScene), "game_options");
	}
	private void GameOverGoOut()
	{
		GetTree().Paused = false;		
		EmitSignal(nameof(ChangeScene), "game_options");
	}
	private void OnGameOverToMenu()
	{
		GetTree().Paused = false;		
		EmitSignal(nameof(ChangeScene), "main_menu");
	}
	private void OnGameOverRestartButton()
	{
		OnRestartButton();
		gameOverMenu.Hide();
	}

}







