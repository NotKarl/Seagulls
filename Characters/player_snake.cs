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
    AnimationNodeStateMachinePlayback animationStates;

    int windowWidth, windowHeight;

    public override void _Ready()
    {
        windowWidth = (int)GetViewport().GetVisibleRect().Size.X;
        windowHeight = (int)GetViewport().GetVisibleRect().Size.Y;

        applespawner = GetTree().Root.GetNode("GameLevel").GetNode<apple_spawner_new>("AppleSpawner");
        animationStates = (AnimationNodeStateMachinePlayback)GetNode<AnimationTree>("AnimationTree").Get("parameters/playback");
    }

    State state = State.Down;
    public enum State
    {
        Up, Down, Left, Right
    }
    public void StateMachine(State state)
    {
        switch (state)
        {
            case State.Up:
                if (InputDirection == Down) { break; }
                InputDirection = new(0, -1);
                animationStates.Travel("AnimUp");
                break;

            case State.Down:
                if (InputDirection == Up) { break; }
                InputDirection = new(0, 1);
                animationStates.Travel("AnimDown");
                break;

            case State.Left:
                if (InputDirection == Right) { break; }
                InputDirection = new(-1, 0);
                animationStates.Travel("AnimLeft");
                break;

            case State.Right:
                if (InputDirection == Left) { break; }
                InputDirection = new(1, 0);
                animationStates.Travel("AnimRight");
                break;
        }
    }
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (Input.IsActionJustPressed("Up"))
        {
            StateMachine(State.Up);
        }
        if (Input.IsActionJustPressed("Down"))
        {
            StateMachine(State.Down);
        }
        if (Input.IsActionJustPressed("Left"))
        {
            StateMachine(State.Left);
        }
        if (Input.IsActionJustPressed("Right"))
        {
            StateMachine(State.Right);
        }

        var collision = MoveAndCollide(InputDirection * speed);
        if (collision != null) { CollectApple(); }

        if (Position.X < 0) { Position = new Vector2(windowWidth, Position.Y); }
        if (Position.X > windowWidth) { Position = new Vector2(0, Position.Y); }

        if (Position.Y < 0) { Position = new Vector2(Position.X, windowHeight); }
        if (Position.Y > windowHeight) { Position = new Vector2(Position.X, 0); }
    }
    public void CollectApple()
    {
        applespawner.SpawnApple();
    }
}
