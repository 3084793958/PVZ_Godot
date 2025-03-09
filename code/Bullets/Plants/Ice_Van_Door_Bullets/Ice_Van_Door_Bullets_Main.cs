using Godot;
using System;

public class Ice_Van_Door_Bullets_Main : Normal_Plants_Bullets
{
    public int Bullets_Type = 1;
    public override void _Ready()
    {
        GetNode<Ice_Bullets_Area>("Area2D").hurt = 20;
        GetNode<AudioStreamPlayer>("Touch").Stream.Set("loop", false);
        GetNode<AnimationPlayer>("Pea_Animation").Play("Pea");
        normal_ZIndex = 17;
    }
    public void Dock_Enter(Area2D area_node)
    {
        try
        {
            if (!(area_node is Control_Area_2D area2D) || !IsInstanceValid(area2D))
            {
                return;
            }
            if (area2D.Area2D_type == "Grid")
            {
                dock_area_2d = (Background_Grid_Main)area2D;
            }
        }
        catch (Exception)
        {
            GD.Print("Warning:空指针");
            return;
        }
    }
    public override void _PhysicsProcess(float delta)
    {
        try
        {
            if (!GetNode<Area2D>("Area2D").IsConnected("area_entered", this, nameof(Touch_Zombies)))
            {
                return;
            }
            if (dock_area_2d != null)
            {
                ZIndex = normal_ZIndex + 20 * (dock_area_2d.pos[0] - 1);
            }
            if (!has_touch)
            {
                this.Position += new Vector2(speed_x * delta * 60, speed_y * delta * 60);
            }
            if (Position.x > 1437)
            {
                this.QueueFree();
            }
        }
        catch (Exception)
        {
            GD.Print("Warning:空指针");
            return;
        }
    }
    public void Touch_Zombies(Area2D area_node)
    {
        try
        {
            if (!(area_node is Control_Area_2D area_2D) || !IsInstanceValid(area_2D))
            {
                return;
            }
            if (!has_touch && area_2D.Area2D_type == "Zombies")
            {
                Top_Zombies_Area = (Normal_Zombies_Area)area_2D;
                if (Top_Zombies_Area.Should_Ignore)
                {
                    return;
                }
                if (Top_Zombies_Area.has_plant)
                {
                    has_touch = true;
                    if (GetNode<Ice_Bullets_Area>("Area2D").Choose_Zombies_Area == null)
                    {
                        GetNode<Ice_Bullets_Area>("Area2D").Choose_Zombies_Area = Top_Zombies_Area;
                    }
                    else
                    {
                        if (Top_Zombies_Area.ZIndex > GetNode<Ice_Bullets_Area>("Area2D").Choose_Zombies_Area.ZIndex || (Top_Zombies_Area.ZIndex == GetNode<Bullets_Area>("Area2D").Choose_Zombies_Area.ZIndex && Top_Zombies_Area.GetParent().GetParent().GetIndex() > GetNode<Bullets_Area>("Area2D").Choose_Zombies_Area.GetParent().GetParent().GetIndex()))
                        {
                            GetNode<Ice_Bullets_Area>("Area2D").Choose_Zombies_Area = Top_Zombies_Area;
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
                if (Bullets_Fire_Area.can_work && Bullets_Type < 3)
                {
                    Bullets_Type++;
                    GetNode<Ice_Bullets_Area>("Area2D").Bullets_Type = Bullets_Type;
                    GetNode<Ice_Bullets_Area>("Area2D").hurt = (int)(GetNode<Ice_Bullets_Area>("Area2D").hurt * Bullets_Fire_Area.MUL_number);
                    if (Bullets_Type == 2)
                    {
                        GetNode<Ice_Bullets_Area>("Area2D").hurt_type = 2;
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
                }
            }
        }
        catch (Exception)
        {
            GD.Print("Warning:空指针");
            return;
        }
    }
    protected async void Free_Self()
    {
        if (GetNode<Area2D>("Area2D").IsConnected("area_entered", this, nameof(Touch_Zombies)))
        {
            GetNode<Area2D>("Area2D").Disconnect("area_entered", this, nameof(Touch_Zombies));
            GetNode<Area2D>("Shovel_Area2D").Disconnect("area_entered", this, nameof(Dock_Enter));
            GetNode<Area2D>("Area2D").Monitoring = false;
            GetNode<Area2D>("Area2D").Monitorable = false;
            GetNode<Area2D>("Shovel_Area2D").Monitoring = false;
            GetNode<Area2D>("Shovel_Area2D").Monitorable = false;
        }
        Hide();
        await ToSignal(GetTree().CreateTimer(0.72f), "timeout");
        if (IsInstanceValid(this))
        {
            this.QueueFree();
        }
    }
}
