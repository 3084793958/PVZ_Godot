using Godot;
using System;

public class C_Boom_Main : Normal_Plants
{
    public void Send_Health()
    {
        if (health > 0)
        {
            GetNode<Normal_Boom_Area>("Main/Boom").hurt = health;
            GetNode<Normal_Boom_Area>("Main/Boom").can_do = true;
            GetNode<Normal_Boom_Area>("Main/Boom").Start_hurting();
        }
    }
    public void Free_Normal_Plant_List()
    {
        Plants_Remove_List();//自用
    }
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
                    Plants_Remove_List();
                    GetNode<AnimationPlayer>("Died").Play("Died");
                    GetNode<AnimationPlayer>("Main/Start_Boom").Stop();
                }
            }
        }
    }
    public override void _Ready()
    {
        health = 3000;
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
        GetNode<Normal_Plants_Area>("Main/Shovel_Area").has_planted = this.has_planted;
        GetNode<AnimationPlayer>("Main/Start_Boom").Play("Start_Boom");
    }
    protected override bool Allow_Plants()
    {
        return ((In_Game_Main.Sun_Number >= this_sun && Dock_Area_2D.Normal_Plant_List.Count == 0 && Dock_Area_2D.now_type[Dock_Area_2D.now_type.Count - 1] == 1) || Public_Main.debuging) && on_lock_grid;
    }
    public override void Free_Self()
    {
        GetNode<Area2D>("Main/Boom").Monitoring = false;
        GetNode<Area2D>("Main/Boom").Monitorable = false;
        base.Free_Self();
    }
}
