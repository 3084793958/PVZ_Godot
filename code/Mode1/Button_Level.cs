using Godot;
using System;

public class Button_Level : Button
{
    [Export] public int Level_Number=0;
    public async override void _Pressed()
    {
        var Click = GetNode<AudioStreamPlayer>("/root/Mode1/button_Click");
        Click.Play();
        await ToSignal(Click, "finished");
        if (Level_Number!=0&&Public_Main.user_name!=string.Empty)
        {
            ConfigFile file = new ConfigFile();
            file.SetValue("Level", "Level_Name", "res://level/Mode1/Mode1_"+Level_Number.ToString()+".cfg");
            file.Save("user://Users/"+Public_Main.user_name+"/level_number.cfg");
        }
        GetTree().ChangeScene("res://scene/In_Game/In_Game.tscn");
    }
}
