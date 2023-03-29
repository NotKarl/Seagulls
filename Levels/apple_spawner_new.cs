using Godot;
using System;

public partial class apple_spawner_new : Node2D
{
	int windowWidth, windowHeight;
	Vector2 applePos;
	Random rand = new Random();
	Node2D apple;

    public override void _Ready()
	{
		windowWidth = (int)GetViewport().GetVisibleRect().Size.X;
		windowHeight = (int)GetViewport().GetVisibleRect().Size.Y;

		apple = GetTree().Root.GetNode("GameLevel").GetNode<Node2D>("CollectableApple");

        _SpawnApple();
	}
	
	public override void _Process(double delta)
	{
	}

	public void _SpawnApple()
	{
		applePos = new Vector2(rand.Next(11,windowWidth-10), rand.Next(11,windowHeight-10));
		apple.Position = applePos;
		GD.Print(applePos);
    }
}
