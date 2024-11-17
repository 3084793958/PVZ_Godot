using Godot;
using System;

public class Crash_Area_2D : Control_Area_2D
{
    [Export] public int hurt = 500;
    public Normal_Zombies_Area Crash_Area = null;
    public bool has_planted = false;
    public override void _Ready()
    {
        Area2D_type = "Crash_Hurt";
    }
}
