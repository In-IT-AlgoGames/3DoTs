using Godot;
using System;

public partial class SceneSwitcher : Node
{
	public Node2D currentNode;
	private Node2D nextNode = null ;
	private AnimationPlayer animationPlayer;
	private string nextScene = "";
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		currentNode = GetNode<Node2D>("MainMenu");
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animationPlayer.Play("fade_out");
		currentNode.Connect("ChangeScene", new Callable(this, nameof(OnChangeScene)));
	}

	 public void OnChangeScene(string nextScene){
		
		string scene_path = "";
	
		switch(nextScene){
			case "game_options":
				scene_path = "res://scenes/menu_screen/game_options/multiplayer_options.tscn";
				break;
			case "multiplayer_menu":
				scene_path = "res://scenes/menu_screen/menu/multiplayer_menu.tscn";
				break;
			case "main_menu":
				scene_path = "res://scenes/menu_screen/menu/main_menu.tscn";
				break;
			case "game_board":
				scene_path = "res://scenes/game_screen/game_screen_multiplayer.tscn";
				break;
			case "oneplayer_game_board":
				scene_path = "res://scenes/game_screen/game_screen_oneplayer.tscn";
				break;
			case "oneplayer_options":
				scene_path = "res://scenes/menu_screen/game_options/oneplayer_options.tscn";
				break;
			case "joker_menu":
				scene_path = "res://scenes/menu_screen/menu/joker_menu.tscn";
				break;
			case "option":
				scene_path = "res://scenes/option_screen/main_option.tscn";
				break;
			case "tutorial":
				scene_path = "res://scenes/tutorial/tutorial.tscn";
				break;
			default:
				GD.Print("scene inconnu");
				break;
		}
		if(nextScene == "game_board") 
		{
			MultiplayerGameScreen multiplayerGameScreen = (MultiplayerGameScreen) ((PackedScene) ResourceLoader.Load(scene_path)).Instantiate();
			MultiplayerGameOptionsController gameOptionsCon = (MultiplayerGameOptionsController) currentNode;
			
			MultiplayerGameOptions gameOptions = gameOptionsCon.GetGameOptions();
			multiplayerGameScreen.SetGameOptions(gameOptions);
			nextNode = multiplayerGameScreen;
			GD.Print(gameOptions.boardSize);
			this.nextScene = nextScene;
		}
		else if (nextScene == "oneplayer_game_board"){
			OneplayerGameScreen oneplayerGameScreen = (OneplayerGameScreen) ((PackedScene) ResourceLoader.Load(scene_path)).Instantiate();
			OneplayerGameOptionsController gameOptionsCon = (OneplayerGameOptionsController) currentNode;
			
			OneplayerGameOptions gameOptions = gameOptionsCon.GetGameOptions();
			
			oneplayerGameScreen.SetGameOptions(gameOptions);
			nextNode = oneplayerGameScreen;
			this.nextScene = nextScene;
		}
		else {
			nextNode = (Node2D) ((PackedScene) ResourceLoader.Load(scene_path)).Instantiate();
		}
		nextNode.Connect("ChangeScene", new Callable(this, nameof(OnChangeScene)));
		
		animationPlayer.Play("fade_in");
		
	}
	
	private void OnAnimationFinished(StringName anim_name)
	{
		// Replace with function body.
		switch(anim_name) {
			case "fade_in":
				
				AddChild(nextNode);				
				currentNode.QueueFree();
				currentNode = nextNode;
				nextNode = null;
				animationPlayer.Play("fade_out");
			 	break;
			case "fade_out":
				 break;	
		}
	}
}



