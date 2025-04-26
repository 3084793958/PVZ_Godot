using Godot;
using System;

public class Mouse_Wave_Main : Node2D
{
    public override void _Ready()
    {
        this.GlobalPosition = GetTree().Root.GetMousePosition();
        GetNode<AnimationPlayer>("Player").Play("Run");
    }
}
