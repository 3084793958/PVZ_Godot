using Godot;
using System;

public class Dancer_Zombies_Main : Normal_Zombies
{
    public JackSon_Zombies_Main Parent_Zombies = null;
    public override void _PhysicsProcess(float delta)
    {
        if (!GetNode<Area2D>("Main/Main/Zombies_Area").IsConnected("area_entered", this, nameof(Plants_Entered)))
        {
            return;
        }
        base._PhysicsProcess(delta);
        if (has_planted)
        {
            if (!IsInstanceValid(Parent_Zombies))
            {
                Parent_Zombies = null;
            }
            if (Parent_Zombies != null)
            {
                dancing_stop = Parent_Zombies.dancer_should_stop;
            }
            else
            {
                dancing_stop = false;
            }
            dancing_stop = dancing_stop && Plants_Area_2D == null;
            dancer_should_stop = Plants_Area_2D != null;
            if (is_Lock_Ice)
            {
                Walk_Mode(false);
                GetNode<AnimationPlayer>("Main/Main/Eating").Stop();
            }
            if (health < 300 && !has_lose_Arm)
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
        health_list.Add(new Health_Container(700, false));
        speed_Normal = -1.5f;
        Hypno_Path = "res://scene/Plants/Zombies/Dancer_Zombies/Plants_Dancer_Zombies.tscn";
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
