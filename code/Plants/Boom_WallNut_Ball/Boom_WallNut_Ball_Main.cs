using Godot;
using System;

public class Boom_WallNut_Ball_Main : Normal_Plants
{
    public float speed_x = 0, speed_y = 0;
    public bool first_Speed_Y = true;
    public int touch_ZIndex = 0;
    public override void _Ready()
    {
        this.AddChild(Android_Timer);
        Android_Timer.WaitTime = 0.3f;
        Android_Timer.Autostart = false;
        Android_Timer.OneShot = true;
        GetNode<AudioStreamPlayer>("Plant_Sound/Ok/Plant1").Stream.Set("loop", false);
        GetNode<AudioStreamPlayer>("Plant_Sound/Ok/Plant2").Stream.Set("loop", false);
        GetNode<AudioStreamPlayer>("Plant_Sound/Ok/Water").Stream.Set("loop", false);
        GetNode<AudioStreamPlayer>("Big_Chmop").Stream.Set("loop", false);
        health = 4000;
        speed_x = 6.5f;
    }
    public void Dock_Enter(Control_Area_2D area2D)
    {
        if (!has_planted && area2D.Area2D_type == "Grid")
        {
            var area2D_Grid = (Background_Grid_Main)area2D;
            Area_Vector2 = area2D_Grid.GlobalPosition;
            dock_area_2d = area2D_Grid;
        }
        else if (area2D.Area2D_type == "Grid")
        {
            dock_area_2d = (Background_Grid_Main)area2D;
        }
    }
    public async void Area_Entered(Control_Area_2D area2D)
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
        if (area2D.Area2D_type == "Zombies")
        {
            Zombies_Area_2D = (Normal_Zombies_Area)area2D;
            if (Zombies_Area_2D.can_hurt)
            {
                Zombies_Area_2D_List.Add(Zombies_Area_2D);
            }
            /*if (Zombies_Area_2D.has_plant)
            {
                if (GetNode<Crash_Area_2D>("Main/Crash_Area").Crash_Area == null)
                {
                    GetNode<Crash_Area_2D>("Main/Crash_Area").Crash_Area = Zombies_Area_2D;
                }
                else
                {
                    if (Zombies_Area_2D.ZIndex > GetNode<Crash_Area_2D>("Main/Crash_Area").Crash_Area.ZIndex || (Zombies_Area_2D.ZIndex == GetNode<Crash_Area_2D>("Main/Crash_Area").Crash_Area.ZIndex && Zombies_Area_2D.GetParent().GetParent().GetIndex() > GetNode<Crash_Area_2D>("Main/Crash_Area").Crash_Area.GetParent().GetParent().GetIndex()))
                    {
                        GetNode<Crash_Area_2D>("Main/Crash_Area").Crash_Area = Zombies_Area_2D;
                    }
                }
            }*/
        }
    }
    public void Area_Exited(Control_Area_2D area2D)
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
        if (area2D.Area2D_type == "Zombies")
        {
            var leave_Area = (Normal_Zombies_Area)area2D;
            Zombies_Area_2D_List.Remove(leave_Area);
            Zombies_Area_2D = null;
            /*if (GetNode<Crash_Area_2D>("Main/Crash_Area").Crash_Area == leave_Area)
            {
                GetNode<Crash_Area_2D>("Main/Crash_Area").Crash_Area = null;
            }*/
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
                if ((Input.IsActionPressed("Left_Mouse") && !Public_Main.for_Android) || (Public_Main.for_Android && Is_Double_Click))
                {
                    card_parent_Button.Set_ColorRect_2(false);
                    Normal_Plants.Choosing = false;
                    if (on_lock_grid && ((In_Game_Main.Sun_Number >= card_parent_Button.sun && dock_area_2d.Normal_Plant_List.Count == 0 && dock_area_2d.type == 1) || Public_Main.debuging))
                    {
                        Is_Double_Click = false;
                        has_planted = true;
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
                        GetNode<AnimationPlayer>("Main/Player1").Play("Player1");
                        GetNode<Normal_Plants_Area>("Main/Shovel_Area").has_planted = this.has_planted;
                        //GetNode<Crash_Area_2D>("Main/Crash_Area").has_planted = this.has_planted;
                        GetNode<AudioStreamPlayer>("bowling").Play();
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
            if (Position.x > 1437)
            {
                this.QueueFree();
            }
            else
            {
                this.Position += new Vector2(speed_x, speed_y);
            }
            if (on_Shovel && ((Input.IsActionPressed("Left_Mouse") && !Public_Main.for_Android) || (Public_Main.for_Android && Is_Double_Click)))
            {
                if (!GetNode<AnimationPlayer>("Free_player").IsPlaying())
                {
                    if (Shovel_Area != null)
                    {
                        if (Shovel_Area.Choose_Plants_Area == GetNode<Normal_Plants_Area>("Main/Shovel_Area"))
                        {
                            Is_Double_Click = false;
                            dock_area_2d.Normal_Plant_List.Remove(this);
                            GetNode<AnimationPlayer>("Free_player").Play("Free");
                        }
                    }
                }
            }
            if (on_Bug && ((Input.IsActionPressed("Left_Mouse") && !Public_Main.for_Android) || (Public_Main.for_Android && Is_Double_Click)))
            {
                if (!GetNode<AnimationPlayer>("Bug_player").IsPlaying())
                {
                    if (Bug_Area != null)
                    {
                        if (Bug_Area.Choose_Plants_Area == GetNode<Normal_Plants_Area>("Main/Shovel_Area") && Bug_Area.playing)
                        {
                            Is_Double_Click = false;
                            GetNode<AnimationPlayer>("Bug_player").Play("Bug");
                        }
                    }
                }
            }
            if (Zombies_Area_2D_List.Count != 0)
            {
                bool can_touch = false;
                for (int i = 0; i < Zombies_Area_2D_List.Count; i++)
                {
                    if (Zombies_Area_2D_List[i] != null)
                    {
                        if (Zombies_Area_2D_List[i].Choose_Plants_Area == GetNode<Normal_Plants_Area>("Main/Shovel_Area"))
                        {
                            if (Zombies_Area_2D_List[i].is_eating_show && !Zombies_Area_2D_List[i].has_lose_head)
                            {
                                Zombies_Area_2D_List[i].is_eating_show = false;
                                health -= Zombies_Area_2D_List[i].hurt;
                                GetNode<AnimationPlayer>("Is_Eated").Play("Is_Eated");
                            }
                        }
                        if (!can_touch && Zombies_Area_2D_List[i] != null&&!GetNode<AnimationPlayer>("Died").IsPlaying())
                        {
                            if (!Zombies_Area_2D_List[i].has_lose_head&&Zombies_Area_2D_List[i].has_plant)
                            {
                                can_touch = true;
                                if (touch_ZIndex != Zombies_Area_2D_List[i].GetParent().GetParent<Node2D>().ZIndex /*&& GetNode<Crash_Area_2D>("Main/Crash_Area").Crash_Area == Zombies_Area_2D_List[i]*/)
                                {
                                    touch_ZIndex = Zombies_Area_2D_List[i].GetParent().GetParent<Node2D>().ZIndex;
                                    speed_x = 0;
                                    GetNode<Normal_Boom_Area>("Main/Boom").hurt = 1800;
                                    GetNode<Normal_Boom_Area>("Main/Boom").can_do = true;
                                    GetNode<Normal_Boom_Area>("Main/Boom").Start_hurting();
                                    GetNode<AnimationPlayer>("Died").Play("Died");
                                    /*if (first_Speed_Y)
                                    {
                                        first_Speed_Y = false;
                                        speed_y = ((GD.Randf() > 0.5) ? -1 : 1) * 3.5f;
                                    }
                                    else
                                    {
                                        speed_y *= -1;
                                    }*/
                                }
                            }
                        }
                        if (health <= 0)
                        {
                            if (!GetNode<AnimationPlayer>("Died").IsPlaying())
                            {
                                GetNode<Normal_Boom_Area>("Main/Boom").hurt = 1800;
                                GetNode<Normal_Boom_Area>("Main/Boom").can_do = true;
                                GetNode<AnimationPlayer>("Died").Play("Died");
                            }
                        }
                        /*if (this.Position.y < 94)
                        {
                            speed_y *= -1;
                        }
                        if (this.Position.y > 588)
                        {
                            speed_y *= -1;
                        }*/
                    }
                }
            }
        }
    }
    public void Bug_Doing()
    {
        //NOTHING
    }
}
