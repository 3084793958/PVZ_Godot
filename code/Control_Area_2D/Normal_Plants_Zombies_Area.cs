using Godot;
using System;

public class Normal_Plants_Zombies_Area : Normal_Plants_Area
{
    public bool is_eating_show = false;
    public bool can_hurt = true;
    public int hurt = 60;
    public Normal_Zombies_Area Choose_Zombies_Area = null;
    public bool has_lose_head = false;
    public override void _Ready()
    {
        Area2D_type = "Plants";
        Sec_Info = "Zombies";
    }
    public void Set_eating_show(bool value)
    {
        is_eating_show = value;
    }
}
