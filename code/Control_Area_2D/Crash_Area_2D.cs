using Godot;
using System;

public class Crash_Area_2D : Control_Area_2D
{
    [Export] public int hurt = 500;
    public Normal_Zombies_Area Crash_Area = null;
    [Export] public bool has_planted = false;
    public bool Re_Used = false;
    public bool has_Hurt = false;
    public override void _Ready()
    {
        Area2D_type = "Crash_Hurt";
    }
    public void Set_Hurt(bool value)
    {
        has_Hurt = value;
    }
}
