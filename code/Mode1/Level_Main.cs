using Godot;
using System;

public class Level_Main : Control
{
    public override void _Ready()
    {
        int Level_Number = this.GetIndex();
        GetNode<Label>("Number/level_number").Text = Public_Main.Level_Mode1[Level_Number].Item1;
        GetNode<TextureRect>("Main/Main").Texture = Public_Main.Level_Mode1[Level_Number].Item3;
    }
    public async void Go_In_Game()
    {
        int Level_Number = this.GetIndex();
        var Click = GetNode<AudioStreamPlayer>("/root/Mode1/button_Click");
        Click.Play();
        await ToSignal(GetTree().CreateTimer(0.72f), "timeout");
        if (Public_Main.user_name != string.Empty)
        {
            ConfigFile file = new ConfigFile();
            file.SetValue("Level", "Level_Name", Public_Main.Level_Mode1[Level_Number].Item2);
            file.Save("user://Users/" + Public_Main.user_name + "/level_number.cfg");
        }
        GetTree().ChangeScene("res://scene/In_Game/In_Game.tscn");
    }
}
