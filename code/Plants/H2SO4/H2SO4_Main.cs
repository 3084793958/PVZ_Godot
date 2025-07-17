using Godot;
using System;
using System.Collections.Generic;

public class H2SO4_Main : Normal_Plants
{
    public bool on_Mg = false;
    [Export] public bool has_open = false;
    [Export] public bool has_Died = false;
    protected List<Water_Area> Water_Area_List = new List<Water_Area>();
    public float N_H = 0.1f;//PH=1
    protected bool has_change_N_H = false;
    public override void _PhysicsProcess(float delta)
    {
        if (!GetNode<Area2D>("Main/Shovel_Area").IsConnected("area_entered", this, nameof(Area_Entered)))
        {
            return;
        }
        base._PhysicsProcess(delta);
        if (has_planted)
        {
            GetNode<H2SO4_Area2D>("Main/H2SO4_Area").has_planted = has_planted;
            if (on_Mg && ((Input.IsActionPressed("Left_Mouse") && !Public_Main.for_Android) || (Public_Main.for_Android && Is_Double_Click)))
            {
                if (!GetNode<AnimationPlayer>("Open").IsPlaying() && !GetNode<H2SO4_Area2D>("Main/H2SO4_Area").has_become) 
                {
                    Is_Double_Click = false;
                    GetNode<H2SO4_Area2D>("Main/H2SO4_Area").has_become = true;
                    has_open = true;
                    GetNode<AnimationPlayer>("Open").Play("Open");
                    In_Game_Main.Plants_Clone_Request("res://scene/Plants/H2/H2.tscn", this.GlobalPosition, this.ZIndex - this.normal_ZIndex + 5);
                }
            }
            if (!GetNode<AnimationPlayer>("Open").IsPlaying() && !has_open && GetNode<H2SO4_Area2D>("Main/H2SO4_Area").has_become) 
            {
                has_open = true;
                GetNode<AnimationPlayer>("Open").Play("Open");
                In_Game_Main.Plants_Clone_Request("res://scene/Plants/H2/H2.tscn", this.GlobalPosition, this.ZIndex - this.normal_ZIndex + 5);
            }
            if (Zombies_Area_2D_List.Count != 0)
            {
                for (int i = 0; i < Zombies_Area_2D_List.Count; i++)
                {
                    if (Zombies_Area_2D_List[i].Choose_Plants_Area == GetNode<Normal_Plants_Area>("Main/Shovel_Area"))
                    {
                        if (Zombies_Area_2D_List[i].is_eating_show && !Zombies_Area_2D_List[i].has_lose_head)
                        {
                            Zombies_Area_2D_List[i].is_eating_show = false;
                            health -= Zombies_Area_2D_List[i].hurt;
                            GetNode<AnimationPlayer>("Is_Eated").Play("Is_Eated");
                        }
                    }
                }
            }
            if (health <= 0)
            {
                if (!GetNode<AnimationPlayer>("Died").IsPlaying())
                {
                    Plants_Remove_List();
                    GetNode<AnimationPlayer>("Died").Play("Died");
                    GetNode<H2SO4_Area2D>("Main/H2SO4_Area").has_died = true;
                }
            }
        }
    }
    public override void _Ready()
    {
        Not_H_Hurt = true;
        health = 100;
        base._Ready();
    }
    protected override void Area_Entered(Area2D area_node)
    {
        if (!(area_node is Control_Area_2D area2D) || !IsInstanceValid(area2D))
        {
            return;
        }
        base.Area_Entered(area2D);
        string Type_string = area2D?.Area2D_type;
        if (Type_string != null && Type_string == "Water_Area")//H2O
        {
            Water_Area_List.Add((Water_Area)area2D);
        }
        if (!(area_node is Mg_Shining_Area area_2D) || !IsInstanceValid(area_2D))
        {
            return;
        }
        if (!area_2D.start)//TODO
        {
            on_Mg = true;
        }
    }
    protected override void Area_Exited(Area2D area_node)
    {
        if (!(area_node is Control_Area_2D area2D) || !IsInstanceValid(area2D))
        {
            return;
        }
        base.Area_Exited(area2D);
        string Type_string = area2D?.Area2D_type;
        if (Type_string != null && Type_string == "Mg_Shining")//TODO
        {
            on_Mg = false;
        }
        if (Type_string != null && Type_string == "Water_Area")//H2O
        {
            if (health > 0)
            {
                Water_Area_List.Remove((Water_Area)area2D);
            }
        }
    }
    protected override void Plants_Add_List()
    {
        if (Dock_Area_2D == null)
        {
            return;
        }
        Dock_Area_2D.Normal_Plant_List.Add(this);
    }
    protected override void Plants_Remove_List()
    {
        if (Dock_Area_2D == null)
        {
            return;
        }
        Dock_Area_2D.Normal_Plant_List.Remove(this);
    }
    protected override void Plants_Init()
    {
        GetNode<Normal_Plants_Area>("Main/Shovel_Area").has_planted = this.has_planted;
    }
    protected override bool Allow_Plants()
    {
        return ((In_Game_Main.Sun_Number >= this_sun && Dock_Area_2D.Normal_Plant_List.Count == 0 && Dock_Area_2D.now_type[Dock_Area_2D.now_type.Count - 1] == 1) || Public_Main.debuging) && on_lock_grid;
    }
    public void Bug_Doing()
    {
        //none
    }
    public void Click_Pressed()
    {
        if (In_Game_Main.is_playing)
        {
            if (!Normal_Plants.Choosing && GetNode<H2SO4_Area2D>("Main/H2SO4_Area").has_become)
            {
                GetNode<AudioStreamPlayer>("Bug").Play();
                var scene = GD.Load<PackedScene>("res://scene/Bug/Bug.tscn");
                var plant_child = (Bug_Main)scene.Instance();
                plant_child.by_H2SO4 = true;
                Normal_Plants.Choosing = true;
                GetNode<Control>("/root/In_Game/Object").AddChild(plant_child);
                Hide();
                Plants_Remove_List();
                GetNode<AnimationPlayer>("Free_player").Play("Free");
            }
        }
    }
    public override void Free_Self()
    {
        if (!on_Shovel && !has_open) 
        {
            if (!has_change_N_H)
            {
                has_change_N_H = true;
                if (Dock_Area_2D != null)
                {
                    if (Dock_Area_2D.type != 2)
                    {
                        Dock_Area_2D.Change_pH(N_H, 0f, 55.6f);
                    }
                }
                for (int i = 0; i < Water_Area_List.Count; i++)
                {
                    Water_Area_List[i].Change_pH(N_H, 0f, 55.6f);
                }
            }
        }
        if (!has_Died && !has_open && !on_Shovel) 
        {
            if (!GetNode<AnimationPlayer>("Died").IsPlaying())
            {
                Plants_Remove_List();
                GetNode<AnimationPlayer>("Died").Play("Died");
                GetNode<H2SO4_Area2D>("Main/H2SO4_Area").has_died = true;
            }
        }
        else
        {
            base.Free_Self();
        }
    }
}
