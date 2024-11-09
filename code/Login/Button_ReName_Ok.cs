using Godot;
using System;

public class Button_ReName_Ok : Button
{
    public override void _Ready()
    {
        if (Public_Main.choose_user != -1)
        {
            var lineedit = GetNode<LineEdit>("../LineEdit");
            Disabled &= lineedit.Text == string.Empty || lineedit.Text == Public_Main.user_list[Public_Main.choose_user];
        }
    }
    public override void _Process(float delta)
    {
        if (Public_Main.choose_user != -1)
        {
            var lineedit = GetNode<LineEdit>("../LineEdit");
            Disabled &= lineedit.Text == string.Empty || lineedit.Text == Public_Main.user_list[Public_Main.choose_user];
        }
    }
    public async override void _Pressed()
    {
        var lineedit = GetNode<LineEdit>("../LineEdit");
        for (int i = 0; i < Public_Main.user_list.Count; i++)
        {
            if (lineedit.Text == Public_Main.user_list[i])
            {
                return;
            }
        }
        Directory directory = new Directory();
        directory.Rename("user://Users/" + Public_Main.user_list[Public_Main.choose_user], "user://Users/" + lineedit.Text);
        Public_Main.user_list[Public_Main.choose_user] = lineedit.Text;
        var Click = GetNode<AudioStreamPlayer>("/root/Login/button_Click");
        Click.Play();
        await ToSignal(Click, "finished");
        var ReName_Main = GetNode<TextureRect>("..");
        ReName_Main.Hide();
        var name_list = GetNode<VBoxContainer>("/root/Login/Main/Create_User/Background/Main/Main");
        var change_name_object=(Name_Label_Main)name_list.GetChild(Public_Main.choose_user);
        change_name_object.Text = lineedit.Text;
        Public_Main.choose_user = -1;
    }
}
