using Godot;
using System;
using System.Collections.Generic;

public class Flag_Control : Control
{
    [Export] public int All_Wave_Number = 0;
    [Export] public int All_Flag_Number = 0;
    [Export] public int Now_Wave_Number = 1;
    [Export] public int Now_Flag_Number = 1;//NO WAY
    public List<Flag_Main> Flag_Main_List = new List<Flag_Main>();
    [Export] public List<int> Flag_In_Wave_Number = new List<int>();
    public void Make_Flag()
    {
        for (int i = 0; i < All_Flag_Number; i++)
        {
            var scene = GD.Load<PackedScene>("res://scene/In_Game/Flag/Flag.tscn");
            var plant_child = (Flag_Main)scene.Instance();
            this.AddChild(plant_child);
            Flag_Main_List.Add(plant_child);
        }
        GetNode<TextureRect>("Head").Raise();
    }
    public void Update_Flag()
    {
        Now_Wave_Number--;//修正
        GetNode<Control>("Line").RectSize = new Vector2((float)Now_Wave_Number / All_Wave_Number *155+6,22);
        GetNode<TextureRect>("Head").RectPosition = new Vector2(139.2f- (float)Now_Wave_Number / All_Wave_Number * 150, -1.6f);
        for (int i = 0; i < Flag_Main_List.Count; i++)
        {
            Flag_Main_List[i].RectPosition = new Vector2(150-(float)Flag_In_Wave_Number[i]*150/All_Wave_Number, 0);
            if (Flag_In_Wave_Number[i] <= Now_Wave_Number && !Flag_Main_List[i]._Played) 
            {
                Flag_Main_List[i].Play_Player();
            }
        }
    }
    public override void _Process(float delta)
    {
        this.GetParent<Node2D>().Visible = In_Game_Main.is_playing;
        this.GetNode<Label>("../Debug").Visible = Public_Main.debuging;
    }
}
