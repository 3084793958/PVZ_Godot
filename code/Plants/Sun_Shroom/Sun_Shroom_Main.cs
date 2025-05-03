using Godot;
using System;

public class Sun_Shroom_Main : Normal_Plants
{
    //lock_protect
    private readonly object _listLock = new object();
    //lock
    [Export] public string Bullets_Path = null;
    protected int dock_small_id = 0;
    protected Vector2 base_pos = Vector2.Zero;
    [Export] public bool Growed = false;
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
                if (dock_small_id == 1)
                {
                    this.GlobalPosition = Dock_Area_2D.GlobalPosition + new Vector2(-15, 15);
                }
                else if (dock_small_id == 2)
                {
                    this.GlobalPosition = Dock_Area_2D.GlobalPosition + new Vector2(0, -15);
                }
                else if (dock_small_id == 3)
                {
                    this.GlobalPosition = Dock_Area_2D.GlobalPosition + new Vector2(15, 15);
                }
                else
                {
                    this.GlobalPosition = Dock_Area_2D.GlobalPosition;
                }
            }
            GetNode<Area2D>("Dock/Area2D").GlobalPosition = Dock_Area_2D.GlobalPosition;
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
        GetNode<Timer>("Timer").Stop();
        GetNode<Timer>("Main/Grow_Timer").Stop();
        GetNode<AudioStreamPlayer>("Sun").Stream.Set("loop", false);
        GD.Randomize();
        can_sleep = true;
        base._Ready();
    }
    protected override void Plants_Add_List()
    {
        if (Dock_Area_2D == null)
        {
            return;
        }
        Dock_Area_2D.Normal_Plant_List.Add(this);
        if (dock_small_id > 0)
        {
            Dock_Area_2D.Small_Plants_List[dock_small_id - 1] = this;
        }
    }
    protected override void Plants_Remove_List()
    {
        if (Dock_Area_2D == null)
        {
            return;
        }
        Dock_Area_2D.Normal_Plant_List.Remove(this);
        has_removed = true;
        if (dock_small_id > 0)
        {
            Dock_Area_2D.Small_Plants_List[dock_small_id - 1] = null;
        }
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
        GetNode<Timer>("Timer").WaitTime = 15f;
        GetNode<Timer>("Timer").Start();
        GetNode<Timer>("Main/Grow_Timer").Start();
    }
    protected override bool Allow_Plants()
    {
        base_pos = Dock_Area_2D.GlobalPosition;
        dock_small_id = Dock_Area_2D.Empty_Small_Pos() + 1;
        return (In_Game_Main.Sun_Number >= this_sun && ((Dock_Area_2D.Normal_Plant_List.Count == 0 || (dock_small_id > 0 && Dock_Area_2D.Is_All_Small_in_normal())) && Dock_Area_2D.now_type[Dock_Area_2D.now_type.Count - 1] == 1) || Public_Main.debuging) && on_lock_grid;
    }
    public void Bug_Doing()
    {
        if (sleep)
        {
            return;
        }
        Sun_Flower_Up();
        Sun_Flower_Up();
        Sun_Flower_Up();
    }
    public void Sun_Flower_Up()
    {
        if (has_planted && health > 0 && !sleep) 
        {
            GetNode<AnimationPlayer>("Make_Sun").Play("Make_Sun");
            if (In_Game_Main.allow_sun)
            {
                GetNode<Timer>("Timer").WaitTime = (float)GD.RandRange(25d, 60d);
            }
            else
            {
                GetNode<Timer>("Timer").WaitTime = (float)GD.RandRange(15d, 30d);
            }
            GetNode<AudioStreamPlayer>("Sun").Play();
            int sun_value;
            float size;
            float random_number = GD.Randf();
            if (Growed)
            {
                if (random_number < 0.3f)
                {
                    sun_value = 50;
                    size = 2.0f;
                }
                else if (random_number > 0.8f)
                {
                    sun_value = 75;
                    size = 3.0f;
                }
                else
                {
                    sun_value = 25;
                    size = 1.0f;
                }
            }
            else
            {
                if (random_number < 0.3f)
                {
                    sun_value = 25;
                    size = 1.0f;
                }
                else if (random_number > 0.8f)
                {
                    sun_value = 50;
                    size = 2.0f;
                }
                else
                {
                    sun_value = 15;
                    size = 0.75f;
                }
            }
            In_Game_Main.Sun_Clone_Request(sun_value, this.GlobalPosition, size, true);
        }
    }
    public void Grow_Timer_out()
    {
        if (!Growed && !GetNode<AnimationPlayer>("Main/Grow_Player").IsPlaying()) 
        {
            GetNode<AnimationPlayer>("Main/Grow_Player").Play("Grow");
        }
    }
}
