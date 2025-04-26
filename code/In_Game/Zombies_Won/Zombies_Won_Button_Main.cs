using Godot;
using System;

public class Zombies_Won_Button_Main : Button
{
    [Export] public int Mode = 1;
    public override void _Pressed()
    {
        Mode = In_Game_Main.from_type;
        if (Mode == 1)
        {
            GetTree().ChangeScene("res://scene/Mode1/Mode1.tscn");
        }
        else if (Mode == 2)
        {
            GetTree().ChangeScene("res://scene/Mode2/Mode2.tscn");
        }
        else
        {
            GetTree().ChangeScene("res://scene/Login/Login.tscn");
        }
    }
}
