using Godot;
using System;

public partial class OneplayerGameScreen : Node2D
{
	[Signal]
	public delegate void ChangeSceneEventHandler(string currentScene);
	
	OneplayerGameOptions gameOptions = new OneplayerGameOptions();
	
	Label timeLeftLabel;
	Timer timer;
	int timeLeft;
	int gameTime;
	// Game paused menu
	CanvasLayer gamePausedMenu;
	CanvasLayer gameOverMenu;
	OneplayerGameBoard gameBoardSection;
	
	// Ressource et combo management
	
	Label coinsLabel;
	Label diamondsLabel;
	
	
	Label playerLabel;
	Label computerLabel;
	Label playerScoreLabel;
	Label computerScoreLabel;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		timeLeftLabel = GetNode<Label>("TimeLeft");
		timer = GetNode<Timer>("Timer");
		// Pause game menu
		gamePausedMenu = GetNode<CanvasLayer>("PauseGame");
		gamePausedMenu.Hide();
		
		// game over menu
		gameOverMenu = GetNode<CanvasLayer>("GameOverMenu");
		gameOverMenu.Hide();
		
		// ressource displaying
		coinsLabel = GetNode<Label>("Coins/CoinValue");
		diamondsLabel = GetNode<Label>("Diamonds/DiamonValue");
		coinsLabel.Text = LocalPlayer.coins.ToString();
		diamondsLabel.Text = LocalPlayer.diamonds.ToString();
		// score displaying 
		playerLabel =GetNode<Label>("PlayerName");
		computerLabel = GetNode<Label>("ComputerName");
		playerScoreLabel = GetNode<Label>("PlayerScore");
		computerScoreLabel = GetNode<Label>("ComputerScore");
		gameBoardSection.SetScoreLabel(playerScoreLabel, computerScoreLabel);
		// Connect the Timer's timeout signal to a method
		timer.Timeout += OnTimerTimeout;
		timer.WaitTime = 1.0f;  
		SetUpScreen();
		}

	public void SetGameOptions(OneplayerGameOptions gameOptions)
	{
		GD.Print(gameOptions.boardSize);
		gameBoardSection = GetNode<OneplayerGameBoard>("GameBoardSection");
		gameBoardSection.SetGameOptions(gameOptions);
		
		
		this.gameOptions = gameOptions;
		timeLeft = gameOptions.gameTime;
		gameTime = timeLeft;
	}

	private void SetUpScreen()
	{
		timer.Start(); 
		playerLabel.Text = LocalPlayer.name;
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
		
		gameBoardSection.NewGame();
		gamePausedMenu.Hide();
	}
	private void OnEndGame()
	{
		int[] score = gameBoardSection.GetScore();
		Label winnerLabel = gameOverMenu.GetNode<Label>("WinnerLabel");
		
		
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
		EmitSignal(nameof(ChangeScene), "oneplayer_options");
	}
	private void GameOverGoOut()
	{
		GetTree().Paused = false;		
		EmitSignal(nameof(ChangeScene), "oneplayer_options");
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
	
	private void OnCombo()
	{
		LocalPlayer.coins ++;
		coinsLabel.Text = LocalPlayer.coins.ToString();
	}

}
