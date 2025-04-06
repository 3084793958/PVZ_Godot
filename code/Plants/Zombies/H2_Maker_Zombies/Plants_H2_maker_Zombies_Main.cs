using Godot;
using System;

public class Plants_H2_maker_Zombies_Main : Normal_Plants_Zombies
{
    public override void _PhysicsProcess(float delta)
    {
        if (!GetNode<Area2D>("Main/Main/Zombies_Area").IsConnected("area_entered", this, nameof(Plants_Entered)))
        {
            return;
        }
        base._PhysicsProcess(delta);
        if (has_planted)
        {
            if (health < 180 && !has_lose_Arm)
            {
                has_lose_Arm = true;
                GetNode<AnimationPlayer>("Main/Main/Lose_Arm").Play("Lose_Arm");
            }
            if (health < 90 && !has_lose_Head)
            {
                has_lose_Head = true;
                GetNode<Normal_Plants_Zombies_Area>("Main/Main/Zombies_Area").has_lose_head = true;
                GetNode<AnimationPlayer>("Main/Main/Lose_Head").Play("Lose_Head");
            }
        }
    }
    public override void _Ready()
    {
        speed_Normal = 2.5f;
        health_list.Clear();
        health_list.Add(new Health_Container(360, false));
        base._Ready();
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
    protected override bool Get_Walk_Mode()
    {
        return GetNode<AnimationPlayer>("Main/Main/Walk").IsPlaying();
    }
    protected void Make_H2_Timer_Timeout()
    {
        if (has_planted && health > 0)
        {
            In_Game_Main.Plants_Clone_Request("res://scene/Plants/H2/H2.tscn", this.GlobalPosition, this.ZIndex - this.normal_ZIndex + 5);
        }
    }
}
