using Godot;
using System;
using System.Collections.Generic;

public class Normal_Plants : Node2D
{
    static public bool Choosing = false;
    //
    [Export] protected Color hover_color;
    [Export] protected Color normal_color;
    protected Vector2 Area_Vector2 = new Vector2(-1000, -1000);
    protected Background_Grid_Main dock_area_2d = null;
    protected bool on_lock_grid = false;
    protected bool has_planted = false;
    protected bool on_Shovel = false;
    protected bool on_Bug = false;
    protected Shovel_Area2D Shovel_Area = null;
    protected Bug_Area2D Bug_Area = null;
    public Card_Click_Button card_parent_Button = null;
    protected Normal_Zombies_Area Zombies_Area_2D = null;
    protected List<Normal_Zombies_Area> Zombies_Area_2D_List = new List<Normal_Zombies_Area>();
    protected Normal_Zombies_Area Bullets_Zombies_Area_2D = null;
    protected List<Normal_Zombies_Area> Bullets_Zombies_Area_2D_List = new List<Normal_Zombies_Area>();
    //
    public int health = 300;
}
