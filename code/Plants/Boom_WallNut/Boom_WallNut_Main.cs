using Godot;
using System;

public class Boom_WallNut_Main : Normal_Plants
{
    public int NUT_type = 3;
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
            if (health >= 1500 && health <= 2500 && NUT_type != 2 && NUT_type != 0)
            {
                NUT_type = 2;
                GetNode<AnimationPlayer>("Main/Hurt1").Play("Hurt");
            }
            else if (health < 1500 && health > 0 && NUT_type != 3 && NUT_type != 0)
            {
                NUT_type = 3;
                GetNode<AnimationPlayer>("Main/Hurt2").Play("Hurt");
            }
            else if (health <= 0)
            {
                if (!GetNode<AnimationPlayer>("Died").IsPlaying())
                {
                    NUT_type = 0;
                    Dock_Area_2D.Normal_Plant_List.Remove(this);
                    GetNode<Normal_Boom_Area>("Main/Boom").hurt = 1800;
                    GetNode<Normal_Boom_Area>("Main/Boom").can_do = true;
                    GetNode<Normal_Boom_Area>("Main/Boom").Start_hurting();
                    GetNode<AnimationPlayer>("Died").Play("Died");
                }
            }
            else if (NUT_type != 1 && NUT_type != 0 && health > 2500)
            {
                NUT_type = 1;
                GetNode<AnimationPlayer>("Main/Hurt3").Play("Hurt");
            }
        }
    }
    public override void _Ready()
    {
        health = 4000;
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
        GetNode<AnimationPlayer>("Main/Player1").Play("Player1");
        GetNode<AnimationPlayer>("Main/Player2").Play("Player1");
        GetNode<Normal_Plants_Area>("Main/Shovel_Area").has_planted = this.has_planted;
        GetNode<AnimationPlayer>("Main/Hurt3").Play("Hurt");
    }
    protected override bool Allow_Plants()
    {
        return ((In_Game_Main.Sun_Number >= card_parent_Button.sun && Dock_Area_2D.Normal_Plant_List.Count == 0 && Dock_Area_2D.now_type[Dock_Area_2D.now_type.Count - 1] == 1) || Public_Main.debuging) && on_lock_grid;
    }
    public void Bug_Doing()
    {
        health += 3000;
        GetNode<AnimationPlayer>("Main/Hurt3").Play("Hurt");
    }
    protected override void Free_Self()
    {
        GetNode<Area2D>("Main/Boom").Monitoring = false;
        GetNode<Area2D>("Main/Boom").Monitorable = false;
        base.Free_Self();
    }
}
