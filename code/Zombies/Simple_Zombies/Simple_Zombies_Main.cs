using Godot;
using System;

public class Simple_Zombies_Main : Normal_Zombies
{
    private bool has_lose_Arm = false;
    private bool has_lose_Head = false;
    public float speed = -1f*(float)GD.RandRange(0.1,0.2);
    public bool eating = false;
    private bool has_touched = false;
    public int hurt_time = 0;
    public override void _Ready()
    {
        GetNode<AudioStreamPlayer>("Plant_Sound/Ok/Plant1").Stream.Set("loop", false);
        GetNode<AudioStreamPlayer>("Plant_Sound/Ok/Plant2").Stream.Set("loop", false);
        GetNode<AudioStreamPlayer>("Plant_Sound/Ok/Water").Stream.Set("loop", false);
        GetNode<AudioStreamPlayer>("Main/Eat_Sound1").Stream.Set("loop", false);
        GetNode<AudioStreamPlayer>("Main/Eat_Sound2").Stream.Set("loop", false);
        GetNode<AudioStreamPlayer>("Main/Fall").Stream.Set("loop", false);
        health = 270;
        if (!player_put)
        {
            has_planted = true;
            GetNode<Normal_Zombies_Area>("Main/Zombies_Area").has_plant = true;
        }
    }
    public void Plants_Entered(Control_Area_2D area2D)
    {
        if (area2D.Area2D_type == "Plants")
        {
            var plant_area2D = (Normal_Plants_Area)area2D;
            if (plant_area2D.plants_type == "Normal")
            {
                if (has_planted)
                {
                    if (plant_area2D.has_planted)
                    {
                        Plants_Area_2D_List.Add(plant_area2D);
                        if (Plants_Area_2D_List[0].has_planted)
                        {
                            Top_Area_2D = Plants_Area_2D_List[0];
                        }
                        else
                        {
                            Top_Area_2D = null;
                        }
                        for (int i = 0; i < Plants_Area_2D_List.Count; i++)
                        {
                            if (Plants_Area_2D_List[i].has_planted)
                            {
                                if (Top_Area_2D == null)
                                {
                                    Top_Area_2D = Plants_Area_2D_List[i];
                                }
                                else
                                {
                                    if (Plants_Area_2D_List[i].ZIndex > Top_Area_2D.ZIndex)
                                    {
                                        Top_Area_2D = Plants_Area_2D_List[i];
                                    }
                                    else if (Plants_Area_2D_List[i].ZIndex == Top_Area_2D.ZIndex)
                                    {
                                        if (Plants_Area_2D_List[i].GetParent().GetParent().GetIndex() > Top_Area_2D.GetParent().GetParent().GetIndex())
                                        {
                                            Top_Area_2D = Plants_Area_2D_List[i];
                                        }
                                    }
                                }
                            }
                        }
                        GetNode<Normal_Zombies_Area>("Main/Zombies_Area").Choose_Plants_Area = Top_Area_2D;
                        if (Top_Area_2D != null)
                        {
                            eating = true;
                            GetNode<AnimationPlayer>("Main/Walk").Stop();
                            GetNode<AnimationPlayer>("Main/Eating").Play("Eating");
                        }
                    }
                    else
                    {
                        has_touched = true;
                        Plants_Area_2D_List.Add(plant_area2D);
                    }
                }
                else
                {
                    if (plant_area2D.has_planted)
                    {
                        has_touched = true;
                        Plants_Area_2D_List.Add(plant_area2D);
                    }
                }
            }
        }
        if (area2D.Area2D_type == "Plants_Bullets")
        {
            var bullets_area2D = (Bullets_Area)area2D;
            if (bullets_area2D.Choose_Zombies_Area==GetNode<Normal_Zombies_Area>("Main/Zombies_Area"))
            {
                hurt_time++;
                this.Modulate = hurt_color;
                health -= bullets_area2D.hurt;
            }
        }
        if (area2D.Area2D_type == "Plants_Boom")
        {
            var Boom_area2D = (Normal_Boom_Area)area2D;
            Boom_Area_2D_List.Add(Boom_area2D);
        }
    }
    public void Plants_Exited(Control_Area_2D area2D)
    {
        if (area2D.Area2D_type == "Plants")
        {
            var plant_area2D = (Normal_Plants_Area)area2D;
            if (plant_area2D.plants_type == "Normal")
            {
                has_touched = false;
                Plants_Area_2D_List.Remove(plant_area2D);
                if (this.has_planted && plant_area2D.has_planted)
                {
                    Plants_Area_2D_List.Remove(plant_area2D);
                    if (Plants_Area_2D_List.Count == 0)
                    {
                        Top_Area_2D = null;
                        eating = false;
                        GetNode<AnimationPlayer>("Main/Walk").Play("Walk");
                        GetNode<AnimationPlayer>("Main/Eating").Stop();
                    }
                    else
                    {
                        if (Plants_Area_2D_List[0].has_planted)
                        {
                            Top_Area_2D = Plants_Area_2D_List[0];
                        }
                        else
                        {
                            Top_Area_2D = null;
                        }
                        for (int i = 0; i < Plants_Area_2D_List.Count; i++)
                        {
                            if (Plants_Area_2D_List[i].has_planted)
                            {
                                if (Top_Area_2D == null)
                                {
                                    Top_Area_2D = Plants_Area_2D_List[i];
                                }
                                else
                                {
                                    if (Plants_Area_2D_List[i].ZIndex > Top_Area_2D.ZIndex)
                                    {
                                        Top_Area_2D = Plants_Area_2D_List[i];
                                    }
                                    else if (Plants_Area_2D_List[i].ZIndex == Top_Area_2D.ZIndex)
                                    {
                                        if (Plants_Area_2D_List[i].GetParent().GetParent().GetIndex() > Top_Area_2D.GetParent().GetParent().GetIndex())
                                        {
                                            Top_Area_2D = Plants_Area_2D_List[i];
                                        }
                                    }
                                }
                            }
                        }
                    }
                    GetNode<Normal_Zombies_Area>("Main/Zombies_Area").Choose_Plants_Area = Top_Area_2D;
                }
            }
        }
        if (area2D.Area2D_type == "Plants_Bullets")
        {
            var bullets_area2D = (Bullets_Area)area2D;
            if (bullets_area2D.Choose_Zombies_Area == GetNode<Normal_Zombies_Area>("Main/Zombies_Area"))
            {
                hurt_time--;
                if (hurt_time == 0)
                {
                    this.Modulate = normal_color;
                }
                else if (hurt_time < 0)
                {
                    hurt_time = 0;
                }
            }
        }
    }
    public void Dock_Enter(Control_Area_2D area2D)
    {
        if (!has_planted && area2D.Area2D_type == "Grid")
        {
            var area2D_Grid = (Background_Grid_Main)area2D;
            Area_Vector2 = area2D_Grid.GlobalPosition;
            dock_area_2d = area2D_Grid;
        }
    }
    public override void _Process(float delta)
    {
        if (!has_planted)
        {
            if (Normal_Plants.Choosing)
            {
                Show();
                this.GlobalPosition = GetTree().Root.GetMousePosition();
                var dock_control = GetNode<Control>("Dock");
                if (Math.Abs(GlobalPosition.x - Area_Vector2.x) < 40 && Math.Abs(GlobalPosition.y - Area_Vector2.y) < 40)
                {
                    dock_control.SetGlobalPosition(Area_Vector2 - new Vector2(0, 16));
                    on_lock_grid = true;
                }
                else
                {
                    dock_control.SetPosition(new Vector2(0, -16));
                    on_lock_grid = false;
                }
                if (Input.IsActionPressed("Right_Mouse"))
                {
                    Normal_Plants.Choosing = false;
                    Hide();
                    Position = new Vector2(-100, -100);
                    GetNode<AudioStreamPlayer>("/root/In_Game/Cancel").Play();
                    card_parent_Button.Set_ColorRect_2(false);
                    this.QueueFree();
                }
                if (Input.IsActionPressed("Left_Mouse"))
                {
                    card_parent_Button.Set_ColorRect_2(false);
                    Normal_Plants.Choosing = false;
                    if ((In_Game_Main.Sun_Number >= card_parent_Button.sun || Public_Main.debuging) && on_lock_grid)
                    {
                        has_planted = true;
                        GetNode<Normal_Zombies_Area>("Main/Zombies_Area").has_plant = true;
                        dock_control.Hide();
                        this.GlobalPosition = Area_Vector2;
                        In_Game_Main.Sun_Number -= card_parent_Button.sun;
                        In_Game_Main.Update_Sun(this);
                        card_parent_Button.now_time = card_parent_Button.wait_time;
                        if (dock_area_2d.type == 1)
                        {
                            if (GD.Randf() > 0.5f)
                            {
                                GetNode<AudioStreamPlayer>("Plant_Sound/Ok/Plant1").Play();
                            }
                            else
                            {
                                GetNode<AudioStreamPlayer>("Plant_Sound/Ok/Plant2").Play();
                            }
                        }
                        else if (dock_area_2d.type == 2)
                        {
                            GetNode<AudioStreamPlayer>("Plant_Sound/Ok/Water").Play();
                        }
                        GetNode<Control>("Dock").Hide();
                        GetNode<Control>("Show").Hide();
                        GetNode<Control>("Main").Show();
                        GetNode<AnimationPlayer>("Main/Walk").Play("Walk");
                    }
                    else
                    {
                        Hide();
                        Position = new Vector2(-40, -40);
                        GetNode<AudioStreamPlayer>("/root/In_Game/Cancel").Play();
                        this.QueueFree();
                    }
                }
            }
            else
            {
                on_lock_grid = false;
            }
        }
        else
        {
            if (!eating && GetNode<AnimationPlayer>("Main/Walk").IsPlaying() && !On_Boom_Effect)
            {
                this.Position += new Vector2(speed * speed_x, 0);
            }
            if (has_touched)
            {
                if (Plants_Area_2D_List[0].has_planted)
                {
                    Top_Area_2D = Plants_Area_2D_List[0];
                }
                else
                {
                    Top_Area_2D = null;
                }
                for (int i = 0; i < Plants_Area_2D_List.Count; i++)
                {
                    if (Plants_Area_2D_List[i].has_planted)
                    {
                        has_touched = false;
                        if (Top_Area_2D == null)
                        {
                            Top_Area_2D = Plants_Area_2D_List[i];
                        }
                        else
                        {
                            if (Plants_Area_2D_List[i].ZIndex > Top_Area_2D.ZIndex)
                            {
                                Top_Area_2D = Plants_Area_2D_List[i];
                            }
                            else if (Plants_Area_2D_List[i].ZIndex == Top_Area_2D.ZIndex)
                            {
                                if (Plants_Area_2D_List[i].GetParent().GetParent().GetIndex() > Top_Area_2D.GetParent().GetParent().GetIndex())
                                {
                                    Top_Area_2D = Plants_Area_2D_List[i];
                                }
                            }
                        }
                    }
                }
                GetNode<Normal_Zombies_Area>("Main/Zombies_Area").Choose_Plants_Area = Top_Area_2D;
                if (Top_Area_2D != null && !has_lose_Head)
                {
                    eating = true;
                    GetNode<AnimationPlayer>("Main/Walk").Stop();
                    GetNode<AnimationPlayer>("Main/Eating").Play("Eating");
                }
            }
            if (health<180&&!has_lose_Arm)
            {
                has_lose_Arm = true;
                GetNode<AnimationPlayer>("Main/Lose_Arm").Play("Lose_Arm");
            }
            if (health<90&&!has_lose_Head)
            {
                has_lose_Head = true;
                GetNode<Normal_Zombies_Area>("Main/Zombies_Area").has_lose_head = true;
                GetNode<AnimationPlayer>("Main/Lose_Head").Play("Lose_Head");
            }
            if (Boom_Area_2D_List.Count != 0)
            {
                for (int i = 0; i < Boom_Area_2D_List.Count; i++)
                {
                    if (Boom_Area_2D_List[i].can_do && !Boom_Area_2D_List[i].end_hurt)
                    {
                        if (Boom_Area_2D_List[i].hurt>=health-90)
                        {
                            On_Boom_Effect = true;
                            GetNode<AnimationPlayer>("Main/Walk").Stop();
                            GetNode<AnimationPlayer>("Main/Eating").Stop();
                            GetNode<AnimationPlayer>("Main/Boom_Effect").Play("Boom_Effect");
                        }
                        else
                        {
                            health -= Boom_Area_2D_List[i].hurt;
                        }
                        Boom_Area_2D_List.RemoveAt(i);
                        i--;
                    }
                }
            }
        }
    }
}
