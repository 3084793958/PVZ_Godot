using Godot;
using System;
using System.Collections.Generic;

public class Mg_Main : Normal_Plants
{
    //lock_protect
    private readonly object _listLock = new object();
    //lock
    public List<C2H5OH_Bullets_Fire_Area> C2H5OH_Fire_Area_2D_List = new List<C2H5OH_Bullets_Fire_Area>();
    public List<H2SO4_Area2D> H2SO4_Area_2D_List = new List<H2SO4_Area2D>();
    public void Shovel_Area_Entered(Area2D area_node)
    {
        if (!(area_node is Control_Area_2D area2D) || !IsInstanceValid(area2D))
        {
            return;
        }
        lock (_listLock)
        {
            if (!IsInstanceValid(area2D))
            {
                return;
            }
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
    }
    public void Shovel_Area_Exited(Area2D area_node)
    {
        if (!(area_node is Control_Area_2D area2D) || !IsInstanceValid(area2D))
        {
            return;
        }
        lock (_listLock)
        {
            if (!IsInstanceValid(area2D))
            {
                return;
            }
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
    }
    public override void _PhysicsProcess(float delta)
    {
        if (!GetNode<Area2D>("Main/Touch_Area").IsConnected("area_entered", this, nameof(Area_Entered)))
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
                Plants_Remove_List();
            }
        }
    }
    public override void _Ready()
    {
        just_for_MG = true;
        Use_Move_Area = false;
        GetNode<Area2D>("Main/Touch_Area").PauseMode = PauseModeEnum.Process;
        GetNode<AudioStreamPlayer>("Burn").Stream.Set("loop", false);
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
        lock (_listLock)
        {
            if (!IsInstanceValid(area2D))
            {
                return;
            }
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
    }
    protected override void Area_Exited(Area2D area_node)
    {
        if (!(area_node is Control_Area_2D area2D) || !IsInstanceValid(area2D))
        {
            return;
        }
        base.Area_Exited(area2D);
        lock (_listLock)
        {
            if (!IsInstanceValid(area2D))
            {
                return;
            }
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
        GetNode<Normal_Plants_Area>("Main/Shovel_Area").is_MG = true;
        GetNode<Normal_Plants_Area>("Main/Touch_Area").has_planted = this.has_planted;
    }
    protected override bool Allow_Plants()
    {
        return ((In_Game_Main.Sun_Number >= this_sun && Dock_Area_2D.Top_Plant_List.Count == 0) || Public_Main.debuging) && on_lock_grid;
    }
    public override void Free_Self()
    {
        if (GetNode<Area2D>("Main/Touch_Area").IsConnected("area_entered", this, nameof(Shovel_Area_Entered)))
        {
            GetNode<Area2D>("Main/Touch_Area").Disconnect("area_entered", this, nameof(Shovel_Area_Entered));
            GetNode<Area2D>("Main/Touch_Area").Disconnect("area_exited", this, nameof(Shovel_Area_Exited));
            GetNode<Area2D>("Main/Touch_Area").Monitoring = false;
            GetNode<Area2D>("Main/Touch_Area").Monitorable = false;
            GetNode<Area2D>("Main/Shining_Area").Monitoring = false;
            GetNode<Area2D>("Main/Shining_Area").Monitorable = false;
        }
        base.Free_Self();
    }
}
