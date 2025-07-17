using Godot;
using System;
using System.Collections.Generic;
public class PH_Meter_Main : Limited_Plants
{
    protected bool real_touching = false;
    [Export] protected Color hover_color = new Color(1.3f, 1.3f, 1.3f, 1f);
    [Export] protected Color normal_color = new Color(1f, 1f, 1f, 1f);
    protected bool on_Shovel = false;
    protected Shovel_Area2D Shovel_Area = null;
    protected string pH_str = null;
    protected List<Water_Area> Water_Area_List = new List<Water_Area>();
    protected bool has_Add_Meter = false;//Dock_Area_2D能够自行处理移除问题
    protected bool D_Is_Double_Click = false;
    protected Timer D_Timer = new Timer();
    public override void _Ready()
    {
        normal_ZIndex = 5;
        GD.Randomize();
        Position = new Vector2(-1437, -1437);
        GetNode<Area2D>("Dock/Area2D").PauseMode = PauseModeEnum.Process;
        AddChild(Android_Timer);
        Android_Timer.WaitTime = 0.5f;
        Android_Timer.Autostart = false;
        Android_Timer.OneShot = true;
        AddChild(D_Timer);
        D_Timer.WaitTime = 0.5f;
        D_Timer.Autostart = false;
        D_Timer.OneShot = true;
        GetNode<Control>("Dock").Show();
        GetNode<Control>("Show").Show();
        GetNode<Control>("Main").Hide();
        if (!player_put)
        {
            has_planted = true;
            GetNode<Control>("Dock").Hide();
            GetNode<Control>("Show").Hide();
            GetNode<Control>("Main").Show();
            GlobalPosition = put_position;
        }
        else
        {
            In_Game_Main.Choosing_List.Add(this);
        }
    }
    public override void _PhysicsProcess(float delta)
    {
        if (!GetNode<Area2D>("Dock/Area2D").IsConnected("area_entered", this, nameof(Dock_Entered)))
        {
            return;
        }
        Android_Timer.OneShot = Public_Main.for_Android;
        if (Input.IsActionJustReleased("Left_Mouse"))
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
        if (Input.IsActionJustReleased("Left_Mouse") && Mouse_In_Region(0f, -60f, 20f, 12f)) 
        {
            if (D_Timer.IsStopped())
            {
                D_Timer.Start();
            }
            else
            {
                D_Is_Double_Click = true;
                D_Timer.Start();
            }
        }
        if (D_Timer.IsStopped() && !Input.IsActionJustReleased("Left_Mouse"))
        {
            D_Is_Double_Click = false;
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
        GetNode<Normal_Plants_Area>("Main/Shovel_Area").has_planted = this.has_planted;
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
                    In_Game_Main.Choosing_List.Remove(this);
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
                if ((Is_Double_Click || (Input.IsActionPressed("Left_Mouse") && !real_touching))) 
                {
                    Is_Double_Click = false;
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
                    In_Game_Main.Choosing_List.Remove(this);
                    if (Dock_Area_2D != null)
                    {
                        if ((In_Game_Main.Sun_Number >= this_sun || Public_Main.debuging) && on_lock_grid && Dock_Area_2D != null && Dock_Area_2D.pH_Meter == null) 
                        {
                            if (Tmp_Card_Used)
                            {
                                Tmp_card_parent.Been_Used();
                            }
                            has_planted = true;
                            this.GlobalPosition = Dock_Area_2D.GlobalPosition;
                            In_Game_Main.Sun_Number -= this_sun;
                            In_Game_Main.Update_Sun(this);
                            if (!Tmp_Card_Used)
                            {
                                card_parent_Button.now_time = card_parent_Button.wait_time;
                            }
                            if (Dock_Area_2D.type == 2)
                            {
                                GetNode<AudioStreamPlayer>("Plant_Sound/Ok/Water").Play();
                            }
                            else
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
            if (D_Is_Double_Click)
            {
                D_Is_Double_Click = false;
                if (Dock_Area_2D != null)
                {
                    for (int i = 0; i < Dock_Area_2D.Normal_Plant_List.Count; i++)
                    {
                        if (Dock_Area_2D.Normal_Plant_List[i] is H2SO4_Main || Dock_Area_2D.Normal_Plant_List[i] is H2O_Main || Dock_Area_2D.Normal_Plant_List[i] is S_Emulsion_Main || Dock_Area_2D.Normal_Plant_List[i] is CaO_Emulsion_Main)
                        {
                            ((Normal_Plants)Dock_Area_2D.Normal_Plant_List[i]).Free_Self();
                        }
                    }
                }
            }
            if (Dock_Area_2D != null)
            {
                if (!has_Add_Meter)
                {
                    has_Add_Meter = true;
                    Dock_Area_2D.pH_Meter = this;
                }
            }
            if (Shovel_Area != null)
            {
                if (Shovel_Area.Choose_Plants_Area == GetNode<Normal_Plants_Area>("Main/Shovel_Area"))
                {
                    this.Modulate = hover_color;
                    on_Shovel = true;
                }
            }
            else
            {
                on_Shovel = false;
            }
            if (on_Shovel && ((Input.IsActionJustReleased("Left_Mouse") && real_touching) || (Input.IsActionJustPressed("Left_Mouse") && !real_touching)))
            {
                Is_Double_Click = false;
                if (!GetNode<AnimationPlayer>("Free_player").IsPlaying())
                {
                    if (Shovel_Area != null)
                    {
                        if (Shovel_Area.Choose_Plants_Area == GetNode<Normal_Plants_Area>("Main/Shovel_Area"))
                        {
                            GetNode<AnimationPlayer>("Free_player").Play("Free");
                        }
                    }
                }
            }
            pH_str = "--";
            if (Water_Area_List.Count != 0)
            {
                pH_str = Math.Round(Water_Area_List[0].pH, 2).ToString();
            }
            else if (Dock_Area_2D != null && Dock_Area_2D.type != 2)
            {
                pH_str = Math.Round(Dock_Area_2D.pH, 2).ToString();
            }
            pH_str = "pH:" + pH_str;
            GetNode<Label>("pH/pH").Text = pH_str;
            GetNode<Node2D>("pH").Visible = this.has_planted;
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
        if (Type_string != null && Type_string == "Water_Area")//H2O
        {
            Water_Area_List.Add((Water_Area)area2D);
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
        if (Type_string != null && Type_string == "Water_Area")//H2O
        {
            Water_Area_List.Remove((Water_Area)area2D);
        }
    }
    protected void Shovel_Area_Entered(Area2D area_node)
    {
        if (!(area_node is Control_Area_2D area2D) || !IsInstanceValid(area2D))
        {
            return;
        }
        string Type_string = area2D?.Area2D_type;
        if (has_planted && Type_string != null && Type_string == "Shovel")
        {
            Shovel_Area = (Shovel_Area2D)area2D;
        }
    }
    protected void Shovel_Area_Exited(Area2D area_node)
    {
        if (!(area_node is Control_Area_2D area2D) || !IsInstanceValid(area2D))
        {
            return;
        }
        string Type_string = area2D?.Area2D_type;
        if (has_planted && Type_string != null && Type_string == "Shovel")
        {
            this.Modulate = normal_color;
            Shovel_Area = null;
            on_Shovel = false;
        }
    }
    public async void Free_Self()
    {
        if (Dock_Area_2D != null)
        {
            if (Dock_Area_2D.pH_Meter == this)
            {
                Dock_Area_2D.pH_Meter = null;
            }
        }
        if (GetNode<Area2D>("Dock/Area2D").IsConnected("area_entered", this, nameof(Dock_Entered)))
        {
            GetNode<Area2D>("Dock/Area2D").Disconnect("area_entered", this, nameof(Dock_Entered));
            GetNode<Area2D>("Dock/Area2D").Disconnect("area_exited", this, nameof(Dock_Exited));
            GetNode<Area2D>("Dock/Area2D").Monitoring = false;
            GetNode<Area2D>("Dock/Area2D").Monitorable = false;
        }
        Hide();
        await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
        if (IsInstanceValid(this))
        {
            this.QueueFree();
        }
    }
    protected bool Mouse_In_Region(float D_To_P_x, float D_To_P_y, float H_W, float H_H)
    {
        float M_X = GetTree().Root.GetMousePosition().x;
        float M_Y = GetTree().Root.GetMousePosition().y;
        float G_P_x = this.GlobalPosition.x + D_To_P_x * this.GlobalScale.x;
        float G_P_y = this.GlobalPosition.y + D_To_P_y * this.GlobalScale.y;
        float D_M_P_X = Math.Abs(M_X - G_P_x);
        float D_M_P_Y = Math.Abs(M_Y - G_P_y);
        return D_M_P_X < H_W && D_M_P_Y < H_H;
    }
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventScreenTouch touchEvent)
        {
            real_touching = touchEvent.Device != -1;//真触摸
        }
    }
}
