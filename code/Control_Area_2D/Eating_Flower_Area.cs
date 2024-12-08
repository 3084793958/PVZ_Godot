using Godot;
using System;

public class Eating_Flower_Area : Control_Area_2D
{
    public Normal_Zombies_Area Choose_Eating_Zombies_Area = null;
    public bool can_eat = false;
    public override void _Ready()
    {
        Area2D_type = "Eating_Flower";
    }
}
