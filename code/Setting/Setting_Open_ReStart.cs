using Godot;
using System;

public class Setting_Open_ReStart : Button
{
    public async override void _Pressed()
    {
        if (In_Game_Main.is_playing)
        {
            var Click = GetNode<AudioStreamPlayer>("/root/Setting/Setting/button_Click");
            Click.Play();
            await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
            var setting = GetNode<Control>("/root/Setting");
            setting.Hide();
            GetTree().ChangeScene("res://scene/Waiting/Waiting.tscn");
        }
    }
}
