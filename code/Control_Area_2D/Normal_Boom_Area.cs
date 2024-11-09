using Godot;
using System;

public class Normal_Boom_Area : Control_Area_2D
{
    public int hurt = 1800;
    public bool can_do = false;
    public bool end_hurt = false;
    public override void _Ready()
    {
        Area2D_type = "Plants_Boom";
    }
    public void Start_hurting()
    {
        /*await ToSignal(GetTree(), "idle_frame");
        end_hurt = true;*/
    }
}
