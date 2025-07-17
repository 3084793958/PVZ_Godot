using Godot;
using System;

public class Normal_Plants_Area : Control_Area_2D
{
    [Export] public string plants_type = "Normal";//Normal,Casing,Top
    public bool has_planted = false;
    public bool lose_health = false;
    public int lose_health_number = 0;
    [Export] public bool Not_Joining_Touching = false;//与"is_MG"相等,Na!=Mg
    public override void _Ready()
    {
        Area2D_type = "Plants";
    }
}
