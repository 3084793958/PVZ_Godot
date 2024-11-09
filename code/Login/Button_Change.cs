using Godot;
using System;

public class Button_Change : TextureButton
{
    public override void _Ready()
    {
        Connect("mouse_entered", this, "Mouse_EnterEvent");
    }
    public override void _Pressed()
    {
        var Click = GetNode<AudioStreamPlayer>("/root/Login/button_Click");
        Click.Play();
        var player = GetNode<AnimationPlayer>("/root/Login/Main/Create_User/AnimationPlayer");
        player.Play("in");
        Public_Main.choose_user = -1;
    }
    public void Mouse_EnterEvent()
    {
        var Hover = GetNode<AudioStreamPlayer>("/root/Login/button_Hover");
        Hover.Play();
    }
}
