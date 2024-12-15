using Godot;
using System;
using System.Collections.Generic;

public class Normal_Zombies : Node2D
{
    //for Android
    protected Timer Android_Timer = new Timer();
    protected bool Is_Double_Click = false;
    //
    public Vector2 put_position = Vector2.Zero;
    //
    [Export] protected Color hurt_color;
    [Export] protected Color normal_color;
    [Export] protected Color Ice_hurt_color;
    [Export] protected Color Ice_normal_color;
    public bool is_Ice = false;
    public bool is_Lock_Ice = false;
    public bool player_put = false;
    protected bool has_planted = false;
    public bool On_Boom_Effect = false;
    public bool Is_Shining = false;
    public List<Mg_Shining_Area> Shining_Area_2D_List = new List<Mg_Shining_Area>();
    public List<H2SO4_Area2D> H2SO4_Area_2D_List = new List<H2SO4_Area2D>();
    protected Vector2 Area_Vector2 = new Vector2(-1000, -1000);
    protected Background_Grid_Main dock_area_2d = null;
    protected bool on_lock_grid = false;
    public Card_Click_Button card_parent_Button = null;
    public List<Normal_Plants_Area> Plants_Area_2D_List=new List<Normal_Plants_Area>();
    //protected Normal_Plants_Area Touch_Area = null;//= new Normal_Plants_Area();
    public Normal_Plants_Area Top_Area_2D = null;
    public List<Normal_Boom_Area> Boom_Area_2D_List = new List<Normal_Boom_Area>();
    public List<C2H5OH_Died_Fire_Area> C2H5OH_Fire_Area_2D_List = new List<C2H5OH_Died_Fire_Area>();
    public List<Crash_Area_2D> Crash_Area_2D_List = new List<Crash_Area_2D>();
    public List<Eating_Flower_Area> Eating_Flower_Area_2D_List = new List<Eating_Flower_Area>();
    //
    public int health = 270;
    public float speed_x = 1f;
    [Export] public int normal_ZIndex = 7;
}
