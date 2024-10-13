using Godot;
using System;

public class Normal_Plants_Area : Control_Area_2D
{
    [Export] public string plants_type = "Normal";//Normal,Casing,...
    public bool has_planted = false;
    public override void _Ready()
    {
        Area2D_type = "Plants";
    }
}
