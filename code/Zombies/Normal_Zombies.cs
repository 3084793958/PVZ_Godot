using Godot;
using System;
using System.Collections.Generic;

public class Normal_Zombies : Node2D
{
    [Export] protected Color hurt_color;
    [Export] protected Color normal_color;
    public bool player_put = false;
    protected bool has_planted = false;
    public bool On_Boom_Effect = false;
    protected Vector2 Area_Vector2 = new Vector2(-1000, -1000);
    protected Background_Grid_Main dock_area_2d = null;
    protected bool on_lock_grid = false;
    public Card_Click_Button card_parent_Button = null;
    public List<Normal_Plants_Area> Plants_Area_2D_List=new List<Normal_Plants_Area>();
    //protected Normal_Plants_Area Touch_Area = null;//= new Normal_Plants_Area();
    public Normal_Plants_Area Top_Area_2D = null;
    public List<Normal_Boom_Area> Boom_Area_2D_List = new List<Normal_Boom_Area>();
    public List<C2H5OH_Died_Fire_Area> C2H5OH_Fire_Area_2D_List = new List<C2H5OH_Died_Fire_Area>();
    //
    public int health = 270;
    public float speed_x = 1f;
}
