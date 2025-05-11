using Godot;
using System;
using System.Threading.Tasks;

public class Paper_Zombies_Main : Normal_Zombies
{
    [Export] protected bool has_Lost_Paper = false;
    public override void _PhysicsProcess(float delta)
    {
        if (!GetNode<Area2D>("Main/Main/Zombies_Area").IsConnected("area_entered", this, nameof(Plants_Entered)))
        {
            return;
        }
        base._PhysicsProcess(delta);
        if (has_planted)
        {
            if (has_Lost_Paper)
            {
                eating_speed = 5;
                speed = -3.0f;
                GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area").hurt = 300;//强大的啃咬伤害
            }
            else
            {
                eating_speed = 0.5f;
                speed_Normal = -1.5f;
                GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area").hurt = 120;
            }
            if (is_Lock_Ice)
            {
                GetNode<AnimationPlayer>("Main/Main/Walk").Stop();
                GetNode<AnimationPlayer>("Main/Main/Eating").Stop();
            }
            int Door_Health = 0;
            int Bucket_Health = 0;
            for (int i = 0; i < health_list.Count; i++)
            {
                if (health_list[i].Info == "Door")
                {
                    Door_Health = health_list[i].Health;
                }
                else if (health_list[i].Info == "Hat")
                {
                    Bucket_Health = health_list[i].Health;
                }
            }
            if (Door_Health <= 1000 && Door_Health >= 600)
            {
                GetNode<Sprite>("Main/Main/Paper/Main/1").Show();
                GetNode<Sprite>("Main/Main/Paper/Main/2").Hide();
                GetNode<Sprite>("Main/Main/Paper/Main/3").Hide();
            }
            else if (Door_Health < 600 && Door_Health >= 250)
            {
                GetNode<Sprite>("Main/Main/Paper/Main/1").Hide();
                GetNode<Sprite>("Main/Main/Paper/Main/2").Show();
                GetNode<Sprite>("Main/Main/Paper/Main/3").Hide();
            }
            else if (Door_Health < 250 && Door_Health >= 30)
            {
                GetNode<Sprite>("Main/Main/Paper/Main/1").Hide();
                GetNode<Sprite>("Main/Main/Paper/Main/2").Hide();
                GetNode<Sprite>("Main/Main/Paper/Main/3").Show();
            }
            else if (Door_Health < 30 && !GetNode<Node2D>("Main/Main/Out_Arm").Visible && !GetNode<AnimationPlayer>("Main/Main/Paper/Turn").IsPlaying())
            {
                GetNode<AnimationPlayer>("Main/Main/Paper/Turn").Play("Turn");
                GetNode<AnimationPlayer>("Main/Main/Angry").Play("Angry");
            }
            if (Bucket_Health <= 1200 && Bucket_Health >= 20)
            {
                GetNode<Glass_Control>("Main/Main/Head/Glass").Glass_1();
            }
            else if (Bucket_Health < 20 && !GetNode<Glass_Control>("Main/Main/Head/Glass").Hat_Run && !GetNode<AnimationPlayer>("Main/Main/Head/Glass/Glass/Hat_Out").IsPlaying())
            {
                GetNode<Glass_Control>("Main/Main/Head/Glass").Glass_2();
            }
            if (health_list[health_list.Count - 1].Health < 500 && !has_lose_Arm)
            {
                has_lose_Arm = true;
                GetNode<AnimationPlayer>("Main/Main/Lose_Arm").Play("Lose_Arm");
            }
            if ((health_list[health_list.Count - 1].Health < 90 || health_list[health_list.Count - 1].Info == "Door") && !has_lose_Head)
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
        eating_speed = 0.5f;
        speed_Normal = -1.5f;
        health_list.Add(new Health_Container(1000, true, "Door", false, "Paper"));//报纸
        health_list.Add(new Health_Container(1200, true, "Hat", true, "Iron"));//眼镜
        health_list.Add(new Health_Container(1800, false));
        Hypno_Path = "res://scene/Plants/Zombies/Paper_Zombies/Plants_Paper_Zombies.tscn";
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
        GetNode<AnimationPlayer>("Main/Main/Walk").PlaybackSpeed = Walk_speed;
    }
}
