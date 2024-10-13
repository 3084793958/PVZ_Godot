using Godot;
using System;

public class Button_Re_name : Button
{
    public override void _Ready()
    {
        Disabled = Public_Main.choose_user == -1;
    }
    public override void _Process(float delta)
    {
        Disabled = Public_Main.choose_user == -1;
    }
    public async override void _Pressed()
    {
        var lineedit = GetNode<LineEdit>("../ReName_Main/LineEdit");
        lineedit.Text=Public_Main.user_list[Public_Main.choose_user];
        var Click = GetNode<AudioStreamPlayer>("/root/Login/button_Click");
        Click.Play();
        await ToSignal(Click, "finished");
        var ReName_Main = GetNode<TextureRect>("../ReName_Main");
        ReName_Main.Show();
    }
}
