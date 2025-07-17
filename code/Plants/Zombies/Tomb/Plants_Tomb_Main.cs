using Godot;
using System;
using System.Collections.Generic;
public class Plants_Tomb_Main : Node2D
{
    [Export] protected Color hurt_color = new Color(1.2f, 0f, 1.3f, 1f);
    [Export] protected Color normal_color = new Color(0.92f, 0f, 1f, 1f);
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
    protected Background_Grid_Main Choose_Dock_Area_2D = null;
    public int Clone_Time = 0;
    protected int health = 2000;
    protected List<All_Boom_Area> All_Boom_Area_2D_List = new List<All_Boom_Area>();
    public int lock_to_number = -1;
    protected bool real_touching = false;
    protected bool has_Add_List = true;
    protected List<string> Clone_List = new List<string>
    {
        "res://scene/Plants/Zombies/Simple_Zombies/Plants_Simple_Zombies.tscn",
        "res://scene/Plants/Zombies/Flag_Zombies/Plants_Flag_Zombies.tscn",
        "res://scene/Plants/Zombies/Cone_Zombies/Plants_Cone_Zombies.tscn",
        "res://scene/Plants/Zombies/Fast_Cone_Zombies/Plants_Fast_Cone_Zombies.tscn",
        "res://scene/Plants/Zombies/Ignore_Zombies/Plants_Ignore_Cone_Zombies.tscn",//5
        "res://scene/Plants/Zombies/Bucket_Zombies/Plants_Bucket_Zombies.tscn",
        "res://scene/Plants/Zombies/Polevaulter_Zombies/Plants_Polevaulter_Zombies.tscn",
        "res://scene/Plants/Zombies/Darts_Polevaulter_Zombies/Plants_Darts_Polevaulter_Zombies.tscn",
        "res://scene/Plants/Zombies/Screen_Door_Zombies/Plants_Screen_Door_Zombies.tscn",
        "res://scene/Plants/Zombies/Fire_Zombies/Plants_Fire_Zombies.tscn",//10
        "res://scene/Plants/Zombies/H2_maker_Zombies/Plants_H2_maker_Zombies.tscn",
        "res://scene/Plants/Zombies/Bucket_Screen_Door_Zombies/Plants_Bucket_Screen_Door_Zombies.tscn",
        "res://scene/Plants/Zombies/Darts_Screen_Door_Zombies/Plants_Darts_Screen_Door_Zombies.tscn",
        "res://scene/Plants/Zombies/Bucket_Tomb_Zombies/Plants_Bucket_Tomb_Zombies.tscn",
        "res://scene/Plants/Zombies/Paper_Zombies/Plants_Paper_Zombies.tscn",//15
        "res://scene/Plants/Zombies/FootBall_Zombies/Plants_FootBall_Zombies.tscn",
        "res://scene/Plants/Zombies/JackSon_Zombies/Plants_JackSon_Zombies.tscn",
        "res://scene/Plants/Zombies/Dancer_Zombies/Plants_Dancer_Zombies.tscn",
        "res://scene/Plants/Zombies/Basketball_Zombies/Plants_Basketball_Zombies.tscn"
    };
    protected List<Texture> Tomb_Texture_List = new List<Texture>
    {
        GD.Load<Texture>("res://image/Zombies/Tomb/0.png"),
        GD.Load<Texture>("res://image/Zombies/Tomb/1.png"),
        GD.Load<Texture>("res://image/Zombies/Tomb/2.png"),
        GD.Load<Texture>("res://image/Zombies/Tomb/3.png"),
        GD.Load<Texture>("res://image/Zombies/Tomb/4.png"),
        GD.Load<Texture>("res://image/Zombies/Tomb/5.png"),
        GD.Load<Texture>("res://image/Zombies/Tomb/6.png"),
        GD.Load<Texture>("res://image/Zombies/Tomb/7.png"),
        GD.Load<Texture>("res://image/Zombies/Tomb/8.png"),
        GD.Load<Texture>("res://image/Zombies/Tomb/9.png"),
        GD.Load<Texture>("res://image/Zombies/Tomb/10.png"),
        GD.Load<Texture>("res://image/Zombies/Tomb/11.png"),
        GD.Load<Texture>("res://image/Zombies/Tomb/12.png"),
        GD.Load<Texture>("res://image/Zombies/Tomb/13.png"),
        GD.Load<Texture>("res://image/Zombies/Tomb/14.png"),
        GD.Load<Texture>("res://image/Zombies/Tomb/15.png"),
        GD.Load<Texture>("res://image/Zombies/Tomb/16.png"),
        GD.Load<Texture>("res://image/Zombies/Tomb/17.png"),
        GD.Load<Texture>("res://image/Zombies/Tomb/18.png"),
        GD.Load<Texture>("res://image/Zombies/Tomb/19.png")
    };
    public override void _Ready()
    {
        GD.Randomize();
        lock_to_number = In_Game_Main.Tomb_Lock_To_Number;
        var tomb_Texture = Tomb_Texture_List[(int)(GD.Randi() % Tomb_Texture_List.Count)];
        GetNode<TextureRect>("Dock/Help/Texture").Texture = tomb_Texture;
        GetNode<TextureRect>("Show/Help/Texture").Texture = tomb_Texture;
        GetNode<Sprite>("Main/Tomb_Texture").Texture = tomb_Texture;
        Position = new Vector2(-1437, -1437);
        GetNode<Area2D>("Dock/Area2D").PauseMode = PauseModeEnum.Process;
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
            Position = put_position;
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
                for (int i = 0; i < Dock_Area_2D.Small_Plants_List.Count; i++)
                {
                    if (Dock_Area_2D.Small_Plants_List[i] is Normal_Plants Plant_object)
                    {
                        Plant_object.Free_Self();
                    }
                }
                Choose_Dock_Area_2D = Dock_Area_2D;
                Dock_Area_2D.Normal_Plant_List.Add(this);
                Dock_Area_2D.Down_Plant_List.Add(this);
            }
        }
        if (!has_planted)
        {
            GetNode<Node2D>("Health").Hide();
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
                            for (int i = 0; i < Dock_Area_2D.Small_Plants_List.Count; i++)
                            {
                                if (Dock_Area_2D.Small_Plants_List[i] is Normal_Plants Plant_object)
                                {
                                    Plant_object.Free_Self();
                                }
                            }
                            Choose_Dock_Area_2D = Dock_Area_2D;
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
            if (health <= 0 || !Public_Main.Show_Plants_Zombies_Health)
            {
                GetNode<Node2D>("Health").Hide();
            }
            else
            {
                GetNode<Label>("Health/Health").Text = "HP:" + health.ToString();
                GetNode<Label>("Health/Health").RectGlobalPosition = new Vector2(this.GlobalPosition.x - GetNode<Label>("Health/Health").RectSize.x * GetNode<Label>("Health/Health").RectScale.x / 2, this.GlobalPosition.y - 40 - GetNode<Label>("Health/Health").RectSize.y * GetNode<Label>("Health/Health").RectScale.y);
                GetNode<Node2D>("Health").ZIndex = 116;
                GetNode<Node2D>("Health").Show();
            }
            if ((Clone_Time > 20 || health <= 0) && !GetNode<AnimationPlayer>("Main/Free_Self").IsPlaying())
            {
                GetNode<AnimationPlayer>("Main/Free_Self").Play("Free");
            }
            for (int i = 0; i < All_Boom_Area_2D_List.Count; i++)
            {
                if (All_Boom_Area_2D_List[i].can_do && !All_Boom_Area_2D_List[i].end_hurt)
                {
                    health -= All_Boom_Area_2D_List[i].hurt;
                    All_Boom_Area_2D_List.RemoveAt(i);
                    i--;
                }
            }
            if (Dock_Area_2D != null && Choose_Dock_Area_2D != null)
            {
                if (Dock_Area_2D != Choose_Dock_Area_2D)
                {
                    Choose_Dock_Area_2D.Normal_Plant_List.Remove(this);
                    Choose_Dock_Area_2D.Down_Plant_List.Remove(this);
                    Choose_Dock_Area_2D = Dock_Area_2D;
                    Dock_Area_2D.Normal_Plant_List.Add(this);
                    Dock_Area_2D.Down_Plant_List.Add(this);
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
    protected void Creating_Timer_timeout()
    {
        if (In_Game_Main.is_playing && has_planted && health > 0)
        {
            GetNode<AnimationPlayer>("Main/Player/Out_Land").Play("Out_Land");
        }
    }
    public void Clone_Zombies()
    {
        Clone_Time++;
        if (lock_to_number <= 0)
        {
            In_Game_Main.Plants_Zombies_Clone_Request(Clone_List[(int)(GD.Randi() % Clone_List.Count)], this.Position, this.ZIndex - normal_ZIndex + 7, true);
        }
        else
        {
            In_Game_Main.Plants_Zombies_Clone_Request(Clone_List[(int)(GD.Randi() % lock_to_number)], this.Position, this.ZIndex - normal_ZIndex + 7, true);
        }
    }
    protected void Plants_Entered(Area2D area_node)
    {
        if (!(area_node is Control_Area_2D area2D) || !IsInstanceValid(area2D))
        {
            return;
        }
        string Type_string = area2D?.Area2D_type;
        if (Type_string != null && Type_string == "All_Boom")
        {
            All_Boom_Area_2D_List.Add((All_Boom_Area)area2D);
        }
    }
    protected void Plants_Exited(Area2D area_node)
    {
        if (!(area_node is Control_Area_2D area2D) || !IsInstanceValid(area2D))
        {
            return;
        }
        string Type_string = area2D?.Area2D_type;
        if (Type_string != null && Type_string == "All_Boom")
        {
            All_Boom_Area_2D_List.Remove((All_Boom_Area)area2D);
        }
    }
    public async void Free_Self()
    {
        if (GetNode<Area2D>("Dock/Area2D").IsConnected("area_entered", this, nameof(Dock_Entered)))
        {
            GetNode<Area2D>("Dock/Area2D").Disconnect("area_entered", this, nameof(Dock_Entered));
            GetNode<Area2D>("Dock/Area2D").Disconnect("area_exited", this, nameof(Dock_Exited));
            GetNode<Area2D>("Dock/Area2D").Monitoring = false;
            GetNode<Area2D>("Dock/Area2D").Monitorable = false;
        }
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
}
