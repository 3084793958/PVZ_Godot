using Godot;
using System;

public class Small_Shroom_Main : Normal_Plants
{
    //lock_protect
    private readonly object _listLock = new object();
    //lock
    [Export] public string Bullets_Path = null;
    protected int dock_small_id = 0;
    protected Vector2 base_pos = Vector2.Zero;
    private bool has_removed = false;
    public override void _PhysicsProcess(float delta)
    {
        if (!GetNode<Area2D>("Main/Shovel_Area").IsConnected("area_entered", this, nameof(Area_Entered)))
        {
            return;
        }
        base._PhysicsProcess(delta);
        if (has_planted)
        {
            if (Dock_Area_2D != null && !has_removed) 
            {
                if (Dock_Area_2D.small_number < dock_small_id)
                {
                    dock_small_id--;
                }
                if (dock_small_id == 1)
                {
                    this.GlobalPosition = Dock_Area_2D.GlobalPosition + new Vector2(-15, 15);
                }
                else if (dock_small_id == 2)
                {
                    this.GlobalPosition = Dock_Area_2D.GlobalPosition + new Vector2(0, -15);
                }
                else
                {
                    this.GlobalPosition = Dock_Area_2D.GlobalPosition + new Vector2(15, 15);
                }
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
                    GetNode<AnimationPlayer>("Died").Play("Died");
                    Dock_Area_2D.Normal_Plant_List.Remove(this);
                    Dock_Area_2D.small_number--;//!
                }
            }
        }
    }
    protected override void Area_Entered(Area2D area_node)
    {
        try
        {
            if (!(area_node is Control_Area_2D area2D) || !IsInstanceValid(area2D))
            {
                return;
            }
            string Type_string = area2D?.Area2D_type;
            if (Type_string != null && Type_string == "Plants" && area2D?.Sec_Info != "Zombies")
            {
                return;
            }
            lock (_listLock)
            {
                if (!IsInstanceValid(area2D))
                {
                    return;
                }
                if (!just_for_MG)
                {
                    if (has_planted && Type_string != null && Type_string == "Shovel"&& Dock_Area_2D.small_number == dock_small_id)
                    {
                        Shovel_Area = (Shovel_Area2D)area2D;
                    }
                    else if (has_planted && Type_string != null && Type_string == "Bug")
                    {
                        Bug_Area = (Bug_Area2D)area2D;
                    }
                }
                if (Type_string != null && Type_string == "Zombies")
                {
                    Zombies_Area_2D = (Normal_Zombies_Area)area2D;
                    if (Zombies_Area_2D != null)
                    {
                        if (Zombies_Area_2D.can_hurt)//屎山造成
                        {
                            Zombies_Area_2D_List.Add(Zombies_Area_2D);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            GD.Print(ex.Message);
        }
    }
    public override void _Ready()
    {
        GD.Randomize();
        can_sleep = true;
        GetNode<Area2D>("Main/Bullets_Way").PauseMode = PauseModeEnum.Process;
        GetNode<AudioStreamPlayer>("Main/Throw").Stream.Set("loop", false);
        base._Ready();
    }
    protected override void Plants_Add_List()
    {
        Dock_Area_2D.Normal_Plant_List.Add(this);
        Dock_Area_2D.small_number++;
    }
    protected override void Plants_Remove_List()
    {
        Dock_Area_2D.Normal_Plant_List.Remove(this);
        has_removed = true;
        Dock_Area_2D.small_number--;
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
            GetNode<Sprite>("Main/eyes/Eye1").Hide();
            GetNode<Sprite>("Main/eyes/Eye2").Show();
            GetNode<Node2D>("Main/Sleep").Show();
            GetNode<AnimationPlayer>("Main/Sleep/Sleep").Play("Sleep");
        }
        GetNode<Normal_Plants_Area>("Main/Shovel_Area").has_planted = this.has_planted;
    }
    protected override bool Allow_Plants()
    {
        dock_small_id = Dock_Area_2D.small_number + 1;
        base_pos = Dock_Area_2D.GlobalPosition;
        if (dock_small_id > 3)
        {
            dock_small_id = 3;
        }
        else if (dock_small_id < 1)
        {
            dock_small_id = 1;
        }
        return ((In_Game_Main.Sun_Number >= this_sun && (Dock_Area_2D.Normal_Plant_List.Count == 0 || (Dock_Area_2D.small_number < 3 && Dock_Area_2D.Is_All_Small_in_normal())) && Dock_Area_2D.now_type[Dock_Area_2D.now_type.Count - 1] == 1) || Public_Main.debuging) && on_lock_grid;
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
    public async void Bug_Doing()
    {
        if (sleep)
        {
            return;
        }
        for (int i = 0; i < 25; i++)
        {
            await ToSignal(GetTree().CreateTimer(0.15f), "timeout");
            Clone_Bullets((float)(GD.RandRange(-60d, 60d)));//sin(x),cos(x)
        }
    }
    public void Clone_Bullets(float sita_y = 0f)
    {
        if (Bullets_Path != null && health > 0)//health<=0 GetNode Error
        {
            if (true)
            {
                In_Game_Main.Plants_Bullets_Clone_Request(Bullets_Path, GetNode<Bullets_Way_Area>("Main/Bullets_Way").GlobalPosition,sita_y);
            }
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
}
