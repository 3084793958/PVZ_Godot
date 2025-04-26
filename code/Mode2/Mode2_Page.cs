using Godot;
using System;

public class Mode2_Page : Control
{
    public override void _Ready()
    {
        var Page1 = GetNode<GridContainer>("Page_Control/Page1");
        var Page2 = GetNode<GridContainer>("Page_Control/Page2");
        for (int i = 0; i < Public_Main.Level_Mode2.Count; i++)
        {
            if (Page1.GetChildCount() > 15)
            { }
            else
            {
                var scene = GD.Load<PackedScene>("res://scene/Level/Level.tscn");
                var card_child = (Level_Main)scene.Instance();
                card_child.from_type = 2;
                if (Page1.GetChildCount() < 15)
                {
                    Page1.AddChild(card_child);
                }
                else if (Page2.GetChildCount() < 15)
                {
                    Page2.AddChild(card_child);
                }
            }
        }
    }
}
