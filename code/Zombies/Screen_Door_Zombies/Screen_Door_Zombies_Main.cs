using Godot;
using System;
using System.Threading.Tasks;

public class Screen_Door_Zombies_Main : Normal_Zombies
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
            }
            if (health <= 1870 && health >= 1095)
            {
                GetNode<Sprite>("Main/Main/Screen_Door/Main/1").Show();
                GetNode<Sprite>("Main/Main/Screen_Door/Main/2").Hide();
                GetNode<Sprite>("Main/Main/Screen_Door/Main/3").Hide();
            }
            else if (health < 1095 && health >= 680)
            {
                GetNode<Sprite>("Main/Main/Screen_Door/Main/1").Hide();
                GetNode<Sprite>("Main/Main/Screen_Door/Main/2").Show();
                GetNode<Sprite>("Main/Main/Screen_Door/Main/3").Hide();
            }
            else if (health < 680 && health >= 300)
            {
                GetNode<Sprite>("Main/Main/Screen_Door/Main/1").Hide();
                GetNode<Sprite>("Main/Main/Screen_Door/Main/2").Hide();
                GetNode<Sprite>("Main/Main/Screen_Door/Main/3").Show();
            }
            else if (health < 300 && !GetNode<Node2D>("Main/Main/Out_Arm").Visible && !GetNode<AnimationPlayer>("Main/Main/Screen_Door/Turn").IsPlaying()) 
            {
                GetNode<AnimationPlayer>("Main/Main/Screen_Door/Turn").Play("Turn");
            }
            if (health_list[health_list.Count - 1].Health < 180 && !has_lose_Arm)
            {
                has_lose_Arm = true;
                GetNode<AnimationPlayer>("Main/Main/Lose_Arm").Play("Lose_Arm");
            }
            if ((health_list[health_list.Count - 1].Health < 90 || health_list[health_list.Count - 1].Info == "Door") && !has_lose_Head) 
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
        health_list.Add(new Health_Container(1600, true, "Door", true));//铁门
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
    protected override void Bullets_Sound_Play()
    {
        GetNode<AudioStreamPlayer>("Bucket").Play();
    }
    protected override bool Get_Walk_Mode()
    {
        return GetNode<AnimationPlayer>("Main/Main/Walk").IsPlaying();
    }
}
