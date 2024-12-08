using Godot;
using System;

public class Van_Door_Bullets_Main : Normal_Plants_Bullets
{
    public int Bullets_Type = 1;
    public override void _Ready()
    {
        GetNode<Bullets_Area>("Area2D").hurt = 20;
        GetNode<AudioStreamPlayer>("Touch").Stream.Set("loop", false);
        GetNode<AnimationPlayer>("Pea_Animation").Play("Pea");
        normal_ZIndex = 17;
    }
    public void Dock_Enter(Control_Area_2D area2D)
    {
        if (area2D.Area2D_type == "Grid")
        {
            dock_area_2d = (Background_Grid_Main)area2D;
        }
    }
    public override void _Process(float delta)
    {
        if (dock_area_2d != null)
        {
            ZIndex = normal_ZIndex + 20 * (dock_area_2d.pos[0] - 1);
        }
        if (!has_touch)
        {
            this.Position += new Vector2(speed_x, speed_y);
        }
        if (Position.x>1437)
        {
            this.QueueFree();
        }
    }
    public void Touch_Zombies(Control_Area_2D area_2D)
    {
        if (!has_touch && area_2D.Area2D_type == "Zombies")
        {
            Top_Zombies_Area = (Normal_Zombies_Area)area_2D;
            if (Top_Zombies_Area.has_plant)
            {
                has_touch = true;
                if (GetNode<Bullets_Area>("Area2D").Choose_Zombies_Area == null)
                {
                    GetNode<Bullets_Area>("Area2D").Choose_Zombies_Area = Top_Zombies_Area;
                }
                else
                {
                    if (Top_Zombies_Area.ZIndex > GetNode<Bullets_Area>("Area2D").Choose_Zombies_Area.ZIndex || (Top_Zombies_Area.ZIndex == GetNode<Bullets_Area>("Area2D").Choose_Zombies_Area.ZIndex && Top_Zombies_Area.GetParent().GetParent().GetIndex() > GetNode<Bullets_Area>("Area2D").Choose_Zombies_Area.GetParent().GetParent().GetIndex()))
                    {
                        GetNode<Bullets_Area>("Area2D").Choose_Zombies_Area = Top_Zombies_Area;
                    }
                }
                if (Bullets_Type == 1)
                {
                    GetNode<AnimationPlayer>("Touch_Animation").Play("Touch");
                }
                else
                {
                    GetNode<AnimationPlayer>("Fire_Animation").Play("Touch");
                }
            }
        }
        if (area_2D.Area2D_type == "Bullets_Fire")
        {
            var Bullets_Fire_Area = (C2H5OH_Bullets_Fire_Area)area_2D;
            if (Bullets_Fire_Area.can_work && Bullets_Type < 4)
            {
                Bullets_Type++;
                GetNode<Bullets_Area>("Area2D").hurt = (int)(GetNode<Bullets_Area>("Area2D").hurt * Bullets_Fire_Area.MUL_number);
                if (Bullets_Type == 2)
                {
                    GetNode<Bullets_Area>("Area2D").hurt_type = 2;
                    GetNode<Node2D>("Pea_Fire/Fire1").Show();
                    GetNode<AnimationPlayer>("Pea_Fire/Fire1/Fire_Run").Play("Run");
                    GetNode<Node2D>("Pea/Pea1").Hide();
                    GetNode<Node2D>("Pea/Pea2").Show();
                    GetNode<Node2D>("Effect/Effect1").Hide();
                    GetNode<Node2D>("Effect/1").Show();
                    GetNode<AnimationPlayer>("Effect/1/turn_fire").Play("fire");
                }
                else if (Bullets_Type == 3)
                {
                    GetNode<Node2D>("Pea_Fire/Fire1").Hide();
                    GetNode<Node2D>("Pea_Fire/Fire2").Show();
                    GetNode<AnimationPlayer>("Pea_Fire/Fire1/Fire_Run").Stop();
                    GetNode<AnimationPlayer>("Pea_Fire/Fire2/Fire_Run").Play("Run");
                    GetNode<Node2D>("Pea/Pea2").Hide();
                    GetNode<Node2D>("Pea/Pea3").Show();
                }
                else if (Bullets_Type == 4)
                {
                    GetNode<Node2D>("Pea_Fire/Fire2").Hide();
                    GetNode<Node2D>("Pea_Fire/Fire3").Show();
                    GetNode<AnimationPlayer>("Pea_Fire/Fire2/Fire_Run").Stop();
                    GetNode<AnimationPlayer>("Pea_Fire/Fire3/Fire_Run").Play("Run");
                    GetNode<Node2D>("Pea/Pea3").Hide();
                    GetNode<Node2D>("Pea/Pea4").Show();
                }
            }
        }
    }
}
