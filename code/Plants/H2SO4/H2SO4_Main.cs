using Godot;
using System;

public class H2SO4_Main : Normal_Plants
{
    public bool on_Mg = false;
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
                    GetNode<H2SO4_Area2D>("Main/H2SO4_Area").has_died = true;
                }
            }
        }
    }
    public override void _Ready()
    {
        health = 100;
        base._Ready();
    }
    protected override void Area_Entered(Control_Area_2D area2D)
    {
        base.Area_Entered(area2D);
        if (area2D.Area2D_type == "Mg_Shining")//TODO
        {
            on_Mg = true;
        }
    }
    protected override void Area_Exited(Control_Area_2D area2D)
    {
        base.Area_Exited(area2D);
        if (area2D.Area2D_type == "Mg_Shining")//TODO
        {
            on_Mg = false;
        }
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
        GetNode<Normal_Plants_Area>("Main/Shovel_Area").has_planted = this.has_planted;
    }
    protected override bool Allow_Plants()
    {
        return ((In_Game_Main.Sun_Number >= card_parent_Button.sun && Dock_Area_2D.Normal_Plant_List.Count == 0 && Dock_Area_2D.now_type[Dock_Area_2D.now_type.Count - 1] == 1) || Public_Main.debuging) && on_lock_grid;
    }
    public void Bug_Doing()
    {
        //none
    }
    public void Click_Pressed()
    {
        if (In_Game_Main.is_playing)
        {
            if (!Normal_Plants.Choosing && GetNode<H2SO4_Area2D>("Main/H2SO4_Area").has_become)
            {
                GetNode<AudioStreamPlayer>("Bug").Play();
                var scene = GD.Load<PackedScene>("res://scene/Bug/Bug.tscn");
                var plant_child = (Bug_Main)scene.Instance();
                plant_child.by_H2SO4 = true;
                Normal_Plants.Choosing = true;
                GetNode<Control>("/root/In_Game/Object").AddChild(plant_child);
                Hide();
                Dock_Area_2D.Normal_Plant_List.Remove(this);
                GetNode<AnimationPlayer>("Free_player").Play("Free");
            }
        }
    }
}
