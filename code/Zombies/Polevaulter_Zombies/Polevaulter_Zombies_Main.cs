using Godot;
using System;
using System.Collections.Generic;

public class Polevaulter_Zombies_Main : Normal_Zombies
{
    [Export] public bool has_lose_pole = false;
    [Export] public bool is_jumping = false;
    [Export] public bool Jumping_Run = false;
    [Export] public bool Jumping_Run_Lock_Pole = false;
    public Vector2 Jump_Pole_Position = Vector2.Zero;
    public bool has_Set_Jump_Pole_Position = false;
    public List<Normal_Plants_Area> Jump_Plants_List = new List<Normal_Plants_Area>();
    public void Set_Pole_Position()
    {
        if (!has_Set_Jump_Pole_Position)
        {
            has_Set_Jump_Pole_Position = true;
            Jump_Pole_Position = GetNode<Node2D>("Main/Main/Pole/Pos").GlobalPosition;
        }
    }
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
    public override void _PhysicsProcess(float delta)
    {
        if (!GetNode<Area2D>("Main/Main/Zombies_Area").IsConnected("area_entered", this, nameof(Plants_Entered)))
        {
            return;
        }
        if (has_planted)
        {
            GetNode<Area2D>("Dock/Area2D").GlobalPosition = new Vector2(GetNode<Area2D>("Main/Main/Zombies_Area").GlobalPosition.x, GetNode<Area2D>("Dock/Area2D").GlobalPosition.y);
            GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area").Should_Ignore = is_jumping;
            can_Eating = has_lose_pole && !is_jumping;
            Doing_jumping = is_jumping;
            if (is_Lock_Ice)
            {
                Walk_Mode(false);
                GetNode<AnimationPlayer>("Main/Main/Eating").Stop();
            }
            if (health < 340 && !has_lose_Arm)
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
                    if (can_work && !GetNode<AnimationPlayer>("In_Water").IsPlaying() && !GetNode<AnimationPlayer>("Out_Water").IsPlaying() && !GetNode<AnimationPlayer>("Out_Land").IsPlaying()) 
                    {
                        if (!GetNode<AnimationPlayer>("Main/Main/Up").IsPlaying() && !has_lose_Head && !is_Lock_Ice) 
                        {
                            Walk_Mode(false);
                            GetNode<AnimationPlayer>("Main/Main/Up").Play("Up");
                        }
                    }
                }
            }
            if (Jumping_Run && !has_lose_pole && GetNode<AnimationPlayer>("Main/Main/Up").IsPlaying())
            {
                this.GlobalPosition += new Vector2(-1.75f, 0);
            }
        }
        base._PhysicsProcess(delta);
    }
    public override void _Process(float delta)//牛逼   process高刷
    {
        if (has_planted)
        {
            if (Jumping_Run && !has_lose_pole && GetNode<AnimationPlayer>("Main/Main/Up").IsPlaying())
            {
                if (Jumping_Run_Lock_Pole)
                {
                    if (has_Set_Jump_Pole_Position)
                    {
                        this.GlobalPosition += Jump_Pole_Position - GetNode<Node2D>("Main/Main/Pole/Pos").GlobalPosition;
                    }
                }
            }
        }
    }
    public override void _Ready()
    {
        Jumping_Run = false;
        speed_Normal = -5.5f;
        can_Eating = false;
        health_list.Clear();
        health_list.Add(new Health_Container(640, false));
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
        health = count_health;
        Hypno_Path = "res://scene/Plants/Zombies/Polevaulter_Zombies/Plants_Polevaulter_Zombies.tscn";
        Spec_Info = "Pole";
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
}
