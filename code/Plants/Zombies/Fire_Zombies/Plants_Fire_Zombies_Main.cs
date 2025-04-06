using Godot;
using System;

public class Plants_Fire_Zombies_Main : Normal_Plants_Zombies
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
                GetNode<C2H5OH_Bullets_Fire_Area>("Main/Main/Bullets_Fire_Area").Monitorable = false;
                GetNode<C2H5OH_Bullets_Fire_Area>("Main/Main/Bullets_Fire_Area").can_work = false;
            }
            if (health >= 90)
            {
                GetNode<C2H5OH_Bullets_Fire_Area>("Main/Main/Bullets_Fire_Area").Monitorable = true;
                GetNode<C2H5OH_Bullets_Fire_Area>("Main/Main/Bullets_Fire_Area").can_work = true;
            }
        }
    }
    public override void _Ready()
    {
        speed_Normal = 3.5f;
        health_list.Clear();
        health_list.Add(new Health_Container(514, false));
        base._Ready();
        GetNode<AnimationPlayer>("Main/Main/In_Arm/3/Hurt_Fire/1/turn_fire").Play("fire");
        GetNode<Normal_Plants_Zombies_Area>("Main/Main/Zombies_Area").hurt = 150;//强大的啃咬伤害
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
