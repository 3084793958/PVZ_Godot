using Godot;
using System;

public class New_Horizons_Bullets_Main : Normal_Plants_Bullets
{
    public int touch_zombies_number = 0;
    public override void _PhysicsProcess(float delta)
    {
        var target = GetNode<Sprite>("Shader/Shader");
        var viewport_size = GetViewportRect().Size;
        var target_size = new Vector2(40, 40);
        var target_pos = target.GlobalPosition;
        var target_size_uv = target_size / viewport_size;
        var target_pos_uv = target_pos / viewport_size;
        target.Material.Set("shader_param/screen_pos", target_pos_uv);
        target.Material.Set("shader_param/screen_size", target_size_uv);
        if (dock_area_2d != null)
        {
            ZIndex = normal_ZIndex + 20 * (dock_area_2d.pos[0] - 1);
        }
        if (touch_zombies_number < 5)
        {
            if (_y_type == 1)
            {
                double sita_rad = speed_y * Math.PI / 180;
                this.Position += new Vector2((float)(speed_x * Math.Cos(sita_rad) * delta * 60), (float)(speed_x * Math.Sin(sita_rad) * delta * 60));
            }
            else
            {
                this.Position += new Vector2(speed_x * delta * 60, speed_y * delta * 60);
            }
            GetNode<New_Horizons_Bullets_Area>("Area2D").hurt = 50 / (touch_zombies_number + 1);
        }
        if (Position.x > 1437 || Position.x < -512 || Position.y > 800 || Position.y < -200)
        {
            if (this.Visible)
            {
                Hide();
                Free_Self();
            }
        }
        if (touch_zombies_number < 5)
        { }
        else
        {
            if (this.Visible)
            {
                Hide();
                Free_Self();
            }
        }
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
    public override void _Ready()
    {
        GetNode<New_Horizons_Bullets_Area>("Area2D").hurt = 50;
        speed_x = 10f;
        normal_ZIndex = 17;
    }
    public void Touch_Zombies(Area2D area_node)
    {
        if (!(area_node is Control_Area_2D area_2D) || !IsInstanceValid(area_2D))
        {
            return;
        }
        if (area_2D.Area2D_type == "Zombies")
        {
            Top_Zombies_Area = (Normal_Zombies_Area)area_2D;
            if (Top_Zombies_Area.Should_Ignore)
            {
                return;
            }
            if (Top_Zombies_Area.has_plant)
            {
                if (GetNode<New_Horizons_Bullets_Area>("Area2D").Choose_Zombies_Area == null)
                {
                    GetNode<New_Horizons_Bullets_Area>("Area2D").Choose_Zombies_Area = Top_Zombies_Area;
                }
                else
                {
                    if (Top_Zombies_Area.ZIndex > GetNode<New_Horizons_Bullets_Area>("Area2D").Choose_Zombies_Area.ZIndex || (Top_Zombies_Area.ZIndex == GetNode<New_Horizons_Bullets_Area>("Area2D").Choose_Zombies_Area.ZIndex && Top_Zombies_Area.GetParent().GetParent().GetParent().GetIndex() > GetNode<New_Horizons_Bullets_Area>("Area2D").Choose_Zombies_Area.GetParent().GetParent().GetParent().GetIndex()))
                    {
                        GetNode<New_Horizons_Bullets_Area>("Area2D").Choose_Zombies_Area = Top_Zombies_Area;
                    }
                }
                touch_zombies_number++;
            }
        }
        else if (area_2D.Area2D_type == "Zombies_Tomb")
        {
            Top_Tomb_Area = (Zombies_Tomb_Area2D)area_2D;
            if (Top_Tomb_Area.has_planted)
            {
                GetNode<New_Horizons_Bullets_Area>("Area2D").on_Tomb = true;
                if (GetNode<New_Horizons_Bullets_Area>("Area2D").Choose_Tomb_Area == null)
                {
                    GetNode<New_Horizons_Bullets_Area>("Area2D").Choose_Tomb_Area = Top_Tomb_Area;
                }
                else
                {
                    if (Top_Tomb_Area.ZIndex > GetNode<New_Horizons_Bullets_Area>("Area2D").Choose_Tomb_Area.ZIndex || (Top_Tomb_Area.ZIndex == GetNode<New_Horizons_Bullets_Area>("Area2D").Choose_Tomb_Area.ZIndex && Top_Tomb_Area.GetParent().GetParent().GetParent().GetIndex() > GetNode<New_Horizons_Bullets_Area>("Area2D").Choose_Tomb_Area.GetParent().GetParent().GetParent().GetIndex()))
                    {
                        GetNode<New_Horizons_Bullets_Area>("Area2D").Choose_Tomb_Area = Top_Tomb_Area;
                    }
                }
                touch_zombies_number++;
            }
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
        await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
        if (IsInstanceValid(this))
        {
            this.QueueFree();
        }
    }
}
