using Godot;
using System;

public class Bullets_Area : Control_Area_2D
{
    public int hurt = 20;
    public Normal_Zombies_Area Choose_Zombies_Area = null;
    public int hurt_type = 1;
    public int Bullets_Type = 1;
    public override void _Ready()
    {
        Area2D_type = "Plants_Bullets";
        Sec_Info = "Pea";
    }
}
