using Godot;
using System;

public class Pause_Back : Button
{
    public override void _Pressed()
    {
        var Click = GetNode<AudioStreamPlayer>("../../button_Click");
        Click.Play();
        var Pause_main = GetNode<Pause_Main>("../../..");
        Pause_main.Hide();
    }
}
