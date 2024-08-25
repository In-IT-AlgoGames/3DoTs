using Godot;
using System;

public partial class main_option : Node2D
{
	[Signal]
	public delegate void ChangeSceneEventHandler(string nextScene);

	private AudioStreamPlayer2D _backgroundMusicPlayer; // Reference to the audio player

	public override void _Ready()
	{
		// Find and set up the AudioStreamPlayer2D
		_backgroundMusicPlayer = GetNode<AudioStreamPlayer2D>("teste");
		
		HSlider volumeSlider = GetNode<HSlider>("EditAudio/HSliderVolume");
	}

	private void _on_back_button_pressed()
	{
		EmitSignal(nameof(ChangeScene), "main_menu");
	}

	private void _on_h_slider_volume_value_changed(float value)
	{
		if (_backgroundMusicPlayer != null)
		{
			_backgroundMusicPlayer.VolumeDb = value; // Adjust volume as needed
		}
	}
}
