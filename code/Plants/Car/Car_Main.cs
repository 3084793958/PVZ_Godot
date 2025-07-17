using Godot;
using System;
using System.Collections.Generic;

public class Car_Main : Node2D
{
    [Export] public float Speed = 5f;
    [Export] public int car_type = 1;
    private bool is_Started = false;
    public bool doing_Start = true;
    public List<Normal_Zombies_Area> Zombies_Area_2D_List = new List<Normal_Zombies_Area>();
    protected List<Background_Grid_Main> Dock_Area_2D_List = new List<Background_Grid_Main>();
    protected bool in_water = false;
    protected bool now_in_water = false;
    [Export] public int normal_ZIndex = 2;
    protected List<Background_Out_Land_Main> Out_Land_Area_2D_List = new List<Background_Out_Land_Main>();
    public override void _Ready()
    {
        GetNode<Node2D>("Car/Main/Car1").Visible = car_type == 1;
        GetNode<Node2D>("Car/Main/Car2").Visible = car_type == 2;
        if (doing_Start)
        {
            GetNode<AnimationPlayer>("Car/Start").Play("Start");
        }
    }
    public void Start_Area2D(Area2D area_node)
    {
        if (!(area_node is Control_Area_2D area2D) || !IsInstanceValid(area2D))
        {
            return;
        }
        if (area2D.Area2D_type == "Zombies")
        {
            Zombies_Area_2D_List.Add((Normal_Zombies_Area)area2D);
        }
    }
    public void Start_Area2D_Out(Area2D area_node)
    {
        if (!(area_node is Control_Area_2D area2D) || !IsInstanceValid(area2D))
        {
            return;
        }
        if (area2D.Area2D_type == "Zombies")
        {
            Zombies_Area_2D_List.Remove((Normal_Zombies_Area)area2D);
        }
    }
    protected void Dock_Entered(Area2D area_node)
    {
        if (!(area_node is Control_Area_2D area2D) || !IsInstanceValid(area2D))
        {
            return;
        }
        string Type_string = area2D?.Area2D_type;
        if (Type_string != null && Type_string == "Grid")
        {
            Dock_Area_2D_List.Add((Background_Grid_Main)area2D);
        }
        else if (Type_string != null && Type_string == "Out_Land")
        {
            Out_Land_Area_2D_List.Add((Background_Out_Land_Main)area2D);
        }
    }
    protected void Dock_Exited(Area2D area_node)
    {
        if (!(area_node is Control_Area_2D area2D) || !IsInstanceValid(area2D))
        {
            return;
        }
        string Type_string = area2D?.Area2D_type;
        if (Type_string != null && Type_string == "Grid")
        {
            Dock_Area_2D_List.Remove((Background_Grid_Main)area2D);
        }
        else if (Type_string != null && Type_string == "Out_Land")
        {
            Out_Land_Area_2D_List.Remove((Background_Out_Land_Main)area2D);
        }
    }
    public override void _PhysicsProcess(float delta)
    {
        if (Dock_Area_2D_List.Count != 0)
        {
            in_water = Dock_Area_2D_List[0].type == 2;
        }
        if (Out_Land_Area_2D_List.Count != 0)
        {
            in_water = Out_Land_Area_2D_List[0].type == 2;
        }
        if (is_Started)
        {
            //
            if (car_type == 2)
            {
                if (in_water && !GetNode<AnimationPlayer>("Car/Main/Car2/In_Water").IsPlaying() && !GetNode<AnimationPlayer>("Car/Main/Car2/Out_Water").IsPlaying() && !now_in_water)
                {
                    GetNode<AnimationPlayer>("Car/Main/Car2/In_Water").Play("Water");
                    now_in_water = true;
                }
                if (!in_water && !GetNode<AnimationPlayer>("Car/Main/Car2/Out_Water").IsPlaying() && !GetNode<AnimationPlayer>("Car/Main/Car2/In_Water").IsPlaying() && now_in_water)
                {
                    GetNode<AnimationPlayer>("Car/Main/Car2/Out_Water").Play("Water");
                    now_in_water = false;
                }
                if (!GetNode<Control>("Car/Main/Car2/Clip").RectClipContent && now_in_water && !GetNode<AnimationPlayer>("Car/Main/Car2/In_Water").IsPlaying() && !GetNode<AnimationPlayer>("Car/Main/Car2/Out_Water").IsPlaying())
                {
                    in_water = false;
                    now_in_water = false;
                }
            }
            //
            this.Position += new Vector2(Speed * delta * 60, 0);
            if (this.Position.x>1437)
            {
                this.QueueFree();
            }
            if (Zombies_Area_2D_List.Count != 0)
            {
                for (int i = 0; i < Zombies_Area_2D_List.Count; i++)
                {
                    if (Zombies_Area_2D_List[i].has_plant && !Zombies_Area_2D_List[i].has_lose_head)
                    {
                        GetNode<AnimationPlayer>("Stop_Speed").Play("Speed");
                        break;
                    }
                }
            }
        }
        else
        {
            if (Zombies_Area_2D_List.Count != 0) 
            {
                for (int i = 0; i < Zombies_Area_2D_List.Count; i++)
                {
                    if (Zombies_Area_2D_List[i].has_plant && !Zombies_Area_2D_List[i].has_lose_head) 
                    {
                        is_Started = true;
                        if (car_type == 1)
                        {
                            GetNode<AnimationPlayer>("Car/Main/Car1/Run").Play("Run");
                        }
                        if (car_type == 2)
                        {
                            GetNode<AnimationPlayer>("Car/Main/Car2/Run").Play("Run");
                        }
                        GetNode<AudioStreamPlayer>("Car_Run").Play();
                        break;
                    }
                }
            }
        }
    }
}
