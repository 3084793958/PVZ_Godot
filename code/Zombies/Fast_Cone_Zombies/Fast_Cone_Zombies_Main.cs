using Godot;
using System;

public class Fast_Cone_Zombies_Main : Normal_Zombies
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
            if (is_Lock_Ice)
            {
                GetNode<AnimationPlayer>("Main/Main/Walk").Stop();
                GetNode<AnimationPlayer>("Main/Main/Eating").Stop();
                GetNode<AnimationPlayer>("Main/Main/Hat/Hat/Walk").Stop();
            }
            if (health <= 760 && health >= 548)
            {
                GetNode<Cone_Hat_Control>("Main/Main/Hat").Cone_1();
            }
            else if (health < 548 && health >= 456)
            {
                GetNode<Cone_Hat_Control>("Main/Main/Hat").Cone_2();
            }
            else if (health < 456 && health >= 364)
            {
                GetNode<Cone_Hat_Control>("Main/Main/Hat").Cone_3();
            }
            else if (health < 364 && !GetNode<Cone_Hat_Control>("Main/Main/Hat").Hat_Run && !GetNode<AnimationPlayer>("Main/Main/Hat/Hat/Hat_Out").IsPlaying())
            {
                GetNode<Cone_Hat_Control>("Main/Main/Hat").Cone_4();
            }
            if (health >= 270 && !GetNode<Cone_Hat_Control>("Main/Main/Hat").Hat_Run && !GetNode<AnimationPlayer>("Main/Main/Hat/Hat/Hat_Out").IsPlaying())
            {
                if (!GetNode<AnimationPlayer>("Main/Main/Hat/Hat/Walk").IsPlaying() && GetNode<AnimationPlayer>("Main/Main/Walk").IsPlaying())
                {
                    GetNode<AnimationPlayer>("Main/Main/Hat/Hat/Walk").Play("Walk");
                }
                else if (!GetNode<AnimationPlayer>("Main/Main/Walk").IsPlaying())
                {
                    GetNode<AnimationPlayer>("Main/Main/Hat/Hat/Walk").Stop();
                }
            }
            else
            {
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
        eating_speed = 3;
        speed_Normal = -4.5f;
        health_list.Clear();
        health_list.Add(new Health_Container(490, false, "Hat"));//路障
        health_list.Add(new Health_Container(270, false));
        Hypno_Path = "res://scene/Plants/Zombies/Fast_Cone_Zombies/Plants_Fast_Cone_Zombies.tscn";
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
}
