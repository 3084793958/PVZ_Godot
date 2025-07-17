using Godot;
using System;

public class PL_Casing_Main : Normal_Plants
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
            if (Dock_Area_2D != null)
            {
                float max_PL_Size = 1f;
                int _Y_Add = 0;
                for (int i = 0; i < Dock_Area_2D.Normal_Plant_List.Count; i++)
                {
                    if (Dock_Area_2D.Normal_Plant_List[i] == this)
                    {
                        continue;
                    }
                    if (Dock_Area_2D.Normal_Plant_List[i] is Normal_Plants plants_object)
                    {
                        if (max_PL_Size < plants_object.PL_Casing_Size)
                        {
                            max_PL_Size = plants_object.PL_Casing_Size;
                            _Y_Add = plants_object.PL_Casing_Y_Add;
                        }
                    }
                }
                PL_Casing_Size = max_PL_Size;
                PL_Casing_Y_Add = _Y_Add;
                GetNode<Sprite>("Main/Head").Scale = new Vector2(0.16f * PL_Casing_Size, 0.16f * PL_Casing_Size);
                GetNode<Sprite>("Main/Head").GlobalPosition = GetNode<Control>("Main").RectGlobalPosition + new Vector2(44, 44) + new Vector2(0, PL_Casing_Y_Add);
            }
            if (health <= 0)
            {
                if (!GetNode<AnimationPlayer>("Died").IsPlaying())
                {
                    Plants_Remove_List();
                    GetNode<AnimationPlayer>("Died").Play("Died");
                }
            }
        }
    }
    public override void _Ready()
    {
        Not_H_Hurt = true;
        health = 1500;
        normal_ZIndex = 4;
        Label_Health_Mode = 1;
        base._Ready();
    }
    protected override void Plants_Add_List()
    {
        if (Dock_Area_2D == null)
        {
            return;
        }
        Dock_Area_2D.Casing_Plant_List.Add(this);
    }
    protected override void Plants_Remove_List()
    {
        if (Dock_Area_2D == null)
        {
            return;
        }
        Dock_Area_2D.Casing_Plant_List.Remove(this);
    }
    protected override void Plants_Init()
    {
        GetNode<AnimationPlayer>("Main/Player1").Play("Player1");
        GetNode<Normal_Plants_Area>("Main/Shovel_Area").has_planted = this.has_planted;
    }
    protected override bool Allow_Plants()
    {
        return ((In_Game_Main.Sun_Number >= this_sun && Dock_Area_2D.Casing_Plant_List.Count == 0) || Public_Main.debuging) && on_lock_grid;
    }
    public void Bug_Doing()
    {
        health += 3000;
    }
    public void Fix_Timer_Gird_timeout()
    {
        if (!has_planted)
        {
            return;
        }
        if (Dock_Area_2D == null)
        {
            return;
        }
        for (int i = 0; i < Dock_Area_2D.Normal_Plant_List.Count; i++)
        {
            if (Dock_Area_2D.Normal_Plant_List[i] is Normal_Plants plants_object)
            {
                if (plants_object.health < plants_object.Exchange_Health - 20)
                {
                    plants_object.health += 20;
                }
                else if (plants_object.health < plants_object.Exchange_Health) 
                {
                    plants_object.health = plants_object.Exchange_Health;
                }
            }
        }
    }
    public void Fix_Timer_timeout()
    {
        if (!has_planted)
        {
            return;
        }
        if (health < 1000)
        {
            health += 500;
        }
        else if (health < 1500)
        {
            health = 1500;
        }
    }
}
