using Godot;
using System;

public class Normal_Zombies_Area : Control_Area_2D
{
    public bool is_eating_show = false;
    public bool can_hurt = true;
    public int hurt = 60;
    public Normal_Plants_Area Choose_Plants_Area = null;
    public bool has_plant = false;
    public bool has_lose_head = false;
    public int health = 270;
    public bool Should_Ignore = false;
    public bool lose_health = false;
    public int lose_health_number = 0;
    public override void _Ready()
    {
        Area2D_type = "Zombies";
    }
    public void Set_eating_show(bool value)
    {
        is_eating_show = value;
    }
}
