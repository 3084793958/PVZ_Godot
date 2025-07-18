using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
public class Normal_Plants_Zombies : Node2D
{
    public Health_List Hypno_Health = new Health_List();
    public Vector2 Hypno_Pos = Vector2.Zero;
    public string Hypno_Spec_Info = null;
    public bool is_Hypnoed = false;
    //define
    [Export] protected int normal_ZIndex = 7;
    [Export] protected Color hurt_color = new Color(1.2f, 0f, 1.3f, 1f);
    [Export] protected Color normal_color = new Color(0.92f, 0f, 1f, 1f);
    protected Timer Android_Timer = new Timer();
    protected bool Is_Double_Click = false;//for Android
    public Vector2 put_position = Vector2.Zero;//In_Game_Main使用
    public bool player_put = false;//In_Game_Main使用
    protected Timer Hurt_Timer = new Timer();
    protected Health_List health_list = new Health_List
    {
        new Health_Container(270,false)
    };
    protected int health = 270;
    [Export] protected float speed_Normal = 1.5f;
    [Export] protected bool can_Eating = true;
    protected bool Doing_jumping = false;//for Pole...
    protected bool has_planted = false;
    public Card_Click_Button card_parent_Button = null;//Card使用
    public bool Tmp_Card_Used = false;
    public Card_Tmp_Main Tmp_card_parent = null;//Card_Tmp使用
    protected bool on_lock_grid = false;
    protected bool has_lose_Arm = false;
    protected bool has_lose_Head = false;
    protected bool has_Lose_Number = false;
    protected bool has_Lose_Hat = false;
    protected float speed = 0f;
    protected bool eating = false;
    protected bool in_water = false;
    protected bool now_in_water = false;
    protected Background_Grid_Main Dock_Area_2D = null;
    protected Normal_Zombies_Area Zombies_Area_2D = null;
    protected List<Background_Grid_Main> Dock_Area_2D_List = new List<Background_Grid_Main>();
    protected List<Normal_Zombies_Area> Zombies_Area_2D_List = new List<Normal_Zombies_Area>();
    protected List<All_Boom_Area> All_Boom_Area_2D_List = new List<All_Boom_Area>();
    protected List<Background_Out_Land_Main> Out_Land_Area_2D_List = new List<Background_Out_Land_Main>();
    protected Background_Out_Land_Main Out_Land_Area_2D = null;
    protected bool real_touching = false;
    [Export] protected bool Never_Died = false;
    [Export] protected bool is_Angry = false;
    protected int label_health_up = 64;
    public bool Use_Out_Land_Ani = false;
    public bool dancing_stop = false;
    public bool dancer_should_stop = false;
    protected List<Water_Area> Water_Area_List = new List<Water_Area>();
    //define
    protected static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (true/*Public_Main.debuging*/)
        {
            GD.Print("异常:" + e.ExceptionObject);
        }
    }
    protected static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
    {
        e.SetObserved();
        if (true/*Public_Main.debuging*/)
        {
            GD.Print("未观测异常:" + e.Exception);
        }
    }
    protected virtual void Self_Hypno_Process()
    { }
    public override void _Ready()
    {
        Position = new Vector2(-1437, -1437);
        GetNode<Area2D>("Main/Main/Zombies_Area").PauseMode = PauseModeEnum.Process;
        GetNode<Area2D>("Dock/Area2D").PauseMode = PauseModeEnum.Process;
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;//C#核心技术
        AddChild(Android_Timer);
        Android_Timer.WaitTime = 0.5f;
        Android_Timer.Autostart = false;
        Android_Timer.OneShot = true;
        AddChild(Hurt_Timer);
        Hurt_Timer.WaitTime = 5f;
        Hurt_Timer.Autostart = false;
        Hurt_Timer.OneShot = false;
        Hurt_Timer.Connect("timeout", this, nameof(Hurt_Timer_Out));
        GetNode<AudioStreamPlayer>("Plant_Sound/Ok/Plant1").Stream.Set("loop", false);
        GetNode<AudioStreamPlayer>("Plant_Sound/Ok/Plant2").Stream.Set("loop", false);
        GetNode<AudioStreamPlayer>("Plant_Sound/Ok/Water").Stream.Set("loop", false);
        GetNode<AudioStreamPlayer>("Main/Main/Eat_Sound1").Stream.Set("loop", false);
        GetNode<AudioStreamPlayer>("Main/Main/Eat_Sound2").Stream.Set("loop", false);
        GetNode<AudioStreamPlayer>("Main/Main/Fall").Stream.Set("loop", false);
        GetNode<Control>("Dock").Show();
        GetNode<Control>("Show").Show();
        GetNode<Control>("Main").Hide();
        speed = speed_Normal * (float)GD.RandRange(0.1, 0.2);
        if (!player_put)
        {
            has_planted = true;
            GetNode<Control>("Dock").Hide();
            GetNode<Control>("Show").Hide();
            GetNode<Control>("Main").Show();
            Hurt_Timer.Start();
            Walk_Mode(true);
            Spec_Doing();
            if (Use_Out_Land_Ani)
            {
                GetNode<AnimationPlayer>("Out_Land").Play("Up");
            }
            if (is_Hypnoed)
            {
                health_list.Clear();
                for (int i = 0; i < Hypno_Health.Count; i++)
                {
                    health_list.Add(Hypno_Health[i]);
                }
                int count_health = 0;
                for (int i = 0; i < health_list.Count; i++)
                {
                    if (health_list[i].Health < 0)
                    {
                        if (health_list[i].Is_lock)
                        {
                            health_list[i].Health = 0;
                        }
                        else
                        {
                            if (i + 1 < health_list.Count)
                            {
                                health_list[i + 1].Health += health_list[i].Health;
                                health_list[i].Health = 0;
                            }
                        }
                    }
                    count_health += health_list[i].Health;
                    if (health_list[i].Health <= 0 && health_list.Count != 1)
                    {
                        health_list.RemoveAt(i);
                        i--;
                    }
                }
                health = count_health;
                if (Hypno_Spec_Info == null)
                {
                    this.GlobalPosition = Hypno_Pos;
                }
                else
                {
                    Self_Hypno_Process();
                }
                Re_Set_Process();
            }
            else
            {
                Position = put_position;
            }
            GetNode<Normal_Plants_Zombies_Area>("Main/Main/Zombies_Area").has_planted = true;
        }
        else
        {
            In_Game_Main.Choosing_List.Add(this);
        }
    }
    public override void _PhysicsProcess(float delta)
    {
        if (!GetNode<Area2D>("Main/Main/Zombies_Area").IsConnected("area_entered", this, nameof(Plants_Entered)))
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
            bool Area_2D_in_List = false;
            for (int i = 0; i < Dock_Area_2D_List.Count; i++)
            {
                if (Dock_Area_2D == Dock_Area_2D_List[i])
                {
                    Area_2D_in_List = true;
                    break;
                }
            }
            if (!Area_2D_in_List)
            {
                Dock_Area_2D = null;
            }
            for (int i = 0; i < Dock_Area_2D_List.Count; i++)
            {
                if (Dock_Area_2D_List[i] == null)
                {
                    Dock_Area_2D_List.RemoveAt(i);
                    i--;
                    continue;
                }
                if (Math.Abs(GetNode<Normal_Plants_Zombies_Area>("Main/Main/Zombies_Area").GlobalPosition.x - Dock_Area_2D_List[i].GlobalPosition.x) >= 40 || Math.Abs(GetNode<Node2D>("Main/Main/Zombies_Area/Shape").GlobalPosition.y - Dock_Area_2D_List[i].GlobalPosition.y) >= 40)
                {
                    continue;
                }
                if (Dock_Area_2D != null)
                {
                    if (Math.Abs(GetNode<Normal_Plants_Zombies_Area>("Main/Main/Zombies_Area").GlobalPosition.x - Dock_Area_2D.GlobalPosition.x) >= 40 || Math.Abs(GetNode<Node2D>("Main/Main/Zombies_Area/Shape").GlobalPosition.y - Dock_Area_2D.GlobalPosition.y) >= 40)
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
        if (Out_Land_Area_2D_List.Count == 0)
        {
            Out_Land_Area_2D = null;
        }
        else
        {
            bool Area_2D_in_List = false;
            for (int i = 0; i < Out_Land_Area_2D_List.Count; i++)
            {
                if (Out_Land_Area_2D == Out_Land_Area_2D_List[i])
                {
                    Area_2D_in_List = true;
                    break;
                }
            }
            if (!Area_2D_in_List)
            {
                Out_Land_Area_2D = null;
            }
            for (int i = 0; i < Out_Land_Area_2D_List.Count; i++)
            {
                if (Out_Land_Area_2D_List[i] == null)
                {
                    Out_Land_Area_2D_List.RemoveAt(i);
                    i--;
                    continue;
                }
                if (Out_Land_Area_2D == null)
                {
                    Out_Land_Area_2D = Out_Land_Area_2D_List[i];
                    break;//will be done in the future
                }
            }
        }
        if (Dock_Area_2D != null)
        {
            ZIndex = normal_ZIndex + 20 * (Dock_Area_2D.pos[0] - 1);
            in_water = Dock_Area_2D.type == 2;
        }
        else
        {
            if (Out_Land_Area_2D != null)
            {
                in_water = Out_Land_Area_2D.type == 2;
            }
        }
        int count_health = 0;
        for (int i = 0; i < health_list.Count; i++)
        {
            if (health_list[i].Health < 0)
            {
                if (health_list[i].Is_lock)
                {
                    health_list[i].Health = 0;
                }
                else
                {
                    if (i + 1 < health_list.Count)
                    {
                        health_list[i + 1].Health += health_list[i].Health;
                        health_list[i].Health = 0;
                    }
                }
            }
            count_health += health_list[i].Health;
            if (health_list[i].Health <= 0 && health_list.Count != 1)
            {
                health_list.RemoveAt(i);
                i--;
            }
        }
        health = count_health;
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
                    In_Game_Main.Choosing_List.Remove(this);
                    if (Dock_Area_2D != null)
                    {
                        if ((In_Game_Main.Sun_Number >= this_sun || Public_Main.debuging) && on_lock_grid)
                        {
                            if (Tmp_Card_Used)
                            {
                                Tmp_card_parent.Been_Used();
                            }
                            has_planted = true;
                            GetNode<Normal_Plants_Zombies_Area>("Main/Main/Zombies_Area").has_planted = true;
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
                            Hurt_Timer.Start();
                            Walk_Mode(true);
                            Spec_Doing();
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
            GetNode<Normal_Plants_Zombies_Area>("Main/Main/Zombies_Area").Choose_Zombies_Area = Zombies_Area_2D;
            GetNode<Normal_Plants_Zombies_Area>("Main/Main/Zombies_Area").has_lose_head = has_lose_Head;
            GetNode<Control>("Health/Health").MouseFilter = Control.MouseFilterEnum.Ignore;
            if (health <= 0 || !Public_Main.Show_Plants_Zombies_Health)
            {
                GetNode<Node2D>("Health").Hide();
            }
            else
            {
                string health_text = null;
                for (int i = 0; i < health_list.Count; i++)
                {
                    if (health_list[i].Health < 0)
                    {
                        if (health_list[i].Is_lock)
                        {
                            health_list[i].Health = 0;
                        }
                        else
                        {
                            if (i + 1 < health_list.Count)
                            {
                                health_list[i + 1].Health += health_list[i].Health;
                                health_list[i].Health = 0;
                            }
                        }
                    }
                    if (health_list[i].Health == 0)
                    {
                        continue;
                    }
                    if (i + 1 == health_list.Count)
                    {
                        health_text = "HP:" + health_list[i].Health.ToString() + health_text;
                    }
                    else
                    {
                        health_text = "+" + health_list[i].Health.ToString() + health_text;
                    }
                }
                GetNode<Label>("Health/Health").Text = health_text;
                GetNode<Label>("Health/Health").RectGlobalPosition = new Vector2(GetNode<Normal_Plants_Zombies_Area>("Main/Main/Zombies_Area").GlobalPosition.x - GetNode<Label>("Health/Health").RectSize.x * GetNode<Label>("Health/Health").RectScale.x / 2, GetNode<Control>("Main/Main").RectGlobalPosition.y - label_health_up - GetNode<Label>("Health/Health").RectSize.y * GetNode<Label>("Health/Health").RectScale.y);
                GetNode<Node2D>("Health").ZIndex = 116;
                GetNode<Node2D>("Health").Show();
            }
            if (GetNode<Normal_Plants_Zombies_Area>("Main/Main/Zombies_Area").lose_health)
            {
                GetNode<Normal_Plants_Zombies_Area>("Main/Main/Zombies_Area").lose_health = false;
                if (!Never_Died)
                {
                    health_list[0].Health -= GetNode<Normal_Plants_Zombies_Area>("Main/Main/Zombies_Area").lose_health_number;
                }
                GetNode<AnimationPlayer>("Is_Eated").Play("Is_Eated");
            }
            if (!Doing_jumping && !GetNode<AnimationPlayer>("Out_Land").IsPlaying())
            {
                if (in_water && !GetNode<AnimationPlayer>("In_Water").IsPlaying() && !GetNode<AnimationPlayer>("Out_Water").IsPlaying() && !now_in_water)
                {
                    GetNode<AnimationPlayer>("In_Water").Play("Water");
                    now_in_water = true;
                }
                if (!in_water && !GetNode<AnimationPlayer>("Out_Water").IsPlaying() && !GetNode<AnimationPlayer>("In_Water").IsPlaying() && now_in_water)
                {
                    GetNode<AnimationPlayer>("Out_Water").Play("Water");
                    now_in_water = false;
                }
            }
            if (!GetNode<Control>("Main").RectClipContent && now_in_water && !GetNode<AnimationPlayer>("In_Water").IsPlaying() && !GetNode<AnimationPlayer>("Out_Water").IsPlaying())
            {
                in_water = false;
                now_in_water = false;
            }

            for (int i = 0; i < All_Boom_Area_2D_List.Count; i++)
            {
                if (All_Boom_Area_2D_List[i].can_do && !All_Boom_Area_2D_List[i].end_hurt)
                {
                    if (!Never_Died)
                    {
                        health_list[0].Health -= All_Boom_Area_2D_List[i].hurt;
                    }
                    count_health = 0;
                    for (int j = 0; j < health_list.Count; j++)
                    {
                        if (health_list[j].Health < 0)
                        {
                            if (health_list[j].Is_lock)
                            {
                                health_list[j].Health = 0;
                            }
                            else
                            {
                                if (j + 1 < health_list.Count)
                                {
                                    health_list[j + 1].Health += health_list[j].Health;
                                    health_list[j].Health = 0;
                                }
                            }
                        }
                        count_health += health_list[j].Health;
                    }
                    health = count_health;
                    All_Boom_Area_2D_List.RemoveAt(i);
                    i--;
                }
            }

            if (Zombies_Area_2D_List.Count == 0)
            {
                Zombies_Area_2D = null;
            }
            Normal_Zombies_Area can_use_area = null;
            bool can_used = false;
            for (int i = 0; i < Zombies_Area_2D_List.Count; i++)
            {
                if (Zombies_Area_2D_List[i] == null)
                {
                    i--;
                    continue;
                }
                if (!Zombies_Area_2D_List[i].has_plant || Zombies_Area_2D_List[i].has_lose_head)
                {
                    continue;
                }
                if (!(Get_Other_ZIndex(Zombies_Area_2D_List[i]) - (this.ZIndex - this.normal_ZIndex) < 20 && Get_Other_ZIndex(Zombies_Area_2D_List[i]) - (this.ZIndex - this.normal_ZIndex) > 0))
                {
                    continue;
                }
                if (!Zombies_Area_2D_List[i].Monitorable)
                {
                    Zombies_Area_2D_List.RemoveAt(i);
                    i--;
                    continue;
                }
                if (can_use_area == null)
                {
                    can_use_area = Zombies_Area_2D_List[i];
                }
                if (Zombies_Area_2D == null)
                {
                    Zombies_Area_2D = Zombies_Area_2D_List[i];
                }
                else
                {
                    if (!Zombies_Area_2D.Monitoring)
                    {
                        Zombies_Area_2D = null;
                        continue;
                    }
                    try
                    {
                        if (Zombies_Area_2D_List[i].ZIndex > Zombies_Area_2D.ZIndex) { }
                    }
                    catch (Exception)
                    {
                        Zombies_Area_2D = Zombies_Area_2D_List[i];//合理密植问题
                    }
                    if (Zombies_Area_2D_List[i].ZIndex > Zombies_Area_2D.ZIndex)
                    {
                        Zombies_Area_2D = Zombies_Area_2D_List[i];
                    }
                    else if (Zombies_Area_2D_List[i].ZIndex == Zombies_Area_2D.ZIndex)
                    {
                        if (Zombies_Area_2D_List[i].GetParent().GetParent().GetIndex() > Zombies_Area_2D.GetParent().GetParent().GetIndex())
                        {
                            Zombies_Area_2D = Zombies_Area_2D_List[i];
                        }
                    }
                    if (Zombies_Area_2D_List.Count == 1)
                    {
                        Zombies_Area_2D = Zombies_Area_2D_List[0];
                    }
                }
                if (Zombies_Area_2D == Zombies_Area_2D_List[i])
                {
                    can_used = true;
                }
            }
            if (!can_used)
            {
                Zombies_Area_2D = can_use_area;
            }
            if (Zombies_Area_2D_List.Count != 0)
            {
                for (int i = 0; i < Zombies_Area_2D_List.Count; i++)
                {
                    if (Zombies_Area_2D_List[i].has_lose_head)
                    {
                        Zombies_Area_2D_List.RemoveAt(i);
                        i--;
                        continue;
                    }
                    if (!(Get_Other_ZIndex(Zombies_Area_2D_List[i])  - (this.ZIndex - this.normal_ZIndex) < 20 && Get_Other_ZIndex(Zombies_Area_2D_List[i]) - (this.ZIndex - this.normal_ZIndex) > 0))
                    {
                        continue;
                    }
                    if (Zombies_Area_2D_List[i].Choose_Plants_Area == GetNode<Normal_Plants_Area>("Main/Main/Zombies_Area"))
                    {
                        if (Zombies_Area_2D_List[i].is_eating_show && !Zombies_Area_2D_List[i].has_lose_head)
                        {
                            Zombies_Area_2D_List[i].is_eating_show = false;
                            if (!Never_Died)
                            {
                                health_list[0].Health -= Zombies_Area_2D_List[i].hurt;
                            }
                            GetNode<AnimationPlayer>("Is_Eated").Play("Is_Eated");
                        }
                    }
                }
            }
            if (!eating && Get_Walk_Mode() && !is_Angry && !dancing_stop) 
            {
                this.Position += new Vector2(speed * delta * 60, 0);
            }
            if (GetNode<Control>("Main/Main").RectRotation < -45)
            {
                GetNode<AnimationPlayer>("Main/Main/Eating").Stop();
                Walk_Mode(false);
            }
            if (Doing_jumping)
            {
                Walk_Mode(false);
            }
            if (is_Angry)
            {
                Walk_Mode(false);
                GetNode<AnimationPlayer>("Main/Main/Eating").Stop();
            }
            if (Zombies_Area_2D != null && !has_lose_Head && can_Eating)
            {
                eating = true;
                Walk_Mode(false);
                if (dancing_stop)
                {
                    GetNode<AnimationPlayer>("Main/Main/Eating").Stop();
                }
                else
                {
                    GetNode<AnimationPlayer>("Main/Main/Eating").Play("Eating");
                }
            }
            else
            {
                eating = false;
                GetNode<AnimationPlayer>("Main/Main/Eating").Stop();
                if (GetNode<Control>("Main/Main").RectRotation > -45 && !Doing_jumping)
                {
                    Walk_Mode(true);
                }
            }
            if (Position.x > 1400)
            {
                GetNode<Normal_Plants_Zombies_Area>("Main/Main/Zombies_Area").Monitoring = false;
                GetNode<Normal_Plants_Zombies_Area>("Main/Main/Zombies_Area").Monitorable = false;
            }
            if (Position.x > 1437)
            {
                Free_Self();
            }
        }
    }
    protected virtual void Plants_Entered(Area2D area_node)//函数名为历史遗留问题
    {
        try
        {
            if (!(area_node is Control_Area_2D area2D) || !IsInstanceValid(area2D))
            {
                return;
            }
            string Type_string = area2D?.Area2D_type;
            if (Type_string != null && Type_string == "Zombies")
            {
                Zombies_Area_2D_List.Add((Normal_Zombies_Area)area2D);
            }
            else if (Type_string != null && Type_string == "All_Boom")
            {
                All_Boom_Area_2D_List.Add((All_Boom_Area)area2D);
            }
        }
        catch (Exception ex)
        {
            GD.Print(ex.Message);
        }
    }
    protected virtual void Plants_Exited(Area2D area_node)
    {
        try
        {
            if (!(area_node is Control_Area_2D area2D) || !IsInstanceValid(area2D))
            {
                return;
            }
            string Type_string = area2D?.Area2D_type;
            if (Type_string != null && Type_string == "Zombies")
            {
                Zombies_Area_2D_List.Remove((Normal_Zombies_Area)area2D);
            }
            else if (Type_string != null && Type_string == "All_Boom")
            {
                All_Boom_Area_2D_List.Remove((All_Boom_Area)area2D);
            }
        }
        catch (Exception ex)
        {
            GD.Print(ex.Message);
        }
    }
    protected virtual void Dock_Entered(Area2D area_node)
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
        else if (Type_string != null && Type_string == "Out_Land")
        {
            Out_Land_Area_2D_List.Add((Background_Out_Land_Main)area2D);
        }
        else if (Type_string != null && Type_string == "Water_Area")//H2O
        {
            Water_Area_List.Add((Water_Area)area2D);
        }
    }
    protected virtual void Dock_Exited(Area2D area_node)
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
        else if (Type_string != null && Type_string == "Out_Land")
        {
            Out_Land_Area_2D_List.Remove((Background_Out_Land_Main)area2D);
        }
        else if (Type_string != null && Type_string == "Water_Area")//H2O
        {
            Water_Area_List.Remove((Water_Area)area2D);
        }
    }
    protected virtual void Walk_Mode(bool is_Walking)
    { }
    protected virtual bool Get_Walk_Mode()
    {
        return false;//形式意义
    }
    protected virtual void Re_Set_Process()
    { }
    protected virtual void Spec_Doing()
    { }
    public void Hurt_Timer_Out()
    {
        //WaterPH
        for (int i = 0; i < Water_Area_List.Count; i++)
        {
            if (Water_Area_List[i].pH < 5)
            {
                health_list[health_list.Get_Can_Through_To_Index()].Health -= (int)(Math.Pow(3, (5f - Water_Area_List[i].pH))) * 2;
            }
            else if (Water_Area_List[i].pH > 9)
            {
                health_list[health_list.Get_Can_Through_To_Index()].Health -= (int)(Math.Pow(3, (Water_Area_List[i].pH - 9))) * 3;
            }
        }
        //WaterPH
        //GroundPH
        if (Dock_Area_2D != null)
        {
            if (Dock_Area_2D.pH < 5)
            {
                health_list[health_list.Get_Can_Through_To_Index()].Health -= (int)(Math.Pow(3, (5f - Dock_Area_2D.pH))) * 2;
            }
            else if (Dock_Area_2D.pH > 9)
            {
                health_list[health_list.Get_Can_Through_To_Index()].Health -= (int)(Math.Pow(3, (Dock_Area_2D.pH - 9))) * 3;
            }
        }
        //GroundPH
    }
    public int Get_Other_ZIndex(Node area_node)
    {
        bool is_zombies = area_node.GetParent().GetParent().Name == "Main";
        if (is_zombies)
        {
            return area_node.GetNode<Node2D>("../../..").ZIndex;
        }
        else
        {
            return area_node.GetNode<Node2D>("../..").ZIndex;
        }
    }
    protected virtual async void Free_Self()
    {
        if (GetNode<Area2D>("Main/Main/Zombies_Area").IsConnected("area_entered", this, nameof(Plants_Entered)))
        {
            GetNode<Area2D>("Main/Main/Zombies_Area").Disconnect("area_entered", this, nameof(Plants_Entered));
            GetNode<Area2D>("Main/Main/Zombies_Area").Disconnect("area_exited", this, nameof(Plants_Exited));
            GetNode<Area2D>("Dock/Area2D").Disconnect("area_entered", this, nameof(Dock_Entered));
            GetNode<Area2D>("Dock/Area2D").Disconnect("area_exited", this, nameof(Dock_Exited));
            GetNode<Area2D>("Main/Main/Zombies_Area").Monitoring = false;
            GetNode<Area2D>("Main/Main/Zombies_Area").Monitorable = false;
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
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventScreenTouch touchEvent)
        {
            real_touching = touchEvent.Device != -1;//真触摸
        }
    }
}
