using Godot;
using System;

public class Ice_Fune_Shroom_Bullets_Area : Fune_Shroom_Bullets_Area
{
    public override void _Ready()
    {
        Area2D_type = "Fune_Shroom";
        Sec_Info = "Ice";
    }
}
