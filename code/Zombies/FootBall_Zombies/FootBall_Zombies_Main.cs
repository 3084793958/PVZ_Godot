using Godot;
using System;
using System.Threading.Tasks;

public class FootBall_Zombies_Main : Normal_Zombies
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
            int Bucket_Health = 0;
            for (int i = 0; i < health_list.Count; i++)
            {
                if (health_list[i].Info == "Hat")
                {
                    Bucket_Health = health_list[i].Health;
                }
            }
            if (Bucket_Health <= 2800 && Bucket_Health >= 1000)
            {
                GetNode<Cone_Hat_Control>("Main/Main/Head/Hat").Cone_1();
            }
            else if (Bucket_Health < 1000 && Bucket_Health >= 550)
            {
                GetNode<Cone_Hat_Control>("Main/Main/Head/Hat").Cone_2();
            }
            else if (Bucket_Health < 550 && Bucket_Health >= 275)
            {
                GetNode<Cone_Hat_Control>("Main/Main/Head/Hat").Cone_3();
            }
            else if (Bucket_Health < 275 && !GetNode<Cone_Hat_Control>("Main/Main/Head/Hat").Hat_Run && !GetNode<AnimationPlayer>("Main/Main/Head/Hat/Hat/Hat_Out").IsPlaying())
            {
                GetNode<Cone_Hat_Control>("Main/Main/Head/Hat").Cone_4();
            }
            if (health < 350 && !has_lose_Arm)
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
        health_list.Add(new Health_Container(2800, true, "Hat", true, "Iron"));//橄榄球
        health_list.Add(new Health_Container(700, false));
        speed_Normal = -5.5f;
        label_health_up = 84;
        Hypno_Path = "res://scene/Plants/Zombies/FootBall_Zombies/Plants_FootBall_Zombies.tscn";
        GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area").hurt = 120;
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
    protected override void Set_Walk_Speed(float Walk_speed)
    {
        GetNode<AnimationPlayer>("Main/Main/Walk").PlaybackSpeed = Walk_speed * 2;
    }
}
