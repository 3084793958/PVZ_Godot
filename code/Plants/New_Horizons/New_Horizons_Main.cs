using Godot;
using System;
using System.Collections.Generic;
public class New_Horizons_Main : Normal_Plants
{
    //lock_protect
    private readonly object _listLock = new object();
    //lock
    [Export] public string Bullets_Path = null;
    protected List<Background_Grid_Main> Find_Area_List = new List<Background_Grid_Main>();
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
                    GetNode<AnimationPlayer>("Died").Play("Died");
                    Plants_Remove_List();
                }
            }
        }
    }
    public override void _Ready()
    {
        GD.Randomize();
        can_sleep = true;//(O^<)休眠机制
        health = 768;//位于冥王星时的传输速度
        GetNode<Area2D>("Main/Bullets_Way").PauseMode = PauseModeEnum.Process;
        GetNode<AudioStreamPlayer>("Main/Throw").Stream.Set("loop", false);
        base._Ready();
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
        if (can_sleep && In_Game_Main.allow_sun && !Public_Main.debuging)
        {
            sleep = true;
        }
        GetNode<AnimationPlayer>("Main/Player1").Play("Player1");
        if (!sleep)
        {
            GetNode<AnimationPlayer>("Main/Player2").Play("Player1");
            GetNode<Node2D>("Main/Sleep").Hide();
        }
        else
        {
            GetNode<AnimationPlayer>("Main/Player1").PlaybackSpeed = 0.5f;
            GetNode<Node2D>("Main/eyes/Eye1").Hide();
            GetNode<Node2D>("Main/eyes/Eye2").Show();
            GetNode<Node2D>("Main/Sleep").Show();
            GetNode<AnimationPlayer>("Main/Sleep/Sleep").Play("Sleep");
        }
        GetNode<Normal_Plants_Area>("Main/Shovel_Area").has_planted = this.has_planted;
    }
    protected override bool Allow_Plants()
    {
        return ((In_Game_Main.Sun_Number >= this_sun && Dock_Area_2D.Normal_Plant_List.Count == 0 && Dock_Area_2D.now_type[Dock_Area_2D.now_type.Count - 1] == 1) || Public_Main.debuging) && on_lock_grid;
    }
    public void Bullets_Way_On(Area2D area_node)
    {
        if (!(area_node is Control_Area_2D area_2D) || !IsInstanceValid(area_2D))
        {
            return;
        }
        lock (_listLock)
        {
            if (!IsInstanceValid(area_2D))
            {
                return;
            }
            string Type_string = area_2D?.Area2D_type;
            if (Type_string != null && Type_string == "Zombies")
            {
                Bullets_Zombies_Area_2D = (Normal_Zombies_Area)area_2D;
                Bullets_Zombies_Area_2D_List.Add(Bullets_Zombies_Area_2D);
            }
        }
    }
    public void Bullets_Way_Off(Area2D area_node)
    {
        if (!(area_node is Control_Area_2D area_2D) || !IsInstanceValid(area_2D))
        {
            return;
        }
        lock (_listLock)
        {
            if (!IsInstanceValid(area_2D))
            {
                return;
            }
            string Type_string = area_2D?.Area2D_type;
            if (Type_string != null && Type_string == "Zombies")
            {
                Bullets_Zombies_Area_2D = (Normal_Zombies_Area)area_2D;
                Bullets_Zombies_Area_2D_List.Remove(Bullets_Zombies_Area_2D);
            }
        }
    }
    public void Bullets_Shot()
    {
        if (Bullets_Zombies_Area_2D_List.Count != 0 && has_planted && !sleep)
        {
            bool can_shoot = false;
            for (int i = 0; i < Bullets_Zombies_Area_2D_List.Count; i++)
            {
                if (Bullets_Zombies_Area_2D_List[i].has_plant && Bullets_Zombies_Area_2D_List[i].Monitorable)
                {
                    can_shoot = true;
                    break;
                }
            }
            if (can_shoot)
            {
                GetNode<AnimationPlayer>("Main/Shoot").Play("Shoot");
            }
        }
    }
    public void Bug_Doing()
    {
        if (sleep)
        {
            return;
        }
        for (int i = 0; i < 10; i++)
        {
            Clone_Bullets(-30f, 1);
            Clone_Bullets(-15f, 1);
            Clone_Bullets(-7f, 1);
            Clone_Bullets(0f, 1);
            Clone_Bullets(7f, 1);
            Clone_Bullets(15f, 1);
            Clone_Bullets(30f, 1);
        }
    }
    public void Clone_Bullets(float sita = 0, int _y_type = 1)
    {
        if (Bullets_Path != null && health > 0)//health<=0 GetNode Error
        {
            In_Game_Main.Plants_Bullets_Clone_Request(Bullets_Path, GetNode<Bullets_Way_Area>("Main/Bullets_Way").GlobalPosition, sita, _y_type);
        }
    }
    public override void Free_Self()
    {
        if (GetNode<Area2D>("Main/Bullets_Way").IsConnected("area_entered", this, nameof(Bullets_Way_On)))
        {
            GetNode<Area2D>("Main/Bullets_Way").Disconnect("area_entered", this, nameof(Bullets_Way_On));
            GetNode<Area2D>("Main/Bullets_Way").Disconnect("area_exited", this, nameof(Bullets_Way_Off));
            GetNode<Area2D>("Main/Bullets_Way").Monitoring = false;
            GetNode<Area2D>("Main/Bullets_Way").Monitorable = false;
        }
        base.Free_Self();
    }
    public void Casing_Timer_timeout()
    {
        if (!has_planted)
        {
            return;
        }
        if (sleep)
        {
            return;
        }
        if (GetNode<Timer>("Casing_Timer").WaitTime < 20f)
        {
            GetNode<Timer>("Casing_Timer").WaitTime = 25f;
            GetNode<Timer>("Casing_Timer").Start();
        }
        bool self_Dock_Clone = true;
        if (Dock_Area_2D != null)
        { 
            if (Dock_Area_2D.on_PL_Casing_Save)
            {
                self_Dock_Clone = false;
            }
        }
        if (self_Dock_Clone)
        {
            In_Game_Main.Plants_Clone_Request("res://scene/Plants/PL_Casing/PL_Casing.tscn", this.GlobalPosition, ZIndex - normal_ZIndex + 4);
        }
        List<int> random_List = new List<int>();
        Find_Area_List.Remove(this.Dock_Area_2D);
        int PL_Casing_Number = 0;
        for (int i = 0; i < Find_Area_List.Count; i++)
        {
            if (Find_Area_List[i].on_New_Horizons)
            {
                continue;
            }
            if (Find_Area_List[i].on_PL_Casing_Save) 
            {
                PL_Casing_Number++;
                continue;
            }
            if (Find_Area_List[i].Normal_Plant_List.Count == 1 && (Find_Area_List[i].Normal_Plant_List[0] is Tomb_Main || Find_Area_List[i].Normal_Plant_List[0] is Plants_Tomb_Main))
            {
                continue;
            }
            if (Find_Area_List[i].Normal_Plant_List.Count + Find_Area_List[i].Down_Plant_List.Count != 0 && Find_Area_List[i].Casing_Plant_List.Count == 0)
            {
                random_List.Add(i);
            }
        }
        if (random_List.Count != 0 && PL_Casing_Number < 2) 
        {
            GD.Randomize();
            int res_number = random_List[(int)(GD.Randi() % random_List.Count)];
            In_Game_Main.Plants_Clone_Request("res://scene/Plants/PL_Casing/PL_Casing.tscn", Find_Area_List[res_number].GlobalPosition, ZIndex - normal_ZIndex + 4);
        }
    }
    protected void Find_Area_Entered(Area2D area_node)
    {
        if (!(area_node is Control_Area_2D area2D) || !IsInstanceValid(area2D))
        {
            return;
        }
        string Type_string = area2D?.Area2D_type;
        if (Type_string != null && Type_string == "Grid")
        {
            Find_Area_List.Add((Background_Grid_Main)area2D);
        }
    }
    protected void Find_Area_Exited(Area2D area_node)
    {
        if (!(area_node is Control_Area_2D area2D) || !IsInstanceValid(area2D))
        {
            return;
        }
        string Type_string = area2D?.Area2D_type;
        if (Type_string != null && Type_string == "Grid")
        {
            Find_Area_List.Remove((Background_Grid_Main)area2D);
        }
    }
}
