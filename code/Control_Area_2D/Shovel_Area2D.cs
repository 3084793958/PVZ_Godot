using Godot;
using System;

public class Shovel_Area2D : Control_Area_2D
{
    public Normal_Plants_Area Choose_Plants_Area = null;
    public override void _Ready()
    {
        Area2D_type = "Shovel";
    }
}
