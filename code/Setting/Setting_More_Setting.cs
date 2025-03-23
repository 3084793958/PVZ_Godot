using Godot;
using System;

public class Setting_More_Setting : Button
{
    public override void _Pressed()
    {
        var Click = GetNode<AudioStreamPlayer>("/root/Setting/Setting/button_Click");
        Click.Play();
        GetNode<TextureRect>("/root/Setting/Setting/More").Show();
    }
}
