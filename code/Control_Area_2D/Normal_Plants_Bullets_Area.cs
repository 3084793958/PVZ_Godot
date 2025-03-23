using Godot;
using System;

public class Normal_Plants_Bullets_Area : Control_Area_2D
{
    public int hurt = 20;
    public Normal_Zombies_Area Choose_Zombies_Area = null;
    public Zombies_Tomb_Area2D Choose_Tomb_Area = null;
    public bool on_Tomb = false;
    public int hurt_type = 1;
    public int Bullets_Type = 1;
}
