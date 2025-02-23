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
    public override void _Ready()
    {
        GetNode<Node2D>("Car/Main/Car1").Visible = car_type == 1;
        if (doing_Start)
        {
            GetNode<AnimationPlayer>("Car/Start").Play("Start");
        }
    }
    public void Start_Area2D(Control_Area_2D area2D)
    {
        if (area2D.Area2D_type == "Zombies")
        {
            Zombies_Area_2D_List.Add((Normal_Zombies_Area)area2D);
        }
    }
    public void Start_Area2D_Out(Control_Area_2D area2D)
    {
        if (area2D.Area2D_type == "Zombies")
        {
            Zombies_Area_2D_List.Remove((Normal_Zombies_Area)area2D);
        }
    }
    public override void _Process(float delta)
    {
        if (is_Started)
        {
            this.Position += new Vector2(Speed, 0);
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
                        GetNode<AudioStreamPlayer>("Car_Run").Play();
                        break;
                    }
                }
            }
        }
    }
}
