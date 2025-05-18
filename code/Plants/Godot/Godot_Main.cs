using Godot;
using System;

public class Godot_Main : Normal_Plants
{
    public bool clone_5_zombies = false;
    public bool has_clone = false;
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
                    Clone_DO();
                    Plants_Remove_List();
                    GetNode<AnimationPlayer>("Died").Play("Died");
                }
            }
        }
    }
    public override void _Ready()
    {
        GD.Randomize();
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
        GetNode<AnimationPlayer>("Main/Player1").Play("Player1");
        GetNode<Normal_Plants_Area>("Main/Shovel_Area").has_planted = this.has_planted;
    }
    protected override bool Allow_Plants()
    {
        return ((In_Game_Main.Sun_Number >= this_sun && Dock_Area_2D.Normal_Plant_List.Count == 0 && Dock_Area_2D.now_type[Dock_Area_2D.now_type.Count - 1] == 1) || Public_Main.debuging) && on_lock_grid;
    }
    public void Bug_Doing()
    {
        clone_5_zombies = true;
    }
    public void Clone_DO()
    {
        if (!has_clone)
        {
            has_clone = true;
            if (clone_5_zombies)
            {
                for (int i = 0; i < 5; i++)
                {
                    Clone_Self_Zombies();
                }
            }
            else
            {
                Clone_Self_Zombies();
            }
        }
    }
    public void Clean_Area()
    {
        Plants_Remove_List();
    }
    public void Clone_Self_Zombies()
    {
        string Clone_Path = null;
        int res_number = (int)(GD.Randi()) % 3;
        if (res_number == 0)
        {
            Clone_Path = "res://scene/Plants/Zombies/Cone_Zombies/Plants_Cone_Zombies.tscn";
        }
        else if (res_number == 1)
        {
            Clone_Path = "res://scene/Plants/Zombies/Bucket_Zombies/Plants_Bucket_Zombies.tscn";
        }
        else
        {
            Clone_Path = "res://scene/Plants/Zombies/Simple_Zombies/Plants_Simple_Zombies.tscn";
        }
        if (In_Game_Main.background_number == 3 || In_Game_Main.background_number == 4)
        {
            for (int i = 1; i <= 6; i++)
            {
                if (true)
                {
                    Vector2 Zombies_put_position;
                    int _ZIndex;
                    if (i == 1)
                    {
                        Zombies_put_position = new Vector2(76, 124);
                        _ZIndex = 7;
                    }
                    else if (i == 2)
                    {
                        Zombies_put_position = new Vector2(76, 216);
                        _ZIndex = 27;
                    }
                    else if (i == 3)
                    {
                        Zombies_put_position = new Vector2(76, 299);
                        _ZIndex = 47;
                    }
                    else if (i == 4)
                    {
                        Zombies_put_position = new Vector2(76, 376);
                        _ZIndex = 67;
                    }
                    else if (i == 5)
                    {
                        Zombies_put_position = new Vector2(76, 477);
                        _ZIndex = 87;
                    }
                    else
                    {
                        Zombies_put_position = new Vector2(76, 558);
                        _ZIndex = 107;
                    }
                    In_Game_Main.Plants_Zombies_Clone_Request(Clone_Path, Zombies_put_position, _ZIndex);
                }
            }
        }
        else if (In_Game_Main.background_number == 1 || In_Game_Main.background_number == 2)
        {
            for (int i = 1; i <= 5; i++)
            {
                if (true)
                {
                    Vector2 Zombies_put_position;
                    int _ZIndex;
                    if (i == 1)
                    {
                        Zombies_put_position = new Vector2(76, 138);
                        _ZIndex = 7;
                    }
                    else if (i == 2)
                    {
                        Zombies_put_position = new Vector2(76, 234);
                        _ZIndex = 27;
                    }
                    else if (i == 3)
                    {
                        Zombies_put_position = new Vector2(76, 338);
                        _ZIndex = 47;
                    }
                    else if (i == 4)
                    {
                        Zombies_put_position = new Vector2(76, 434);
                        _ZIndex = 67;
                    }
                    else
                    {
                        Zombies_put_position = new Vector2(76, 530);
                        _ZIndex = 87;
                    }
                    In_Game_Main.Plants_Zombies_Clone_Request(Clone_Path, Zombies_put_position, _ZIndex);
                }
            }
        }
    }
    public override void Free_Self()
    {
        if (!has_clone && !on_Shovel) 
        {
            if (!GetNode<AnimationPlayer>("Died").IsPlaying())
            {
                Clone_DO();
                Plants_Remove_List();
                GetNode<AnimationPlayer>("Died").Play("Died");
            }
        }
        else
        {
            base.Free_Self();
        }
    }
}
