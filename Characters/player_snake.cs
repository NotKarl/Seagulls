using Godot;
using System;

public partial class player_snake : CharacterBody2D
{
    [Export] int speed = 1;
	Vector2 InputDirection = new(0, 0);
    Vector2 Up = new(0, -1);
    Vector2 Down = new(0, 1);
    Vector2 Left = new(-1, 0);
    Vector2 Right = new(1, 0);


	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		if (Input.IsActionJustPressed("Up") && !(InputDirection == Down)) {
			InputDirection = new(0, -1);
		}
        if (Input.IsActionJustPressed("Down") && !(InputDirection == Up))
        {
            InputDirection = new(0, 1);
        }
        if (Input.IsActionJustPressed("Left") && !(InputDirection == Right))
        {
            InputDirection = new(-1, 0);
        }
        if (Input.IsActionJustPressed("Right") && !(InputDirection == Left))
        {
            InputDirection = new(1, 0);
        }
        GD.Print(InputDirection);

        Velocity = InputDirection*speed;

        MoveAndSlide();
	}

    public override void _Process(double delta)
    {
    }
}
