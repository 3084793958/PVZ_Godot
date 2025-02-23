using Godot;
using System;

public class Simple_Zombies_Main : Normal_Zombies
{
    public override void _Process(float delta)
    {
        base._Process(delta);
        if (has_planted)
        {
            if (is_Lock_Ice)
            {
                GetNode<AnimationPlayer>("Main/Main/Walk").Stop();
                GetNode<AnimationPlayer>("Main/Main/Eating").Stop();
                GetNode<AnimationPlayer>("Main/Main/Hat/Hat/Walk").Stop();
            }
            if (health < 180 && !has_lose_Arm)
            {
                has_lose_Arm = true;
                GetNode<AnimationPlayer>("Main/Main/Lose_Arm").Play("Lose_Arm");
            }
            if (health < 90 && !has_lose_Head)
            {
                has_lose_Head = true;
                GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area").has_lose_head = true;
                if (is_Ice)
                {
                    GetNode<AnimationPlayer>("Main/Main/Lose_Head_ICE").Play("Lose_Head");
                }
                else
                {
                    GetNode<AnimationPlayer>("Main/Main/Lose_Head").Play("Lose_Head");
                }
            }
        }
    }
    public override void _Ready()
    {
        health_list.Clear();
        health_list.Add(new Health_Container(270, false));
        base._Ready();
    }
    protected override void Plants_Entered(Control_Area_2D area2D)
    {
        base.Plants_Entered(area2D);
    }
    protected override void Plants_Exited(Control_Area_2D area2D)
    {
        base.Plants_Exited(area2D);
    }
    protected override void Dock_Entered(Control_Area_2D area2D)
    {
        base.Dock_Entered(area2D);
    }
    protected override void Dock_Exited(Control_Area_2D area2D)
    {
        base.Dock_Exited(area2D);
    }
    protected override void Ice_Timer_timeout()
    {
        base.Ice_Timer_timeout();
    }
    protected override void Remove_Zombies_Number()
    {
        base.Remove_Zombies_Number();
    }
    protected override void Ice_Lock_Timer_timeout()
    {
        base.Ice_Lock_Timer_timeout();
    }
    protected override void Free_Self()
    {
        base.Free_Self();
    }
    protected override void Walk_Mode(bool is_Walking)
    {
        if (is_Walking)
        {
            GetNode<AnimationPlayer>("Main/Main/Walk").Play("Walk");
        }
        else
        {
            GetNode<AnimationPlayer>("Main/Main/Walk").Stop();
        }
    }
    protected override void Bullets_Sound_Play()
    {
        base.Bullets_Sound_Play();
    }
    protected override bool Get_Walk_Mode()
    {
        return GetNode<AnimationPlayer>("Main/Main/Walk").IsPlaying();
    }
}
