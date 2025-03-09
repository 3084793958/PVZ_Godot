using Godot;
using System;

public class SunFlower_Main : Normal_Plants
{
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
        GetNode<Timer>("Timer").Stop();
        GetNode<AudioStreamPlayer>("Sun").Stream.Set("loop", false);
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
        GetNode<AnimationPlayer>("Main/Player3").Play("eyes");
        GetNode<AnimationPlayer>("Main/Player1").Play("Mouth");
        GetNode<AnimationPlayer>("Main/Player2").Play("Normal");
        GetNode<Normal_Plants_Area>("Main/Shovel_Area").has_planted = this.has_planted;
        GetNode<Timer>("Timer").WaitTime = 15f;
        GetNode<Timer>("Timer").Start();
    }
    protected override bool Allow_Plants()
    {
        return ((In_Game_Main.Sun_Number >= card_parent_Button.sun && Dock_Area_2D.Normal_Plant_List.Count == 0 && Dock_Area_2D.now_type[Dock_Area_2D.now_type.Count - 1] == 1) || Public_Main.debuging) && on_lock_grid;
    }
    public void Bug_Doing()
    {
        Sun_Flower_Up();
        Sun_Flower_Up();
        Sun_Flower_Up();
    }
    public void Sun_Flower_Up()
    {
        if (has_planted && health > 0)
        {
            GetNode<AnimationPlayer>("Make_Sun").Play("Make_Sun");
            if (In_Game_Main.allow_sun)
            {
                GetNode<Timer>("Timer").WaitTime = (float)GD.RandRange(10d, 30d);
            }
            else
            {
                GetNode<Timer>("Timer").WaitTime = (float)GD.RandRange(20d, 60d);
            }
            GetNode<AudioStreamPlayer>("Sun").Play();
            int sun_value;
            float size;
            float random_number = GD.Randf();
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
            In_Game_Main.Sun_Clone_Request(sun_value, this.GlobalPosition, size, true);
        }
    }
}
