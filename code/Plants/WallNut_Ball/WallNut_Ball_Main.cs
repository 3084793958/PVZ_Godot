using Godot;
using System;

public class WallNut_Ball_Main : Normal_Plants
{
    public float speed_x = 0, speed_y = 0;
    public bool first_Speed_Y = true;
    public int touch_ZIndex = 0;
    public override void _Process(float delta)
    {
        base._Process(delta);
        if (has_planted)
        {
            if (Position.x > 1437)
            {
                this.QueueFree();
            }
            else
            {
                this.Position += new Vector2(speed_x, speed_y);
            }
            if (Zombies_Area_2D_List.Count != 0)
            {
                bool can_touch = false;
                for (int i = 0; i < Zombies_Area_2D_List.Count; i++)
                {
                    if (Zombies_Area_2D_List[i] != null)
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
                        if (!can_touch && Zombies_Area_2D_List[i] != null)
                        {
                            if (!Zombies_Area_2D_List[i].has_lose_head)
                            {
                                can_touch = true;
                                GetNode<Crash_Area_2D>("Main/Crash_Area").Crash_Area = Zombies_Area_2D_List[i];
                                if (touch_ZIndex != Zombies_Area_2D_List[i].GetParent().GetParent().GetParent<Node2D>().ZIndex)
                                {
                                    touch_ZIndex = Zombies_Area_2D_List[i].GetParent().GetParent().GetParent<Node2D>().ZIndex;
                                    if (first_Speed_Y)
                                    {
                                        first_Speed_Y = false;
                                        if (this.Position.y < 124)
                                        {
                                            speed_y = 3.5f;
                                        }
                                        else if (this.Position.y > 558)
                                        {
                                            speed_y = -3.5f;
                                        }
                                        else
                                        {
                                            speed_y = ((GD.Randf() > 0.5) ? -1 : 1) * 3.5f;
                                        }
                                    }
                                    else
                                    {
                                        speed_y *= -1;
                                    }
                                    GetNode<AudioStreamPlayer>("bowling_impact").Play();
                                }
                            }
                        }
                    }
                }
            }
            if (!first_Speed_Y)
            {
                if (this.Position.y < 94)
                {
                    speed_y = 3.5f;
                }
                if (this.Position.y > 588)
                {
                    speed_y = -3.5f;
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
        health = 4000;
        speed_x = 5.5f;
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
    {}
    protected override void Plants_Remove_List()
    {}
    protected override void Plants_Init()
    {
        GetNode<AnimationPlayer>("Main/Player1").Play("Player1");
        GetNode<Normal_Plants_Area>("Main/Shovel_Area").has_planted = this.has_planted;
        GetNode<Crash_Area_2D>("Main/Crash_Area").has_planted = this.has_planted;
        GetNode<AudioStreamPlayer>("bowling").Play();
    }
    protected override bool Allow_Plants()
    {
        return ((In_Game_Main.Sun_Number >= card_parent_Button.sun && Dock_Area_2D.Normal_Plant_List.Count == 0 && Dock_Area_2D.now_type[Dock_Area_2D.now_type.Count - 1] == 1) || Public_Main.debuging) && on_lock_grid;
    }
    public void Bug_Doing()
    {
        //NOTHING
    }
}
