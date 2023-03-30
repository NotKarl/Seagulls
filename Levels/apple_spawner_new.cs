using Godot;
using System;

public partial class apple_spawner_new : Node2D
{
    int countNumber = -1;
    int windowWidth, windowHeight;
    Vector2 applePos;
    readonly Random rand = new();
    Node2D apple;
    Label counter;

    public override void _Ready()
    {
        windowWidth = (int)GetViewport().GetVisibleRect().Size.X;
        windowHeight = (int)GetViewport().GetVisibleRect().Size.Y;

        apple = GetTree().Root.GetNode("GameLevel").GetNode<Node2D>("CollectableApple");
        counter = GetTree().Root.GetNode("GameLevel").GetNode<Control>("Control").GetNode<Label>("Count");

        SpawnApple();
    }

    public override void _Process(double delta)
    {
    }

    public void SpawnApple()
    {
        applePos = new Vector2(rand.Next(11, windowWidth - 10), rand.Next(11, windowHeight - 10));
        apple.Position = applePos;
        countNumber++;
        GD.Print(countNumber, "  ", applePos);
        counter.Text = countNumber.ToString() + " points";
    }
}
