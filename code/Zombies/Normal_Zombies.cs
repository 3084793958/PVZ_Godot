using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
public class Normal_Zombies : Node2D
{
    //lock_protect
    private readonly object _listLock = new object();
    //lock
    //define
    [Export] protected int normal_ZIndex = 7;
    [Export] protected Color hurt_color = new Color(1.3f, 1.3f, 1.3f, 1f);
    [Export] protected Color normal_color = new Color(1f, 1f, 1f, 1f);
    [Export] protected Color Ice_hurt_color = new Color(0f, 0.77f, 1.3f, 1f);
    [Export] protected Color Ice_normal_color = new Color(0f, 0.58f, 1f, 1f);
    protected Timer Android_Timer = new Timer();
    protected bool Is_Double_Click = false;//for Android
    public Vector2 put_position = Vector2.Zero;//In_Game_Main使用
    public bool player_put = false;//In_Game_Main使用
    protected List<Health_Container> health_list = new List<Health_Container>()
    {
        new Health_Container(270,false)
    };
    protected int health = 270;
    [Export] protected float speed_Normal = -1.5f;
    [Export] protected bool can_Eating = true;
    protected bool Doing_jumping = false;//for Pole...
    protected int eating_speed = 1;
    protected bool Allow_Hurt_Time = true;
    protected float speed_x = 1f;
    protected bool has_planted = false;
    protected bool is_Ice = false;
    protected bool is_Lock_Ice = false;
    protected bool On_Boom_Effect = false;
    protected bool Is_Shining = false;
    protected bool on_lock_grid = false;
    protected bool has_lose_Arm = false;
    protected bool has_lose_Head = false;
    protected bool has_Lose_Number = false;
    protected bool has_Lose_Hat = false;
    protected bool has_Bullets_Sound = false;//特殊音效
    protected float speed = 0f;
    protected bool eating = false;
    protected int hurt_time = 0;
    protected bool in_water = false;
    protected bool now_in_water = false;
    public Card_Click_Button card_parent_Button = null;//Card使用
    public bool Tmp_Card_Used = false;
    public Card_Tmp_Main Tmp_card_parent = null;//Card_Tmp使用
    protected List<Normal_Plants_Bullets_Area> Bullets_Area_2D_List = new List<Normal_Plants_Bullets_Area>();
    protected List<Mg_Shining_Area> Shining_Area_2D_List = new List<Mg_Shining_Area>();
    protected List<H2SO4_Area2D> H2SO4_Area_2D_List = new List<H2SO4_Area2D>();
    protected List<Normal_Plants_Area> Plants_Area_2D_List = new List<Normal_Plants_Area>();
    protected List<Normal_Boom_Area> Boom_Area_2D_List = new List<Normal_Boom_Area>();
    protected List<C2H5OH_Died_Fire_Area> C2H5OH_Fire_Area_2D_List = new List<C2H5OH_Died_Fire_Area>();
    protected List<Crash_Area_2D> Crash_Area_2D_List = new List<Crash_Area_2D>();
    protected List<Eating_Flower_Area> Eating_Flower_Area_2D_List = new List<Eating_Flower_Area>();
    protected List<Car_Area2D> Car_Area_2D_List = new List<Car_Area2D>();
    protected List<Background_Grid_Main> Dock_Area_2D_List = new List<Background_Grid_Main>();
    protected Background_Grid_Main Dock_Area_2D = null;
    protected Normal_Plants_Area Plants_Area_2D = null;
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
        GetNode<AudioStreamPlayer>("Plant_Sound/Ok/Plant1").Stream.Set("loop", false);
        GetNode<AudioStreamPlayer>("Plant_Sound/Ok/Plant2").Stream.Set("loop", false);
        GetNode<AudioStreamPlayer>("Plant_Sound/Ok/Water").Stream.Set("loop", false);
        GetNode<AudioStreamPlayer>("Main/Main/Eat_Sound1").Stream.Set("loop", false);
        GetNode<AudioStreamPlayer>("Main/Main/Eat_Sound2").Stream.Set("loop", false);
        GetNode<AudioStreamPlayer>("Main/Main/Fall").Stream.Set("loop", false);
        speed = speed_Normal * (float)GD.RandRange(0.1, 0.2);
        if (!player_put)
        {
            has_planted = true;
            GetNode<Control>("Dock").Hide();
            GetNode<Control>("Show").Hide();
            GetNode<Control>("Main").Show();
            Walk_Mode(true);
            Position = put_position;
            GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area").has_plant = true;
            In_Game_Main.Zombies_Number++;
        }
    }
    public override void _PhysicsProcess(float delta)
    {
        if (!GetNode<Area2D>("Main/Main/Zombies_Area").IsConnected("area_entered", this, nameof(Plants_Entered)))
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
            in_water = Dock_Area_2D.type == 2;
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
                            }
                            has_planted = true;
                            GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area").has_plant = true;
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
                            Walk_Mode(true);
                            In_Game_Main.Zombies_Number++;
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
            GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area").Choose_Plants_Area = Plants_Area_2D;
            GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area").has_lose_head = has_lose_Head;
            GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area").health = health;
            if (health <= 0 || !Public_Main.Show_Zombies_Health || On_Boom_Effect) 
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
                GetNode<Label>("Health/Health").RectGlobalPosition = new Vector2(GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area").GlobalPosition.x - GetNode<Label>("Health/Health").RectSize.x * GetNode<Label>("Health/Health").RectScale.x / 2, GetNode<Control>("Main/Main").RectGlobalPosition.y - 64 - GetNode<Label>("Health/Health").RectSize.y * GetNode<Label>("Health/Health").RectScale.y);
                GetNode<Node2D>("Health").ZIndex = 116;
                GetNode<Node2D>("Health").Show();
            }
            if (Position.x < -1437 / 2)
            {
                this.Free_Self();
                this.Remove_Zombies_Number();
            }
            if (!In_Game_Main.has_Lost_Brain && GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area").GlobalPosition.x < -50 && !has_lose_Head)
            {
                In_Game_Main.Lost_Brain = true;
            }
            if (GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area").lose_health)
            {
                GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area").lose_health = false;
                health_list[0].Health -= GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area").lose_health_number;
            }
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
            if (!has_lose_Head)
            {
                if (!is_Ice)
                {
                    speed_x = 1f;
                }
                else
                {
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
            GetNode<AnimationPlayer>("Main/Main/Eating").PlaybackSpeed = eating_speed * speed_x;
            Allow_Hurt_Time = !GetNode<AnimationPlayer>("Is_Eated").IsPlaying();
            if (Car_Area_2D_List.Count > 0)
            {
                if (this.has_planted)
                {
                    this.has_lose_Head = true;
                    this.eating = false;
                    this.speed = 0f;
                    Walk_Mode(false);
                    GetNode<AnimationPlayer>("Main/Main/Eating").Stop();
                    if (is_Ice)
                    {
                        Allow_Hurt_Time = false;
                        this.Modulate = this.Ice_hurt_color;
                        GetNode<AnimationPlayer>("Main/Main/Lose_Head_ICE").Play("Lose_Head");
                    }
                    else
                    {
                        Allow_Hurt_Time = false;
                        this.Modulate = this.hurt_color;
                        GetNode<AnimationPlayer>("Main/Main/Lose_Head").Play("Lose_Head");
                    }
                }
            }
            for (int i = 0; i < Eating_Flower_Area_2D_List.Count; i++)
            {
                if (!Eating_Flower_Area_2D_List[i].can_eat)
                {
                    continue;
                }
                if (has_planted && !GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area").Should_Ignore && !Doing_jumping) 
                {
                    if (Eating_Flower_Area_2D_List[i].Choose_Eating_Zombies_Area != null) 
                    {
                        continue;
                    }
                    Eating_Flower_Area_2D_List[i].Choose_Eating_Zombies_Area = GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area");
                    if (!GetNode<AnimationPlayer>("Be_Eated").IsPlaying())
                    {
                        GetNode<AnimationPlayer>("Be_Eated").Play("Eat");
                    }
                    //GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area").Choose_Eating_Flower_Area = Eating_Flower_Area_2D_List[i];
                    Eating_Flower_Area_2D_List.Clear();//不留后患
                    break;
                }
            }
            for (int i = 0; i < Crash_Area_2D_List.Count; i++)
            {
                if (Crash_Area_2D_List[i].has_planted && Crash_Area_2D_List[i].Crash_Area == GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area"))
                {
                    health_list[0].Health -= Crash_Area_2D_List[i].hurt;
                    if (!this.On_Boom_Effect)
                    {
                        if (is_Ice)
                        {
                            GetNode<AnimationPlayer>("Is_Eated_Ice").Play("Is_Eated");
                        }
                        else
                        {
                            GetNode<AnimationPlayer>("Is_Eated").Play("Is_Eated");
                        }
                    }
                    Crash_Area_2D_List.RemoveAt(i);
                    i--;
                }
            }
            for (int i = 0; i < Boom_Area_2D_List.Count; i++)
            {
                if (Boom_Area_2D_List[i].can_do && !Boom_Area_2D_List[i].end_hurt)
                {
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
                    if (Boom_Area_2D_List[i].hurt >= health - 90)
                    {
                        On_Boom_Effect = true;
                        Walk_Mode(false);
                        GetNode<AnimationPlayer>("Main/Main/Eating").Stop();
                        GetNode<AnimationPlayer>("Main/Main/Boom_Effect").Play("Boom_Effect");
                    }
                    else
                    {
                        health_list[0].Health -= Boom_Area_2D_List[i].hurt;
                    }
                    Boom_Area_2D_List.RemoveAt(i);
                    i--;
                }
            }
            if (On_Boom_Effect)
            {
                health = -1437;//防止复活
                Walk_Mode(false);
            }
            if (GetNode<Timer>("Main/Main/Fire_Hurt").IsStopped())
            {
                for (int i = 0; i < C2H5OH_Fire_Area_2D_List.Count; i++)
                {
                    if (C2H5OH_Fire_Area_2D_List[i].died)
                    {
                        health_list[0].Health -= C2H5OH_Fire_Area_2D_List[i].hurt;
                        if (!this.On_Boom_Effect)
                        {
                            Allow_Hurt_Time = false;
                            this.Modulate = hurt_color;
                            is_Ice = false;
                            is_Lock_Ice = false;
                        }
                    }
                }
                for (int i = 0; i < Shining_Area_2D_List.Count; i++)
                {
                    if (Shining_Area_2D_List[i].start)
                    {
                        Is_Shining = true;
                        if (hurt_time < 20)
                        {
                            hurt_time += 10;
                        }
                        health_list[0].Health -= 10;
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
            if (Shining_Area_2D_List.Count == 0)
            {
                Is_Shining = false;
            }
            for (int i = 0; i < H2SO4_Area_2D_List.Count; i++)
            {
                if (H2SO4_Area_2D_List[i].has_died && !H2SO4_Area_2D_List[i].has_become)
                {
                    health_list[0].Health -= H2SO4_Area_2D_List[i].hurt;
                    H2SO4_Area_2D_List.RemoveAt(i);
                    i--;
                }
            }
            for (int i = 0; i < Bullets_Area_2D_List.Count; i++)
            {
                if (Bullets_Area_2D_List[i] == null) 
                {
                    Bullets_Area_2D_List.RemoveAt(i);
                    i--;
                    continue;
                }
                if (!Bullets_Area_2D_List[i].Monitoring)
                {
                    Bullets_Area_2D_List.RemoveAt(i);
                    i--;
                    continue;
                }
                if (Bullets_Area_2D_List[i].Sec_Info == "Pea")
                {
                    if (Bullets_Area_2D_List[i].hurt_type == 2 || Bullets_Area_2D_List[i].Choose_Zombies_Area == GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area"))
                    {
                        if (hurt_time < 20)
                        {
                            hurt_time += 15;
                        }
                        health_list[0].Health -= Bullets_Area_2D_List[i].hurt;
                        if (has_Bullets_Sound && !has_Lose_Hat) 
                        {
                            Bullets_Sound_Play();
                        }
                    }
                }
                else if (Bullets_Area_2D_List[i].Sec_Info == "Ice_Pea")
                {
                    if (Bullets_Area_2D_List[i].hurt_type == 2 || Bullets_Area_2D_List[i].Choose_Zombies_Area == GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area"))
                    {
                        if (hurt_time < 20)
                        {
                            hurt_time += 15;
                        }
                        health_list[0].Health -= Bullets_Area_2D_List[i].hurt;
                        if (has_Bullets_Sound && !has_Lose_Hat)
                        {
                            Bullets_Sound_Play();
                        }
                        is_Ice = true;
                        if (Bullets_Area_2D_List[i].Bullets_Type == 3)
                        {
                            is_Lock_Ice = true;
                            GetNode<Sprite>("Main/Main/Ice_Lock").Show();
                            speed_x = 0f;
                            GetNode<Timer>("Ice_Lock_Timer").Start();
                        }
                        GetNode<Timer>("Ice_Timer").Start();
                    }
                }
                Bullets_Area_2D_List.RemoveAt(i);
                i--;
            }
            if (hurt_time > 0)
            {
                hurt_time -= (int)(delta * 60);
            }
            if (!On_Boom_Effect && Allow_Hurt_Time) 
            {
                if (hurt_time == 0)
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
            if (Plants_Area_2D_List.Count == 0)
            {
                Plants_Area_2D = null;
            }
            Normal_Plants_Area can_use_area = null;
            bool can_used = false;
            for (int i = 0; i < Plants_Area_2D_List.Count; i++)
            {
                if (Plants_Area_2D_List[i] == null)
                {
                    Plants_Area_2D_List.RemoveAt(i);
                    i--;
                    continue;
                }
                if (!Plants_Area_2D_List[i].has_planted) 
                {
                    continue;
                }
                if (!Plants_Area_2D_List[i].Monitorable)
                {
                    Plants_Area_2D_List.RemoveAt(i);
                    i--;
                    continue;
                }
                if (Plants_Area_2D_List[i].is_MG)
                {
                    Plants_Area_2D_List.RemoveAt(i);
                    i--;
                    continue;
                }
                if (Plants_Area_2D_List[i].Sec_Info == "Zombies")
                {
                    if (((Normal_Plants_Zombies_Area)Plants_Area_2D_List[i]).has_lose_head)
                    {
                        Plants_Area_2D_List.RemoveAt(i);
                        i--;
                        continue;
                    }
                }
                if (can_use_area == null)
                {
                    can_use_area = Plants_Area_2D_List[i];
                }
                if (Plants_Area_2D == null)
                {
                    Plants_Area_2D = Plants_Area_2D_List[i];
                }
                else
                {
                    if (!Plants_Area_2D.Monitoring)
                    {
                        Plants_Area_2D = null;
                        continue;
                    }
                    if (Plants_Area_2D_List[i].Sec_Info == "Zombies")
                    {
                        Plants_Area_2D = Plants_Area_2D_List[i];
                    }
                    else
                    {
                        if (Plants_Area_2D.plants_type == Plants_Area_2D_List[i].plants_type)
                        {
                            try
                            {
                                if (Plants_Area_2D_List[i].ZIndex > Plants_Area_2D.ZIndex) { }
                            }
                            catch(Exception)
                            {
                                Plants_Area_2D = Plants_Area_2D_List[i];//合理密植问题
                            }
                            if (Plants_Area_2D_List[i].ZIndex > Plants_Area_2D.ZIndex)
                            {
                                Plants_Area_2D = Plants_Area_2D_List[i];
                            }
                            else if (Plants_Area_2D_List[i].ZIndex == Plants_Area_2D.ZIndex)
                            {
                                if (Plants_Area_2D_List[i].GetParent().GetParent().GetIndex() > Plants_Area_2D.GetParent().GetParent().GetIndex())
                                {
                                    Plants_Area_2D = Plants_Area_2D_List[i];
                                }
                            }
                        }
                        else
                        {
                            int Top_Area_2D_Plants_type = 1;
                            int List_Plants_type = 1;
                            if (Plants_Area_2D.plants_type == "Casing")
                            {
                                Top_Area_2D_Plants_type = 4;
                            }
                            else if (Plants_Area_2D.plants_type == "Top")
                            {
                                Top_Area_2D_Plants_type = 3;
                            }
                            else if (Plants_Area_2D.plants_type == "Normal")
                            {
                                Top_Area_2D_Plants_type = 2;
                            }
                            else if (Plants_Area_2D.plants_type == "Down")
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
                                Plants_Area_2D = Plants_Area_2D_List[i];
                            }
                        }
                    }
                }
                if (Plants_Area_2D == Plants_Area_2D_List[i])
                {
                    can_used = true;
                }
            }
            if (!can_used)
            {
                Plants_Area_2D = can_use_area;
            }
            for (int i = 0; i < Plants_Area_2D_List.Count; i++)
            {
                if (Plants_Area_2D_List[i].Sec_Info == "Zombies")
                {
                    var Plants_Zombies = (Normal_Plants_Zombies_Area)Plants_Area_2D_List[i];
                    if (Plants_Zombies.Choose_Zombies_Area == GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area"))
                    {
                        if (Plants_Zombies.has_lose_head)
                        {
                            Plants_Area_2D_List.RemoveAt(i);
                            i--;
                            continue;
                        }
                        if (Plants_Zombies.is_eating_show && !Plants_Zombies.has_lose_head)
                        {
                            Plants_Zombies.is_eating_show = false;
                            health_list[0].Health -= Plants_Zombies.hurt;
                            if (is_Ice)
                            {
                                GetNode<AnimationPlayer>("Is_Eated_Ice").Play("Is_Eated");
                            }
                            else
                            {
                                GetNode<AnimationPlayer>("Is_Eated").Play("Is_Eated");
                            }
                        }
                    }
                }
            }//zombies
            if (!eating && Get_Walk_Mode() && !On_Boom_Effect && !Is_Shining && !is_Lock_Ice)
            {
                this.Position += new Vector2(speed * speed_x * delta * 60, 0);
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
            if (Plants_Area_2D != null && !has_lose_Head && can_Eating)
            {
                eating = true;
                Walk_Mode(false);
                GetNode<AnimationPlayer>("Main/Main/Eating").Play("Eating");
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
        }
    }
    protected virtual void Plants_Entered(Area2D area_node)
    {
        try
        {
            if (!(area_node is Control_Area_2D area2D) || !IsInstanceValid(area2D))
            {
                return;
            }
            string Type_string = area2D?.Area2D_type;
            if (Type_string != null && (Type_string == "Zombies" || Type_string == "Shovel" || Type_string == "Bug")) 
            {
                return;
            }
            lock (_listLock)
            {
                if (!IsInstanceValid(area2D))
                {
                    return;
                }
                if (Type_string != null && Type_string == "Plants")
                {
                    Plants_Area_2D_List.Add((Normal_Plants_Area)area2D);
                }
                else if (Type_string != null && Type_string == "Plants_Bullets")
                {
                    Bullets_Area_2D_List.Add((Normal_Plants_Bullets_Area)area2D);
                }
                else if (Type_string != null && Type_string == "Plants_Boom")
                {
                    Boom_Area_2D_List.Add((Normal_Boom_Area)area2D);
                }
                else if (Type_string != null && Type_string == "Died_Fire")
                {
                    C2H5OH_Fire_Area_2D_List.Add((C2H5OH_Died_Fire_Area)area2D);
                }
                else if (Type_string != null && Type_string == "Mg_Shining")
                {
                    Shining_Area_2D_List.Add((Mg_Shining_Area)area2D);
                }
                else if (Type_string != null && Type_string == "H2SO4")
                {
                    H2SO4_Area_2D_List.Add((H2SO4_Area2D)area2D);
                }
                else if (Type_string != null && Type_string == "Crash_Hurt")
                {
                    Crash_Area_2D_List.Add((Crash_Area_2D)area2D);
                }
                else if (Type_string != null && Type_string == "Eating_Flower")
                {
                    Eating_Flower_Area_2D_List.Add((Eating_Flower_Area)area2D);
                }
                else if (Type_string != null && Type_string == "Car")
                {
                    Car_Area_2D_List.Add((Car_Area2D)area2D);
                }
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
            if (!IsInstanceValid(area2D)) 
            {
                return;
            }
            string Type_string = area2D?.Area2D_type;
            if (Type_string != null && (Type_string == "Zombies" || Type_string == "Shovel" || Type_string == "Bug"))
            {
                return;
            }
            lock (_listLock)
            {
                if (!IsInstanceValid(area2D))
                {
                    return;
                }
                if (Type_string != null && Type_string == "Plants")
                {
                    Plants_Area_2D_List.Remove((Normal_Plants_Area)area2D);
                }
                else if (Type_string != null && Type_string == "Plants_Bullets")
                {
                    Bullets_Area_2D_List.Remove((Normal_Plants_Bullets_Area)area2D);
                }
                else if (Type_string != null && Type_string == "Plants_Boom")
                {
                    Boom_Area_2D_List.Remove((Normal_Boom_Area)area2D);
                }
                else if (Type_string != null && Type_string == "Died_Fire")
                {
                    C2H5OH_Fire_Area_2D_List.Remove((C2H5OH_Died_Fire_Area)area2D);
                }
                else if (Type_string != null && Type_string == "Mg_Shining")
                {
                    Shining_Area_2D_List.Remove((Mg_Shining_Area)area2D);
                }
                else if (Type_string != null && Type_string == "H2SO4")
                {
                    H2SO4_Area_2D_List.Remove((H2SO4_Area2D)area2D);
                }
                else if (Type_string != null && Type_string == "Crash_Hurt")
                {
                    Crash_Area_2D_List.Remove((Crash_Area_2D)area2D);
                }
                else if (Type_string != null && Type_string == "Eating_Flower")
                {
                    Eating_Flower_Area_2D_List.Remove((Eating_Flower_Area)area2D);
                }
                else if (Type_string != null && Type_string == "Car")
                {
                    Car_Area_2D_List.Remove((Car_Area2D)area2D);
                }
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
    }
    protected virtual void Remove_Zombies_Number()
    {
        if (!has_Lose_Number)
        {
            has_Lose_Number = true;
            In_Game_Main.Zombies_Number--;
            In_Game_Main.Last_Zombies_Pos = GetNode<Normal_Zombies_Area>("Main/Main/Zombies_Area").GlobalPosition;
        }
    }
    protected virtual void Ice_Timer_timeout()
    {
        if (!has_lose_Head)
        {
            is_Ice = false;
            this.Modulate = normal_color;
        }
    }
    protected virtual void Ice_Lock_Timer_timeout()
    {
        if (!has_lose_Head)
        {
            is_Lock_Ice = false;
            GetNode<Sprite>("Main/Main/Ice_Lock").Hide();
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
        await ToSignal(GetTree().CreateTimer(0.72f), "timeout");
        if (IsInstanceValid(this))
        {
            this.QueueFree();
        }
    }
    protected virtual void Walk_Mode(bool is_Walking)
    { }
    protected virtual bool Get_Walk_Mode()
    {
        return false;//形式意义
    }
    protected virtual void Bullets_Sound_Play()
    { }
}
