using Godot;
using System;

public class Mg_Shining_Area : Control_Area_2D
{
    [Export] public bool start = false;
    public override void _Ready()
    {
        Area2D_type = "Mg_Shining";
    }
}
