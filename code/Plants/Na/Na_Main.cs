using Godot;
using System;
using System.Collections.Generic;

public class Na_Main : Normal_Plants
{
    // Na+O2->Na2O2
    // Na+O2->Na2O
    // Na+H+->NaOH+H2 PH:!
    //
    //Died:Boom
    //H+:H2
    //
    //lock_protect
    private readonly object _listLock = new object();
    //lock
    [Export] public bool has_boom = false;
    public List<C2H5OH_Bullets_Fire_Area> C2H5OH_Fire_Area_2D_List = new List<C2H5OH_Bullets_Fire_Area>();
    protected List<Water_Area> Water_Area_List = new List<Water_Area>();
    protected List<H2SO4_Area2D> H2SO4_Area_2D_List = new List<H2SO4_Area2D>();
    protected List<AQ_Area> AQ_Area_List = new List<AQ_Area>();
    public bool has_fire = false;
    public bool H2_can_boom = false;
    public bool has_Make_H2 = false;
    protected bool H2_will_boom = false;
    [Export] protected bool Na_on_Shovel = false;
    public float N_OH = 0.1f;
    protected bool has_change_N_H = false;
    protected bool Will_Change_pH = false;
    public void Make_H2()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                var scene = GD.Load<PackedScene>("res://scene/Plants/H2/H2.tscn");
                var H2_child = (H2_Main)scene.Instance();
                H2_child.ZIndex = this.ZIndex - this.normal_ZIndex + 5;
                H2_child.put_position = this.GlobalPosition + new Vector2(-80 + i * 80, -80 + j * 80);
                H2_child.player_put = false;
                H2_child.will_boom = H2_will_boom;
                GetNode<Control>("/root/In_Game/Object").AddChild(H2_child);
            }
        }
        Free_Self();
    }
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
            if (GetNode<Normal_Plants_Area>("Main/Touch_Area").lose_health)
            {
                GetNode<Normal_Plants_Area>("Main/Touch_Area").lose_health = false;
                health -= GetNode<Normal_Plants_Area>("Main/Touch_Area").lose_health_number;
                GetNode<AnimationPlayer>("Is_Eated").Play("Is_Eated");
            }
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
            bool can_fire = false;
            if (C2H5OH_Fire_Area_2D_List.Count != 0)
            {
                for (int i = 0; i < C2H5OH_Fire_Area_2D_List.Count; i++)
                {
                    if (C2H5OH_Fire_Area_2D_List[i].can_work)
                    {
                        can_fire = true;
                        break;
                    }
                }
            }
            if (can_fire && !GetNode<AnimationPlayer>("Main/Player").IsPlaying() && !has_fire)
            {
                has_fire = true;
                GetNode<AnimationPlayer>("Main/Player").Play("Player1");
            }
            if (!can_fire)
            {
                if (H2SO4_Area_2D_List.Count != 0)
                {
                    for (int i = 0; i < H2SO4_Area_2D_List.Count; i++)
                    {
                        if (H2SO4_Area_2D_List[i].has_planted && !has_Make_H2)  
                        {
                            has_Make_H2 = true;
                            H2_will_boom = true;
                            Make_H2();
                            Will_Change_pH = true;
                            break;
                        }
                    }
                }
                if (AQ_Area_List.Count != 0)
                {
                    for (int i = 0; i < AQ_Area_List.Count; i++)
                    {
                        if (AQ_Area_List[i].has_planted && !has_Make_H2) 
                        {
                            has_Make_H2 = true;
                            H2_will_boom = false;
                            Make_H2();
                            Will_Change_pH = true;
                            break;
                        }
                    }
                }
                if (Dock_Area_2D != null)
                {
                    if (Dock_Area_2D.pH <= 3.5f)
                    {
                        if (!has_Make_H2)
                        {
                            has_Make_H2 = true;
                            Make_H2();
                            Will_Change_pH = true;
                        }
                    }
                }
                if (Water_Area_List.Count != 0)
                {
                    if (!has_Make_H2)
                    {
                        has_Make_H2 = true;
                        for (int i = 0; i < Water_Area_List.Count; i++)
                        {
                            if (Water_Area_List[i].pH <= 3.5f)
                            {
                                H2_will_boom = true;
                                break;
                            }
                        }
                        Make_H2();
                        Will_Change_pH = true;
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
        just_for_MG = true;
        Use_Move_Area = false;
        GetNode<Area2D>("Main/Touch_Area").PauseMode = PauseModeEnum.Process;
        GetNode<AudioStreamPlayer>("Burn").Stream.Set("loop", false);
        health = 230;
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
            if (Type_string != null && Type_string == "H2SO4")//H2O
            {
                H2SO4_Area_2D_List.Add((H2SO4_Area2D)area2D);
            }
            if (Type_string != null && Type_string == "Water_Area")//H2O
            {
                Water_Area_List.Add((Water_Area)area2D);
            }
            if (Type_string != null && Type_string == "AQ")//H2O
            {
                AQ_Area_List.Add((AQ_Area)area2D);
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
            if (Type_string != null && Type_string == "H2SO4")//H2O
            {
                H2SO4_Area_2D_List.Remove((H2SO4_Area2D)area2D);
            }
            if (Type_string != null && Type_string == "Water_Area")//H2O
            {
                Water_Area_List.Remove((Water_Area)area2D);
            }
            if (Type_string != null && Type_string == "AQ")//H2O
            {
                AQ_Area_List.Remove((AQ_Area)area2D);
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

        GetNode<Normal_Plants_Area>("Main/Shovel_Area").has_planted = this.has_planted;
        GetNode<Normal_Plants_Area>("Main/Touch_Area").has_planted = this.has_planted;
    }
    protected override bool Allow_Plants()
    {
        return ((In_Game_Main.Sun_Number >= this_sun && Dock_Area_2D.Top_Plant_List.Count == 0) || Public_Main.debuging) && on_lock_grid;
    }
    public override void Free_Self()
    {
        if ((Dock_Area_2D != null && Dock_Area_2D.pH > 10f) || (Water_Area_List.Count != 0 && Water_Area_List[0].pH > 10f))
        {
            if (!GetNode<AnimationPlayer>("Main/Boom").IsPlaying())
            {
                has_boom = true;
            }
            if (!has_change_N_H)
            {
                has_change_N_H = true;
                for (int i = 0; i < Water_Area_List.Count; i++)
                {
                    Water_Area_List[0].Change_pH(0f, N_OH, -N_OH);
                }
            }
        }
        if (!Na_on_Shovel && !has_boom) //Na位于空中,铲子无触碰
        {
            if (!has_fire)
            {
                GetNode<All_Boom_Area>("Main/Boom1").hurt = 750;
                GetNode<All_Boom_Area>("Main/Boom1").can_do = true;
                GetNode<All_Boom_Area>("Main/Boom1").Start_hurting();
            }
            else
            {
                GetNode<All_Boom_Area>("Main/Boom2").hurt = 1500;
                GetNode<All_Boom_Area>("Main/Boom2").can_do = true;
                GetNode<All_Boom_Area>("Main/Boom2").Start_hurting();
            }
            if (!GetNode<AnimationPlayer>("Main/Boom").IsPlaying())
            {
                GetNode<AnimationPlayer>("Main/Boom").Play("Died");
            }
            if (!has_change_N_H&&Will_Change_pH)
            {
                has_change_N_H = true;
                if (Dock_Area_2D != null)
                {
                    if (Dock_Area_2D.type != 2) 
                    {
                        Dock_Area_2D.Change_pH(0f, N_OH, -N_OH);
                    }
                }
                for (int i = 0; i < Water_Area_List.Count; i++)
                {
                    Water_Area_List[0].Change_pH(0f, N_OH, -N_OH);
                }
            }
        }
        Plants_Remove_List();
        if (!has_boom && !Na_on_Shovel) 
        {
            return;
        }
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
