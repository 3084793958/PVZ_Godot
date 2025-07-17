using Godot;
using System;
using System.Collections.Generic;
public class Crater_Main : Node2D
{
    [Export] protected int normal_ZIndex = 2;
    protected Timer Android_Timer = new Timer();
    protected bool Is_Double_Click = false;//for Android
    public Vector2 put_position = Vector2.Zero;//In_Game_Main使用
    public bool player_put = false;//In_Game_Main使用
    protected bool on_lock_grid = false;
    public bool has_planted = false;
    public Card_Click_Button card_parent_Button = null;//Card使用
    public bool Tmp_Card_Used = false;
    public Card_Tmp_Main Tmp_card_parent = null;//Card_Tmp使用
    protected List<Background_Grid_Main> Dock_Area_2D_List = new List<Background_Grid_Main>();
    protected Background_Grid_Main Dock_Area_2D = null;
    protected bool real_touching = false;
    protected bool has_Add_List = true;
    protected int Texture_Number = 0;
    protected List<Texture> Texture_1_List = new List<Texture>
    {
        GD.Load<Texture>("res://image/Crater/Day1.png"),
        GD.Load<Texture>("res://image/Crater/Night1.png"),
        GD.Load<Texture>("res://image/Crater/Pool1.png")
    };
    protected List<Texture> Texture_2_List = new List<Texture>
    {
        GD.Load<Texture>("res://image/Crater/Day2.png"),
        GD.Load<Texture>("res://image/Crater/Night2.png"),
        GD.Load<Texture>("res://image/Crater/Pool2.png")
    };
    public override void _Ready()
    {
        Position = new Vector2(-1437, -1437);
        AddChild(Android_Timer);
        Android_Timer.WaitTime = 0.5f;
        Android_Timer.Autostart = false;
        Android_Timer.OneShot = true;
        GetNode<AudioStreamPlayer>("Plant_Sound/Ok/Plant1").Stream.Set("loop", false);
        GetNode<AudioStreamPlayer>("Plant_Sound/Ok/Plant2").Stream.Set("loop", false);
        GetNode<AudioStreamPlayer>("Plant_Sound/Ok/Water").Stream.Set("loop", false);
        if (!player_put)
        {
            has_planted = true;
            GetNode<Control>("Dock").Hide();
            GetNode<Control>("Show").Hide();
            GetNode<Control>("Main").Show();
            GetNode<Timer>("Clear_Timer").Start();
            GlobalPosition = put_position;
            has_Add_List = false;
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
            if (!has_Add_List)
            {
                has_Add_List = true;
                for (int i = 0; i < Dock_Area_2D.Normal_Plant_List.Count; i++)
                {
                    if (Dock_Area_2D.Normal_Plant_List[i] is Normal_Plants Plant_object && !(Dock_Area_2D.Normal_Plant_List[i] is Doom_Shroom_Main))
                    {
                        Plant_object.Free_Self();
                    }
                }
                for (int i = 0; i < Dock_Area_2D.Down_Plant_List.Count; i++)
                {
                    if (Dock_Area_2D.Normal_Plant_List[i] is Normal_Plants Plant_object && !(Dock_Area_2D.Normal_Plant_List[i] is Doom_Shroom_Main))
                    {
                        Plant_object.Free_Self();
                    }
                }
                for (int i = 0; i < Dock_Area_2D.Small_Plants_List.Count; i++)
                {
                    if (Dock_Area_2D.Small_Plants_List[i] is Normal_Plants Plant_object && !(Dock_Area_2D.Small_Plants_List[i] is Doom_Shroom_Main))
                    {
                        Plant_object.Free_Self();
                    }
                }
                Dock_Area_2D.Normal_Plant_List.Add(this);
                Dock_Area_2D.Down_Plant_List.Add(this);
            }
        }
        if (Dock_Area_2D != null)
        {
            if (Dock_Area_2D.type == 3 || Dock_Area_2D.type == 1)
            {
                if (In_Game_Main.allow_sun)
                {
                    Texture_Number = 0;
                }
                else
                {
                    Texture_Number = 1;
                }
            }
            else if (Dock_Area_2D.type == 2)
            {
                if (In_Game_Main.allow_sun)
                {
                    Texture_Number = 2;
                }
                else
                {
                    Texture_Number = 3;
                }
            }
        }
        if (Texture_Number < 0)
        {
            if (In_Game_Main.allow_sun)
            {
                Texture_Number = 0;
            }
            else
            {
                Texture_Number = 1;
            }
        }
        else if (Texture_Number >= Texture_1_List.Count)
        {
            if (In_Game_Main.allow_sun)
            {
                Texture_Number = 0;
            }
            else
            {
                Texture_Number = 1;
            }
        }
        GetNode<TextureRect>("Dock/Help/Texture").Texture = Texture_1_List[Texture_Number];
        GetNode<TextureRect>("Show/Help/Texture").Texture = Texture_1_List[Texture_Number];
        GetNode<Sprite>("Main/T1").Texture = Texture_1_List[Texture_Number];
        GetNode<Sprite>("Main/T2").Texture = Texture_2_List[Texture_Number];
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
                if (Is_Double_Click || (Input.IsActionPressed("Left_Mouse") && !real_touching))
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
                                if (Dock_Area_2D.Normal_Plant_List[i] is Normal_Plants Plant_object && !(Dock_Area_2D.Normal_Plant_List[i] is Doom_Shroom_Main)) 
                                {
                                    Plant_object.Free_Self();
                                }
                            }
                            for (int i = 0; i < Dock_Area_2D.Down_Plant_List.Count; i++)
                            {
                                if (Dock_Area_2D.Normal_Plant_List[i] is Normal_Plants Plant_object && !(Dock_Area_2D.Normal_Plant_List[i] is Doom_Shroom_Main))
                                {
                                    Plant_object.Free_Self();
                                }
                            }
                            for (int i = 0; i < Dock_Area_2D.Small_Plants_List.Count; i++)
                            {
                                if (Dock_Area_2D.Small_Plants_List[i] is Normal_Plants Plant_object && !(Dock_Area_2D.Small_Plants_List[i] is Doom_Shroom_Main))
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
                            GetNode<Timer>("Clear_Timer").Start();
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
    public async void Free_Self()
    {
        if (Dock_Area_2D != null)
        {
            Dock_Area_2D.Normal_Plant_List.Remove(this);
            Dock_Area_2D.Down_Plant_List.Remove(this);
        }
        await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
        if (IsInstanceValid(this))
        {
            this.QueueFree();
        }
    }
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventScreenTouch touchEvent)
        {
            real_touching = touchEvent.Device != -1;//真触摸
        }
    }
    protected void Clear_Timer_timeout()
    {
        if (GetNode<Sprite>("Main/T1").Visible)
        {
            GetNode<Sprite>("Main/T2").Show();
            GetNode<Sprite>("Main/T1").Hide();
        }
        else
        {
            GetNode<AnimationPlayer>("Main/Free_Self").Play("Free");
        }
    }
}
