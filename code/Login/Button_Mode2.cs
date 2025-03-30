using Godot;
using System;

public class Button_Mode2 : TextureButton
{
    public override void _Ready()
    {
        Connect("mouse_entered", this, "Mouse_EnterEvent");
    }
    public async override void _Pressed()
    {
        var Click = GetNode<AudioStreamPlayer>("/root/Login/button_Click");
        Click.Play();
        await ToSignal(GetTree().CreateTimer(0.72f), "timeout");
        ConfigFile file = new ConfigFile();
        file.SetValue("Sound", "bgm", Public_Main.bgm_value);
        file.SetValue("Sound", "sound", Public_Main.sound_value);
        file.Save("user://Users/" + Public_Main.user_name + "/Sound_Data.cfg");
        GetTree().ChangeScene("res://scene/Mode2/Mode2.tscn");
    }
    public void Mouse_EnterEvent()
    {
        var Hover = GetNode<AudioStreamPlayer>("/root/Login/button_Hover");
        Hover.Play();
    }
}
