using Godot;
using System;

public class In_Game_Menu : Button
{
    public override void _Pressed()
    {
        var Click = GetNode<AudioStreamPlayer>("/root/In_Game/button_Click");
        Click.Play();
        var setting = GetNode<Control>("/root/Setting");
        setting.Raise();
        setting.Show();
    }
}
