using Godot;
using System;
using System.Threading.Tasks;

public class H2_Maker_Zombies_Main : Normal_Zombies
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
            if (health_list.Get_Can_Ignore_Ice_Bullets())
            {
                is_Lock_Ice = false;
                is_Ice = false;
            }
            if (is_Lock_Ice)
            {
                GetNode<AnimationPlayer>("Main/Main/Walk").Stop();
                GetNode<AnimationPlayer>("Main/Main/Eating").Stop();
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
        speed_Normal = -2.5f;
        health_list.Clear();
        health_list.Add(new Health_Container(360, false));
        Hypno_Path = "res://scene/Plants/Zombies/H2_maker_Zombies/Plants_H2_maker_Zombies.tscn";
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
    protected void Make_H2_Timer_Timeout()
    {
        if (has_planted && health > 0)
        {
            In_Game_Main.Plants_Clone_Request("res://scene/Plants/H2/H2.tscn", this.GlobalPosition, this.ZIndex - this.normal_ZIndex + 5);
        }
    }
}
