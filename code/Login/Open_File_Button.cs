using Godot;
using System;

public class Open_File_Button : TextureButton
{
    public override void _Ready()
    {
        GetNode<FileDialog>("../Open_File").Hide();
    }
    public override void _Pressed()
    {

        if (OS.RequestPermissions()/*just Android*/) 
        {
            GetNode<FileDialog>("../Open_File").CurrentDir = "/storage/emulated/0/";
            GetNode<FileDialog>("../Open_File").Popup_();
            GetNode<FileDialog>("../Open_File").Hide();
            GetNode<FileDialog>("../Open_File").CurrentDir = "/storage/emulated/0/";
        }
        GetNode<FileDialog>("../Open_File").Popup_();
    }
    public async void Choose_File_OK(string res)
    {
        if (Public_Main.user_name != string.Empty)
        {
            ConfigFile file = new ConfigFile();
            file.SetValue("Level", "Level_Name", res);
            file.Save("user://Users/" + Public_Main.user_name + "/level_number.cfg");
            In_Game_Main.from_type = 0;
            var Click = GetNode<AudioStreamPlayer>("/root/Login/button_Click");
            Click.Play();
            await ToSignal(GetTree().CreateTimer(0.72f), "timeout");
            ConfigFile file2 = new ConfigFile();
            file2.SetValue("Sound", "bgm", Public_Main.bgm_value);
            file2.SetValue("Sound", "sound", Public_Main.sound_value);
            file2.Save("user://Users/" + Public_Main.user_name + "/Sound_Data.cfg");
            GetTree().ChangeScene("res://scene/In_Game/In_Game.tscn");
        }
    }
}
