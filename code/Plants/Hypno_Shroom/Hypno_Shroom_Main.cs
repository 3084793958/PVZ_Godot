using Godot;
using System;

public class Hypno_Shroom_Main : Normal_Plants
{
    public bool has_been_touched = false;
    protected Normal_Zombies_Area Touch_Zombies_Area = null;
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
                    if (Zombies_Area_2D_List[i].Choose_Plants_Area == GetNode<Normal_Plants_Area>("Main/Shovel_Area") && !has_been_touched) 
                    {
                        if (Zombies_Area_2D_List[i].is_eating_show && !Zombies_Area_2D_List[i].has_lose_head)
                        {
                            Zombies_Area_2D_List[i].is_eating_show = false;
                            has_been_touched = true;
                            health = -1437;
                            Touch_Zombies_Area = Zombies_Area_2D_List[i];
                            Hypno_Zombies();
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
        can_sleep = true;
        base._Ready();
    }
    protected void Hypno_Zombies()
    { 
        if (!has_been_touched)
        {
            return;
        }
        if (Touch_Zombies_Area == null)
        {
            return;
        }
        Normal_Zombies Touch_Zombies = Touch_Zombies_Area.GetNode<Normal_Zombies>("../../..");
        string RES_Zombies_Path = Touch_Zombies.Hypno_Path;
        Vector2 RES_Zombies_Pos = Touch_Zombies_Area.GlobalPosition + new Vector2(0, 10);
        Health_List RES_Zombies_Health = Touch_Zombies.health_list;
        string Spec_Info = Touch_Zombies.Spec_Info;
        In_Game_Main.Plants_Hypno_Clone_Request(RES_Zombies_Path, RES_Zombies_Pos, RES_Zombies_Health, Spec_Info);
        Touch_Zombies.Free_Self();
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
    public void Bug_Doing()
    {
        if (sleep)
        {
            return;
        }
    }
}
