using Godot;
using System;

public class Button_ReName_Cancel : Button
{
    public async override void _Pressed()
    {
        var Click = GetNode<AudioStreamPlayer>("/root/Login/button_Click");
        Click.Play();
        await ToSignal(Click, "finished");
        var ReName_Main = GetNode<TextureRect>("..");
        ReName_Main.Hide();
        Public_Main.choose_user = -1;
    }
}
