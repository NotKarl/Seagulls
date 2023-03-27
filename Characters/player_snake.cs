using Godot;
using System;

public partial class player_snake : CharacterBody2D
{
	int speed = 1;
	Vector2 InputDirection = new(0, 0);


	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		//Input.ActionPress("");
		//Input.GetActionStrength("");
		GD.Print(InputDirection);
	}
}
