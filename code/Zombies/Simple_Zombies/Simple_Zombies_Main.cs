using Godot;
using System;

public class Simple_Zombies_Main : Normal_Zombies
{
    private bool has_lose_Arm = false;
    private bool has_lose_Head = false;
    private bool has_Lose_Number = false;
    public float speed = -1.5f*(float)GD.RandRange(0.1,0.2);
    public bool eating = false;
    public int hurt_time = 0;
    private bool in_water = false;
    private bool now_in_water = false;
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
        health = 270;
        if (!player_put)
        {
            has_planted = true;
            GetNode<Control>("Dock").Hide();
            GetNode<Control>("Show").Hide();
            GetNode<Control>("Main").Show();
            GetNode<AnimationPlayer>("Main/Main/Walk").Play("Walk");
            Position = put_position;
            GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area").has_plant = true;
            In_Game_Main.Zombies_Number++;
        }
    }
    public async void Plants_Entered(Control_Area_2D area2D)
    {
        if (has_lose_Head)
        {
            return;
        }
        if (area2D == null)
        {
            return;
        }
        if (area2D.Area2D_type == "Plants")
        {
            var plant_area2D = (Normal_Plants_Area)area2D;
            if (true/*plant_area2D.plants_type == "Normal"*/)
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
                                    if (Plants_Area_2D_List[i].Sec_Info == "Zombies")
                                    {
                                        Top_Area_2D = Plants_Area_2D_List[i];
                                    }
                                    else
                                    {
                                        if (Top_Area_2D.plants_type == Plants_Area_2D_List[i].plants_type)
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
                                        else
                                        {
                                            int Top_Area_2D_Plants_type=1;
                                            int List_Plants_type=1;
                                            if (Top_Area_2D.plants_type == "Casing")
                                            {
                                                Top_Area_2D_Plants_type = 4;
                                            }
                                            else if (Top_Area_2D.plants_type == "Top")
                                            {
                                                Top_Area_2D_Plants_type = 3;
                                            }
                                            else if (Top_Area_2D.plants_type == "Normal")
                                            {
                                                Top_Area_2D_Plants_type = 2;
                                            }
                                            else if (Top_Area_2D.plants_type == "Down")
                                            {
                                                Top_Area_2D_Plants_type = 1;
                                            }
                                            if (Plants_Area_2D_List[i].plants_type == "Casing")
                                            {
                                                List_Plants_type = 4;
                                            }
                                            else if (Plants_Area_2D_List[i].plants_type == "Top")
                                            {
                                                List_Plants_type = 3;
                                            }
                                            else if (Plants_Area_2D_List[i].plants_type == "Normal")
                                            {
                                                List_Plants_type = 2;
                                            }
                                            else if (Plants_Area_2D_List[i].plants_type == "Down")
                                            {
                                                List_Plants_type = 1;
                                            }
                                            if (List_Plants_type > Top_Area_2D_Plants_type)
                                            {
                                                Top_Area_2D = Plants_Area_2D_List[i];
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area").Choose_Plants_Area = Top_Area_2D;
                        if (Top_Area_2D != null)
                        {
                            eating = true;
                            GetNode<AnimationPlayer>("Main/Main/Walk").Stop();
                            GetNode<AnimationPlayer>("Main/Main/Eating").Play("Eating");
                        }
                    }
                    else
                    {
                        Plants_Area_2D_List.Add(plant_area2D);
                    }
                }
                else
                {
                    if (plant_area2D.has_planted)
                    {
                        Plants_Area_2D_List.Add(plant_area2D);
                    }
                }
            }
        }
        if (area2D.Area2D_type == "Plants_Bullets" && has_planted)
        {
            await ToSignal(GetTree().CreateTimer(0.03f), "timeout");
            if (area2D == null)
            {
                return;
            }
            if (area2D.Sec_Info=="Pea")
            {
                var bullets_area2D = (Bullets_Area)area2D;
                if (bullets_area2D.hurt_type == 2 || bullets_area2D.Choose_Zombies_Area == GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area"))
                {
                    hurt_time++;
                    if (bullets_area2D.Bullets_Type == 1)
                    {
                        if (is_Ice)
                        {
                            this.Modulate = Ice_hurt_color;
                        }
                        else
                        {
                            this.Modulate = hurt_color;
                        }
                    }
                    else
                    {
                        this.Modulate = hurt_color;
                    }
                    health -= bullets_area2D.hurt;
                }
            }
            else if (area2D.Sec_Info == "Ice_Pea")
            {
                var bullets_area2D = (Ice_Bullets_Area)area2D;
                if (bullets_area2D.hurt_type == 2 || bullets_area2D.Choose_Zombies_Area == GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area"))
                {
                    hurt_time++;
                    this.Modulate = Ice_hurt_color;
                    health -= bullets_area2D.hurt;
                    is_Ice = true;
                    if (bullets_area2D.Bullets_Type == 3)
                    {
                        is_Lock_Ice = true;
                        GetNode<Sprite>("Main/Main/Ice_Lock").Show();
                        speed_x = 0f;
                        GetNode<Timer>("Ice_Lock_Timer").Start();
                    }
                    GetNode<Timer>("Ice_Timer").Start();
                }
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
        }
        if (area2D.Area2D_type == "H2SO4")
        {
            H2SO4_Area_2D_List.Add((H2SO4_Area2D)area2D);
        }
        if (area2D.Area2D_type == "Crash_Hurt")
        {
            Crash_Area_2D_List.Add((Crash_Area_2D)area2D);
        }
        if (area2D.Area2D_type == "Eating_Flower")
        {
            Eating_Flower_Area_2D_List.Add((Eating_Flower_Area)area2D);
        }
        if (area2D.Area2D_type == "Car")
        {
            Car_Area_2D_List.Add((Car_Area2D)area2D);
        }
    }
    public async void Plants_Exited(Control_Area_2D area2D)
    {
        if (area2D == null)
        {
            return;
        }
        if (area2D.Area2D_type == "Plants")
        {
            var plant_area2D = (Normal_Plants_Area)area2D;
            if (true/*plant_area2D.plants_type == "Normal"*/)
            {
                Plants_Area_2D_List.Remove(plant_area2D);
                /*if (!this.has_planted || !plant_area2D.has_planted || Plants_Area_2D_List.Count != 0) 
                {
                    has_touched = false;
                }*/
                if (this.has_planted)
                {
                    if (Plants_Area_2D_List.Count == 0)
                    {
                        Top_Area_2D = null;
                        eating = false;
                        if (health >= 90)
                        {
                            GetNode<AnimationPlayer>("Main/Main/Walk").Play("Walk");
                            GetNode<AnimationPlayer>("Main/Main/Eating").Stop();
                        }
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
                                    if (Plants_Area_2D_List[i].Sec_Info == "Zombies")
                                    {
                                        Top_Area_2D = Plants_Area_2D_List[i];
                                    }
                                    else
                                    {
                                        if (Top_Area_2D.plants_type == Plants_Area_2D_List[i].plants_type)
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
                                        else
                                        {
                                            if (Top_Area_2D.plants_type == "Casing")
                                            {
                                                Top_Area_2D = Plants_Area_2D_List[i];
                                            }
                                            else if (Plants_Area_2D_List[i].plants_type == "Casing")
                                            { }
                                            else if (Top_Area_2D.plants_type == "Normal" && Plants_Area_2D_List[i].plants_type != "Casing")
                                            {
                                                Top_Area_2D = Plants_Area_2D_List[i];
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area").Choose_Plants_Area = Top_Area_2D;
                    if (Top_Area_2D == null)
                    {
                        eating = false;
                        if (health >= 90)
                        {
                            GetNode<AnimationPlayer>("Main/Main/Walk").Play("Walk");
                            GetNode<AnimationPlayer>("Main/Main/Eating").Stop();
                        }
                    }
                }
            }
        }
        if (area2D.Area2D_type == "Plants_Bullets")
        {
            await ToSignal(GetTree().CreateTimer(0.03f), "timeout");
            if (area2D == null)
            {
                return;
            }
            if (area2D.Sec_Info == "Pea")
            {
                var bullets_area2D = (Bullets_Area)area2D;
                if (bullets_area2D.hurt_type == 2 || bullets_area2D.Choose_Zombies_Area == GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area"))
                {
                    hurt_time--;
                    if (hurt_time == 0)
                    {
                        if (bullets_area2D.Bullets_Type == 1)
                        {
                            if (is_Ice)
                            {
                                this.Modulate = Ice_normal_color;
                            }
                            else
                            {
                                this.Modulate = normal_color;
                            }
                        }
                        else
                        {
                            this.Modulate = normal_color;
                        }
                    }
                    else if (hurt_time < 0)
                    {
                        hurt_time = 0;
                    }
                    if (bullets_area2D.Bullets_Type != 1)
                    {
                        is_Ice = false;
                        is_Lock_Ice = false;
                    }
                }
            }
            else if (area2D.Sec_Info == "Ice_Pea")
            {
                var bullets_area2D = (Ice_Bullets_Area)area2D;
                if (bullets_area2D.hurt_type == 2 || bullets_area2D.Choose_Zombies_Area == GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area"))
                {
                    hurt_time--;
                    if (hurt_time == 0)
                    {
                        this.Modulate = Ice_normal_color;
                    }
                    else if (hurt_time < 0)
                    {
                        hurt_time = 0;
                    }
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
        }
        if (area2D.Area2D_type == "H2SO4")
        {
            H2SO4_Area_2D_List.Remove((H2SO4_Area2D)area2D);
        }
        if (area2D.Area2D_type == "Crash_Hurt")
        {
            Crash_Area_2D_List.Remove((Crash_Area_2D)area2D);
        }
        if (area2D.Area2D_type == "Plants_Boom")
        {
            var Boom_area2D = (Normal_Boom_Area)area2D;
            Boom_Area_2D_List.Remove(Boom_area2D);
        }
        if (area2D.Area2D_type == "Eating_Flower")
        {
            Eating_Flower_Area_2D_List.Remove((Eating_Flower_Area)area2D);
        }
        if (area2D.Area2D_type == "Car")
        {
            Car_Area_2D_List.Remove((Car_Area2D)area2D);
        }
    }
    public void Dock_Enter(Control_Area_2D area2D)
    {
        if (has_lose_Head)
        {
            return;
        }
        if (area2D == null)
        {
            return;
        }
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
                    this.Free();
                }
                if ((Input.IsActionPressed("Left_Mouse") && !Public_Main.for_Android) || (Public_Main.for_Android && Is_Double_Click))
                {
                    card_parent_Button.Set_ColorRect_2(false);
                    Normal_Plants.Choosing = false;
                    if ((In_Game_Main.Sun_Number >= card_parent_Button.sun || Public_Main.debuging) && on_lock_grid)
                    {
                        Is_Double_Click = false;
                        has_planted = true;
                        GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area").has_plant = true;
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
                        GetNode<AnimationPlayer>("Main/Main/Walk").Play("Walk");
                        In_Game_Main.Zombies_Number++;
                    }
                    else
                    {
                        Hide();
                        Position = new Vector2(-40, -40);
                        GetNode<AudioStreamPlayer>("/root/In_Game/Cancel").Play();
                        this.Free();
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
            if (Position.x < -1437 / 2)
            {
                this.Free();
                this.Remove_Zombies_Number();
            }
            if (!In_Game_Main.has_Lost_Brain && Position.x < -20 && !has_lose_Head)
            {
                In_Game_Main.Lost_Brain = true;
            }
            if (GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area").lose_health)
            {
                GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area").lose_health = false;
                health -= GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area").lose_health_number;
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
            if (!eating && GetNode<AnimationPlayer>("Main/Main/Walk").IsPlaying() && !On_Boom_Effect && !Is_Shining && !is_Lock_Ice) 
            {
                this.Position += new Vector2(speed * speed_x, 0);
            }
            if (is_Lock_Ice)
            {
                GetNode<AnimationPlayer>("Main/Main/Walk").Stop();
                GetNode<AnimationPlayer>("Main/Main/Eating").Stop();
            }
            if (On_Boom_Effect)
            {
                health = -1437;
                GetNode<AnimationPlayer>("Main/Main/Walk").Stop();
            }
            if (true/*Top_Area_2D != null*//*无效*/)
            {
                if (Plants_Area_2D_List != null)
                {
                    if (Plants_Area_2D_List.Count != 0)
                    {
                        if (Plants_Area_2D_List[0] != null)
                        {
                            if (Plants_Area_2D_List[0].has_planted && Plants_Area_2D_List[0].Monitorable)
                            {
                                Top_Area_2D = Plants_Area_2D_List[0];
                            }
                            else
                            {
                                Top_Area_2D = null;
                            }
                        }
                        for (int i = 0; i < Plants_Area_2D_List.Count; i++)
                        {
                            if (Plants_Area_2D_List[i] != null)
                            {
                                if (Plants_Area_2D_List[i].has_planted && Plants_Area_2D_List[i].Monitorable)
                                {
                                    //has_touched = false;时代余辉
                                    if (Top_Area_2D == null)
                                    {
                                        Top_Area_2D = Plants_Area_2D_List[i];
                                    }
                                    else
                                    {
                                        if (Plants_Area_2D_List[i].Sec_Info == "Zombies")
                                        {
                                            Top_Area_2D = Plants_Area_2D_List[i];
                                        }
                                        else
                                        {
                                            if (Top_Area_2D.plants_type == Plants_Area_2D_List[i].plants_type)
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
                                            else
                                            {
                                                if (Top_Area_2D.plants_type == "Casing")
                                                {
                                                    Top_Area_2D = Plants_Area_2D_List[i];
                                                }
                                                else if (Plants_Area_2D_List[i].plants_type == "Casing")
                                                { }
                                                else if (Top_Area_2D.plants_type == "Normal" && Plants_Area_2D_List[i].plants_type != "Casing")
                                                {
                                                    Top_Area_2D = Plants_Area_2D_List[i];
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area").Choose_Plants_Area = Top_Area_2D;
                        if (Top_Area_2D != null && !has_lose_Head)
                        {
                            eating = true;
                            GetNode<AnimationPlayer>("Main/Main/Walk").Stop();
                            GetNode<AnimationPlayer>("Main/Main/Eating").Play("Eating");
                        }
                        else if (Top_Area_2D == null && !has_lose_Head)
                        {
                            eating = false;
                            GetNode<AnimationPlayer>("Main/Main/Walk").Play("Walk");
                            GetNode<AnimationPlayer>("Main/Main/Eating").Stop();
                        }
                    }
                    else
                    {
                        Top_Area_2D = null;
                        if (health >= 90)
                        {
                            eating = false;
                            GetNode<AnimationPlayer>("Main/Main/Walk").Play("Walk");
                            GetNode<AnimationPlayer>("Main/Main/Eating").Stop();
                        }
                    }
                }
            }
            if (Plants_Area_2D_List.Count != 0)
            {
                for (int i = 0; i < Plants_Area_2D_List.Count; i++)
                {
                    if (Plants_Area_2D_List[i].Sec_Info == "Zombies")
                    {
                        var Plants_Zombies = (Normal_Plants_Zombies_Area)Plants_Area_2D_List[i];
                        if (Plants_Zombies.Choose_Zombies_Area == GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area"))
                        {
                            if (Plants_Zombies.is_eating_show && !Plants_Zombies.has_lose_head)
                            {
                                Plants_Zombies.is_eating_show = false;
                                health -= Plants_Zombies.hurt;
                                GetNode<AnimationPlayer>("Is_Eated").Play("Is_Eated");
                            }
                        }
                    }
                }
            }
            if (health < 180 && !has_lose_Arm)
            {
                has_lose_Arm = true;
                GetNode<AnimationPlayer>("Main/Main/Lose_Arm").Play("Lose_Arm");
            }
            if (health < 90 && !has_lose_Head)
            {
                has_lose_Head = true;
                GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area").has_lose_head = true;
                if (is_Ice)
                {
                    GetNode<AnimationPlayer>("Main/Main/Lose_Head_ICE").Play("Lose_Head");
                }
                else
                {
                    GetNode<AnimationPlayer>("Main/Main/Lose_Head").Play("Lose_Head");
                }
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
                            GetNode<AnimationPlayer>("Main/Main/Walk").Stop();
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
                                is_Ice = false;
                                is_Lock_Ice = false;
                            }
                        }
                    }
                    GetNode<Timer>("Main/Main/Fire_Hurt").Start();
                }
                else
                {
                    if (hurt_time <= 0 && !this.On_Boom_Effect)
                    {
                        if (is_Ice)
                        {
                            this.Modulate = Ice_normal_color;
                        }
                        else
                        {
                            this.Modulate = normal_color;
                        }
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
                                if (is_Ice)
                                {
                                    this.Modulate = Ice_hurt_color;
                                }
                                else
                                {
                                    this.Modulate = hurt_color;
                                }
                            }
                        }
                    }
                    GetNode<Timer>("Main/Main/Fire_Hurt").Start();
                }
                else
                {
                    if (hurt_time <= 0 && !this.On_Boom_Effect)
                    {
                        if (is_Ice)
                        {
                            this.Modulate = Ice_normal_color;
                        }
                        else
                        {
                            this.Modulate = normal_color;
                        }
                    }
                }
            }
            else
            {
                Is_Shining = false;
            }
            if (H2SO4_Area_2D_List.Count != 0)
            {
                for (int i = 0; i < H2SO4_Area_2D_List.Count; i++)
                {
                    if (H2SO4_Area_2D_List[i].has_died && !H2SO4_Area_2D_List[i].has_become)
                    {
                        health -= H2SO4_Area_2D_List[i].hurt;
                        H2SO4_Area_2D_List.RemoveAt(i);
                        i--;
                    }
                }
            }
            if (Crash_Area_2D_List.Count != 0)
            {
                for (int i = 0; i < Crash_Area_2D_List.Count; i++)
                {
                    if (Crash_Area_2D_List[i].has_planted && Crash_Area_2D_List[i].Crash_Area == GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area"))
                    {
                        health -= Crash_Area_2D_List[i].hurt;
                        if (!this.On_Boom_Effect)
                        {
                            GetNode<AnimationPlayer>("Is_Eated").Play("Is_Eated");
                        }
                        Crash_Area_2D_List.RemoveAt(i);
                        i--;
                    }
                }
            }
            GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area").health = this.health;
            if (Eating_Flower_Area_2D_List.Count != 0)
            {
                for (int i = 0; i < Eating_Flower_Area_2D_List.Count; i++)
                {
                    if (Eating_Flower_Area_2D_List[i].Choose_Eating_Zombies_Area == GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area") && has_planted && Eating_Flower_Area_2D_List[i].can_eat)
                    {
                        if (!GetNode<AnimationPlayer>("Be_Eated").IsPlaying())
                        {
                            GetNode<AnimationPlayer>("Be_Eated").Play("Eat");
                        }
                        break;
                    }
                }
            }
            if (Car_Area_2D_List.Count != 0)
            {
                if (this.has_planted)
                {
                    this.has_lose_Head = true;
                    this.eating = false;
                    this.speed = 0f;
                    GetNode<AnimationPlayer>("Main/Main/Walk").Stop();
                    GetNode<AnimationPlayer>("Main/Main/Eating").Stop();
                    GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area").has_lose_head = true;
                    if (is_Ice)
                    {
                        this.Modulate = this.Ice_hurt_color;
                        GetNode<AnimationPlayer>("Main/Main/Lose_Head_ICE").Play("Lose_Head");
                    }
                    else
                    {
                        this.Modulate = this.hurt_color;
                        GetNode<AnimationPlayer>("Main/Main/Lose_Head").Play("Lose_Head");
                    }
                }
            }
            if (!has_lose_Head)
            {
                if (this.Modulate == normal_color)
                {
                    is_Ice = false;
                    speed_x = 1f;
                }
                else if (this.Modulate == Ice_normal_color)
                {
                    is_Ice = true;
                    if (is_Lock_Ice)
                    {
                        speed_x = 0f;
                    }
                    else
                    {
                        speed_x = 0.35f;
                    }
                }
            }
        }
    }
    public void Ice_Timer_timeout()
    {
        if (!has_lose_Head)
        {
            is_Ice = false;
            this.Modulate = normal_color;
        }
    }
    public void Remove_Zombies_Number()
    {
        if (!has_Lose_Number)
        {
            has_Lose_Number = true;
            In_Game_Main.Zombies_Number--;
            In_Game_Main.Last_Zombies_Pos = this.Position;
        }
    }
    public void Ice_Lock_Timer_timeout()
    {
        if (!has_lose_Head)
        {
            is_Lock_Ice = false;
            GetNode<Sprite>("Main/Main/Ice_Lock").Hide();
        }
    }
    public void Free_Self()
    {
        this.QueueFree();
    }
}
