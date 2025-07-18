using Godot;
using System;
using System.Collections.Generic;

public class Plants_Basketball_Zombies_Main : Normal_Plants_Zombies
{
    [Export] public bool has_lose_pole = false;
    [Export] public bool is_jumping = false;
    [Export] public bool Jumping_Run = false;
    public bool has_touch_Basketball_Zombies = false;
    public List<Stop_Plants_Basketball_Zombies_Area2D> Stop_Basketball_List = new List<Stop_Plants_Basketball_Zombies_Area2D>();
    public List<Normal_Zombies_Area> Jump_Plants_List = new List<Normal_Zombies_Area>();
    protected override void Re_Set_Process()
    {
        if (health < 400)
        {
            has_lose_Arm = true;
            GetNode<Node2D>("Main/Main/All_Body/Out_Arm/4").Show();
            GetNode<Node2D>("Main/Main/All_Body/Out_Arm/3").Hide();
            GetNode<Node2D>("Main/Main/All_Body/Out_Arm/2").Hide();
            GetNode<Node2D>("Main/Main/All_Body/Out_Arm/1").Hide();
        }
    }
    protected override void Self_Hypno_Process()
    {
        //338
        GetNode<AnimationPlayer>("Main/Main/Up").Play("Just_Set");
        this.GlobalPosition = Hypno_Pos;
    }
    public void Jump_Area2D_area_entered(Area2D area_node)
    {
        if (!(area_node is Control_Area_2D area2D) || !IsInstanceValid(area2D))
        {
            return;
        }
        if (area2D.Area2D_type == "Zombies")
        {
            Jump_Plants_List.Add((Normal_Zombies_Area)area2D);
        }
    }
    public void Jump_Area2D_area_exited(Area2D area_node)
    {
        if (!(area_node is Control_Area_2D area2D) || !IsInstanceValid(area2D))
        {
            return;
        }
        if (area2D == null)
        {
            return;
        }
        if (area2D.Area2D_type == "Zombies")
        {
            Jump_Plants_List.Remove((Normal_Zombies_Area)area2D);
        }
    }
    public void Basketball_Stop_Entered(Area2D area_node)
    {
        if (!(area_node is Control_Area_2D area2D) || !IsInstanceValid(area2D))
        {
            return;
        }
        if (area2D.Area2D_type == "Stop_Plants_Basketball_Zombies")
        {
            Stop_Basketball_List.Add((Stop_Plants_Basketball_Zombies_Area2D)area2D);
        }
    }
    public void Basketball_Stop_Exited(Area2D area_node)
    {
        if (!(area_node is Control_Area_2D area2D) || !IsInstanceValid(area2D))
        {
            return;
        }
        if (area2D.Area2D_type == "Stop_Plants_Basketball_Zombies")
        {
            Stop_Basketball_List.Remove((Stop_Plants_Basketball_Zombies_Area2D)area2D);
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
            GetNode<Stop_Basketball_Zombies_Area2D>("Main/Main/Stop_Basketball").has_planted = has_planted;
            GetNode<Stop_Basketball_Zombies_Area2D>("Main/Main/Stop_Basketball").has_jumped = has_lose_pole;
            GetNode<Area2D>("Dock/Area2D").GlobalPosition = new Vector2(GetNode<Area2D>("Main/Main/Zombies_Area").GlobalPosition.x, GetNode<Area2D>("Dock/Area2D").GlobalPosition.y);
            can_Eating = has_lose_pole && !is_jumping;
            Doing_jumping = is_jumping;
            if (health < 400 && !has_lose_Arm)
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
                this.GlobalPosition += new Vector2(5.5f, 0);
            }
        }
    }
    public override void _Ready()
    {
        speed_Normal = 5.5f;
        health_list.Clear();
        health_list.Add(new Health_Container(700, false));
        GetNode<Normal_Plants_Zombies_Area>("Main/Main/Zombies_Area").hurt = 120;
        GetNode<AnimationPlayer>("Main/Main/All_Body/BasketBall/Run").Play("Run");
        base._Ready();
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
    protected override void Free_Self()
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
}
