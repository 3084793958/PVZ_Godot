using Godot;
using System;

public class Setting_Back_All : Button
{
    public override void _Pressed()
    {
        var Click = GetNode<AudioStreamPlayer>("/root/Setting/Setting/button_Click");
        Click.Play();
        var setting = GetNode<Control>("/root/Setting");
        setting.Hide();
        GetTree().ChangeScene("res://scene/Login/Login.tscn");
    }
}
