using Godot;
using System;

public class AQ_Area : Control_Area_2D
{
    public bool has_planted = false;
    public override void _Ready()
    {
        Area2D_type = "AQ";
    }
}
