using Godot;
using System;

public class Potato_Main : Normal_Plants
{
    public bool has_up = false;
    public bool doing_bug = false;
    [Export] public bool has_boom = false;
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
                bool can_touch = false;
                for (int i = 0; i < Zombies_Area_2D_List.Count; i++)
                {
                    if (Zombies_Area_2D_List[i] != null)
                    {
                        if (Zombies_Area_2D_List[i].Should_Ignore)
                        {
                            continue;
                        }
                        if (Zombies_Area_2D_List[i].Choose_Plants_Area == GetNode<Normal_Plants_Area>("Main/Shovel_Area"))
                        {
                            if (Zombies_Area_2D_List[i].is_eating_show && !Zombies_Area_2D_List[i].has_lose_head)
                            {
                                Zombies_Area_2D_List[i].is_eating_show = false;
                                health -= Zombies_Area_2D_List[i].hurt;
                                GetNode<AnimationPlayer>("Is_Eated").Play("Is_Eated");
                            }
                        }
                        if (!can_touch && Zombies_Area_2D_List[i] != null && !GetNode<AnimationPlayer>("Died").IsPlaying())
                        {
                            if (!Zombies_Area_2D_List[i].has_lose_head && Zombies_Area_2D_List[i].has_plant)
                            {
                                can_touch = true;
                                if (has_up && !has_boom)
                                {
                                    if (doing_bug)
                                    {
                                        GetNode<Normal_Boom_Area>("Main/Boom").hurt = 3000;
                                    }
                                    else
                                    {
                                        GetNode<Normal_Boom_Area>("Main/Boom").hurt = 1800;
                                    }
                                    GetNode<Normal_Boom_Area>("Main/Boom").can_do = true;
                                    GetNode<Normal_Boom_Area>("Main/Boom").Start_hurting();
                                    GetNode<AnimationPlayer>("Died").Play("Died");
                                    Plants_Remove_List();
                                }
                            }
                        }
                    }
                }
            }
            if (health <= 0)
            {
                if (has_up && !has_boom) 
                {
                    if (!GetNode<AnimationPlayer>("Died").IsPlaying())
                    {
                        if (doing_bug)
                        {
                            GetNode<Normal_Boom_Area>("Main/Boom").hurt = 3000;
                        }
                        else
                        {
                            GetNode<Normal_Boom_Area>("Main/Boom").hurt = 1800;
                        }
                        GetNode<Normal_Boom_Area>("Main/Boom").can_do = true;
                        GetNode<Normal_Boom_Area>("Main/Boom").Start_hurting();
                        GetNode<AnimationPlayer>("Died").Play("Died");
                        Plants_Remove_List();
                    }
                }
                else
                {
                    if (!GetNode<AnimationPlayer>("Died2").IsPlaying())
                    {
                        GetNode<AnimationPlayer>("Died2").Play("Died");
                        Plants_Remove_List();
                    }
                }
            }
        }
    }
    public override void _Ready()
    {
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
        GetNode<AnimationPlayer>("Main/Player1").Play("Player");
        GetNode<Normal_Plants_Area>("Main/Shovel_Area").has_planted = this.has_planted;
        GetNode<Timer>("Up_Timer").Start();
    }
    protected override bool Allow_Plants()
    {
        return ((In_Game_Main.Sun_Number >= this_sun && Dock_Area_2D.Normal_Plant_List.Count == 0 && Dock_Area_2D.now_type[Dock_Area_2D.now_type.Count - 1] == 1) || Public_Main.debuging) && on_lock_grid;
    }
    public void Bug_Doing()
    {
        if (!has_up)
        {
            GetNode<Timer>("Up_Timer").Stop();
            Up_Timer_timeout();
            doing_bug = true;
        }
    }
    public void Just_Up()
    {
        has_up = true;
        GetNode<Timer>("Up_Timer").Stop();
        GetNode<AnimationPlayer>("Main/Out_Land").Play("Out_Land");
    }
    public void Up_Timer_timeout()
    {
        if (health > 0 && !has_up) 
        {
            has_up = true;
            GetNode<Timer>("Up_Timer").Stop();
            GetNode<AnimationPlayer>("Main/Out_Land").Play("Out_Land");
        }
    }
    public override void Free_Self()
    {
        if (!has_boom && has_up && !on_Shovel)
        {
            if (doing_bug)
            {
                GetNode<Normal_Boom_Area>("Main/Boom").hurt = 3000;
            }
            else
            {
                GetNode<Normal_Boom_Area>("Main/Boom").hurt = 1800;
            }
            GetNode<Normal_Boom_Area>("Main/Boom").can_do = true;
            GetNode<Normal_Boom_Area>("Main/Boom").Start_hurting();
            GetNode<AnimationPlayer>("Died").Play("Died");
            Plants_Remove_List();
        }
        else
        {
            GetNode<Area2D>("Main/Boom").Monitoring = false;
            GetNode<Area2D>("Main/Boom").Monitorable = false;
            base.Free_Self();
        }
    }
}
