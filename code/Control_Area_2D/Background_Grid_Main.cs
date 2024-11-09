using Godot;
using System;
using System.Collections.Generic;

public class Background_Grid_Main : Control_Area_2D
{
    [Export] public int type=1;
    [Export] public int[] pos = { 1, 1 };//行,列
    public List<Node2D> Normal_Plant_List=new List<Node2D>();
    public List<Node2D> Top_Plant_List = new List<Node2D>();
    public override void _Ready()
    {
        pos[1] = int.Parse(this.GetParent().Name);
        pos[0] = int.Parse(this.Name);
        Area2D_type = "Grid";
    }
}
