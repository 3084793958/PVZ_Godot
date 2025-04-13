using Godot;
using System;

public class Fune_Shroom_Bullets_Main : Normal_Plants_Bullets
{
    public override void _Ready()
    {
        GetNode<Sprite>("Shader").Show();
        GetNode<AnimationPlayer>("Shader_Ani").Play("Shader");
        GetNode<Fune_Shroom_Bullets_Area>("Touch_Area").hurt = 20;
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
            if (dock_area_2d != null)
            {
                ZIndex = normal_ZIndex + 20 * (dock_area_2d.pos[0] - 1);
            }
        }
        catch (Exception)
        {
            GD.Print("Warning:空指针");
            return;
        }
    }
}
