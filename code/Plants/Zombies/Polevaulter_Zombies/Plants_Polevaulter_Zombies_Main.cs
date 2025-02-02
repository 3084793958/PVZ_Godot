using Godot;
using System;
using System.Collections.Generic;

public class Plants_Polevaulter_Zombies_Main : Normal_Plants_Zombies
{
    private bool has_lose_Arm = false;
    private bool has_lose_Head = false;
    public float speed = 5.5f * (float)GD.RandRange(0.1, 0.2);
    public bool eating = false;
    private bool has_touched = false;
    public int hurt_time = 0;
    private bool in_water = false;
    private bool now_in_water = false;
    [Export] public bool has_lose_pole = false;
    [Export] public bool is_jumping = false;
    public List<Normal_Zombies_Area> Jump_Plants_List = new List<Normal_Zombies_Area>();
    public override void _Ready()
    {
        this.AddChild(Android_Timer);
        Android_Timer.WaitTime = 0.5f;
        Android_Timer.Autostart = false;
        Android_Timer.OneShot = true;
        GetNode<AudioStreamPlayer>("Plant_Sound/Ok/Plant1").Stream.Set("loop", false);
        GetNode<AudioStreamPlayer>("Plant_Sound/Ok/Plant2").Stream.Set("loop", false);
        GetNode<AudioStreamPlayer>("Plant_Sound/Ok/Water").Stream.Set("loop", false);
        GetNode<AudioStreamPlayer>("Main/Main/Eat_Sound1").Stream.Set("loop", false);
        GetNode<AudioStreamPlayer>("Main/Main/Eat_Sound2").Stream.Set("loop", false);
        GetNode<AudioStreamPlayer>("Main/Main/Fall").Stream.Set("loop", false);
        health = 640;
        if (!player_put)
        {
            has_planted = true;
            GetNode<Control>("Dock").Hide();
            GetNode<Control>("Show").Hide();
            GetNode<Control>("Main").Show();
            Walk_Mode(true);
            Position = put_position;
            GetNode<Normal_Plants_Zombies_Area>("Main/Main/Zombies_Area").has_planted = true;
        }
    }
    public /*async*/ void Plants_Entered(Control_Area_2D area2D)
    {
        if (area2D == null)
        {
            return;
        }
        if (area2D.Area2D_type == "Zombies")
        {
            var plant_area2D = (Normal_Zombies_Area)area2D;
            if (true/*plant_area2D.plants_type == "Normal"*/)
            {
                if (has_planted)
                {
                    if (plant_area2D.has_plant)
                    {
                        Zombies_Area_2D_List.Add(plant_area2D);
                        if (Zombies_Area_2D_List[0].has_plant)
                        {
                            Top_Area_2D = Zombies_Area_2D_List[0];
                        }
                        else
                        {
                            Top_Area_2D = null;
                        }
                        for (int i = 0; i < Zombies_Area_2D_List.Count; i++)
                        {
                            if (Zombies_Area_2D_List[i].has_plant)
                            {
                                if (Top_Area_2D == null)
                                {
                                    Top_Area_2D = Zombies_Area_2D_List[i];
                                }
                                else
                                {
                                    if (true/*Top_Area_2D.plants_type == Zombies_Area_2D_List[i].plants_type*/)
                                    {
                                        if (Zombies_Area_2D_List[i].ZIndex > Top_Area_2D.ZIndex)
                                        {
                                            Top_Area_2D = Zombies_Area_2D_List[i];
                                        }
                                        else if (Zombies_Area_2D_List[i].ZIndex == Top_Area_2D.ZIndex)
                                        {
                                            if (Zombies_Area_2D_List[i].GetParent().GetParent().GetIndex() > Top_Area_2D.GetParent().GetParent().GetIndex())
                                            {
                                                Top_Area_2D = Zombies_Area_2D_List[i];
                                            }
                                        }
                                    }
                                    else
                                    {
                                        /*if (Top_Area_2D.plants_type == "Casing")
                                        {
                                            Top_Area_2D = Zombies_Area_2D_List[i];
                                        }
                                        else if (Zombies_Area_2D_List[i].plants_type == "Casing")
                                        { }
                                        else if (Top_Area_2D.plants_type == "Normal" && Zombies_Area_2D_List[i].plants_type != "Casing")
                                        {
                                            Top_Area_2D = Zombies_Area_2D_List[i];
                                        }*/
                                    }
                                }
                            }
                        }
                        GetNode<Normal_Plants_Zombies_Area>("Main/Main/Zombies_Area").Choose_Zombies_Area = Top_Area_2D;
                        if (!has_lose_pole)
                        {
                            if (!GetNode<AnimationPlayer>("Main/Main/Up").IsPlaying() && Top_Area_2D != null && !has_lose_Head)
                            {
                                Walk_Mode(false);
                                GetNode<AnimationPlayer>("Main/Main/Up").Play("Up");
                            }
                        }
                        else
                        {
                            if (Top_Area_2D != null)
                            {
                                eating = true;
                                Walk_Mode(false);
                                GetNode<AnimationPlayer>("Main/Main/Eating").Play("Eating");
                            }
                        }
                    }
                    else
                    {
                        has_touched = true;
                        Zombies_Area_2D_List.Add(plant_area2D);
                    }
                }
                else
                {
                    if (plant_area2D.has_plant)
                    {
                        has_touched = true;
                        Zombies_Area_2D_List.Add(plant_area2D);
                    }
                }
            }
        }
        /*if (area2D.Area2D_type == "Plants_Bullets" && has_planted)
        {
            await ToSignal(GetTree().CreateTimer(0.03f), "timeout");
            var bullets_area2D = (Bullets_Area)area2D;
            if (bullets_area2D.hurt_type == 2 || bullets_area2D.Choose_Zombies_Area == GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area"))
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
        if (area2D.Area2D_type == "Died_Fire")
        {
            C2H5OH_Fire_Area_2D_List.Add((C2H5OH_Died_Fire_Area)area2D);
        }
        if (area2D.Area2D_type == "Mg_Shining")
        {
            Shining_Area_2D_List.Add((Mg_Shining_Area)area2D);
        }*/
    }
    public /*async*/ void Plants_Exited(Control_Area_2D area2D)
    {
        if (area2D == null)
        {
            return;
        }
        if (area2D.Area2D_type == "Zombies")
        {
            var plant_area2D = (Normal_Zombies_Area)area2D;
            if (true/*plant_area2D.plants_type == "Normal"*/)
            {
                Zombies_Area_2D_List.Remove(plant_area2D);
                if (!this.has_planted || !plant_area2D.has_plant || Zombies_Area_2D_List.Count != 0)
                {
                    has_touched = false;
                }
                if (this.has_planted && plant_area2D.has_plant)
                {
                    Zombies_Area_2D_List.Remove(plant_area2D);
                    if (Zombies_Area_2D_List.Count == 0)
                    {
                        Top_Area_2D = null;
                        eating = false;
                        if (health >= 90 && !is_jumping)
                        {
                            Walk_Mode(true);
                            GetNode<AnimationPlayer>("Main/Main/Eating").Stop();
                        }

                    }
                    else
                    {
                        if (Zombies_Area_2D_List[0].has_plant)
                        {
                            Top_Area_2D = Zombies_Area_2D_List[0];
                        }
                        else
                        {
                            Top_Area_2D = null;
                        }
                        for (int i = 0; i < Zombies_Area_2D_List.Count; i++)
                        {
                            if (Zombies_Area_2D_List[i].has_plant)
                            {
                                if (Top_Area_2D == null)
                                {
                                    Top_Area_2D = Zombies_Area_2D_List[i];
                                }
                                else
                                {
                                    if (true/*Top_Area_2D.plants_type == Zombies_Area_2D_List[i].plants_type*/)
                                    {
                                        if (Zombies_Area_2D_List[i].ZIndex > Top_Area_2D.ZIndex)
                                        {
                                            Top_Area_2D = Zombies_Area_2D_List[i];
                                        }
                                        else if (Zombies_Area_2D_List[i].ZIndex == Top_Area_2D.ZIndex)
                                        {
                                            if (Zombies_Area_2D_List[i].GetParent().GetParent().GetIndex() > Top_Area_2D.GetParent().GetParent().GetIndex())
                                            {
                                                Top_Area_2D = Zombies_Area_2D_List[i];
                                            }
                                        }
                                    }
                                    else
                                    {
                                        /*if (Top_Area_2D.plants_type == "Casing")
                                        {
                                            Top_Area_2D = Zombies_Area_2D_List[i];
                                        }
                                        else if (Zombies_Area_2D_List[i].plants_type == "Casing")
                                        { }
                                        else if (Top_Area_2D.plants_type == "Normal" && Zombies_Area_2D_List[i].plants_type != "Casing")
                                        {
                                            Top_Area_2D = Zombies_Area_2D_List[i];
                                        }*/
                                    }
                                }
                            }
                        }
                    }
                    GetNode<Normal_Plants_Zombies_Area>("Main/Main/Zombies_Area").Choose_Zombies_Area = Top_Area_2D;
                    if (Top_Area_2D == null)
                    {
                        eating = false;
                        if (health >= 90 && !is_jumping)
                        {
                            Walk_Mode(true);
                            GetNode<AnimationPlayer>("Main/Main/Eating").Stop();
                        }
                    }
                }
            }
        }
        /*if (area2D.Area2D_type == "Plants_Bullets")
        {
            await ToSignal(GetTree().CreateTimer(0.03f), "timeout");
            var bullets_area2D = (Bullets_Area)area2D;
            if (bullets_area2D.hurt_type == 2 || bullets_area2D.Choose_Zombies_Area == GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area"))
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
        if (area2D.Area2D_type == "Died_Fire")
        {
            if (hurt_time <= 0 && !this.On_Boom_Effect)
            {
                this.Modulate = normal_color;
            }
        }
        if (area2D.Area2D_type == "Mg_Shining")
        {
            Shining_Area_2D_List.Remove((Mg_Shining_Area)area2D);
        }*/
    }
    public void Jump_Area2D_area_entered(Control_Area_2D area2D)
    {
        if (area2D == null)
        {
            return;
        }
        if (area2D.Area2D_type == "Zombies")
        {
            Jump_Plants_List.Add((Normal_Zombies_Area)area2D);
        }
    }
    public void Jump_Area2D_area_exited(Control_Area_2D area2D)
    {
        if (area2D == null)
        {
            return;
        }
        if (area2D.Area2D_type == "Zombies")
        {
            Jump_Plants_List.Remove((Normal_Zombies_Area)area2D);
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
        if (area2D.Area2D_type == "Grid")
        {
            var area2D_Grid = (Background_Grid_Main)area2D;
            in_water = area2D_Grid.type == 2;
        }
    }
    public override void _Process(float delta)
    {
        if (Public_Main.for_Android && Input.IsActionJustReleased("Left_Mouse"))
        {
            if (Android_Timer.IsStopped())
            {
                Android_Timer.Start();
            }
            else
            {
                Is_Double_Click = true;
                Android_Timer.Start();
            }
        }
        if (Android_Timer.IsStopped() && Public_Main.for_Android && !Input.IsActionJustReleased("Left_Mouse"))
        {
            Is_Double_Click = false;
        }
        if (on_lock_grid && dock_area_2d != null)
        {
            ZIndex = normal_ZIndex + 20 * (dock_area_2d.pos[0] - 1);
        }
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
                if ((Input.IsActionPressed("Left_Mouse") && !Public_Main.for_Android) || (Public_Main.for_Android && Is_Double_Click))
                {
                    card_parent_Button.Set_ColorRect_2(false);
                    Normal_Plants.Choosing = false;
                    if ((In_Game_Main.Sun_Number >= card_parent_Button.sun || Public_Main.debuging) && on_lock_grid)
                    {
                        Is_Double_Click = false;
                        has_planted = true;
                        GetNode<Normal_Plants_Zombies_Area>("Main/Main/Zombies_Area").has_planted = true;
                        dock_control.Hide();
                        this.GlobalPosition = Area_Vector2 + new Vector2(0, 10);
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
                        Walk_Mode(true);
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
            if (GetNode<Normal_Plants_Zombies_Area>("Main/Main/Zombies_Area").lose_health)
            {
                GetNode<Normal_Plants_Zombies_Area>("Main/Main/Zombies_Area").lose_health = false;
                health -= GetNode<Normal_Plants_Zombies_Area>("Main/Main/Zombies_Area").lose_health_number;
            }
            if (in_water && !GetNode<AnimationPlayer>("In_Water").IsPlaying() && !now_in_water)
            {
                GetNode<AnimationPlayer>("In_Water").Play("Water");
                now_in_water = true;
            }
            if (!in_water && !GetNode<AnimationPlayer>("Out_Water").IsPlaying() && now_in_water)
            {
                GetNode<AnimationPlayer>("Out_Water").Play("Water");
                now_in_water = false;
            }
            if (!eating && (GetNode<AnimationPlayer>("Main/Main/Walk1").IsPlaying() || GetNode<AnimationPlayer>("Main/Main/Walk2").IsPlaying()) && !On_Boom_Effect && !Is_Shining)
            {
                this.Position += new Vector2(speed * speed_x, 0);
            }
            if (On_Boom_Effect)
            {
                Walk_Mode(false);
            }
            if (has_touched && Zombies_Area_2D_List.Count != 0)
            {
                if (Zombies_Area_2D_List[0].has_plant && Zombies_Area_2D_List[0].Monitorable)
                {
                    Top_Area_2D = Zombies_Area_2D_List[0];
                }
                else
                {
                    Top_Area_2D = null;
                }
                for (int i = 0; i < Zombies_Area_2D_List.Count; i++)
                {
                    if (Zombies_Area_2D_List[i].has_plant && Zombies_Area_2D_List[0].Monitorable)
                    {
                        has_touched = false;
                        if (Top_Area_2D == null)
                        {
                            Top_Area_2D = Zombies_Area_2D_List[i];
                        }
                        else
                        {
                            if (true/*Top_Area_2D.plants_type == Zombies_Area_2D_List[i].plants_type*/)
                            {
                                if (Zombies_Area_2D_List[i].ZIndex > Top_Area_2D.ZIndex)
                                {
                                    Top_Area_2D = Zombies_Area_2D_List[i];
                                }
                                else if (Zombies_Area_2D_List[i].ZIndex == Top_Area_2D.ZIndex)
                                {
                                    if (Zombies_Area_2D_List[i].GetParent().GetParent().GetIndex() > Top_Area_2D.GetParent().GetParent().GetIndex())
                                    {
                                        Top_Area_2D = Zombies_Area_2D_List[i];
                                    }
                                }
                            }
                            else
                            {
                                /*if (Top_Area_2D.plants_type == "Casing")
                                {
                                    Top_Area_2D = Zombies_Area_2D_List[i];
                                }
                                else if (Zombies_Area_2D_List[i].plants_type == "Casing")
                                { }
                                else if (Top_Area_2D.plants_type == "Normal" && Zombies_Area_2D_List[i].plants_type != "Casing")
                                {
                                    Top_Area_2D = Zombies_Area_2D_List[i];
                                }*/
                            }
                        }
                    }
                }
                GetNode<Normal_Plants_Zombies_Area>("Main/Main/Zombies_Area").Choose_Zombies_Area = Top_Area_2D;
                if (!has_lose_pole)
                {
                    if (!GetNode<AnimationPlayer>("Main/Main/Up").IsPlaying() && Top_Area_2D != null && !has_lose_Head)
                    {
                        Walk_Mode(false);
                        GetNode<AnimationPlayer>("Main/Main/Up").Play("Up");
                    }
                }
                else
                {
                    if (Top_Area_2D != null && !has_lose_Head)
                    {
                        eating = true;
                        Walk_Mode(false);
                        GetNode<AnimationPlayer>("Main/Main/Eating").Play("Eating");
                    }
                    else if (Top_Area_2D == null && !has_lose_Head)
                    {
                        eating = false;
                        Walk_Mode(true);
                        GetNode<AnimationPlayer>("Main/Main/Eating").Stop();
                    }
                }
            }
            if (Top_Area_2D != null)
            {
                if (!Top_Area_2D.Monitorable)
                {
                    Zombies_Area_2D_List.Remove(Top_Area_2D);
                    if (Zombies_Area_2D_List.Count != 0)
                    {
                        if (Zombies_Area_2D_List[0].has_plant)
                        {
                            Top_Area_2D = Zombies_Area_2D_List[0];
                        }
                        else
                        {
                            Top_Area_2D = null;
                        }
                        for (int i = 0; i < Zombies_Area_2D_List.Count; i++)
                        {
                            if (Zombies_Area_2D_List[i].has_plant)
                            {
                                has_touched = false;
                                if (Top_Area_2D == null)
                                {
                                    Top_Area_2D = Zombies_Area_2D_List[i];
                                }
                                else
                                {
                                    if (true/*Top_Area_2D.plants_type == Zombies_Area_2D_List[i].plants_type*/)
                                    {
                                        if (Zombies_Area_2D_List[i].ZIndex > Top_Area_2D.ZIndex)
                                        {
                                            Top_Area_2D = Zombies_Area_2D_List[i];
                                        }
                                        else if (Zombies_Area_2D_List[i].ZIndex == Top_Area_2D.ZIndex)
                                        {
                                            if (Zombies_Area_2D_List[i].GetParent().GetParent().GetIndex() > Top_Area_2D.GetParent().GetParent().GetIndex())
                                            {
                                                Top_Area_2D = Zombies_Area_2D_List[i];
                                            }
                                        }
                                    }
                                    else
                                    {
                                        /*if (Top_Area_2D.plants_type == "Casing")
                                        {
                                            Top_Area_2D = Zombies_Area_2D_List[i];
                                        }
                                        else if (Zombies_Area_2D_List[i].plants_type == "Casing")
                                        { }
                                        else if (Top_Area_2D.plants_type == "Normal" && Zombies_Area_2D_List[i].plants_type != "Casing")
                                        {
                                            Top_Area_2D = Zombies_Area_2D_List[i];
                                        }*/
                                    }
                                }
                            }
                        }
                        GetNode<Normal_Plants_Zombies_Area>("Main/Main/Zombies_Area").Choose_Zombies_Area = Top_Area_2D;
                        if (!has_lose_pole)
                        {
                            if (!GetNode<AnimationPlayer>("Main/Main/Up").IsPlaying() && Top_Area_2D != null && !has_lose_Head)
                            {
                                Walk_Mode(false);
                                GetNode<AnimationPlayer>("Main/Main/Up").Play("Up");
                            }
                        }
                        else
                        {
                            if (Top_Area_2D != null && !has_lose_Head)
                            {
                                eating = true;
                                Walk_Mode(false);
                                GetNode<AnimationPlayer>("Main/Main/Eating").Play("Eating");
                            }
                        }
                    }
                    else
                    {
                        Top_Area_2D = null;
                        if (health >= 90 && !is_jumping)
                        {
                            eating = false;
                            Walk_Mode(true);
                            GetNode<AnimationPlayer>("Main/Main/Eating").Stop();
                        }
                    }
                }
            }
            if (Zombies_Area_2D_List.Count != 0)
            {
                for (int i = 0; i < Zombies_Area_2D_List.Count; i++)
                {
                    if (Zombies_Area_2D_List[i].Choose_Plants_Area == GetNode<Normal_Plants_Area>("Main/Main/Zombies_Area"))
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
            if (health < 340 && !has_lose_Arm)
            {
                has_lose_Arm = true;
                GetNode<AnimationPlayer>("Main/Main/Lose_Arm").Play("Lose_Arm");
            }
            if (health < 90 && !has_lose_Head)
            {
                has_lose_Head = true;
                GetNode<Normal_Plants_Zombies_Area>("Main/Main/Zombies_Area").has_lose_head = true;
                GetNode<AnimationPlayer>("Main/Main/Lose_Head").Play("Lose_Head");
            }
            if (Boom_Area_2D_List.Count != 0)
            {
                for (int i = 0; i < Boom_Area_2D_List.Count; i++)
                {
                    if (Boom_Area_2D_List[i].can_do && !Boom_Area_2D_List[i].end_hurt)
                    {
                        if (Boom_Area_2D_List[i].hurt >= health - 90)
                        {
                            On_Boom_Effect = true;
                            Walk_Mode(false);
                            GetNode<AnimationPlayer>("Main/Main/Eating").Stop();
                            GetNode<AnimationPlayer>("Main/Main/Boom_Effect").Play("Boom_Effect");
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
            if (C2H5OH_Fire_Area_2D_List.Count != 0)
            {
                if (GetNode<Timer>("Main/Main/Fire_Hurt").IsStopped())
                {
                    for (int i = 0; i < C2H5OH_Fire_Area_2D_List.Count; i++)
                    {
                        if (C2H5OH_Fire_Area_2D_List[i].died)
                        {
                            health -= C2H5OH_Fire_Area_2D_List[i].hurt;
                            if (!this.On_Boom_Effect)
                            {
                                this.Modulate = hurt_color;
                            }
                        }
                    }
                    GetNode<Timer>("Main/Main/Fire_Hurt").Start();
                }
                else
                {
                    if (hurt_time <= 0 && !this.On_Boom_Effect)
                    {
                        this.Modulate = normal_color;
                    }
                }
            }
            if (Shining_Area_2D_List.Count != 0)
            {
                if (GetNode<Timer>("Main/Main/Fire_Hurt").IsStopped())
                {
                    for (int i = 0; i < Shining_Area_2D_List.Count; i++)
                    {
                        if (Shining_Area_2D_List[i].start)
                        {
                            Is_Shining = true;
                            health -= 5;
                            if (!this.On_Boom_Effect)
                            {
                                this.Modulate = hurt_color;
                            }
                        }
                    }
                    GetNode<Timer>("Main/Main/Fire_Hurt").Start();
                }
                else
                {
                    if (hurt_time <= 0 && !this.On_Boom_Effect)
                    {
                        this.Modulate = normal_color;
                    }
                }
            }
            else
            {
                Is_Shining = false;
            }
            if (!has_lose_pole && !is_jumping && !GetNode<AnimationPlayer>("Main/Main/Up").IsPlaying() && Jump_Plants_List != null)
            {
                if (Jump_Plants_List.Count != 0)
                {
                    bool can_work = false;
                    for (int i = 0; i < Jump_Plants_List.Count; i++)
                    {
                        if (Jump_Plants_List[i] != null)
                        {
                            if (Jump_Plants_List[i].has_plant)
                            {
                                can_work = true;
                                break;
                            }
                        }
                    }
                    if (can_work)
                    {
                        if (!GetNode<AnimationPlayer>("Main/Main/Up").IsPlaying() && !has_lose_Head)
                        {
                            Walk_Mode(false);
                            GetNode<AnimationPlayer>("Main/Main/Up").Play("Up");
                        }
                    }
                }
            }
        }
        if (Position.x > 1437)
        {
            this.QueueFree();
        }
    }
    public void Walk_Mode(bool is_Walking)
    {
        if (is_Walking)
        {
            if (has_lose_pole)
            {
                GetNode<AnimationPlayer>("Main/Main/Walk1").Stop();
                GetNode<AnimationPlayer>("Main/Main/Walk2").Play("Walk");
            }
            else
            {
                GetNode<AnimationPlayer>("Main/Main/Walk1").Play("Walk");
                GetNode<AnimationPlayer>("Main/Main/Walk2").Stop();
            }
        }
        else
        {
            GetNode<AnimationPlayer>("Main/Main/Walk1").Stop();
            GetNode<AnimationPlayer>("Main/Main/Walk2").Stop();
        }
    }
}
