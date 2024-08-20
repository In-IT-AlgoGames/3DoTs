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
		GD.Print("change scene");
		switch(nextScene){
			case "game_options":
				scene_path = "res://scenes/menu_screen/game_options/game_options.tscn";
				break;
			case "multiplayer_menu":
				scene_path = "res://scenes/menu_screen/main_menu/multiplayer_menu.tscn";
				break;
			case "main_menu":
				scene_path = "res://scenes/menu_screen/main_menu/main_menu.tscn";
				break;
			case "game_board":
				scene_path = "res://scenes/game_screen/game_screen.tscn";
				break;
			default:
				GD.Print("scene inconnu");
				break;
		}
		if(nextScene == "game_board") 
		{
			GameScreen gameScreen = (GameScreen) ((PackedScene) ResourceLoader.Load(scene_path)).Instantiate();
			GameOptionsController gameOptionsCon = (GameOptionsController) currentNode;
			
			GameOptions gameOptions = gameOptionsCon.GetGameOptions();
			gameScreen.SetGameOptions(gameOptions);
			nextNode = gameScreen;
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



