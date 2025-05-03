using Godot;
using System;

public class Dropped_Level_Main : TextureRect
{
    public override void _Ready()
    {
        Hide();
    }
    public void Start()
    {
        GetNode<LineEdit>("LineEdit").Text = Login_Main.Dropped_File_Name;
        Show();
    }
    public override void _Process(float delta)
    {
        GetNode<Button>("Ok").Disabled = GetNode<LineEdit>("LineEdit").Text == string.Empty;
    }
    public void Cancel()
    {
        Hide();
        Login_Main.Dropped_File_Name = null;
        GetNode<LineEdit>("LineEdit").Text = null;
    }
    public async void Ok()
    {
        if (Public_Main.user_name != string.Empty)
        {
            ConfigFile file = new ConfigFile();
            file.SetValue("Level", "Level_Name", GetNode<LineEdit>("LineEdit").Text);
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
