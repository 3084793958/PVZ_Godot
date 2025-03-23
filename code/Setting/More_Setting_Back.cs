using Godot;
using System;

public class More_Setting_Back : Button
{
    public override void _Pressed()
    {
        var Click = GetNode<AudioStreamPlayer>("/root/Setting/Setting/button_Click");
        Click.Play();
        GetNode<TextureRect>("/root/Setting/Setting/More").Hide();
    }
}
