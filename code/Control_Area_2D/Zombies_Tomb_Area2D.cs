using Godot;
using System;

public class Zombies_Tomb_Area2D : Control_Area_2D
{
    public bool has_planted = false;
    public override void _Ready()
    {
        Area2D_type = "Zombies_Tomb";
    }
}
