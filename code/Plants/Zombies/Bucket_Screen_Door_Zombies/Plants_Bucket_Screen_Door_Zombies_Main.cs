using Godot;
using System;

public class Plants_Bucket_Screen_Door_Zombies_Main : Normal_Plants_Zombies
{
    protected override void Re_Set_Process()
    {
        if (health < 1400)
        {
            GetNode<Node2D>("Main/Main/Screen_Door").Hide();
            GetNode<Node2D>("Main/Main/Out_Arm").Show();
        }
        if (health < 545)
        {
            GetNode<Cone_Hat_Control>("Main/Main/Head/Hat").Hide();
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
            if (health <= 3170 && health >= 2195)
            {
                GetNode<Sprite>("Main/Main/Screen_Door/Main/1").Show();
                GetNode<Sprite>("Main/Main/Screen_Door/Main/2").Hide();
                GetNode<Sprite>("Main/Main/Screen_Door/Main/3").Hide();
            }
            else if (health < 2195 && health >= 1780)
            {
                GetNode<Sprite>("Main/Main/Screen_Door/Main/1").Hide();
                GetNode<Sprite>("Main/Main/Screen_Door/Main/2").Show();
                GetNode<Sprite>("Main/Main/Screen_Door/Main/3").Hide();
            }
            else if (health < 1780 && health >= 1400)
            {
                GetNode<Sprite>("Main/Main/Screen_Door/Main/1").Hide();
                GetNode<Sprite>("Main/Main/Screen_Door/Main/2").Hide();
                GetNode<Sprite>("Main/Main/Screen_Door/Main/3").Show();
            }
            else if (health < 1400 && !GetNode<Node2D>("Main/Main/Out_Arm").Visible && !GetNode<AnimationPlayer>("Main/Main/Screen_Door/Turn").IsPlaying())
            {
                GetNode<AnimationPlayer>("Main/Main/Screen_Door/Turn").Play("Turn");
            }
            if (health < 1400)
            {
                if (health <= 1370 && health >= 1095)
                {
                    GetNode<Cone_Hat_Control>("Main/Main/Head/Hat").Cone_1();
                }
                else if (health < 1095 && health >= 820)
                {
                    GetNode<Cone_Hat_Control>("Main/Main/Head/Hat").Cone_2();
                }
                else if (health < 820 && health >= 545)
                {
                    GetNode<Cone_Hat_Control>("Main/Main/Head/Hat").Cone_3();
                }
                else if (health < 545 && !GetNode<Cone_Hat_Control>("Main/Main/Head/Hat").Hat_Run && !GetNode<AnimationPlayer>("Main/Main/Head/Hat/Hat/Hat_Out").IsPlaying())
                {
                    GetNode<Cone_Hat_Control>("Main/Main/Head/Hat").Cone_4();
                }
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
        health_list.Clear();
        health_list.Add(new Health_Container(1800, true, "Door", true, "Iron"));//铁门
        health_list.Add(new Health_Container(1100, false, "Hat", true, "Iron"));//铁桶
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
