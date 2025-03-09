using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
public class Normal_Plants : Node2D
{
    //lock_protect
    private readonly object _listLock = new object();
    //lock
    //SPEC
    static public bool Choosing = false;
    //SPEC
    //define
    protected Timer Android_Timer = new Timer();
    protected bool Is_Double_Click = false;
    [Export] protected Color hover_color = new Color(1.3f, 1.3f, 1.3f, 1f);
    [Export] protected Color normal_color = new Color(1f, 1f, 1f, 1f);
    public Card_Click_Button card_parent_Button = null;//Card使用
    protected List<Background_Grid_Main> Dock_Area_2D_List = new List<Background_Grid_Main>();
    protected Background_Grid_Main Dock_Area_2D = null;
    protected bool on_lock_grid = false;
    protected bool has_planted = false;
    protected bool on_Shovel = false;
    protected bool on_Bug = false;
    protected Shovel_Area2D Shovel_Area = null;
    protected Bug_Area2D Bug_Area = null;
    protected Normal_Zombies_Area Zombies_Area_2D = null;
    protected List<Normal_Zombies_Area> Zombies_Area_2D_List = new List<Normal_Zombies_Area>();
    protected Normal_Zombies_Area Bullets_Zombies_Area_2D = null;//Van_Door
    protected List<Normal_Zombies_Area> Bullets_Zombies_Area_2D_List = new List<Normal_Zombies_Area>();//Van_Door
    [Export] protected int health = 300;
    [Export] protected int normal_ZIndex = 3;
    protected bool just_for_MG = false;
    protected bool just_for_C2H5OH = false;
    //define
    protected static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (true/*Public_Main.debuging*/)
        {
            GD.Print("异常:" + e.ExceptionObject);//Console似乎无反应
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
        GetNode<Area2D>("Main/Shovel_Area").PauseMode = PauseModeEnum.Process;
        GetNode<Area2D>("Dock/Area2D").PauseMode = PauseModeEnum.Process;
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;//C#核心技术
        this.AddChild(Android_Timer);
        Android_Timer.WaitTime = 0.3f;
        Android_Timer.Autostart = false;
        Android_Timer.OneShot = true;
        GetNode<AudioStreamPlayer>("Plant_Sound/Ok/Plant1").Stream.Set("loop", false);
        GetNode<AudioStreamPlayer>("Plant_Sound/Ok/Plant2").Stream.Set("loop", false);
        GetNode<AudioStreamPlayer>("Plant_Sound/Ok/Water").Stream.Set("loop", false);
        GetNode<AudioStreamPlayer>("Big_Chmop").Stream.Set("loop", false);
    }
    public override void _PhysicsProcess(float delta)
    {
        if (!just_for_MG && !GetNode<Area2D>("Main/Shovel_Area").IsConnected("area_entered", this, nameof(Area_Entered))) 
        {
            return;
        }
        if (just_for_MG && !GetNode<Area2D>("Main/Touch_Area").IsConnected("area_entered", this, nameof(Area_Entered)))
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
                    card_parent_Button.Set_ColorRect_2(false);
                    this.Free();
                }
                if ((Input.IsActionPressed("Left_Mouse") && !Public_Main.for_Android) || (Public_Main.for_Android && Is_Double_Click))
                {
                    card_parent_Button.Set_ColorRect_2(false);
                    Normal_Plants.Choosing = false;
                    if (Dock_Area_2D != null)
                    {
                        if (Allow_Plants())
                        {
                            has_planted = true;
                            Plants_Add_List();
                            this.GlobalPosition = Dock_Area_2D.GlobalPosition;
                            In_Game_Main.Sun_Number -= card_parent_Button.sun;
                            In_Game_Main.Update_Sun(this);
                            card_parent_Button.now_time = card_parent_Button.wait_time;
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
                            Plants_Init();
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
            if (Bug_Area != null)
            {
                if (Bug_Area.Choose_Plants_Area == GetNode<Normal_Plants_Area>("Main/Shovel_Area"))
                {
                    this.Modulate = hover_color;
                    on_Bug = true;
                }
            }
            else
            {
                on_Bug = false;
            }
            if (!GetNode<AnimationPlayer>("Is_Eated").IsPlaying() && !on_Bug && !on_Shovel) 
            {
                this.Modulate = normal_color;
            }
            if (just_for_C2H5OH)
            {
                if (GetNode<AnimationPlayer>("Died1").IsPlaying() || GetNode<AnimationPlayer>("Died2").IsPlaying()) 
                {
                    GetNode<Normal_Plants_Area>("Main/Shovel_Area").Monitoring = false;
                    GetNode<Normal_Plants_Area>("Main/Shovel_Area").Monitorable = false;
                }
            }
            else
            {
                if (GetNode<AnimationPlayer>("Died").IsPlaying())
                {
                    GetNode<Normal_Plants_Area>("Main/Shovel_Area").Monitoring = false;
                    GetNode<Normal_Plants_Area>("Main/Shovel_Area").Monitorable = false;
                }
            }
            if (GetNode<Normal_Plants_Area>("Main/Shovel_Area").lose_health)
            {
                GetNode<Normal_Plants_Area>("Main/Shovel_Area").lose_health = false;
                health -= GetNode<Normal_Plants_Area>("Main/Shovel_Area").lose_health_number;
            }
            if (on_Shovel && ((Input.IsActionPressed("Left_Mouse") && !Public_Main.for_Android) || (Public_Main.for_Android && Is_Double_Click)))
            {
                if (!GetNode<AnimationPlayer>("Free_player").IsPlaying())
                {
                    if (Shovel_Area != null)
                    {
                        if (Shovel_Area.Choose_Plants_Area == GetNode<Normal_Plants_Area>("Main/Shovel_Area"))
                        {
                            Plants_Remove_List();
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
                            GetNode<AnimationPlayer>("Bug_player").Play("Bug");
                        }
                    }
                }
            }
        }
    }
    protected virtual void Area_Entered(Area2D area_node)
    {
        try
        {
            if (!(area_node is Control_Area_2D area2D) || !IsInstanceValid(area2D)) 
            {
                return;
            }
            string Type_string = area2D?.Area2D_type;
            if (Type_string != null && Type_string == "Plants" && area2D?.Sec_Info != "Zombies") 
            {
                return;
            }
            lock (_listLock)
            {
                if (!IsInstanceValid(area2D))
                {
                    return;
                }
                if (!just_for_MG)
                {
                    if (has_planted && Type_string != null && Type_string == "Shovel")
                    {
                        Shovel_Area = (Shovel_Area2D)area2D;
                    }
                    else if (has_planted && Type_string != null && Type_string == "Bug")
                    {
                        Bug_Area = (Bug_Area2D)area2D;
                    }
                }
                if (Type_string != null && Type_string == "Zombies")
                {
                    Zombies_Area_2D = (Normal_Zombies_Area)area2D;
                    if (Zombies_Area_2D != null)
                    {
                        if (Zombies_Area_2D.can_hurt)//屎山造成
                        {
                            Zombies_Area_2D_List.Add(Zombies_Area_2D);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            GD.Print(ex.Message);
        }
    }
    protected virtual void Area_Exited(Area2D area_node)
    {
        try
        {
            if (!(area_node is Control_Area_2D area2D) || !IsInstanceValid(area2D))
            {
                return;
            }
            string Type_string = area2D?.Area2D_type;
            if (Type_string != null && Type_string == "Plants" && area2D?.Sec_Info != "Zombies")
            {
                return;
            }
            lock (_listLock)
            {
                if (!IsInstanceValid(area2D))
                {
                    return;
                }
                if (!just_for_MG)
                {
                    if (has_planted && Type_string != null && Type_string == "Shovel")
                    {
                        this.Modulate = normal_color;
                        Shovel_Area = null;
                        on_Shovel = false;
                    }
                    else if (has_planted && Type_string != null && Type_string == "Bug")
                    {
                        this.Modulate = normal_color;
                        Bug_Area = null;
                        on_Bug = false;
                    }
                }
                if (Type_string != null && Type_string == "Zombies")
                {
                    Zombies_Area_2D_List.Remove((Normal_Zombies_Area)area2D);
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
    protected virtual void Plants_Add_List()
    {
        //Dock_Area_2D.Normal_Plant_List.Add(this);
    }
    protected virtual void Plants_Remove_List()
    {
        //Dock_Area_2D.Normal_Plant_List.Remove(this);
    }
    protected virtual bool Allow_Plants()
    {
        //((In_Game_Main.Sun_Number >= card_parent_Button.sun && Dock_Area_2D.Normal_Plant_List.Count == 0 && Dock_Area_2D.now_type[Dock_Area_2D.now_type.Count - 1] == 1) || Public_Main.debuging) && on_lock_grid
        return false;
    }
    protected virtual void Plants_Init()
    { }
    protected virtual async void Free_Self()
    {
        if (GetNode<Area2D>("Main/Shovel_Area").IsConnected("area_entered", this, nameof(Area_Entered)))
        {
            GetNode<Area2D>("Main/Shovel_Area").Disconnect("area_entered", this, nameof(Area_Entered));
            GetNode<Area2D>("Main/Shovel_Area").Disconnect("area_exited", this, nameof(Area_Exited));
            GetNode<Area2D>("Dock/Area2D").Disconnect("area_entered", this, nameof(Dock_Entered));
            GetNode<Area2D>("Dock/Area2D").Disconnect("area_exited", this, nameof(Dock_Exited));
            GetNode<Area2D>("Main/Shovel_Area").Monitoring = false;
            GetNode<Area2D>("Main/Shovel_Area").Monitorable = false;
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
}
