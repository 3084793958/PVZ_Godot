using Godot;
using System;

public class Login_Main : Node2D
{
    public override void _Ready()
    {
        Public_Main.Starting = false;
        var Click = GetNode<AudioStreamPlayer>("button_Click");
        Click.Stream.Set("loop", false);
        var Hover = GetNode<AudioStreamPlayer>("button_Hover");
        Hover.Stream.Set("loop", false);
        var player1 = GetNode<AnimationPlayer>("Main/User_Animation");
        player1.Play("load");
        var player2 = GetNode<AnimationPlayer>("Main/Mode_Animation");
        player2.Play("load");
    }
}
