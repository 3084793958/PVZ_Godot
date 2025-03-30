using Godot;
using System;

public class In_Game_Next_Wave : Button
{
    public override void _Pressed()
    {
        var Click = GetNode<AudioStreamPlayer>("/root/In_Game/button_Click");
        Click.Play();
    }
    public override void _Process(float delta)
    {
        Visible = Public_Main.debuging;
    }
}
