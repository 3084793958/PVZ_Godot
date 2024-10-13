using Godot;
using System;

public class Setting_Back : Button
{
    public override void _Pressed()
    {
        var Click = GetNode<AudioStreamPlayer>("/root/Setting/Setting/button_Click");
        Click.Play();
        var setting = GetNode<Control>("/root/Setting");
        setting.Hide();
    }
}
