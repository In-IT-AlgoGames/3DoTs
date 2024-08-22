using Godot;
using System;

public partial class LocalPlayer : Node
{
	static public string name = "test";
	static public int coins = 0;
	static public int diamonds = 0;
	static public int hint = 1; // un hint tous les heures
	static public int additionalMove = 0;
	static public int moveDot = 0;
	static public int sniperMode;
}
