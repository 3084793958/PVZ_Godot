using Godot;
using System;
using System.Collections.Generic;

public class Basketball_Zombies_Main : Normal_Zombies
{
    [Export] public bool has_lose_pole = false;
    [Export] public bool is_jumping = false;
    [Export] public bool Jumping_Run = false;
    public bool has_touch_Basketball_Zombies = false;
    public List<Stop_Basketball_Zombies_Area2D> Stop_Basketball_List = new List<Stop_Basketball_Zombies_Area2D>();
    public List<Normal_Plants_Area> Jump_Plants_List = new List<Normal_Plants_Area>();
    public void Jump_Area2D_area_entered(Area2D area_node)
    {
        if (!(area_node is Control_Area_2D area2D) || !IsInstanceValid(area2D))
        {
            return;
        }
        if (area2D == null)
        {
            return;
        }
        if (area2D.Area2D_type == "Plants")
        {
            Jump_Plants_List.Add((Normal_Plants_Area)area2D);
        }
    }
    public void Jump_Area2D_area_exited(Area2D area_node)
    {
        if (!(area_node is Control_Area_2D area2D) || !IsInstanceValid(area2D))
        {
            return;
        }
        if (area2D.Area2D_type == "Plants")
        {
            Jump_Plants_List.Remove((Normal_Plants_Area)area2D);
        }
    }
    public void Basketball_Stop_Entered(Area2D area_node)
    {
        if (!(area_node is Control_Area_2D area2D) || !IsInstanceValid(area2D))
        {
            return;
        }
        if (area2D.Area2D_type == "Stop_Basketball_Zombies")
        {
            Stop_Basketball_List.Add((Stop_Basketball_Zombies_Area2D)area2D);
        }
    }
    public void Basketball_Stop_Exited(Area2D area_node)
    {
        if (!(area_node is Control_Area_2D area2D) || !IsInstanceValid(area2D))
        {
            return;
        }
        if (area2D.Area2D_type == "Stop_Basketball_Zombies")
        {
            Stop_Basketball_List.Remove((Stop_Basketball_Zombies_Area2D)area2D);
        }
    }
    public override void _PhysicsProcess(float delta)
    {
        if (!GetNode<Area2D>("Main/Main/Zombies_Area").IsConnected("area_entered", this, nameof(Plants_Entered)))
        {
            return;
        }
        if (has_planted)
        {
            GetNode<Stop_Plants_Basketball_Zombies_Area2D>("Main/Main/Stop_Basketball").has_planted = has_planted;
            GetNode<Stop_Plants_Basketball_Zombies_Area2D>("Main/Main/Stop_Basketball").has_jumped = has_lose_pole;
            GetNode<Area2D>("Dock/Area2D").GlobalPosition = new Vector2(GetNode<Area2D>("Main/Main/Zombies_Area").GlobalPosition.x, GetNode<Area2D>("Dock/Area2D").GlobalPosition.y);
            GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area").Should_Ignore = is_jumping;
            can_Eating = has_lose_pole && !is_jumping;
            Doing_jumping = is_jumping;
            if (is_Lock_Ice)
            {
                Walk_Mode(false);
                GetNode<AnimationPlayer>("Main/Main/Eating").Stop();
            }
            if (health < 400 && !has_lose_Arm)
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
            if (!has_lose_pole && !is_jumping && !GetNode<AnimationPlayer>("Main/Main/Up").IsPlaying() && Jump_Plants_List != null)
            {
                if (Jump_Plants_List.Count != 0)
                {
                    bool can_work = false;
                    for (int i = 0; i < Jump_Plants_List.Count; i++)
                    {
                        if (Jump_Plants_List[i] != null)
                        {
                            if (Jump_Plants_List[i].has_planted && Jump_Plants_List[i].plants_type != "Top")
                            {
                                can_work = true;
                                break;
                            }
                        }
                    }
                    if (can_work)
                    {
                        if (!GetNode<AnimationPlayer>("Main/Main/Up").IsPlaying() && !has_lose_Head && !is_Lock_Ice)
                        {
                            Walk_Mode(false);
                            GetNode<AnimationPlayer>("Main/Main/Up").Play("Up");
                        }
                    }
                }
            }
            if (Stop_Basketball_List.Count != 0)
            {
                bool can_work = false;
                for (int i = 0; i < Stop_Basketball_List.Count; i++)
                {
                    if (Stop_Basketball_List[i] != null)
                    {
                        if (Stop_Basketball_List[i].has_planted && !Stop_Basketball_List[i].has_jumped) 
                        {
                            can_work = true;
                            break;
                        }
                    }
                }
                has_touch_Basketball_Zombies = can_work;
            }
            else
            {
                has_touch_Basketball_Zombies = false;
            }
            if (Jumping_Run && !has_lose_pole && GetNode<AnimationPlayer>("Main/Main/Up").IsPlaying() && !has_touch_Basketball_Zombies) 
            {
                this.GlobalPosition += new Vector2(-5.5f, 0);
            }
        }
        base._PhysicsProcess(delta);
    }
    public override void _Ready()
    {
        Jumping_Run = false;
        speed_Normal = -5.5f;
        can_Eating = false;
        health_list.Clear();
        health_list.Add(new Health_Container(700, false));
        GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area").hurt = 120;
        int count_health = 0;
        for (int i = 0; i < health_list.Count; i++)
        {
            if (health_list[i].Health < 0)
            {
                if (health_list[i].Is_lock)
                {
                    health_list[i].Health = 0;
                }
                else
                {
                    if (i + 1 < health_list.Count)
                    {
                        health_list[i + 1].Health += health_list[i].Health;
                        health_list[i].Health = 0;
                    }
                }
            }
            count_health += health_list[i].Health;
        }
        GetNode<AnimationPlayer>("Main/Main/All_Body/BasketBall/Run").Play("Run");
        health = count_health;
        Hypno_Path = "res://scene/Plants/Zombies/Basketball_Zombies/Plants_Basketball_Zombies.tscn";
        Spec_Info = "Basketball";
        base._Ready();
    }
    public override void Free_Self()
    {
        if (GetNode<Area2D>("Main/Main/Jump_Area2D").IsConnected("area_entered", this, nameof(Jump_Area2D_area_entered)))
        {
            GetNode<Area2D>("Main/Main/Jump_Area2D").Disconnect("area_entered", this, nameof(Jump_Area2D_area_entered));
            GetNode<Area2D>("Main/Main/Jump_Area2D").Disconnect("area_exited", this, nameof(Jump_Area2D_area_exited));
            GetNode<Area2D>("Main/Main/Jump_Area2D").Monitoring = false;
            GetNode<Area2D>("Main/Main/Jump_Area2D").Monitorable = false;
        }
        base.Free_Self();
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
                    if (Jump_Plants_List[i].has_planted && Jump_Plants_List[i].plants_type != "Top")
                    {
                        Jump_Plants_List[i].lose_health_number = 2000;
                        Jump_Plants_List[i].lose_health = true;
                    }
                }
            }
        }
    }
}
