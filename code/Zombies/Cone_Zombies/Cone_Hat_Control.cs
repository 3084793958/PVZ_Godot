using Godot;
using System;

public class Cone_Hat_Control : Node2D
{
    [Export] public bool Hat_Run = false;
    public Vector2 Run_Position=Vector2.Zero;
    public override void _Ready()
    {
        Cone_1();
    }
    public void Cone_1()
    {
        GetNode<Sprite>("Hat/1").Show();
        GetNode<Sprite>("Hat/2").Hide();
        GetNode<Sprite>("Hat/3").Hide();
    }
    public void Cone_2()
    {
        GetNode<Sprite>("Hat/1").Hide();
        GetNode<Sprite>("Hat/2").Show();
        GetNode<Sprite>("Hat/3").Hide();
    }
    public void Cone_3()
    {
        GetNode<Sprite>("Hat/1").Hide();
        GetNode<Sprite>("Hat/2").Hide();
        GetNode<Sprite>("Hat/3").Show();
    }
    public void Cone_4()
    {
        GetNode<AnimationPlayer>("Hat/Hat_Out").Play("Out");
    }
    public override void _Process(float delta)
    {
        if (!Hat_Run)
        {
            Run_Position = this.GlobalPosition;
        }
        if (Hat_Run && GetNode<AnimationPlayer>("Hat/Hat_Out").IsPlaying()) 
        {
            this.GlobalPosition = Run_Position;
        }
    }
}