using Godot;
using System;

public class Background_Out_Land_Main : Control_Area_2D
{
    [Export] public int type = 1;//1:land(just land) 2:water
    public override void _Ready()
    {
        Area2D_type = "Out_Land";
    }
}
