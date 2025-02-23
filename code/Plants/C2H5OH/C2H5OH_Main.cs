using Godot;
using System;

public class C2H5OH_Main : Normal_Plants
{
    public bool is_fire = true;
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
                if (is_fire)
                {
                    GetNode<Node2D>("Main/Hurt_Fire").ZIndex = normal_ZIndex + 20 * (Dock_Area_2D.pos[0] - 1) + 6;
                    if (!GetNode<AnimationPlayer>("Died2").IsPlaying())
                    {
                        Dock_Area_2D.Normal_Plant_List.Remove(this);
                        GetNode<AnimationPlayer>("Died2").Play("Died");
                    }
                }
                else
                {
                    if (!GetNode<AnimationPlayer>("Died1").IsPlaying())
                    {
                        Dock_Area_2D.Normal_Plant_List.Remove(this);
                        GetNode<AnimationPlayer>("Died1").Play("Died");
                    }
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
        GetNode<C2H5OH_Bullets_Fire_Area>("Main/Bullets_Fire_Area").Monitorable = true;
        GetNode<C2H5OH_Bullets_Fire_Area>("Main/Bullets_Fire_Area").can_work = true;
        GetNode<AnimationPlayer>("Main/Player1").Play("Fire");
        GetNode<Normal_Plants_Area>("Main/Shovel_Area").has_planted = this.has_planted;
        GetNode<Timer>("Timer").Start();
    }
    protected override bool Allow_Plants()
    {
        return ((In_Game_Main.Sun_Number >= card_parent_Button.sun && Dock_Area_2D.Normal_Plant_List.Count == 0 && Dock_Area_2D.now_type[Dock_Area_2D.now_type.Count - 1] == 1) || Public_Main.debuging) && on_lock_grid;
    }
    public void C2H5OH_Up()
    {
        is_fire = false;
        GetNode<Node2D>("Main/Fire").Hide();
        GetNode<AnimationPlayer>("Main/Player1").Stop();
        GetNode<C2H5OH_Bullets_Fire_Area>("Main/Bullets_Fire_Area").can_work = false;
    }
    public void Send_To_Area()
    {
        if (is_fire)
        {
            GetNode<C2H5OH_Died_Fire_Area>("Main/Died_Fire_Area").died = true;
        }
    }
    public void Bug_Doing()
    {
        GetNode<Timer>("Timer").Start();
        is_fire = true;
        GetNode<Node2D>("Main/Fire").Show();
        GetNode<AnimationPlayer>("Main/Player1").Play("Fire");
        GetNode<C2H5OH_Bullets_Fire_Area>("Main/Bullets_Fire_Area").can_work = true;
    }
}
