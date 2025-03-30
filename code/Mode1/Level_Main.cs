using Godot;
using System;

public class Level_Main : Control
{
    public int from_type = 1; 
    public override void _Ready()
    {
        int Level_Number = this.GetIndex();
        if (from_type == 1)
        {
            GetNode<Label>("Number/level_number").Text = Public_Main.Level_Mode1[Level_Number].Item1;
            GetNode<TextureRect>("Main/Main").Texture = Public_Main.Level_Mode1[Level_Number].Item3;
            GetNode<TextureRect>("Background/1").Visible = Public_Main.Level_Mode1[Level_Number].Item4 == 1;
            GetNode<TextureRect>("Background/2").Visible = Public_Main.Level_Mode1[Level_Number].Item4 == 2;
            GetNode<TextureRect>("Background/3").Visible = Public_Main.Level_Mode1[Level_Number].Item4 == 3;
        }
        else
        {
            GetNode<Label>("Number/level_number").Text = Public_Main.Level_Mode2[Level_Number].Item1;
            GetNode<TextureRect>("Main/Main").Texture = Public_Main.Level_Mode2[Level_Number].Item3;
            GetNode<TextureRect>("Background/1").Visible = Public_Main.Level_Mode2[Level_Number].Item4 == 1;
            GetNode<TextureRect>("Background/2").Visible = Public_Main.Level_Mode2[Level_Number].Item4 == 2;
            GetNode<TextureRect>("Background/3").Visible = Public_Main.Level_Mode2[Level_Number].Item4 == 3;
        }
    }
    public async void Go_In_Game()
    {
        int Level_Number = this.GetIndex();
        AudioStreamPlayer Click;
        if (from_type == 1)
        {
            Click = GetNode<AudioStreamPlayer>("/root/Mode1/button_Click");
        }
        else
        {
            Click = GetNode<AudioStreamPlayer>("/root/Mode2/button_Click");
        }
        Click.Play();
        await ToSignal(GetTree().CreateTimer(0.72f), "timeout");
        if (Public_Main.user_name != string.Empty)
        {
            ConfigFile file = new ConfigFile();
            if (from_type == 1)
            {
                file.SetValue("Level", "Level_Name", Public_Main.Level_Mode1[Level_Number].Item2);
            }
            else
            {
                file.SetValue("Level", "Level_Name", Public_Main.Level_Mode2[Level_Number].Item2);
            }
            file.Save("user://Users/" + Public_Main.user_name + "/level_number.cfg");
        }
        In_Game_Main.from_type = from_type;
        GetTree().ChangeScene("res://scene/In_Game/In_Game.tscn");
    }
}
