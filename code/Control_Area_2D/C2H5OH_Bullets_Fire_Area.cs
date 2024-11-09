using Godot;
using System;

public class C2H5OH_Bullets_Fire_Area : Control_Area_2D
{
    [Export] public float MUL_number = 2f;
    [Export] public bool can_work = false;
    public override void _Ready()
    {
        Area2D_type = "Bullets_Fire";
    }
}
