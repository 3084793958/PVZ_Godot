using Godot;
using System;

public class Start_Main : Node2D
{
    public async override void _Ready()
    {
        var Shadow = GetNode<ColorRect>("Main/Shadow");
        Shadow.Show();
        ConfigFile file = new ConfigFile();
        Error error = file.Load("user://Users/User_Data.cfg");
        if (error == Error.Ok)
        {
            Public_Main.user_name = (string)file.GetValue("User", "User", string.Empty);
            int load_number = 0;
            while (true)
            {
                string User_Name = (string)file.GetValue("User_List", load_number.ToString(), string.Empty);
                if (User_Name == string.Empty)
                {
                    break;
                }
                Public_Main.user_list.Add(User_Name);
                load_number++;
            }
            if (Public_Main.user_name == string.Empty && Public_Main.user_list[0] != string.Empty)
            {
                Public_Main.user_name = Public_Main.user_list[0];
            }
            if (Public_Main.user_name != string.Empty)
            { 
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
                    Public_Main.Using_Clone_Limit = (bool)file3.GetValue("Setting", "Limit", true);
                    Public_Main.Max_Object_Clone_In_F = (int)file3.GetValue("Setting", "Limit_Number", 5);
                    Public_Main.Show_Zombies_Health = (bool)file3.GetValue("Setting", "Zombies_Health", true);
                    Public_Main.Show_Plants_Health = (bool)file3.GetValue("Setting", "Plants_Health", true);
                    Public_Main.Show_Plants_Zombies_Health = (bool)file3.GetValue("Setting", "Plants_Zombies_Health", true);
                    Public_Main.Show_Mouse_Effect = (bool)file3.GetValue("Setting", "Mouse_Effect", true);
                }
                else
                {
                    Public_Main.debuging = false;
                    Public_Main.for_Android = false;
                    Public_Main.Using_Clone_Limit = true;
                    Public_Main.Max_Object_Clone_In_F = 5;
                    Public_Main.Show_Zombies_Health = true;
                    Public_Main.Show_Plants_Health = true;
                    Public_Main.Show_Plants_Zombies_Health = true;
                    Public_Main.Show_Mouse_Effect = true;
                }
            }
        }
        var ChangeScene_Start = GetNode<Change_Scene>("/root/Change_Scene");
        await ToSignal(GetTree().CreateTimer(1), "timeout");
        GetNode<Setting_bgm_HSlider>("/root/Setting/Setting/Menu/BGM/HSlider").Update_This();
        GetNode<Setting_sound_HSlider>("/root/Setting/Setting/Menu/Sound/HSlider").Update_This();
        GetNode<Setting_Button_Debug>("/root/Setting/Setting/Menu/Debug").Update_This();
        GetNode<Setting_Button_Android>("/root/Setting/Setting/Menu/Android").Update_This();
        GetNode<Setting_Button_Limit>("/root/Setting/Setting/More/More/Limit").Update_This();
        GetNode<Setting_Zombies_Health>("/root/Setting/Setting/More/More/Zombies_Health").Update_This();
        GetNode<Setting_Plants_Health>("/root/Setting/Setting/More/More/Plants_Health").Update_This();
        GetNode<Setting_Plants_Zombies_Health>("/root/Setting/Setting/More/More/Plants_Zombies_Health").Update_This();
        GetNode<Setting_Mouse_Effect>("/root/Setting/Setting/More/More/Mouse_Effect").Update_This();
        GetNode<LineEdit>("/root/Setting/Setting/More/More/Limit_LineEdit").Text = Public_Main.Max_Object_Clone_In_F.ToString();
        ChangeScene_Start.B_to_E(Shadow);
    }
}