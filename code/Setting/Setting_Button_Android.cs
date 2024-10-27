using Godot;
using System;

public class Setting_Button_Android : Button
{
    public void Update_This()
    {
        this.Pressed = Public_Main.for_Android;
    }
    public override void _Pressed()
    {
        var Click = GetNode<AudioStreamPlayer>("/root/Setting/Setting/button_Click");
        Click.Play();
        Public_Main.for_Android = this.Pressed;
        ConfigFile file = new ConfigFile();
        file.SetValue("Setting", "debug", Public_Main.debuging);
        file.SetValue("Setting", "Android", Public_Main.for_Android);
        file.Save("user://Users/" + Public_Main.user_name + "/Develop_Setting.cfg");
    }
}
