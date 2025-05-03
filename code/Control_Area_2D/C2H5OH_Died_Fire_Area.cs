using Godot;
using System;

public class C2H5OH_Died_Fire_Area : Control_Area_2D
{
    [Export] public bool died = false;
    [Export] public int hurt = 70;
    public override void _Ready()
    {
        Area2D_type = "Died_Fire";
    }
}
