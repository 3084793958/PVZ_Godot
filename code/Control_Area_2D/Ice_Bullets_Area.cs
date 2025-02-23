using Godot;
using System;

public class Ice_Bullets_Area : Normal_Plants_Bullets_Area
{
    public override void _Ready()
    {
        Area2D_type = "Plants_Bullets";
        Sec_Info = "Ice_Pea";
    }
}
