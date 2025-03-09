using Godot;
using System;

public class Van_Door_Main : Normal_Plants
{
    //lock_protect
    private readonly object _listLock = new object();
    //lock
    [Export] public string Bullets_Path = null;
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
                    Dock_Area_2D.Normal_Plant_List.Remove(this);
                    GetNode<AnimationPlayer>("Died").Play("Died");
                }
            }
        }
    }
    public override void _Ready()
    {
        GetNode<Area2D>("Main/Bullets_Way").PauseMode = PauseModeEnum.Process;
        GetNode<AudioStreamPlayer>("Main/Throw").Stream.Set("loop", false);
        base._Ready();
    }
    protected override void Plants_Add_List()
    {
        Dock_Area_2D.Normal_Plant_List.Add(this);
    }
    protected override void Plants_Remove_List()
    {
        Dock_Area_2D.Normal_Plant_List.Remove(this);
    }
    protected override void Plants_Init()
    {
        GetNode<AnimationPlayer>("Main/Player1").Play("leaves");
        GetNode<AnimationPlayer>("Main/Player2").Play("eyes");
        GetNode<Normal_Plants_Area>("Main/Shovel_Area").has_planted = this.has_planted;
    }
    protected override bool Allow_Plants()
    {
        return ((In_Game_Main.Sun_Number >= card_parent_Button.sun && Dock_Area_2D.Normal_Plant_List.Count == 0 && Dock_Area_2D.now_type[Dock_Area_2D.now_type.Count - 1] == 1) || Public_Main.debuging) && on_lock_grid;
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
        if (Bullets_Zombies_Area_2D_List.Count != 0 && has_planted)
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
		for (int i=0;i<25;i++)
		{
			await ToSignal(GetTree(), "idle_frame");
			Clone_Bullets();
		}
	}
    public void Clone_Bullets()
    {
        if (Bullets_Path != null && health > 0)//health<=0 GetNode Error
        {
            if (true)
            {
                In_Game_Main.Plants_Bullets_Clone_Request(Bullets_Path, GetNode<Bullets_Way_Area>("Main/Bullets_Way").GlobalPosition);
            }
        }
    }
    protected override void Free_Self()
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
