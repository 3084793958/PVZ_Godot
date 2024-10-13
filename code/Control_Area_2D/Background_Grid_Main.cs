using Godot;
using System;
using System.Collections.Generic;

public class Background_Grid_Main : Control_Area_2D
{
    [Export] public int type=1;
    public List<Node2D> Normal_Plant_List=new List<Node2D>();
    public override void _Ready()
    {
        Area2D_type = "Grid";
    }
}
