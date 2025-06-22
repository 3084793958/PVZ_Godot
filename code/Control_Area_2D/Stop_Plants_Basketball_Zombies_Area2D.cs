using Godot;
using System;

public class Stop_Plants_Basketball_Zombies_Area2D : Control_Area_2D
{
    public bool has_planted = false;
    public bool has_jumped = false;
    public override void _Ready()
    {
        Area2D_type = "Stop_Plants_Basketball_Zombies";
    }
}
