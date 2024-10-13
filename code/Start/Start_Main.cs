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
            }
        }
        var ChangeScene_Start = GetNode<Change_Scene>("/root/Change_Scene");
        await ToSignal(GetTree().CreateTimer(1), "timeout");
        ChangeScene_Start.B_to_E(Shadow);
    }
}