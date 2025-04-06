using Godot;
using System;

public class Zombies_Fire_Area : Control_Area_2D
{
    [Export] public bool can_work = false;
    public override void _Ready()
    {
        Area2D_type = "Zombies_Fire";
    }
}
