using Godot;
using System;

public class Fune_Shroom_Bullets_Area : Normal_Plants_Bullets_Area
{
    public override void _Ready()
    {
        Area2D_type = "Fune_Shroom";
        Sec_Info = "Fune_Shroom";//无效
    }
}
