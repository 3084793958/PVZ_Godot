using Godot;
using System;

public class Van_Door_Bullets_Main : Normal_Plants_Bullets
{
    public override void _Ready()
    {
        GetNode<Bullets_Area>("Area2D").hurt = 20;
        GetNode<AudioStreamPlayer>("Touch").Stream.Set("loop", false);
        GetNode<AnimationPlayer>("Pea_Animation").Play("Pea");
    }
    public override void _Process(float delta)
    {
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
                GetNode<AnimationPlayer>("Touch_Animation").Play("Touch");
            }
        }
    }
}
