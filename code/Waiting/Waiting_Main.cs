using Godot;
using System;

public class Waiting_Main : Control
{
    public async override void _Ready()
    {
        await ToSignal(GetTree().CreateTimer(1.0f), "timeout");
        GetTree().ChangeScene("res://scene/In_Game/In_Game.tscn");
    }
}
