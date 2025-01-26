using Godot;
using System;

public class Setting_Button_Debug : Button
{
    public void Update_This()
    {
        this.Pressed = Public_Main.debuging;
    }
    public override void _Pressed()
    {
        var Click = GetNode<AudioStreamPlayer>("/root/Setting/Setting/button_Click");
        Click.Play();
        Public_Main.debuging = this.Pressed;
        ConfigFile file = new ConfigFile();
        file.SetValue("Setting", "debug", Public_Main.debuging);
        file.SetValue("Setting", "Android", Public_Main.for_Android);
        file.SetValue("Setting", "Limit", Public_Main.Using_Clone_Limit);
        file.SetValue("Setting", "Limit_Number", Public_Main.Max_Object_Clone_In_F);
        file.Save("user://Users/" + Public_Main.user_name + "/Develop_Setting.cfg");
    }
}
