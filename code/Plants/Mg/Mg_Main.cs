using Godot;
using System;
using System.Collections.Generic;

public class Mg_Main : Normal_Plants
{
    public List<C2H5OH_Bullets_Fire_Area> C2H5OH_Fire_Area_2D_List = new List<C2H5OH_Bullets_Fire_Area>();
    public List<H2SO4_Area2D> H2SO4_Area_2D_List = new List<H2SO4_Area2D>();
    public void Shovel_Area_Entered(Control_Area_2D area2D)
    {
        string Type_string = area2D?.Area2D_type;
        if (has_planted && Type_string != null && Type_string == "Shovel")
        {
            Shovel_Area = (Shovel_Area2D)area2D;
        }
        if (has_planted && Type_string != null && Type_string == "Bug")
        {
            Bug_Area = (Bug_Area2D)area2D;
        }
    }
    public void Shovel_Area_Exited(Control_Area_2D area2D)
    {
        string Type_string = area2D?.Area2D_type;
        if (has_planted && Type_string != null && Type_string == "Shovel")
        {
            if (Shovel_Area != null)
            {
                this.Modulate = normal_color;
                Shovel_Area = null;
                on_Shovel = false;
            }
        }
        if (has_planted && Type_string != null && Type_string == "Bug")
        {
            if (Bug_Area != null)
            {
                this.Modulate = normal_color;
                Bug_Area = null;
                on_Bug = false;
            }
        }
    }
    public override void _Process(float delta)
    {
        base._Process(delta);
        if (has_planted)
        {
            if (Zombies_Area_2D_List.Count != 0)
            {
                for (int i = 0; i < Zombies_Area_2D_List.Count; i++)
                {
                    if (Zombies_Area_2D_List[i].Choose_Plants_Area == GetNode<Normal_Plants_Area>("Main/Touch_Area"))
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
                Dock_Area_2D.Top_Plant_List.Remove(this);
            }
        }
    }
    public override void _Ready()
    {
        GetNode<AudioStreamPlayer>("Burn").Stream.Set("loop", false);
        health = 100;
        base._Ready();
    }
    protected override void Area_Entered(Control_Area_2D area2D)
    {
        base.Area_Entered(area2D);
        string Type_string = area2D?.Area2D_type;
        if (Type_string != null && Type_string == "Bullets_Fire")
        {
            C2H5OH_Fire_Area_2D_List.Add((C2H5OH_Bullets_Fire_Area)area2D);
        }
        if (Type_string != null && Type_string == "H2SO4")
        {
            H2SO4_Area_2D_List.Add((H2SO4_Area2D)area2D);
        }
    }
    protected override void Area_Exited(Control_Area_2D area2D)
    {
        base.Area_Exited(area2D);
        string Type_string = area2D?.Area2D_type;
        if (Type_string != null && Type_string == "Bullets_Fire")
        {
            C2H5OH_Fire_Area_2D_List.Remove((C2H5OH_Bullets_Fire_Area)area2D);
        }
        if (Type_string != null && Type_string == "H2SO4")
        {
            H2SO4_Area_2D_List.Remove((H2SO4_Area2D)area2D);
        }
    }
    protected override void Dock_Entered(Control_Area_2D area2D)
    {
        base.Dock_Entered(area2D);
    }
    protected override void Dock_Exited(Control_Area_2D area2D)
    {
        base.Dock_Exited(area2D);
    }
    protected override void Plants_Add_List()
    {
        Dock_Area_2D.Top_Plant_List.Add(this);
    }
    protected override void Plants_Remove_List()
    {
        Dock_Area_2D.Top_Plant_List.Remove(this);
    }
    protected override void Plants_Init()
    {
        if (C2H5OH_Fire_Area_2D_List.Count != 0)
        {
            bool has_fire = false;
            for (int i = 0; i < C2H5OH_Fire_Area_2D_List.Count; i++)
            {
                if (C2H5OH_Fire_Area_2D_List[i].can_work)
                {
                    has_fire = true;
                    break;
                }
            }
            if (has_fire)
            {
                GetNode<AnimationPlayer>("Main/Player2").Play("Player1");
            }
            else
            {
                GetNode<AnimationPlayer>("Main/Player1").Play("Player1");
            }
        }
        else if (H2SO4_Area_2D_List.Count != 0)
        {
            for (int i = 0; i < H2SO4_Area_2D_List.Count; i++)
            {
                H2SO4_Area_2D_List[i].has_become = true;
            }
            GetNode<AnimationPlayer>("Main/Player3").Play("Player1");
        }
        else
        {
            GetNode<AnimationPlayer>("Main/Player1").Play("Player1");
        }
        GetNode<Normal_Plants_Area>("Main/Shovel_Area").has_planted = this.has_planted;
        GetNode<Normal_Plants_Area>("Main/Touch_Area").has_planted = this.has_planted;
    }
    protected override bool Allow_Plants()
    {
        return ((In_Game_Main.Sun_Number >= card_parent_Button.sun && Dock_Area_2D.Top_Plant_List.Count == 0) || Public_Main.debuging) && on_lock_grid;
    }
}
