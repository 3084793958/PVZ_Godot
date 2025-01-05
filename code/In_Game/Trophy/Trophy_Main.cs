using Godot;
using System;

public class Trophy_Main : Node2D
{
    public bool Is_Shining = false;
    [Export] public int Mode = 1;
    public override void _Ready()
    {
        GetNode<AnimationPlayer>("Run").Play("Player");
    }
    public void Button_pressed()
    { 
        if (!Is_Shining)
        {
            GetNode<AnimationPlayer>("Shining/Shining_Player").Play("Player");
        }
    }
    public void Back_Mode()
    {
        if (!In_Game_Main.has_Lost_Brain)
        {
            if (Mode == 1)
            {
                GetTree().ChangeScene("res://scene/Mode1/Mode1.tscn");
            }
        }
    }
}
