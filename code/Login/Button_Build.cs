using Godot;
using System;

public class Button_Build : Button
{
    public override void _Ready()
    {
        var Build_Main = GetNode<TextureRect>("../Build_Main");
        if (Public_Main.user_name==string.Empty)
        {
            Build_Main.Show();
        }
        else
        {
            Build_Main.Hide();
        }
    }
    public async override void _Pressed()
    {
        var lineedit = GetNode<LineEdit>("../Build_Main/LineEdit");
        lineedit.Clear();
        var Click = GetNode<AudioStreamPlayer>("/root/Login/button_Click");
        Click.Play();
        await ToSignal(Click, "finished");
        var Build_Main = GetNode<TextureRect>("../Build_Main");
        Build_Main.Show();
        Public_Main.choose_user = -1;
    }
}
