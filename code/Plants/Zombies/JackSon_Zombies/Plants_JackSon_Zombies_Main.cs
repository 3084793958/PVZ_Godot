using Godot;
using System;
using System.Collections.Generic;
public class Plants_JackSon_Zombies_Main : Normal_Plants_Zombies
{
    public List<Plants_Dancer_Zombies_Main> Child_Zombies_List = new List<Plants_Dancer_Zombies_Main>();
    public bool has_touch_plants = false;
    public int clone_time = 0;
    public void Call_Up()
    {
        /*if (has_lose_Head)
        {
            return;
        }*///故意不写的,加大压力
        GetNode<AudioStreamPlayer>("Main/Main/Call").Play();
        clone_time++;
        for (int i = 0; i < Child_Zombies_List.Count; i++)
        {
            Child_Zombies_List[i].Parent_Zombies = null;
        }
        Child_Zombies_List.Clear();
        for (int i = 0; i < 4; i++)
        {
            var scene = GD.Load<PackedScene>("res://scene/Plants/Zombies/Dancer_Zombies/Plants_Dancer_Zombies.tscn");
            var plant_child = (Plants_Dancer_Zombies_Main)scene.Instance();
            plant_child.ZIndex = this.ZIndex;
            if (i == 0)//never change
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
                if (GetNode<Control>("/root/In_Game/Object").GetChild(i) is Plants_Tomb_Main tomb_object)
                {
                    if (tomb_object.Clone_Time < 20)
                    {
                        tomb_object.Clone_Zombies();
                    }
                }
            }
        }
    }
    protected override void Self_Hypno_Process()
    {
        has_touch_plants = true;
        GetNode<AnimationPlayer>("Main/Main/Up").Play("Up");
        speed = 0.15f;
        Walk_Mode(false);
    }
    protected override void Re_Set_Process()
    {
        if (health < 500)
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
            dancing_stop = GetNode<AnimationPlayer>("Main/Main/Up").IsPlaying();
            dancer_should_stop = GetNode<AnimationPlayer>("Main/Main/Up").IsPlaying() || Zombies_Area_2D != null;
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
            dancing_stop = dancing_stop && Zombies_Area_2D == null;
            if (GetNode<AnimationPlayer>("Main/Main/Up").IsPlaying())
            {
                dancing_stop = true;
            }
            if (!has_touch_plants && Zombies_Area_2D != null)
            {
                has_touch_plants = true;
                GetNode<AnimationPlayer>("Main/Main/Up").Play("Up");
                speed = 0.15f;
                Walk_Mode(false);
            }
            if (health < 500 && !has_lose_Arm)
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
        GetNode<AudioStreamPlayer>("Main/Main/Dancer").Stream.Set("loop", false);
        GetNode<AudioStreamPlayer>("Main/Main/Call").Stream.Set("loop", false);
        health_list.Clear();
        health_list.Add(new Health_Container(1437, false));
        speed_Normal = 8.5f;
        GetNode<Normal_Plants_Zombies_Area>("Main/Main/Zombies_Area").hurt = 150;//强大的啃咬伤害
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
