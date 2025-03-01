using Godot;
using System;

public class Lotus_Main : Normal_Plants
{
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
                    Dock_Area_2D.Down_Plant_List.Remove(this);
                    Dock_Area_2D.now_type.Remove(1);
                    GetNode<AnimationPlayer>("Died").Play("Died");
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
        Dock_Area_2D.Down_Plant_List.Add(this);
        Dock_Area_2D.now_type.Add(1);
    }
    protected override void Plants_Remove_List()
    {
        Dock_Area_2D.Down_Plant_List.Remove(this);
        Dock_Area_2D.now_type.Remove(1);
    }
    protected override void Plants_Init()
    {
        if (Dock_Area_2D.type == 2)
        {
            GetNode<AudioStreamPlayer>("Plant_Sound/Ok/Water").Play();
            GetNode<AnimationPlayer>("Main/Splash_Player").Play("Splash");
        }
        GetNode<AnimationPlayer>("Main/Player1").Play("Player1");
        GetNode<AnimationPlayer>("Main/Player2").Play("Player1");
        GetNode<Normal_Plants_Area>("Main/Shovel_Area").has_planted = this.has_planted;
    }
    protected override bool Allow_Plants()
    {
        return ((In_Game_Main.Sun_Number >= card_parent_Button.sun && Dock_Area_2D.Down_Plant_List.Count == 0 && Dock_Area_2D.now_type[Dock_Area_2D.now_type.Count - 1] == 2) || Public_Main.debuging) && on_lock_grid;
    }
    public void Bug_Doing()
    {
        //NONE
    }
}
