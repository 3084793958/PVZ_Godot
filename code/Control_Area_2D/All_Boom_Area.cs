using Godot;
using System;

public class All_Boom_Area : Control_Area_2D
{
    protected Timer Boom_Timer = new Timer();
    public int hurt = 500;
    public bool can_do = false;
    public bool end_hurt = false;
    public override void _Ready()
    {
        this.AddChild(Boom_Timer);
        Boom_Timer.WaitTime = 0.3f;
        Boom_Timer.Autostart = false;
        Boom_Timer.OneShot = true;
        Boom_Timer.Connect("timeout", this, "Timer_End");
        Area2D_type = "All_Boom";
    }
    public void Start_hurting()
    {
        Boom_Timer.Start();
    }
    public void Timer_End()
    {
        end_hurt = true;
    }
}
