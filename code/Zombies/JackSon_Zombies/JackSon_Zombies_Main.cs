using Godot;
using System;
using System.Collections.Generic;
public class JackSon_Zombies_Main : Normal_Zombies
{
    public List<Dancer_Zombies_Main> Child_Zombies_List = new List<Dancer_Zombies_Main>();
    public bool has_touch_plants = false;
    public int clone_time = 0;
    public void Call_Up()
    {
        if (is_Lock_Ice)
        {
            return;
        }
        GetNode<AudioStreamPlayer>("Main/Main/Call").Play();
        clone_time++;
        for (int i = 0; i < Child_Zombies_List.Count; i++)
        {
            Child_Zombies_List[i].Parent_Zombies = null;
        }
        Child_Zombies_List.Clear();
        for (int i = 0; i < 4; i++)
        {
            var scene = GD.Load<PackedScene>("res://scene/Zombies/Dancer_Zombies/Dancer_Zombies.tscn");
            var plant_child = (Dancer_Zombies_Main)scene.Instance();
            plant_child.ZIndex = this.ZIndex;
            if (i == 0)
            {
                plant_child.put_position = this.GlobalPosition + new Vector2(-80, 0);
            }
            else if (i == 1)
            {
                plant_child.put_position = this.GlobalPosition + new Vector2(80, 0);
            }
            else if (i == 2)
            {
                if ((Dock_Area_2D == null && GetNode<Control>("/root/In_Game/Main/Background/").GetChild(0).GetNode<Background_Grid_Main>("Grid/0/" + (this.ZIndex / 20 + 1).ToString()).pos[0] == 1) || (Dock_Area_2D != null && Dock_Area_2D.pos[0] == 1)) 
                {
                    plant_child.put_position = this.GlobalPosition;
                }
                else
                {
                    if (Dock_Area_2D == null)
                    {
                        plant_child.put_position = new Vector2(this.GlobalPosition.x, GetNode<Control>("/root/In_Game/Main/Background/").GetChild(0).GetNode<Node2D>("Grid/0/" + (this.ZIndex / 20).ToString()).GlobalPosition.y + 10);
                    }
                    else
                    {
                        plant_child.put_position = new Vector2(this.GlobalPosition.x, Dock_Area_2D.GetNode<Node2D>("../" + (Dock_Area_2D.pos[0] - 1).ToString()).GlobalPosition.y + 10);

                    }
                    plant_child.ZIndex = this.ZIndex - 20;
                }
            }
            else
            {
                if ((Dock_Area_2D == null && (((In_Game_Main.background_number == 1 || In_Game_Main.background_number == 2) && GetNode<Control>("/root/In_Game/Main/Background/").GetChild(0).GetNode<Background_Grid_Main>("Grid/0/" + (this.ZIndex / 20 + 1).ToString()).pos[0] == 5) || ((In_Game_Main.background_number == 3 || In_Game_Main.background_number == 4) && GetNode<Control>("/root/In_Game/Main/Background/").GetChild(0).GetNode<Background_Grid_Main>("Grid/0/" + (this.ZIndex / 20 + 1).ToString()).pos[0] == 6))) || (Dock_Area_2D != null && (((In_Game_Main.background_number == 1 || In_Game_Main.background_number == 2) && Dock_Area_2D.pos[0] == 5) || ((In_Game_Main.background_number == 3 || In_Game_Main.background_number == 4) && Dock_Area_2D.pos[0] == 6)))) 
                {
                    plant_child.put_position = this.GlobalPosition;
                }
                else
                {
                    if (Dock_Area_2D == null)
                    {
                        plant_child.put_position = new Vector2(this.GlobalPosition.x, GetNode<Control>("/root/In_Game/Main/Background/").GetChild(0).GetNode<Node2D>("Grid/0/" + (this.ZIndex / 20 + 2).ToString()).GlobalPosition.y + 10);
                    }
                    else
                    {
                        plant_child.put_position = new Vector2(this.GlobalPosition.x, Dock_Area_2D.GetNode<Node2D>("../" + (Dock_Area_2D.pos[0] + 1).ToString()).GlobalPosition.y + 10);
                    }
                    plant_child.ZIndex = this.ZIndex + 20;
                }
            }
            plant_child.player_put = false;
            plant_child.Use_Out_Land_Ani = true;
            GetNode<Control>("/root/In_Game/Object").AddChild(plant_child);
            plant_child.Parent_Zombies = this;
            Child_Zombies_List.Add(plant_child);
        }
        if (clone_time % 3 == 0) 
        {
            for (int i = 0; i < GetNode<Control>("/root/In_Game/Object").GetChildCount(); i++)
            {
                if (GetNode<Control>("/root/In_Game/Object").GetChild(i) is Tomb_Main tomb_object)
                {
                    if (tomb_object.Clone_Time < 20)
                    {
                        tomb_object.Clone_Zombies();
                    }
                }
            }
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
            dancing_stop = GetNode<AnimationPlayer>("Main/Main/Up").IsPlaying();
            dancer_should_stop = GetNode<AnimationPlayer>("Main/Main/Up").IsPlaying() || Plants_Area_2D != null;
            if (!dancing_stop)
            {
                for (int i = 0; i < Child_Zombies_List.Count; i++)
                { 
                    if (Child_Zombies_List[i].dancer_should_stop)
                    {
                        dancing_stop = true;
                        break;
                    }
                }
            }
            dancing_stop = dancing_stop && Plants_Area_2D == null;
            if (GetNode<AnimationPlayer>("Main/Main/Up").IsPlaying())
            {
                dancing_stop = true;
            }
            if (!has_touch_plants && Plants_Area_2D != null && !is_Lock_Ice) 
            {
                has_touch_plants = true;
                GetNode<AnimationPlayer>("Main/Main/Up").Play("Up");
                speed = -0.15f;
                Walk_Mode(false);
            }
            if (is_Lock_Ice)
            {
                Walk_Mode(false);
                GetNode<AnimationPlayer>("Main/Main/Eating").Stop();
            }
            if (health < 500 && !has_lose_Arm)
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
        GetNode<AudioStreamPlayer>("Main/Main/Dancer").Stream.Set("loop", false);
        GetNode<AudioStreamPlayer>("Main/Main/Call").Stream.Set("loop", false);
        health_list.Clear();
        health_list.Add(new Health_Container(1437, false));
        speed_Normal = -8.5f;
        GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area").hurt = 150;//强大的啃咬伤害
        Hypno_Path = "res://scene/Plants/Zombies/JackSon_Zombies/Plants_JackSon_Zombies.tscn";
        base._Ready();
    }
    protected override void Walk_Mode(bool is_Walking)
    {
        if (is_Walking)
        {
            if (!has_touch_plants)
            {
                GetNode<AnimationPlayer>("Main/Main/Walk1").Play("Walk");
            }
            else
            {
                GetNode<AnimationPlayer>("Main/Main/Walk2").Play("Walk");
            }
        }
        else
        {
            GetNode<AnimationPlayer>("Main/Main/Walk1").Stop();
            GetNode<AnimationPlayer>("Main/Main/Walk2").Stop();
        }
    }
    protected override void Spec_Doing()
    {
        //GetNode<AudioStreamPlayer>("Main/Main/Dancer").Play();
    }
    protected override bool Get_Walk_Mode()
    {
        return GetNode<AnimationPlayer>("Main/Main/Walk1").IsPlaying() || GetNode<AnimationPlayer>("Main/Main/Walk2").IsPlaying();
    }
    protected void Dance_Timer_timeout()
    {
        if (has_planted && has_touch_plants)
        {
            if (!GetNode<AnimationPlayer>("Main/Main/Up").IsPlaying())
            {
                GetNode<AnimationPlayer>("Main/Main/Up").Play("Up");
            }
        }
    }
}
