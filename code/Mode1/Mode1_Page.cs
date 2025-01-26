using Godot;
using System;

public class Mode1_Page : Control
{
    public override void _Ready()
    {
        var Page1 = GetNode<GridContainer>("Page1");
        for (int i = 0; i < Public_Main.Level_Mode1.Count; i++)
        {
            if (Page1.GetChildCount() > 15)
            { }
            else
            {
                var scene = GD.Load<PackedScene>("res://scene/Level/Level.tscn");
                var card_child = (Level_Main)scene.Instance();
                Page1.AddChild(card_child);
            }
        }
    }
}
