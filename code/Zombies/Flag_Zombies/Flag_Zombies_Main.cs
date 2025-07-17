using Godot;
using System;

public class Flag_Zombies_Main : Normal_Zombies
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
            if (health < 180 && !has_lose_Arm)
            {
                has_lose_Arm = true;
                GetNode<AnimationPlayer>("Main/Main/Lose_Arm").Play("Lose_Arm");
                GetNode<Sprite>("Main/Main/In_Arm/3/Flag1").Hide();
                GetNode<Sprite>("Main/Main/In_Arm/3/Flag2").Show();
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
        eating_speed = 3;
        speed_Normal = -4.5f;
        health_list.Clear();
        health_list.Add(new Health_Container(360, false));
        Hypno_Path = "res://scene/Plants/Zombies/Flag_Zombies/Plants_Flag_Zombies.tscn";
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
    public void Clone_Self_Zombies()
    {
        if (In_Game_Main.background_number == 3 || In_Game_Main.background_number == 4)
        {
            for (int i = 1; i <= 6; i++)
            {
                if (true)
                {
                    Vector2 Zombies_put_position;
                    int _ZIndex;
                    if (i == 1)
                    {
                        Zombies_put_position = new Vector2(1024, 124);
                        _ZIndex = 7;
                    }
                    else if (i == 2)
                    {
                        Zombies_put_position = new Vector2(1024, 216);
                        _ZIndex = 27;
                    }
                    else if (i == 3)
                    {
                        Zombies_put_position = new Vector2(1024, 313);
                        _ZIndex = 47;
                    }
                    else if (i == 4)
                    {
                        Zombies_put_position = new Vector2(1024, 398);
                        _ZIndex = 67;
                    }
                    else if (i == 5)
                    {
                        Zombies_put_position = new Vector2(1024, 477);
                        _ZIndex = 87;
                    }
                    else
                    {
                        Zombies_put_position = new Vector2(1024, 558);
                        _ZIndex = 107;
                    }
                    In_Game_Main.Zombies_Clone_Request("res://scene/Zombies/Simple_Zombies/Simple_Zombies.tscn", Zombies_put_position, _ZIndex);
                }
            }
        }
        else if (In_Game_Main.background_number == 1 || In_Game_Main.background_number == 2)
        {
            for (int i = 1; i <= 5; i++)
            {
                if (true)
                {
                    Vector2 Zombies_put_position;
                    int _ZIndex;
                    if (i == 1)
                    {
                        Zombies_put_position = new Vector2(1024, 138);
                        _ZIndex = 7;
                    }
                    else if (i == 2)
                    {
                        Zombies_put_position = new Vector2(1024, 234);
                        _ZIndex = 27;
                    }
                    else if (i == 3)
                    {
                        Zombies_put_position = new Vector2(1024, 338);
                        _ZIndex = 47;
                    }
                    else if (i == 4)
                    {
                        Zombies_put_position = new Vector2(1024, 434);
                        _ZIndex = 67;
                    }
                    else
                    {
                        Zombies_put_position = new Vector2(1024, 530);
                        _ZIndex = 87;
                    }
                    In_Game_Main.Zombies_Clone_Request("res://scene/Zombies/Simple_Zombies/Simple_Zombies.tscn", Zombies_put_position, _ZIndex);
                }
            }
        }
    }
}