using Godot;
using System;
using System.Collections.Generic;
public class Limited_Plants : Node2D
{
    [Export] protected int normal_ZIndex = 3;
    protected Timer Android_Timer = new Timer();
    protected bool Is_Double_Click = false;//for Android
    public Vector2 put_position = Vector2.Zero;//In_Game_Main使用
    public bool player_put = false;//In_Game_Main使用
    protected bool on_lock_grid = false;
    protected bool has_planted = false;
    public Card_Click_Button card_parent_Button = null;//Card使用
    public bool Tmp_Card_Used = false;
    public Card_Tmp_Main Tmp_card_parent = null;//Card_Tmp使用
    protected List<Background_Grid_Main> Dock_Area_2D_List = new List<Background_Grid_Main>();
    protected Background_Grid_Main Dock_Area_2D = null;
    protected bool has_first_cilck = false;
}
