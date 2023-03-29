using Godot;
using System;

public partial class player_snake : CharacterBody2D
{
    [Export] int speed = 3;
	Vector2 InputDirection = new(0, 0);
    Vector2 Up = new(0, -1);
    Vector2 Down = new(0, 1);
    Vector2 Left = new(-1, 0);
    Vector2 Right = new(1, 0);

    apple_spawner_new applespawner;

    int windowWidth, windowHeight;
    public override void _Ready()
    {
        windowWidth = (int)GetViewport().GetVisibleRect().Size.X;
        windowHeight = (int)GetViewport().GetVisibleRect().Size.Y;

        applespawner = GetTree().Root.GetNode("GameLevel").GetNode<apple_spawner_new>("AppleSpawner");
    }
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

        var collision = MoveAndCollide(InputDirection*speed);
        if (collision != null) { _CollectApple(); }


        if (Position.X < 0) { Position = new Vector2(windowWidth, Position.Y); }
        if (Position.X > windowWidth) { Position = new Vector2(0, Position.Y); }

        if (Position.Y < 0) { Position = new Vector2(Position.X, windowHeight); }
        if (Position.Y > windowHeight) { Position = new Vector2(Position.X, 0); }
    }
    public void _CollectApple()
    {
        applespawner._SpawnApple();
    }
}
