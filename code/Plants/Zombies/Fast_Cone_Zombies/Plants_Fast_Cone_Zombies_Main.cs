using Godot;
using System;

public class Plants_Fast_Cone_Zombies_Main : Normal_Plants_Zombies
{
    protected override void Re_Set_Process()
    {
        if (health < 364)
        {
            GetNode<Cone_Hat_Control>("Main/Main/Hat").Hide();
        }
        if (health < 180)
        {
            has_lose_Arm = true;
            GetNode<Node2D>("Main/Main/Out_Arm/4").Show();
            GetNode<Node2D>("Main/Main/Out_Arm/3").Hide();
            GetNode<Node2D>("Main/Main/Out_Arm/2").Hide();
            GetNode<Node2D>("Main/Main/Out_Arm/1").Hide();
        }
    }
    public override void _PhysicsProcess(float delta)
    {
        if (!GetNode<Area2D>("Main/Main/Zombies_Area").IsConnected("area_entered", this, nameof(Plants_Entered)))
        {
            return;
        }
        base._PhysicsProcess(delta);
        if (has_planted)
        {
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
                GetNode<Normal_Plants_Zombies_Area>("Main/Main/Zombies_Area").has_lose_head = true;
                GetNode<AnimationPlayer>("Main/Main/Lose_Head").Play("Lose_Head");
            }
        }
    }
    public override void _Ready()
    {
        speed_Normal = 4.5f;
        health_list.Clear();
        health_list.Add(new Health_Container(490, false));
        health_list.Add(new Health_Container(270, false));
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
