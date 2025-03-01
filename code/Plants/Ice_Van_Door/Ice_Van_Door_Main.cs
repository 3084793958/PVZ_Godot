using Godot;
using System;

public class Ice_Van_Door_Main : Normal_Plants
{
    [Export] public string Bullets_Path = null;
    public override void _Process(float delta)
    {
        base._Process(delta);
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
        GetNode<AudioStreamPlayer>("Main/Throw").Stream.Set("loop", false);
        base._Ready();
    }
    protected override void Area_Entered(Control_Area_2D area2D)
    {
        base.Area_Entered(area2D);
    }
    protected override void Area_Exited(Control_Area_2D area2D)
    {
        base.Area_Exited(area2D);
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
        Dock_Area_2D.Normal_Plant_List.Add(this);
    }
    protected override void Plants_Remove_List()
    {
        Dock_Area_2D.Normal_Plant_List.Remove(this);
    }
    protected override void Plants_Init()
    {
        GetNode<AnimationPlayer>("Main/Player1").Play("Player1");
        GetNode<AnimationPlayer>("Main/Player2").Play("Player1");
        GetNode<AnimationPlayer>("Main/Player3").Play("Player1");
        GetNode<Normal_Plants_Area>("Main/Shovel_Area").has_planted = this.has_planted;
    }
    protected override bool Allow_Plants()
    {
        return ((In_Game_Main.Sun_Number >= card_parent_Button.sun && Dock_Area_2D.Normal_Plant_List.Count == 0 && Dock_Area_2D.now_type[Dock_Area_2D.now_type.Count - 1] == 1) || Public_Main.debuging) && on_lock_grid;
    }
    public void Bullets_Way_On(Control_Area_2D area_2D)
    {
        string Type_string = area_2D?.Area2D_type;
        if (Type_string != null && Type_string == "Zombies")
        {
            Bullets_Zombies_Area_2D = (Normal_Zombies_Area)area_2D;
            Bullets_Zombies_Area_2D_List.Add(Bullets_Zombies_Area_2D);
        }
    }
    public void Bullets_Way_Off(Control_Area_2D area_2D)
    {
        string Type_string = area_2D?.Area2D_type;
        if (Type_string != null && Type_string == "Zombies")
        {
            Bullets_Zombies_Area_2D = (Normal_Zombies_Area)area_2D;
            Bullets_Zombies_Area_2D_List.Remove(Bullets_Zombies_Area_2D);
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
        for (int i = 0; i < 5; i++)
        {
            await ToSignal(GetTree(), "idle_frame");
            Clone_Bullets(0f);
            Clone_Bullets(1.5f);
            Clone_Bullets(-1.5f);
        }
    }
    public void Clone_Bullets(float speed_y = 0f)
    {
        if (Bullets_Path != null && health > 0)//health<=0 GetNode Error
        {
            if (true)
            {
                In_Game_Main.Plants_Bullets_Clone_Request(Bullets_Path, GetNode<Bullets_Way_Area>("Main/Bullets_Way").GlobalPosition, speed_y);
            }
        }
    }
}
