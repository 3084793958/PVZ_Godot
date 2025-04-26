using Godot;
using System;

public class Mouse_Wave : Node2D
{
    public override void _PhysicsProcess(float delta)
    {
        if (Input.IsActionJustPressed("Left_Mouse") && Public_Main.Show_Mouse_Effect) 
        {
            var scene = GD.Load<PackedScene>("res://scene/Mouse_Wave/Mouse_Wave_Main.tscn");
            var plant_child = (Mouse_Wave_Main)scene.Instance();
            AddChild(plant_child);
        }
    }
}
