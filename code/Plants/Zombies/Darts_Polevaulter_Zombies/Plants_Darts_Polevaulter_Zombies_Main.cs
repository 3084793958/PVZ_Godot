using Godot;
using System;
using System.Collections.Generic;

public class Plants_Darts_Polevaulter_Zombies_Main : Normal_Plants_Zombies
{
    [Export] public bool has_lose_pole = false;
    [Export] public bool is_jumping = false;
    public List<Normal_Zombies_Area> Jump_Plants_List = new List<Normal_Zombies_Area>();
    public void Jump_Area2D_area_entered(Control_Area_2D area2D)
    {
        if (area2D.Area2D_type == "Zombies")
        {
            Jump_Plants_List.Add((Normal_Zombies_Area)area2D);
        }
    }
    public void Jump_Area2D_area_exited(Control_Area_2D area2D)
    {

        if (area2D == null)
        {
            return;
        }
        if (area2D.Area2D_type == "Zombies")
        {
            Jump_Plants_List.Remove((Normal_Zombies_Area)area2D);
        }

    }
    public override void _Process(float delta)
    {
        base._Process(delta);
        if (has_planted)
        {
            can_Eating = has_lose_pole && !is_jumping;
            Doing_jumping = is_jumping;
            if (health < 340 && !has_lose_Arm)
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
            if (!has_lose_pole && !is_jumping && !GetNode<AnimationPlayer>("Main/Main/Up").IsPlaying() && Jump_Plants_List != null)
            {
                if (Jump_Plants_List.Count != 0)
                {
                    bool can_work = false;
                    for (int i = 0; i < Jump_Plants_List.Count; i++)
                    {
                        if (Jump_Plants_List[i] != null)
                        {
                            if (Jump_Plants_List[i].has_plant)
                            {
                                can_work = true;
                                break;
                            }
                        }
                    }
                    if (can_work)
                    {
                        if (!GetNode<AnimationPlayer>("Main/Main/Up").IsPlaying() && !has_lose_Head)
                        {
                            Walk_Mode(false);
                            GetNode<AnimationPlayer>("Main/Main/Up").Play("Up");
                        }
                    }
                }
            }
        }
    }
    public override void _Ready()
    {
        speed_Normal = 5.5f;
        health_list.Clear();
        health_list.Add(new Health_Container(640, false));
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
    protected override void Walk_Mode(bool is_Walking)
    {
        if (is_Walking)
        {
            if (has_lose_pole)
            {
                GetNode<AnimationPlayer>("Main/Main/Walk1").Stop();
                GetNode<AnimationPlayer>("Main/Main/Walk2").Play("Walk");
            }
            else
            {
                GetNode<AnimationPlayer>("Main/Main/Walk1").Play("Walk");
                GetNode<AnimationPlayer>("Main/Main/Walk2").Stop();
            }
        }
        else
        {
            GetNode<AnimationPlayer>("Main/Main/Walk1").Stop();
            GetNode<AnimationPlayer>("Main/Main/Walk2").Stop();
        }
    }
    protected override bool Get_Walk_Mode()
    {
        return GetNode<AnimationPlayer>("Main/Main/Walk1").IsPlaying() || GetNode<AnimationPlayer>("Main/Main/Walk2").IsPlaying();
    }
    public void Darts_Hurt()
    {
        if (Jump_Plants_List != null)
        {
            if (Jump_Plants_List.Count != 0)
            {
                for (int i = 0; i < Jump_Plants_List.Count; i++)
                {
                    if (Jump_Plants_List[i].has_plant)
                    {
                        Jump_Plants_List[i].lose_health_number = 2000;
                        Jump_Plants_List[i].lose_health = true;
                    }
                }
            }
        }
    }
}
