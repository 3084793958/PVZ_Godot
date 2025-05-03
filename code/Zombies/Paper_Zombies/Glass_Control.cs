using Godot;
using System;

public class Glass_Control : Node2D
{
    [Export] public bool Hat_Run = false;
    public Vector2 Run_Position = Vector2.Zero;
    public override void _Ready()
    {
        Glass_1();
    }
    public void Glass_1()
    {
        GetNode<Sprite>("Glass/1").Show();
    }
    public void Glass_2()
    {
        GetNode<AnimationPlayer>("Glass/Hat_Out").Play("Out");
    }
    public override void _Process(float delta)
    {
        if (!Hat_Run)
        {
            Run_Position = this.GlobalPosition;
        }
        if (Hat_Run && GetNode<AnimationPlayer>("Glass/Hat_Out").IsPlaying())
        {
            this.GlobalPosition = Run_Position;
        }
    }
}
