using Godot;
using System;

public class Login_Debug : Button
{
    public override void _Pressed()
    {
        var Click = GetNode<AudioStreamPlayer>("/root/Setting/button_Click");
        Click.Play();
    }
    public override void _Process(float delta)
    {
        var show_debug = GetNode<TextureRect>("..");
        show_debug.Visible = Public_Main.debuging;
    }
}
