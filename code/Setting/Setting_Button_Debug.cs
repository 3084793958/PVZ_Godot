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
        Public_Main.Save_Value();
    }
}
