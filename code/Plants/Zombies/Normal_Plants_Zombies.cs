using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
public class Normal_Plants_Zombies : Node2D
{
    //define
    [Export] protected int normal_ZIndex = 7;
    [Export] protected Color hurt_color = new Color(1.3f, 1.3f, 1.3f, 1f);
    [Export] protected Color normal_color = new Color(1f, 1f, 1f, 1f);
    protected Timer Android_Timer = new Timer();
    protected bool Is_Double_Click = false;//for Android
    public Vector2 put_position = Vector2.Zero;//In_Game_Main使用
    public bool player_put = false;//In_Game_Main使用
    protected List<Health_Container> health_list = new List<Health_Container>()
    {
        new Health_Container(270,false)
    };
    protected int health = 270;
    [Export] protected float speed_Normal = 1.5f;
    [Export] protected bool can_Eating = true;
    protected bool Doing_jumping = false;//for Pole...
    protected bool has_planted = false;
    public Card_Click_Button card_parent_Button = null;//Card使用
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
    //define
    protected static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (Public_Main.debuging)
        {
            Console.WriteLine("异常:" + e.ExceptionObject);
        }
    }
    protected static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
    {
        e.SetObserved();
        if (Public_Main.debuging)
        {
            Console.WriteLine("未观测异常:" + e.Exception);
        }
    }
    public override void _Ready()
    {
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
            GetNode<Normal_Plants_Zombies_Area>("Main/Main/Zombies_Area").has_planted = true;
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
                    if ((In_Game_Main.Sun_Number >= card_parent_Button.sun || Public_Main.debuging) && on_lock_grid)
                    {
                        has_planted = true;
                        GetNode<Normal_Plants_Zombies_Area>("Main/Main/Zombies_Area").has_planted = true;
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
            }
        }
        else
        {
            GetNode<Normal_Plants_Zombies_Area>("Main/Main/Zombies_Area").Choose_Zombies_Area = Zombies_Area_2D;
            GetNode<Normal_Plants_Zombies_Area>("Main/Main/Zombies_Area").has_lose_head = has_lose_Head;
            if (GetNode<Normal_Plants_Zombies_Area>("Main/Main/Zombies_Area").lose_health)
            {
                GetNode<Normal_Plants_Zombies_Area>("Main/Main/Zombies_Area").lose_health = false;
                health_list[0].Health -= GetNode<Normal_Plants_Zombies_Area>("Main/Main/Zombies_Area").lose_health_number;
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
            if (Zombies_Area_2D_List.Count == 0)
            {
                Zombies_Area_2D = null;
            }
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
                if (Zombies_Area_2D == null)
                {
                    Zombies_Area_2D = Zombies_Area_2D_List[i];
                }
                else
                {
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
                }
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
                    if (Zombies_Area_2D_List[i].Choose_Plants_Area == GetNode<Normal_Plants_Area>("Main/Main/Zombies_Area"))
                    {
                        if (Zombies_Area_2D_List[i].is_eating_show && !Zombies_Area_2D_List[i].has_lose_head)
                        {
                            Zombies_Area_2D_List[i].is_eating_show = false;
                            health_list[0].Health -= Zombies_Area_2D_List[i].hurt;
                            GetNode<AnimationPlayer>("Is_Eated").Play("Is_Eated");
                        }
                    }
                }
            }
            if (!eating && Get_Walk_Mode())
            {
                this.Position += new Vector2(speed, 0);
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
            if (Zombies_Area_2D != null && !has_lose_Head && can_Eating)
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
            if (Position.x > 1437)
            {
                this.QueueFree();
            }
        }
    }
    protected virtual void Plants_Entered(Control_Area_2D area2D)//函数名为历史遗留问题
    {
        if (area2D.Area2D_type == "Zombies")
        {
            Zombies_Area_2D_List.Add((Normal_Zombies_Area)area2D);
        }
    }
    protected virtual void Plants_Exited(Control_Area_2D area2D)
    {
        if (area2D.Area2D_type == "Zombies")
        {
            Zombies_Area_2D_List.Remove((Normal_Zombies_Area)area2D);
        }
    }
    protected virtual void Dock_Entered(Control_Area_2D area2D)
    {
        if (area2D.Area2D_type == "Grid")
        {
            Dock_Area_2D_List.Add((Background_Grid_Main)area2D);
        }
    }
    protected virtual void Dock_Exited(Control_Area_2D area2D)
    {
        if (area2D.Area2D_type == "Grid")
        {
            Dock_Area_2D_List.Remove((Background_Grid_Main)area2D);
        }
    }
    protected virtual void Walk_Mode(bool is_Walking)
    { }
    protected virtual bool Get_Walk_Mode()
    {
        return false;//形式意义
    }
}
