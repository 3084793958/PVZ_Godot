using Godot;
using System;

public class Lotus_Main : Normal_Plants
{
    public bool has_Add_Pos = false;
    public bool has_Remove_Pos = false;
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
                }
            }
            if (Dock_Area_2D != null && health > 0 && !on_Shovel) 
            {
                bool founded = false;
                int for2_number = 0;
                for (int i = 0; i < Dock_Area_2D.Down_Plant_List.Count; i++)
                {
                    if (Dock_Area_2D.Down_Plant_List[i] == this)
                    {
                        for2_number = i;
                        break;
                    }
                }
                for (int i = for2_number; i > 0; i--)
                {
                    if (Dock_Area_2D.Down_Plant_List[i] is Small_Shroom_Pot_Main) 
                    {
                        Up_Number = i + 1;
                        founded = true;
                        break;
                    }
                }
                if (!founded)
                {
                    Up_Number = 0;
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
        Dock_Area_2D.Down_Plant_List.Add(this);
        if (!has_Add_Pos)
        {
            has_Add_Pos = true;
            Dock_Area_2D.now_type.Add(1);
        }
    }
    protected override void Plants_Remove_List()
    {
        if (Dock_Area_2D == null)
        {
            return;
        }
        Dock_Area_2D.Down_Plant_List.Remove(this);
        if (!has_Remove_Pos)
        {
            has_Remove_Pos = true;
            Dock_Area_2D.now_type.Remove(1);
        }
    }
    protected override void Plants_Init()
    {
        if (Dock_Area_2D != null && Dock_Area_2D.type == 2) 
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
        return ((In_Game_Main.Sun_Number >= this_sun && Dock_Area_2D.Allow_Down_Plant(1) && Dock_Area_2D.now_type[Dock_Area_2D.now_type.Count - 1] == 2) || Public_Main.debuging) && on_lock_grid;
    }
    public void Bug_Doing()
    {
        //NONE
    }
}
