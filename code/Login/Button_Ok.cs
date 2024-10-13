using Godot;
using System;

public class Button_Ok : Button
{
    public override void _Ready()
    {
        Disabled = Public_Main.choose_user == -1;
    }
    public override void _Process(float delta)
    {
        Disabled = Public_Main.choose_user == -1;
    }
    public override void _Pressed()
    {
        ConfigFile file = new ConfigFile();
        file.SetValue("User", "User", Public_Main.user_list[Public_Main.choose_user]);
        Public_Main.user_name = Public_Main.user_list[Public_Main.choose_user];
        for (int i = 0; i < Public_Main.user_list.Count; i++)
        {
            file.SetValue("User_List", i.ToString(), Public_Main.user_list[i]);
        }
        file.Save("user://Users/User_Data.cfg");
        var player = GetNode<AnimationPlayer>("/root/Login/Main/Create_User/AnimationPlayer");
        player.Play("out");
        var name_label = GetNode<Label>("/root/Login/Main/User/Show/Name_Rect/Name");
        name_label.Text = Public_Main.user_name;
    }
}
