using Godot;
using System;

public class Normal_Plants_Area : Control_Area_2D
{
    [Export] public string plants_type = "Normal";//Normal,Casing,Top
    public bool has_planted = false;
    public bool lose_health = false;
    public int lose_health_number = 0;
    public bool is_MG = false;
    public override void _Ready()
    {
        Area2D_type = "Plants";
    }
}
