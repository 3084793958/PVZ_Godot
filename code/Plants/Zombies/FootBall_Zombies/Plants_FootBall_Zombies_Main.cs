using Godot;
using System;

public class Plants_FootBall_Zombies_Main : Normal_Plants_Zombies
{
    protected override void Re_Set_Process()
    {
        if (health<975)
        {
            GetNode<Node2D>("Main/Main/Head/Hat").Hide();
        }
        if (health < 350)
        {
            has_lose_Arm = true;
            GetNode<Node2D>("Main/Main/Right_Hand/1/2").Show();
            GetNode<Node2D>("Main/Main/Right_Hand/3").Hide();
            GetNode<Node2D>("Main/Main/Right_Hand/2").Hide();
            GetNode<Node2D>("Main/Main/Right_Hand/1/1").Hide();
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
            if (health <= 3500 && health >= 1700)
            {
                GetNode<Cone_Hat_Control>("Main/Main/Head/Hat").Cone_1();
            }
            else if (health < 1700 && health >= 1250)
            {
                GetNode<Cone_Hat_Control>("Main/Main/Head/Hat").Cone_2();
            }
            else if (health < 1250 && health >= 975)
            {
                GetNode<Cone_Hat_Control>("Main/Main/Head/Hat").Cone_3();
            }
            else if (health < 975 && !GetNode<Cone_Hat_Control>("Main/Main/Head/Hat").Hat_Run && !GetNode<AnimationPlayer>("Main/Main/Head/Hat/Hat/Hat_Out").IsPlaying())
            {
                GetNode<Cone_Hat_Control>("Main/Main/Head/Hat").Cone_4();
            }
            if (health < 350 && !has_lose_Arm)
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
        health_list.Clear();
        health_list.Add(new Health_Container(2800, true, "Hat", true, "Iron"));//橄榄球
        health_list.Add(new Health_Container(700, false));
        speed_Normal = 5.5f;
        label_health_up = 84;
        GetNode<Normal_Plants_Zombies_Area>("Main/Main/Zombies_Area").hurt = 120;
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
