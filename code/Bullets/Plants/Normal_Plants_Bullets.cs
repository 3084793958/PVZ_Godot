using Godot;
using System;
using System.Collections.Generic;

public class Normal_Plants_Bullets : Node2D
{
    public bool has_touch = false;
    protected Normal_Zombies_Area Top_Zombies_Area = null;
    //protected List<Normal_Zombies_Area> Zombies_Area_List = new List<Normal_Zombies_Area>();
    public float speed_x = 5f;
    public float speed_y = 0f;
}
