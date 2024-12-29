using Godot;
using System;

public class Godot_Main : Normal_Plants
{
    public bool clone_5_zombies = false;
    public bool has_clone = false;
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
        health = 300;
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
                    if (on_lock_grid && ((In_Game_Main.Sun_Number >= card_parent_Button.sun && dock_area_2d.Normal_Plant_List.Count == 0 && dock_area_2d.now_type[dock_area_2d.now_type.Count - 1] == 1) || Public_Main.debuging))
                    {
                        Is_Double_Click = false;
                        has_planted = true;
                        dock_area_2d.Normal_Plant_List.Add(this);
                        dock_control.Hide();
                        this.GlobalPosition = Area_Vector2;
                        In_Game_Main.Sun_Number -= card_parent_Button.sun;
                        In_Game_Main.Update_Sun(this);
                        card_parent_Button.now_time = card_parent_Button.wait_time;
                        if (dock_area_2d.now_type[dock_area_2d.now_type.Count - 1] == 1)
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
                for (int i = 0; i < Zombies_Area_2D_List.Count; i++)
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
                }
            }
            if (health <= 0)
            {
                if (!GetNode<AnimationPlayer>("Died").IsPlaying())
                {
                    Clone_DO();
                    dock_area_2d.Normal_Plant_List.Remove(this);
                    GetNode<AnimationPlayer>("Died").Play("Died");
                }
            }
        }
    }
    public void Bug_Doing()
    {
        clone_5_zombies = true;
    }
    public void Clone_DO()
    {
        if (!has_clone)
        {
            has_clone = true;
            if (clone_5_zombies)
            {
                for (int i = 0; i < 5; i++)
                {
                    Clone_Self_Zombies();
                }
            }
            else
            {
                Clone_Self_Zombies();
            }
        }
    }
    public void Clone_Self_Zombies()
    {
        for (int i = 1; i <= 6; i++)
        {
            var scene = GD.Load<PackedScene>("res://scene/Plants/Zombies/Simple_Zombies/Plants_Simple_Zombies.tscn");
            var plant_child = (Normal_Plants_Zombies)scene.Instance();
            if (i == 1)
            {
                plant_child.put_position = new Vector2(76, 124);
                plant_child.ZIndex = 7;
            }
            else if (i == 2)
            {
                plant_child.put_position = new Vector2(76, 216);
                plant_child.ZIndex = 27;
            }
            else if (i == 3)
            {
                plant_child.put_position = new Vector2(76, 299);
                plant_child.ZIndex = 47;
            }
            else if (i == 4)
            {
                plant_child.put_position = new Vector2(76, 376);
                plant_child.ZIndex = 67;
            }
            else if (i == 5)
            {
                plant_child.put_position = new Vector2(76, 477);
                plant_child.ZIndex = 87;
            }
            else if (i == 6)
            {
                plant_child.put_position = new Vector2(76, 558);
                plant_child.ZIndex = 107;
            }
            plant_child.player_put = false;
            //plant_child._Ready();//Warning:Can't Add Child
            GetNode<Control>("/root/In_Game/Object").AddChild(plant_child);
        }
    }
}
