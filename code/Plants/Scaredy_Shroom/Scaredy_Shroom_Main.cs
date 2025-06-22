using Godot;
using System;
using System.Collections.Generic;
public class Scaredy_Shroom_Main : Normal_Plants
{
    //lock_protect
    private readonly object _listLock = new object();
    //lock
    [Export] public string Bullets_Path = null;
    [Export] public bool Scared_doing = false;
    public bool can_scared = false;
    protected List<Normal_Zombies_Area> Scaredy_Area_2D_List = new List<Normal_Zombies_Area>();
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
            can_scared = false;
            if (Scaredy_Area_2D_List.Count != 0)
            {
                for (int i = 0; i < Scaredy_Area_2D_List.Count; i++)
                {
                    if (Scaredy_Area_2D_List[i].has_plant && !Scaredy_Area_2D_List[i].has_lose_head)
                    {
                        can_scared = true;
                        break;
                    }
                }
            }
            if (on_Shovel)
            {
                can_scared = true;
            }
            if (Math.Abs((this.GlobalPosition - GetTree().Root.GetMousePosition()).x) < 20 && Math.Abs((this.GlobalPosition - GetTree().Root.GetMousePosition()).y) < 20 && !on_Bug && Input.IsActionJustPressed("Left_Mouse")) 
            {
                can_scared = true;
            }
            if (can_scared && !GetNode<Timer>("Scared_Timer").Paused) 
            {
                GetNode<Timer>("Scared_Timer").Start();
                GetNode<Timer>("Scared_Timer").Paused = true;
            }
            if (!can_scared && Scared_doing && GetNode<Timer>("Scared_Timer").Paused)
            {
                GetNode<Timer>("Scared_Timer").Paused = false;
                GetNode<Timer>("Scared_Timer").Start();
            }
            if (can_scared && !sleep && !Scared_doing && !GetNode<AnimationPlayer>("Main/Scared").IsPlaying()) 
            {
                GetNode<AnimationPlayer>("Main/Scared").Play("Down");
            }
            if ((sleep || !can_scared) && Scared_doing && !GetNode<AnimationPlayer>("Main/Scared").IsPlaying() && GetNode<Timer>("Scared_Timer").IsStopped()) 
            {
                GetNode<AnimationPlayer>("Main/Scared").Play("Up");
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
        health = 750;
        GD.Randomize();
        can_sleep = true;
        GetNode<Area2D>("Main/Bullets_Way").PauseMode = PauseModeEnum.Process;
        GetNode<AudioStreamPlayer>("Main/Throw").Stream.Set("loop", false);
        Scared_doing = false;
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
        GetNode<AnimationPlayer>("Main/Player1").Play("Player");
        if (!sleep)
        {
            GetNode<AnimationPlayer>("Main/Player2").Play("Player");
            GetNode<Node2D>("Main/Sleep").Hide();
        }
        else
        {
            GetNode<Node2D>("Main/eyes/Eyes/Eye1").Hide();
            GetNode<Node2D>("Main/eyes/Eyes/Eye2").Show();
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
        if (Bullets_Zombies_Area_2D_List.Count != 0 && has_planted && !sleep && !Scared_doing) 
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
    public async void Bug_Doing()
    {
        if (sleep || Scared_doing) 
        {
            return;
        }
        for (int i = 0; i < 10; i++)
        {
            await ToSignal(GetTree(), "idle_frame");
            Clone_Bullets(-30f, 1);
            Clone_Bullets(-20f, 1);
            Clone_Bullets(-10f, 1);
            Clone_Bullets(0f, 1);
            Clone_Bullets(10f, 1);
            Clone_Bullets(20f, 1);
            Clone_Bullets(30f, 1);
        }
    }
    public void Clone_Bullets(float sita_y = 0f, int _y_type = 1)
    {
        if (Bullets_Path != null && health > 0)//health<=0 GetNode Error
        {
            In_Game_Main.Plants_Bullets_Clone_Request(Bullets_Path, GetNode<Bullets_Way_Area>("Main/Bullets_Way").GlobalPosition, sita_y, _y_type);
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
    protected void Sared_Area_entered(Area2D area_node)
    {
        if (!(area_node is Control_Area_2D area2D) || !IsInstanceValid(area2D))
        {
            return;
        }
        string Type_string = area2D?.Area2D_type;
        if (Type_string != null && Type_string == "Zombies")
        {
            Scaredy_Area_2D_List.Add((Normal_Zombies_Area)area2D);
        }
    }
    protected void Sared_Area_exited(Area2D area_node)
    {
        if (!(area_node is Control_Area_2D area2D) || !IsInstanceValid(area2D))
        {
            return;
        }
        string Type_string = area2D?.Area2D_type;
        if (Type_string != null && Type_string == "Zombies")
        {
            Scaredy_Area_2D_List.Remove((Normal_Zombies_Area)area2D);
        }
    }
}
