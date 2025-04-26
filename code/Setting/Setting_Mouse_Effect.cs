using Godot;
using System;

public class Setting_Mouse_Effect : Button
{
    public void Update_This()
    {
        this.Pressed = Public_Main.Show_Mouse_Effect;
    }
    public override void _Pressed()
    {
        var Click = GetNode<AudioStreamPlayer>("/root/Setting/Setting/button_Click");
        Click.Play();
        Public_Main.Show_Mouse_Effect = this.Pressed;
        Public_Main.Save_Value();
    }
}
