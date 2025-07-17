using Godot;
using System;

public class H2SO4_Area2D : Control_Area_2D
{
    public bool has_become = false;
    public bool has_planted = false;
    [Export] public int hurt = 200;
    public bool has_died = false;
    public override void _Ready()
    {
        Area2D_type = "H2SO4";
    }
}
