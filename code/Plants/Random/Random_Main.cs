using Godot;
using System;
using System.Collections.Generic;
public class Random_Main : Node2D
{
    [Export] protected int normal_ZIndex = 3;
    protected Timer Android_Timer = new Timer();
    protected bool Is_Double_Click = false;//for Android
    public Vector2 put_position = Vector2.Zero;//In_Game_Main使用
    public bool player_put = false;//In_Game_Main使用
    protected bool on_lock_grid = false;
    protected bool has_planted = false;
    public Card_Click_Button card_parent_Button = null;//Card使用
    public bool Tmp_Card_Used = false;
    public Card_Tmp_Main Tmp_card_parent = null;//Card_Tmp使用
    protected List<Background_Grid_Main> Dock_Area_2D_List = new List<Background_Grid_Main>();
    protected Background_Grid_Main Dock_Area_2D = null;
    public override void _Ready()
    {
        GD.Randomize();
        Position = new Vector2(-1437, -1437);
        GetNode<Area2D>("Dock/Area2D").PauseMode = PauseModeEnum.Process;
        AddChild(Android_Timer);
        Android_Timer.WaitTime = 1f;
        Android_Timer.Autostart = false;
        Android_Timer.OneShot = true;
        if (!player_put)
        {
            has_planted = true;
            GetNode<Control>("Dock").Hide();
            GetNode<Control>("Show").Hide();
            GlobalPosition = put_position;
        }
    }
    public override void _PhysicsProcess(float delta)
    {
        if (!GetNode<Area2D>("Dock/Area2D").IsConnected("area_entered", this, nameof(Dock_Entered)))
        {
            return;
        }
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
        if (Dock_Area_2D_List.Count == 0)
        {
            Dock_Area_2D = null;
        }
        else
        {
            for (int i = 0; i < Dock_Area_2D_List.Count; i++)
            {
                if (Dock_Area_2D_List[i] == null)
                {
                    Dock_Area_2D_List.RemoveAt(i);
                    i--;
                    continue;
                }
                if (Math.Abs(GlobalPosition.x - Dock_Area_2D_List[i].GlobalPosition.x) >= 40 || Math.Abs(GlobalPosition.y - Dock_Area_2D_List[i].GlobalPosition.y) >= 40)
                {
                    continue;
                }
                if (Dock_Area_2D != null)
                {
                    if (Math.Abs(GlobalPosition.x - Dock_Area_2D.GlobalPosition.x) >= 40 || Math.Abs(GlobalPosition.y - Dock_Area_2D.GlobalPosition.y) >= 40)
                    {
                        Dock_Area_2D = Dock_Area_2D_List[i];
                    }
                }
                if (Dock_Area_2D == null)
                {
                    Dock_Area_2D = Dock_Area_2D_List[i];
                }
                else
                {
                    if (Dock_Area_2D.pos[1] < Dock_Area_2D_List[i].pos[1])
                    {
                        Dock_Area_2D = Dock_Area_2D_List[i];
                    }
                    else if (Dock_Area_2D.pos[1] == Dock_Area_2D_List[i].pos[1])
                    {
                        if (Dock_Area_2D.pos[0] < Dock_Area_2D_List[i].pos[0])
                        {
                            Dock_Area_2D = Dock_Area_2D_List[i];
                        }
                    }
                }
            }
        }
        if (Dock_Area_2D != null)
        {
            ZIndex = normal_ZIndex + 20 * (Dock_Area_2D.pos[0] - 1);
        }
        if (!has_planted)
        {
            if (Normal_Plants.Choosing)
            {
                GlobalPosition = GetTree().Root.GetMousePosition();
                if (Dock_Area_2D != null)
                {
                    if (Math.Abs(GlobalPosition.x - Dock_Area_2D.GlobalPosition.x) < 40 && Math.Abs(GlobalPosition.y - Dock_Area_2D.GlobalPosition.y) < 40)
                    {
                        GetNode<Control>("Dock").RectGlobalPosition += Dock_Area_2D.GlobalPosition - GetNode<Area2D>("Dock/Area2D").GlobalPosition;
                        on_lock_grid = true;
                    }
                    else
                    {
                        GetNode<Control>("Dock").RectGlobalPosition = GetNode<Control>("Show").RectGlobalPosition;
                        on_lock_grid = false;
                    }
                }
                else
                {
                    GetNode<Control>("Dock").RectGlobalPosition = GetNode<Control>("Show").RectGlobalPosition;
                    on_lock_grid = false;
                }
                if (Input.IsActionPressed("Right_Mouse"))
                {
                    on_lock_grid = false;
                    Normal_Plants.Choosing = false;
                    Position = new Vector2(-1437, -1437);
                    GetNode<AudioStreamPlayer>("/root/In_Game/Cancel").Play();
                    if (Tmp_Card_Used)
                    {
                        Tmp_card_parent.Hide_Shadow();
                    }
                    else
                    {
                        card_parent_Button.Set_ColorRect_2(false);
                    }
                    this.Free();
                }
                if ((Input.IsActionPressed("Left_Mouse") && !Public_Main.for_Android) || (Public_Main.for_Android && Is_Double_Click))
                {
                    int this_sun = 0;
                    if (Tmp_Card_Used)
                    {
                        this_sun = Tmp_card_parent.Sun;
                        Tmp_card_parent.Hide_Shadow();
                    }
                    else
                    {
                        this_sun = card_parent_Button.sun;
                        card_parent_Button.Set_ColorRect_2(false);
                    }
                    Normal_Plants.Choosing = false;
                    if (Dock_Area_2D != null)
                    {
                        if ((In_Game_Main.Sun_Number >= this_sun || Public_Main.debuging) && on_lock_grid)
                        {
                            if (Tmp_Card_Used)
                            {
                                Tmp_card_parent.Been_Used();
                            }//TODO delete Plant
                            for (int i = 0; i < Dock_Area_2D.Normal_Plant_List.Count; i++)
                            {
                                if (Dock_Area_2D.Normal_Plant_List[i] is Normal_Plants Plant_object)
                                {
                                    Plant_object.Free_Self();
                                }
                            }
                            for (int i = 0; i < Dock_Area_2D.Down_Plant_List.Count; i++)
                            {
                                if (Dock_Area_2D.Normal_Plant_List[i] is Normal_Plants Plant_object)
                                {
                                    Plant_object.Free_Self();
                                }
                            }
                            Dock_Area_2D.Normal_Plant_List.Add(this);
                            Dock_Area_2D.Down_Plant_List.Add(this);
                            has_planted = true;
                            this.GlobalPosition = Dock_Area_2D.GlobalPosition;
                            In_Game_Main.Sun_Number -= this_sun;
                            In_Game_Main.Update_Sun(this);
                            if (!Tmp_Card_Used)
                            {
                                card_parent_Button.now_time = card_parent_Button.wait_time;
                            }
                            GetNode<Control>("Dock").Hide();
                            GetNode<Control>("Show").Hide();
                        }
                        else
                        {
                            Hide();
                            Position = new Vector2(-1437, -1437);
                            GetNode<AudioStreamPlayer>("/root/In_Game/Cancel").Play();
                            this.Free();
                        }
                    }
                    else
                    {
                        Hide();
                        Position = new Vector2(-1437, -1437);
                        GetNode<AudioStreamPlayer>("/root/In_Game/Cancel").Play();
                        this.Free();
                    }
                }
            }
        }
        else
        {
            int Clone_Type = (int)(GD.Randi() % 3);
            if (Clone_Type == 0)
            {
                In_Game_Main.Plants_Clone_Request(Public_Main.Plant_list[(int)(GD.Randi() % (Public_Main.Plant_list.Count - 1))].Rest.Item2, this.GlobalPosition, this.ZIndex - normal_ZIndex + 3);
            }
            else if (Clone_Type == 1)
            {
                In_Game_Main.Plants_Zombies_Clone_Request(Public_Main.Plants_Zombies_list[(int)(GD.Randi() % Public_Main.Plants_Zombies_list.Count)].Rest.Item2, this.GlobalPosition, this.ZIndex - normal_ZIndex + 7);
            }
            else
            {
                In_Game_Main.Zombies_Clone_Request(Public_Main.Zombies_Path_List[(int)(GD.Randi() % Public_Main.Zombies_Path_List.Count)], this.GlobalPosition, this.ZIndex - normal_ZIndex + 7);
            }
            this.QueueFree();
        }
    }
    protected void Dock_Entered(Area2D area_node)
    {
        if (!(area_node is Control_Area_2D area2D) || !IsInstanceValid(area2D))
        {
            return;
        }
        string Type_string = area2D?.Area2D_type;
        if (Type_string != null && Type_string == "Grid")
        {
            Dock_Area_2D_List.Add((Background_Grid_Main)area2D);
        }
    }
    protected void Dock_Exited(Area2D area_node)
    {
        if (!(area_node is Control_Area_2D area2D) || !IsInstanceValid(area2D))
        {
            return;
        }
        string Type_string = area2D?.Area2D_type;
        if (Type_string != null && Type_string == "Grid")
        {
            Dock_Area_2D_List.Remove((Background_Grid_Main)area2D);
        }
    }
}
