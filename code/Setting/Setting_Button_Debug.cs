using Godot;
using System;

public class Setting_Button_Debug : Button
{
    public override void _Pressed()
    {
        var Click = GetNode<AudioStreamPlayer>("/root/Setting/Setting/button_Click");
        Click.Play();
    }
    public override void _Process(float delta)
    {
        Public_Main.debuging = this.Pressed;
    }
}
