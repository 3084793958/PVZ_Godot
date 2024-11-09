using Godot;
using System;

public class Create_User_Main : Control
{
    public override void _Ready()
    {
        if (Public_Main.user_name==string.Empty)
        {
            var player = GetNode<AnimationPlayer>("AnimationPlayer");
            player.Play("in");
        }
        else
        {
            var name_label = GetNode<Label>("/root/Login/Main/User/Show/Name_Rect/Name");
            name_label.Text = Public_Main.user_name;
        }
    }
}
