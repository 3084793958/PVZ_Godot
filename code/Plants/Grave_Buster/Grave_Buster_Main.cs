using Godot;
using System;

public class Grave_Buster_Main : Normal_Plants
{
    protected bool has_Make_Potato = false;
    public override void _PhysicsProcess(float delta)
    {
        if (!GetNode<Area2D>("Main/Shovel_Area").IsConnected("area_entered", this, nameof(Area_Entered)))
        {
            return;
        }
        base._PhysicsProcess(delta);
        if (has_planted)
        {
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
                }
            }
        }
    }
    public override void _Ready()
    {
        Not_H_Hurt = true;
        just_for_Grave_Buster = true;
        Use_Move_Area = false;
        base._Ready();
    }
    protected void Delete_Tomb()
    {
        if (Tomb_Area_2D != null)
        {
            Tomb_Area_2D.GetNode<Tomb_Main>("../..").Free_Self();
        }
        else if (Dock_Area_2D != null)
        {
            for (int i = 0; i < Dock_Area_2D.Normal_Plant_List.Count; i++)
            {
                if (Dock_Area_2D.Normal_Plant_List[i] is Tomb_Main Tomb_Object)
                {
                    Tomb_Object.Free_Self();
                    break;
                }
            }
        }
    }
    protected void Make_Potato()
    {
        //NOWAY:In_Game_Main.Plants_Clone_Request("res://scene/Plants/Potato/Potato.tscn", this.GlobalPosition, ZIndex - normal_ZIndex + 3);
        if (!has_Make_Potato)
        {
            has_Make_Potato = true;
            var scene = GD.Load<PackedScene>("res://scene/Plants/Potato/Potato.tscn");
            var plant_child = (Potato_Main)scene.Instance();
            plant_child.ZIndex = ZIndex - normal_ZIndex + 3;
            plant_child.put_position = this.GlobalPosition;
            plant_child.player_put = false;
            plant_child.Just_Up();
            GetNode<Control>("/root/In_Game/Object").AddChild(plant_child);
        }
    }
    protected override void Plants_Add_List()
    {
        if (Dock_Area_2D == null)
        {
            return;
        }
        Dock_Area_2D.Top_Plant_List.Add(this);
    }
    protected override void Plants_Remove_List()
    {
        if (Dock_Area_2D == null)
        {
            return;
        }
        Dock_Area_2D.Top_Plant_List.Remove(this);
    }
    protected override void Plants_Init()
    {
        GetNode<AnimationPlayer>("Main/Player").Play("Player");
        GetNode<Normal_Plants_Area>("Main/Shovel_Area").has_planted = this.has_planted;
    }
    protected override bool Allow_Plants()
    {
        return (In_Game_Main.Sun_Number >= this_sun && Dock_Area_2D.Top_Plant_List.Count == 0 || Public_Main.debuging) && on_lock_grid;
    }
    public void Bug_Doing()
    {
        //NOTHING
    }
}
