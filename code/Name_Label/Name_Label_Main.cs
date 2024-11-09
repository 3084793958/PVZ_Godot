using Godot;
using System;

public class Name_Label_Main : Button
{
    [Export] public int list_number; 
    public override void _Ready()
    {
        
    }
    public override void _Pressed()
    {
        Public_Main.choose_user = list_number;
    }
}
