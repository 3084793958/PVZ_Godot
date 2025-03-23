using Godot;
using System;

public class Setting_Zombies_Health : Button
{
    public void Update_This()
    {
        this.Pressed = Public_Main.Show_Zombies_Health;
    }
    public override void _Pressed()
    {
        var Click = GetNode<AudioStreamPlayer>("/root/Setting/Setting/button_Click");
        Click.Play();
        Public_Main.Show_Zombies_Health = this.Pressed;
        Public_Main.Save_Value();
    }
}
