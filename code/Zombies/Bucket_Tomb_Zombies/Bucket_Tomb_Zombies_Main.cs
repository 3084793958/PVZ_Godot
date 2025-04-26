using Godot;
using System;
using System.Threading.Tasks;

public class Bucket_Tomb_Zombies_Main : Normal_Zombies
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
            if (Bucket_Health <= 1100 && Bucket_Health >= 275)
            {
                GetNode<Cone_Hat_Control>("Main/Main/Head/Hat").Cone_1();
            }
            else if (Bucket_Health < 275 && !GetNode<Cone_Hat_Control>("Main/Main/Head/Hat").Hat_Run && !GetNode<AnimationPlayer>("Main/Main/Head/Hat/Hat/Hat_Out").IsPlaying())
            {
                GetNode<Cone_Hat_Control>("Main/Main/Head/Hat").Cone_4();
            }

            if (health_list[health_list.Count - 1].Health < 180 && !has_lose_Arm)
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
    protected void Make_Tomb()
    {
        var scene = GD.Load<PackedScene>("res://scene/Zombies/Tomb/Tomb.tscn");
        var plant_child = (Tomb_Main)scene.Instance();
        plant_child.ZIndex = ZIndex - normal_ZIndex + 3;
        plant_child.put_position = this.GlobalPosition;
        plant_child.player_put = false;
        GetNode<Control>("/root/In_Game/Object").AddChild(plant_child);
    }
    public override void _Ready()
    {
        health_list.Clear();
        health_list.Add(new Health_Container(1100, false, "Hat", true));//铁桶
        health_list.Add(new Health_Container(270, false));
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
}
