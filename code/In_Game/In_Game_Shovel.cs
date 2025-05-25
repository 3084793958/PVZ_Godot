using Godot;
using System;

public class In_Game_Shovel : Button
{
    public override void _Ready()
    {
        GetNode<AudioStreamPlayer>("Shovel").Stream.Set("loop", false);
    }
    public async override void _Process(float delta)
    {
        if (In_Game_Main.is_playing && !GetTree().Paused)
        {
            if (Input.IsActionJustPressed("shovel"))
            {
                if (Normal_Plants.Choosing)
                {
                    Input.ActionPress("Right_Mouse");
                    await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
                    Input.ActionRelease("Right_Mouse");
                }
                else
                {
                    _Pressed();
                }
            }
        }
    }
    public override void _Pressed()
    {
        if (In_Game_Main.is_playing)
        {
            if (In_Game_Main.Choosing_List.Count == 0)
            {
                Normal_Plants.Choosing = false;
            }
            if (!Normal_Plants.Choosing)
            {
                GetNode<AudioStreamPlayer>("Shovel").Play();
                GetNode<TextureRect>("../Shovel").Hide();
                var scene = GD.Load<PackedScene>("res://scene/Shovel/Shovel.tscn");
                var plant_child = scene.Instance();
                Normal_Plants.Choosing = true;
                GetNode<Control>("/root/In_Game/Object").AddChild(plant_child);
            }
        }
    }
}
