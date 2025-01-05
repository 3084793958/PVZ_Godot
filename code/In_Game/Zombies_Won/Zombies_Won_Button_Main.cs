using Godot;
using System;

public class Zombies_Won_Button_Main : Button
{
    [Export] public int Mode = 1;
    public override void _Pressed()
    {
        if (Mode == 1)
        {
            GetTree().ChangeScene("res://scene/Mode1/Mode1.tscn");
        }
    }
}
