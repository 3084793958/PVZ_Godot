using Godot;
using System;
using System.Collections.Generic;

public class Background_Grid_Main : Control_Area_2D
{
    [Export] public int type=1;//1:land 2:water
    [Export] public int[] pos = { 1, 1 };//行,列
    public List<Node2D> Normal_Plant_List=new List<Node2D>();
    public List<Node2D> Down_Plant_List = new List<Node2D>();
    public List<Node2D> Top_Plant_List = new List<Node2D>();
    public List<int> now_type = new List<int>();//末项
    public int small_number = 0;
    public override void _Ready()
    {
        now_type.Add(type);
        pos[1] = int.Parse(this.GetParent().Name);
        pos[0] = int.Parse(this.Name);
        Area2D_type = "Grid";
    }
    public bool Is_All_Small_in_normal()
    {
        bool result = true;
        for (int i = 0; i < Normal_Plant_List.Count; i++)
        {
            if (!(Normal_Plant_List[i] is Small_Shroom_Main)) 
            {
                result = false;
                break;
            }
        }
        return result;
    }
}
