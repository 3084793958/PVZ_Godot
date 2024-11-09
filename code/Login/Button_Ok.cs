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
        ConfigFile file2 = new ConfigFile();
        Error error2 = file2.Load("user://Users/" + Public_Main.user_name + "/Sound_Data.cfg");
        if (error2 == Error.Ok)
        {
            Public_Main.bgm_value = (int)file2.GetValue("Sound", "bgm", 100);
            Public_Main.sound_value = (int)file2.GetValue("Sound", "sound", 100);
        }
        else
        {
            Public_Main.bgm_value = 100;
            Public_Main.sound_value = 100;
        }
        ConfigFile file3 = new ConfigFile();
        Error error3 = file3.Load("user://Users/" + Public_Main.user_name + "/Develop_Setting.cfg");
        if (error3 == Error.Ok)
        {
            Public_Main.debuging = (bool)file3.GetValue("Setting", "debug", false);
            Public_Main.for_Android = (bool)file3.GetValue("Setting", "Android", false);
        }
        else
        {
            Public_Main.debuging = false;
            Public_Main.for_Android = false;
        }
        GetNode<Setting_bgm_HSlider>("/root/Setting/Setting/Menu/BGM/HSlider").Update_This();
        GetNode<Setting_sound_HSlider>("/root/Setting/Setting/Menu/Sound/HSlider").Update_This();
        GetNode<Setting_Button_Debug>("/root/Setting/Setting/Menu/Debug").Update_This();
        GetNode<Setting_Button_Android>("/root/Setting/Setting/Menu/Android").Update_This();
        var player = GetNode<AnimationPlayer>("/root/Login/Main/Create_User/AnimationPlayer");
        player.Play("out");
        var name_label = GetNode<Label>("/root/Login/Main/User/Show/Name_Rect/Name");
        name_label.Text = Public_Main.user_name;
    }
}
