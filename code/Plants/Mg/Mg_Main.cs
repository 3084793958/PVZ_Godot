using Godot;
using System;
using System.Collections.Generic;

public class Mg_Main : Normal_Plants
{
    public List<C2H5OH_Bullets_Fire_Area> C2H5OH_Fire_Area_2D_List = new List<C2H5OH_Bullets_Fire_Area>();
    public override void _Ready()
    {
        this.AddChild(Android_Timer);
        Android_Timer.WaitTime = 0.3f;
        Android_Timer.Autostart = false;
        Android_Timer.OneShot = true;
        GetNode<AudioStreamPlayer>("Burn").Stream.Set("loop", false);
        GetNode<AudioStreamPlayer>("Plant_Sound/Ok/Plant1").Stream.Set("loop", false);
        GetNode<AudioStreamPlayer>("Plant_Sound/Ok/Plant2").Stream.Set("loop", false);
        GetNode<AudioStreamPlayer>("Plant_Sound/Ok/Water").Stream.Set("loop", false);
        GetNode<AudioStreamPlayer>("Big_Chmop").Stream.Set("loop", false);
        health = 100;
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
    public async void Shovel_Area_Entered(Control_Area_2D area2D)
    {
        if (has_planted && area2D.Area2D_type == "Shovel")
        {
            Shovel_Area = (Shovel_Area2D)area2D;
            if (Shovel_Area != null)
            {
                await ToSignal(GetTree(), "idle_frame");//保险
                if (Shovel_Area.Choose_Plants_Area == GetNode<Normal_Plants_Area>("Main/Shovel_Area"))
                {
                    this.Modulate = hover_color;
                    on_Shovel = true;
                }
            }
        }
        if (has_planted && area2D.Area2D_type == "Bug")
        {
            Bug_Area = (Bug_Area2D)area2D;
            if (Bug_Area != null)
            {
                await ToSignal(GetTree(), "idle_frame");//保险
                if (Bug_Area.Choose_Plants_Area == GetNode<Normal_Plants_Area>("Main/Shovel_Area"))
                {
                    this.Modulate = hover_color;
                    on_Bug = true;
                }
            }
        }
    }
    public void Shovel_Area_Exited(Control_Area_2D area2D)
    {
        if (has_planted && area2D.Area2D_type == "Shovel")
        {
            if (Shovel_Area != null)
            {
                this.Modulate = normal_color;
                Shovel_Area = null;
                on_Shovel = false;
            }
        }
        if (has_planted && area2D.Area2D_type == "Bug")
        {
            if (Bug_Area != null)
            {
                this.Modulate = normal_color;
                Bug_Area = null;
                on_Bug = false;
            }
        }
    }
    public void Area_Entered(Control_Area_2D area2D)
    {
        if (area2D.Area2D_type == "Zombies")
        {
            Zombies_Area_2D = (Normal_Zombies_Area)area2D;
            if (Zombies_Area_2D.can_hurt)
            {
                Zombies_Area_2D_List.Add(Zombies_Area_2D);
            }
        }
        if (area2D.Area2D_type == "Bullets_Fire")
        {
            C2H5OH_Fire_Area_2D_List.Add((C2H5OH_Bullets_Fire_Area)area2D);
        }
    }
    public void Area_Exited(Control_Area_2D area2D)
    {
        if (area2D.Area2D_type == "Zombies")
        {
            Zombies_Area_2D_List.Remove(Zombies_Area_2D);
            Zombies_Area_2D = null;
        }
        if (area2D.Area2D_type == "Bullets_Fire")
        {
            C2H5OH_Fire_Area_2D_List.Remove((C2H5OH_Bullets_Fire_Area)area2D);
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
        if (!has_planted)
        {
            if (Normal_Plants.Choosing)
            {
                Show();
                this.GlobalPosition = GetTree().Root.GetMousePosition();
                var dock_control = GetNode<Control>("Dock");
                if (Math.Abs(GlobalPosition.x - Area_Vector2.x) < 40 && Math.Abs(GlobalPosition.y - Area_Vector2.y) < 40)
                {
                    dock_control.SetGlobalPosition(Area_Vector2 - new Vector2(40, 40));
                    on_lock_grid = true;
                }
                else
                {
                    dock_control.SetPosition(new Vector2(-40, -40));
                    on_lock_grid = false;
                }
                if (Input.IsActionPressed("Right_Mouse"))
                {
                    Normal_Plants.Choosing = false;
                    Hide();
                    Position = new Vector2(-40, -40);
                    GetNode<AudioStreamPlayer>("/root/In_Game/Cancel").Play();
                    card_parent_Button.Set_ColorRect_2(false);
                    this.QueueFree();
                }
                if ((Input.IsActionPressed("Left_Mouse")&&!Public_Main.for_Android)||(Public_Main.for_Android&&Is_Double_Click))
                {
                    card_parent_Button.Set_ColorRect_2(false);
                    Normal_Plants.Choosing = false;
                    if (on_lock_grid && ((In_Game_Main.Sun_Number>=card_parent_Button.sun&&dock_area_2d.Top_Plant_List.Count == 0 && dock_area_2d.type == 1) || Public_Main.debuging))
                    {
                        Is_Double_Click = false;
                        has_planted = true;
                        dock_area_2d.Top_Plant_List.Add(this);
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
                        GetNode<Control>("Dock").Hide();
                        GetNode<Control>("Show").Hide();
                        GetNode<Control>("Main").Show();
                        if (C2H5OH_Fire_Area_2D_List.Count != 0)
                        {
                            bool has_fire = false;
                            for (int i = 0; i < C2H5OH_Fire_Area_2D_List.Count; i++)
                            {
                                if (C2H5OH_Fire_Area_2D_List[i].can_work)
                                {
                                    has_fire = true;
                                    break;
                                }
                            }
                            if (has_fire)
                            {
                                GetNode<AnimationPlayer>("Main/Player2").Play("Player1");
                            }
                            else
                            {
                                GetNode<AnimationPlayer>("Main/Player1").Play("Player1");
                            }
                        }
                        else
                        {
                            GetNode<AnimationPlayer>("Main/Player1").Play("Player1");
                        }
                        GetNode<Normal_Plants_Area>("Main/Shovel_Area").has_planted = this.has_planted;
                        GetNode<Normal_Plants_Area>("Main/Touch_Area").has_planted = this.has_planted;
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
            if (on_Shovel && Input.IsActionPressed("Left_Mouse"))
            { 
                if (!GetNode<AnimationPlayer>("Free_player").IsPlaying())
                {
                    if (Shovel_Area != null)
                    {
                        if (Shovel_Area.Choose_Plants_Area == GetNode<Normal_Plants_Area>("Main/Touch_Area"))
                        {
                            Is_Double_Click = false;
                            GetNode<AnimationPlayer>("Free_player").Play("Free");
                            dock_area_2d.Top_Plant_List.Remove(this);
                        }
                    }
                }
            }
            if (on_Bug && Input.IsActionPressed("Left_Mouse"))
            {
                //NO_WAY
            }
            if (Zombies_Area_2D_List.Count != 0)
            {
                for (int i=0;i<Zombies_Area_2D_List.Count;i++)
                {
                    if (Zombies_Area_2D_List[i].Choose_Plants_Area == GetNode<Normal_Plants_Area>("Main/Touch_Area"))
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
            if (health <= 0)
            {
                dock_area_2d.Top_Plant_List.Remove(this);
            }
        }
    }
}