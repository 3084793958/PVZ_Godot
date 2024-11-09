using Godot;
using System;

public class Name_List_Main : VBoxContainer
{
    [Export] public Theme child_theme;
    public override void _Ready()
    {
        this.Theme = child_theme;
        if (Public_Main.user_list.Count!=0)
        {
            for (int i = 0; i < Public_Main.user_list.Count; i++)
            {
                var User_Name_Label = new Name_Label_Main
                {
                    Text = Public_Main.user_list[i],
                    Flat = true,
                    Theme = child_theme
                };
                User_Name_Label.list_number = i;
                this.AddChild(User_Name_Label);
            }
        }
    }
    static public void Update_this(Node node)
    {
        if (Public_Main.user_list.Count != 0)
        {
            var name_list = node.GetNode<VBoxContainer>("/root/Login/Main/Create_User/Background/Main/Main");
            for (int i = 0; i < Public_Main.user_list.Count; i++)
            {
                if (i<name_list.GetChildCount())
                {
                    continue;
                }
                var User_Name_Label = new Name_Label_Main
                {
                    Text = Public_Main.user_list[i],
                    Flat = true,
                    Theme = name_list.Theme,
                };
                User_Name_Label.list_number = i;
                name_list.AddChild(User_Name_Label);
            }
        }
    }
    static public void Call_Change_number(Node node)
    {
        var name_list = node.GetNode<VBoxContainer>("/root/Login/Main/Create_User/Background/Main/Main");
        for (int i = 0; i < Public_Main.user_list.Count; i++)
        {
            Name_Label_Main User_Name_Label = (Name_Label_Main)name_list.GetChild(i);
            if (User_Name_Label.list_number>Public_Main.choose_user)
            {
                User_Name_Label.list_number -= 1;
            }
        }
    }
}
