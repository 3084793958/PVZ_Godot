using Godot;
using System;

public class Plants_Flag_Zombies_Main : Normal_Plants_Zombies
{
    protected override void Re_Set_Process()
    {
        if (health < 180)
        {
            has_lose_Arm = true;
            GetNode<Node2D>("Main/Main/Out_Arm/4").Show();
            GetNode<Node2D>("Main/Main/Out_Arm/3").Hide();
            GetNode<Node2D>("Main/Main/Out_Arm/2").Hide();
            GetNode<Node2D>("Main/Main/Out_Arm/1").Hide();
            GetNode<Sprite>("Main/Main/In_Arm/3/Flag1").Hide();
            GetNode<Sprite>("Main/Main/In_Arm/3/Flag2").Show();
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
                GetNode<Normal_Plants_Zombies_Area>("Main/Main/Zombies_Area").has_lose_head = true;
                GetNode<AnimationPlayer>("Main/Main/Lose_Head").Play("Lose_Head");
            }
        }
    }
    public override void _Ready()
    {
        speed_Normal = 4.5f;
        health_list.Clear();
        health_list.Add(new Health_Container(360, false));
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
                        Zombies_put_position = new Vector2(76, 124);
                        _ZIndex = 7;
                    }
                    else if (i == 2)
                    {
                        Zombies_put_position = new Vector2(76, 216);
                        _ZIndex = 27;
                    }
                    else if (i == 3)
                    {
                        Zombies_put_position = new Vector2(76, 299);
                        _ZIndex = 47;
                    }
                    else if (i == 4)
                    {
                        Zombies_put_position = new Vector2(76, 376);
                        _ZIndex = 67;
                    }
                    else if (i == 5)
                    {
                        Zombies_put_position = new Vector2(76, 477);
                        _ZIndex = 87;
                    }
                    else
                    {
                        Zombies_put_position = new Vector2(76, 558);
                        _ZIndex = 107;
                    }
                    In_Game_Main.Plants_Zombies_Clone_Request("res://scene/Plants/Zombies/Simple_Zombies/Plants_Simple_Zombies.tscn", Zombies_put_position, _ZIndex);
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
                        Zombies_put_position = new Vector2(76, 138);
                        _ZIndex = 7;
                    }
                    else if (i == 2)
                    {
                        Zombies_put_position = new Vector2(76, 234);
                        _ZIndex = 27;
                    }
                    else if (i == 3)
                    {
                        Zombies_put_position = new Vector2(76, 338);
                        _ZIndex = 47;
                    }
                    else if (i == 4)
                    {
                        Zombies_put_position = new Vector2(76, 434);
                        _ZIndex = 67;
                    }
                    else
                    {
                        Zombies_put_position = new Vector2(76, 530);
                        _ZIndex = 87;
                    }
                    In_Game_Main.Plants_Zombies_Clone_Request("res://scene/Plants/Zombies/Simple_Zombies/Plants_Simple_Zombies.tscn", Zombies_put_position, _ZIndex);
                }
            }
        }

    }
}
