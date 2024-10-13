using Godot;
using System;

public class Button_Back : Button
{
    public async override void _Pressed()
    {
        var Click = GetNode<AudioStreamPlayer>("/root/Mode1/button_Click");
        Click.Play();
        await ToSignal(Click, "finished");
        GetTree().ChangeScene("res://scene/Login/Login.tscn");
    }
}
