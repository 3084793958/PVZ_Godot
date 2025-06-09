using Godot;
using System;

public class Plants_Dancer_Zombies_Main : Normal_Plants_Zombies
{
    public Plants_JackSon_Zombies_Main Parent_Zombies = null;
    protected override void Re_Set_Process()
    {
        if (health < 300)
        {
            has_lose_Arm = true;
            GetNode<Node2D>("Main/Main/Main/Out_Arm/1/2").Show();
            GetNode<Node2D>("Main/Main/Main/Out_Arm/3").Hide();
            GetNode<Node2D>("Main/Main/Main/Out_Arm/2").Hide();
            GetNode<Node2D>("Main/Main/Main/Out_Arm/1/1").Hide();
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
            if (!IsInstanceValid(Parent_Zombies))
            {
                Parent_Zombies = null;
            }
            if (Parent_Zombies != null)
            {
                dancing_stop = Parent_Zombies.dancing_stop;
            }
            else
            {
                dancing_stop = false;
            }
            dancing_stop = dancing_stop && Zombies_Area_2D == null;
            dancer_should_stop = Zombies_Area_2D != null;
            if (health < 300 && !has_lose_Arm)
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
        health_list.Add(new Health_Container(700, false));
        speed_Normal = 1.5f;
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
