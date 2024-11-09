using Godot;
using System;

public class Button_Build_Ok : Button
{
    public override void _Ready()
    {
        var lineedit = GetNode<LineEdit>("../LineEdit");
        Disabled &= lineedit.Text==string.Empty;
    }
    public override void _Process(float delta)
    {
        var lineedit = GetNode<LineEdit>("../LineEdit");
        Disabled &= lineedit.Text == string.Empty;
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
        Public_Main.user_list.Add(lineedit.Text);
        Directory directory = new Directory();
        directory.MakeDir("user://Users/");
        directory.MakeDir("user://Users/" + lineedit.Text);
        /*ConfigFile file = new ConfigFile();
        file.SetValue("Level", "Level_Name", "res://level/Mode1/Mode1_1.cfg");
        file.Save("user://Users/" + lineedit.Text + "/level_number.cfg");*/
        var Click = GetNode<AudioStreamPlayer>("/root/Login/button_Click");
        Click.Play();
        await ToSignal(Click, "finished");
        var Build_Main = GetNode<TextureRect>("..");
        Build_Main.Hide();
        Name_List_Main.Update_this(this);
        Public_Main.choose_user = -1;
    }
}
