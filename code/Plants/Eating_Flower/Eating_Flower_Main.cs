using Godot;
using System;
using System.Collections.Generic;

public class Eating_Flower_Main : Normal_Plants
{
    public bool is_eating = false;
    public List<Normal_Zombies_Area> Eating_Zombies_List = new List<Normal_Zombies_Area>();
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
            GetNode<Eating_Flower_Area>("Main/Eating_Area").can_eat = has_planted && !is_eating;
            if (health > 0 && !is_eating && !GetNode<AnimationPlayer>("Main/Eating_End").IsPlaying())
            {
                if (health > 0)
                {
                    if (GetNode<Eating_Flower_Area>("Main/Eating_Area").Choose_Eating_Zombies_Area != null)
                    {
                        if (!GetNode<Eating_Flower_Area>("Main/Eating_Area").Choose_Eating_Zombies_Area.Should_Ignore)
                        {
                            is_eating = true;
                            GetNode<Timer>("Timer").WaitTime = 1f + GetNode<Eating_Flower_Area>("Main/Eating_Area").Choose_Eating_Zombies_Area.health / 75f;
                            GetNode<Timer>("Timer").Start();
                            GetNode<AnimationPlayer>("Main/Eat").Play("Eat");
                        }
                    }
                }
            }
        }
    }
    public override void _Ready()
    {
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
        GetNode<Normal_Plants_Area>("Main/Shovel_Area").has_planted = this.has_planted;
    }
    protected override bool Allow_Plants()
    {
        return ((In_Game_Main.Sun_Number >= card_parent_Button.sun && Dock_Area_2D.Normal_Plant_List.Count == 0 && Dock_Area_2D.now_type[Dock_Area_2D.now_type.Count - 1] == 1) || Public_Main.debuging) && on_lock_grid;
    }
    public void Bug_Doing()
    {
        GetNode<Timer>("Timer").Stop();
        Eating_timeout();
    }
    public void Eating_timeout()
    {
        GetNode<Eating_Flower_Area>("Main/Eating_Area").Choose_Eating_Zombies_Area = null;
        if (health > 0)
        {
            is_eating = false;
            if (!GetNode<AnimationPlayer>("Main/Eating_End").IsPlaying())
            {
                GetNode<AnimationPlayer>("Main/Eating").Stop();
                GetNode<AnimationPlayer>("Main/Eating_End").Play("Eat");
            }
        }
    }
}
