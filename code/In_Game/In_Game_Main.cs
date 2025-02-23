using Godot;
using System;
using System.Collections.Generic;

public class In_Game_Main : Node2D
{
    //Clone_List
    static public List<Tuple<string, Vector2,int>> Zombies_Clone_Request_List = new List<Tuple<string, Vector2,int>>();
    static public List<Tuple<string, Vector2, int>> Plant_Zombies_Clone_Request_List = new List<Tuple<string, Vector2, int>>();
    static public List<Tuple<int, Vector2, float, bool>> Sun_Clone_Request_List = new List<Tuple<int, Vector2, float, bool>>();
    static public List<Tuple<string, Vector2, float>> Plants_Bullets_Clone_Request_List = new List<Tuple<string, Vector2, float>>();
    //Clone_List
    static public int Sun_Number;
    static public int background_number;
    static public bool is_playing=false;
    static public bool allow_sun = false;
    private string level_file = null;
    private int Wave_number = 0;
    private bool Wave_Auto_Next = false;
    private int Flag_number = 0;
    private int now_Wave_number = 1;
    private int now_Flag_number = 1;
    static public int Zombies_Number = 0;
    private bool Ended = false;
    private bool Has_Made_Trophy = false;
    static public bool Lost_Brain = false;
    static public bool has_Lost_Brain = false;
    static public Vector2 Last_Zombies_Pos = new Vector2(512, 300);
    static public bool Cold_Timer_End = false;
    public override async void _Ready()
    {
        Zombies_Number = 0;
        Public_Main.now_card_number = 0;
        is_playing = false;
        var Click = GetNode<AudioStreamPlayer>("button_Click");
        GetNode<AudioStreamPlayer>("Cancel").Stream.Set("loop", false);
        Click.Stream.Set("loop", false);
        var ready_Set = GetNode<AudioStreamPlayer>("Ready_Animation/AudioStreamPlayer");
        ready_Set.Stream.Set("loop", false);
        if (Public_Main.user_name != string.Empty)
        {
            ConfigFile file = new ConfigFile();
            Error error = file.Load("user://Users/" + Public_Main.user_name + "/level_number.cfg");
            if (error == Error.Ok)
            {
                level_file = (string)file.GetValue("Level", "Level_Name", string.Empty);
                if (level_file != string.Empty)
                {
                    ConfigFile file2 = new ConfigFile();
                    Error error2 = file2.Load(level_file);
                    if (error2==Error.Ok)
                    {
                        background_number = (int)file2.GetValue("Background", "background", 0);
                        if (background_number==0)
                        {
                            GD.PushError("错误:关卡文件错误!background_number=0");
                            GD.Print("错误:关卡文件错误!background_number=0");
                            GetTree().Quit();
                        }
                        else
                        {
                            var Background1 = GetNode<TextureRect>("Main/Background/Background1");
                            var Background2 = GetNode<TextureRect>("Main/Background/Background2");
                            var Background3 = GetNode<TextureRect>("Main/Background/Background3");
                            var Background4 = GetNode<TextureRect>("Main/Background/Background4");
                            var Background5 = GetNode<TextureRect>("Main/Background/Background5");
                            var Background6 = GetNode<TextureRect>("Main/Background/Background6");
                            Background1.Visible = background_number == 1;
                            Background2.Visible = background_number == 2;
                            Background3.Visible = background_number == 3;
                            Background4.Visible = background_number == 4;
                            Background5.Visible = background_number == 5;
                            Background6.Visible = background_number == 6;
                            if (background_number != 1)
                            {
                                Background1.QueueFree();
                            }
                            if (background_number != 2)
                            {
                                Background2.QueueFree();
                            }
                            if (background_number != 3)
                            {
                                Background3.QueueFree();
                            }
                            if (background_number != 4)
                            {
                                Background4.QueueFree();
                            }
                            if (background_number != 5)
                            {
                                Background5.QueueFree();
                            }
                            if (background_number != 6)
                            {
                                Background6.QueueFree();
                            }
                            if (background_number == 1 || background_number == 3 || background_number == 5)
                            {
                                allow_sun = true;
                            }
                            else
                            {
                                allow_sun = false;
                            }
                        }
                        int Level_Mode = (int)file2.GetValue("Level_Mode", "mode", -1);
                        if (Level_Mode == -1)
                        {
                            GD.PushError("错误:关卡文件错误!Level_Mode=-1");
                            GD.Print("错误:关卡文件错误!Level_Mode=-1");
                            GetTree().Quit();
                        }
                        else if (Level_Mode == 0 || Level_Mode == 1) //正常模式
                        {
                            int Sun = (int)file2.GetValue("Bank", "Sun", 1437);
                            Sun_Number = Sun;
                            bool Shovel = (bool)file2.GetValue("Bank","Shovel",true);
                            bool Bug = (bool)file2.GetValue("Bank", "Bug", true);
                            var Sun_Label = GetNode<Label>("Main/Card/SeedBank/Sun/Sun_Text");
                            Sun_Label.Text = Sun.ToString();
                            var Shovel_Main = GetNode<TextureRect>("Main/Card/ShovelBank");
                            var Bug_Main = GetNode<TextureRect>("Main/Card/BugBank");
                            Shovel_Main.Visible = Shovel;
                            Bug_Main.Visible = Bug;
                            int Card = (int)file2.GetValue("Bank","Card",13);
                            int Car_Number = (int)file2.GetValue("Car", "Number", 5);
                            if (background_number == 3 || background_number == 4)
                            {
                                for (int i = 1; i <= 6; i++)
                                {
                                    var scene = GD.Load<PackedScene>("res://scene/Plants/Car/Car.tscn");
                                    var plant_child = (Car_Main)scene.Instance();
                                    int Car_type = (int)file2.GetValue("Car", i.ToString(), 1);
                                    if (Car_type == 0)
                                    {
                                        continue;
                                    }
                                    if (i == 1)
                                    {
                                        plant_child.Position = new Vector2(25, 124);
                                        plant_child.ZIndex = 2;
                                    }
                                    else if (i == 2)
                                    {
                                        plant_child.Position = new Vector2(25, 216);
                                        plant_child.ZIndex = 22;
                                    }
                                    else if (i == 3)
                                    {
                                        plant_child.Position = new Vector2(25, 299);
                                        plant_child.ZIndex = 42;
                                    }
                                    else if (i == 4)
                                    {
                                        plant_child.Position = new Vector2(25, 376);
                                        plant_child.ZIndex = 62;
                                    }
                                    else if (i == 5)
                                    {
                                        plant_child.Position = new Vector2(25, 477);
                                        plant_child.ZIndex = 82;
                                    }
                                    else if (i == 6)
                                    {
                                        plant_child.Position = new Vector2(25, 558);
                                        plant_child.ZIndex = 102;
                                    }
                                    plant_child.doing_Start = true;
                                    plant_child.car_type = Car_type;
                                    //plant_child._Ready();//Warning:Can't Add Child
                                    GetNode<Control>("/root/In_Game/Object").AddChild(plant_child);
                                }
                            }
                            else if (background_number == 1 || background_number == 2)
                            {
                                for (int i = 1; i <= 5; i++)
                                {
                                    var scene = GD.Load<PackedScene>("res://scene/Plants/Car/Car.tscn");
                                    var plant_child = (Car_Main)scene.Instance();
                                    int Car_type = (int)file2.GetValue("Car", i.ToString(), 1);
                                    if (Car_type == 0)
                                    {
                                        continue;
                                    }
                                    if (i == 1)
                                    {
                                        plant_child.Position = new Vector2(25, 128);
                                        plant_child.ZIndex = 2;
                                    }
                                    else if (i == 2)
                                    {
                                        plant_child.Position = new Vector2(25, 224);
                                        plant_child.ZIndex = 22;
                                    }
                                    else if (i == 3)
                                    {
                                        plant_child.Position = new Vector2(25, 328);
                                        plant_child.ZIndex = 42;
                                    }
                                    else if (i == 4)
                                    {
                                        plant_child.Position = new Vector2(25, 424);
                                        plant_child.ZIndex = 62;
                                    }
                                    else if (i == 5)
                                    {
                                        plant_child.Position = new Vector2(25, 520);
                                        plant_child.ZIndex = 82;
                                    }
                                    plant_child.doing_Start = true;
                                    plant_child.car_type = Car_type;
                                    //plant_child._Ready();//Warning:Can't Add Child
                                    GetNode<Control>("/root/In_Game/Object").AddChild(plant_child);
                                }
                            }
                            Wave_number = (int)file2.GetValue("Zombies", "Wave", 0);
                            Flag_number = (int)file2.GetValue("Zombies", "Flag", 0);
                            GetNode<Flag_Control>("Main/Info/Flag").All_Flag_Number = Flag_number;
                            GetNode<Flag_Control>("Main/Info/Flag").All_Wave_Number = Wave_number;
                            for (int i = 1; i <= Flag_number; i++)
                            {
                                GetNode<Flag_Control>("Main/Info/Flag").Flag_In_Wave_Number.Add((int)file2.GetValue("Flag" + i.ToString(), "Wave", 0));
                            }
                            GetNode<Flag_Control>("Main/Info/Flag").Make_Flag();
                            GetNode<Flag_Control>("Main/Info/Flag").Update_Flag();
                            GetNode<Label>("Main/Info/Name").Text = (string)file2.GetValue("Name", "Name", "PVZ_Godot");
                            Public_Main.allow_card_number = Card;
                            var player1 = GetNode<AnimationPlayer>("Main/AnimationPlayer");
                            var player2 = GetNode<AnimationPlayer>("Main/Card_Player");
                            player1.Play("C_to_R");
                            await ToSignal(player1, "animation_finished");
                            player2.Play("load");
                            int Max_Plant_Id = 0;
                            int Max_Plants_Zombies_Id = 0;
                            int Max_Zombies_Id = 0;
                            int Card_Type= (int)file2.GetValue("Card", "Type", 1);
                            List<int> Card3_id_List = new List<int>();
                            if (Card_Type == 1)
                            {
                                for (int i = 0; i < Public_Main.Plant_list.Count; i++)
                                {
                                    if (Public_Main.Plant_list[i].Item1 > Max_Plant_Id)
                                    {
                                        Max_Plant_Id = Public_Main.Plant_list[i].Item1;
                                    }
                                }
                            }
                            else if (Card_Type == 2)
                            {
                                Max_Plant_Id = (int)file2.GetValue("Card", "LockTo", 4);
                            }
                            else if (Card_Type == 3)
                            {
                                int Adding_Number = 1;
                                while (true)
                                {
                                    int Ans_Number = (int)file2.GetValue("Card", Adding_Number.ToString(), -1);
                                    if (Ans_Number != -1)
                                    {
                                        Card3_id_List.Add(Ans_Number);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                    Adding_Number++;
                                }
                                Max_Plant_Id = Card3_id_List.Count;
                            }
                            for (int i = 0; i < Public_Main.Plants_Zombies_list.Count; i++)
                            {
                                if (Public_Main.Plants_Zombies_list[i].Item1 > Max_Plants_Zombies_Id)
                                {
                                    Max_Plants_Zombies_Id = Public_Main.Plants_Zombies_list[i].Item1;
                                }
                            }
                            for (int i = 0; i < Public_Main.Zombies_list.Count; i++)
                            {
                                if (Public_Main.Zombies_list[i].Item1 > Max_Zombies_Id)
                                {
                                    Max_Zombies_Id = Public_Main.Zombies_list[i].Item1;
                                }
                            }
                            List<int> LevelMode3_id_List = new List<int>();
                            if (Level_Mode == 1)
                            {
                                int Adding_Number = 1;
                                while (true)
                                {
                                    int Ans_Number = (int)file2.GetValue("Level_Mode", Adding_Number.ToString(), -1);
                                    if (Ans_Number != -1)
                                    {
                                        LevelMode3_id_List.Add(Ans_Number);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                    Adding_Number++;
                                }
                            }
                            var Page1 = GetNode<GridContainer>("/root/In_Game/Main/Choose_Card/Background/Page/Page1");
                            if (Card_Type == 3)
                            {
                                if (Page1.GetChildCount() > 50)
                                {
                                }
                                else
                                {
                                    for (int j = 0; j < Card3_id_List.Count; j++)
                                    {
                                        var scene = GD.Load<PackedScene>("res://scene/Card/Card.tscn");
                                        var card_child = (Card_Main)scene.Instance();
                                        card_child.Card_Number[0] = 1;
                                        card_child.Card_Number[1] = Card3_id_List[j];
                                        for (int k = 0; k < LevelMode3_id_List.Count; k++)
                                        {
                                            if (LevelMode3_id_List[k] == card_child.Card_Number[1])
                                            {
                                                card_child.self_Wait_Time_Set = true;
                                                card_child.wait_time = 0f;
                                            }
                                        }
                                        card_child._Ready();
                                        Page1.AddChild(card_child);
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < Max_Plant_Id; i++)
                                {
                                    if (Page1.GetChildCount() > 50)
                                    { }
                                    else
                                    {
                                        var scene = GD.Load<PackedScene>("res://scene/Card/Card.tscn");
                                        var card_child = (Card_Main)scene.Instance();
                                        card_child.Card_Number[0] = 1;
                                        card_child.Card_Number[1] = i + 1;
                                        for (int k = 0; k < LevelMode3_id_List.Count; k++)
                                        {
                                            if (LevelMode3_id_List[k] == card_child.Card_Number[1])
                                            {
                                                card_child.self_Wait_Time_Set = true;
                                                card_child.wait_time = 0f;
                                            }
                                        }
                                        card_child._Ready();
                                        Page1.AddChild(card_child);
                                    }
                                }
                            }
                            for (int i = 0; i < Max_Plants_Zombies_Id; i++)
                            {

                                if (Page1.GetChildCount() > 50)
                                { }
                                else
                                {
                                    var scene = GD.Load<PackedScene>("res://scene/Card/Card.tscn");
                                    var card_child = (Card_Main)scene.Instance();
                                    card_child.Card_Number[0] = 3;
                                    card_child.Card_Number[1] = i + 1;
                                    for (int k = 0; k < LevelMode3_id_List.Count; k++)
                                    {
                                        if (LevelMode3_id_List[k] == card_child.Card_Number[1])
                                        {
                                            card_child.self_Wait_Time_Set = true;
                                            card_child.wait_time = 0f;
                                        }
                                    }
                                    card_child._Ready();
                                    Page1.AddChild(card_child);
                                }
                            }
                            for (int i = 0; i < Max_Zombies_Id; i++)
                            {
                                if (Page1.GetChildCount() > 50)
                                { }
                                else
                                {
                                    var scene = GD.Load<PackedScene>("res://scene/Card/Card.tscn");
                                    var card_child = (Card_Main)scene.Instance();
                                    card_child.Card_Number[0] = 2;
                                    card_child.Card_Number[1] = i + 1;
                                    for (int k = 0; k < LevelMode3_id_List.Count; k++)
                                    {
                                        if (LevelMode3_id_List[k] == card_child.Card_Number[1])
                                        {
                                            card_child.self_Wait_Time_Set = true;
                                            card_child.wait_time = 0f;
                                        }
                                    }
                                    card_child._Ready();
                                    Page1.AddChild(card_child);
                                }
                            }
                        }
                    }
                }
                else
                {
                    GD.PushError("错误:关卡路径为空!error:level_file is Empty!\nfile.Load(\"user://Users/\" + Public_Main.user_name + \"/level_number.cfg\");\nString level_file = (String)file.GetValue(\"Level\", \"Level_Name\", String.Empty);");
                    GD.Print("错误:关卡路径为空!\nerror:level_file is Empty!\nfile.Load(\"user://Users/\" + Public_Main.user_name + \"/level_number.cfg\");\nString level_file = (String)file.GetValue(\"Level\", \"Level_Name\", String.Empty);");
                    GetTree().Quit();
                }
            }
            else
            {
                GD.PushError("错误:用户名为空!error:Public_Main.user_name is Empty!");
                GD.Print("错误:用户名为空!\nerror:Public_Main.user_name is Empty!");
                GetTree().Quit();
            }
        }
    }
    static public void Update_Sun(Node node)
    {
        var Sun_Label = node.GetNode<Label>("/root/In_Game/Main/Card/SeedBank/Sun/Sun_Text");
        Sun_Label.Text = Sun_Number.ToString();
    }
    static public void Warning_Sun(Node node)
    {
        node.GetNode<AudioStreamPlayer>("/root/In_Game/Main/Card/SeedBank/Sun/Pause").Stream.Set("loop", false);
        var Sun_Warning = node.GetNode<AnimationPlayer>("/root/In_Game/Main/Card/SeedBank/Sun/Warning");
        Sun_Warning.Play("Warning");
        node.GetNode<AudioStreamPlayer>("/root/In_Game/Main/Card/SeedBank/Sun/Pause").Play();
    }
    public override async void _Process(float delta)
    {
        if (is_playing && GetNode<Timer>("Timer").IsStopped() && !GetTree().Paused)
        {
            GetNode<Timer>("Timer").Start();
        }
        if (is_playing && GetNode<Timer>("Wave_Timer").IsStopped() && !GetTree().Paused)
        {
            GetNode<Timer>("Wave_Timer").Start();
        }
        if (Ended && Zombies_Number <= 0 && !Has_Made_Trophy && Cold_Timer_End)   
        {
            Has_Made_Trophy = true;
            var scene = GD.Load<PackedScene>("res://scene/In_Game/Trophy/Trophy.tscn");
            var plant_child = (Trophy_Main)scene.Instance();
            GetNode<Control>("/root/In_Game/Object").AddChild(plant_child);
            if (Last_Zombies_Pos.x <= 50 || Last_Zombies_Pos.x >= 850)
            {
                plant_child.Position = new Vector2(512, 300);
            }
            else
            {
                plant_child.Position = Last_Zombies_Pos;
            }
        }
        if (Lost_Brain&&!has_Lost_Brain)
        {
            has_Lost_Brain = true;
            GetNode<AnimationPlayer>("Lost_Brain").Play("Brain");
        }
        if (is_playing) //可能会吞僵尸
        {
            int Clone_Number = 0;
            while (Clone_Number < Public_Main.Max_Object_Clone_In_F || !Public_Main.Using_Clone_Limit)
            {
                bool Must_Quit = true;
                if (Sun_Clone_Request_List.Count != 0)
                {
                    var scene = GD.Load<PackedScene>("res://scene/Plants/SunFlower/Sun/Sun.tscn");
                    var sun_child = (Sun_Main)scene.Instance();
                    sun_child.size = Sun_Clone_Request_List[0].Item3;
                    sun_child.sun_value = Sun_Clone_Request_List[0].Item1;
                    sun_child.Position = Sun_Clone_Request_List[0].Item2;
                    sun_child.is_from_plant = Sun_Clone_Request_List[0].Item4;
                    GetNode<Control>("/root/In_Game/Object").AddChild(sun_child);
                    Sun_Clone_Request_List.RemoveAt(0);
                    Clone_Number++;
                    Must_Quit = false;
                }
                if (Plants_Bullets_Clone_Request_List.Count != 0)
                {
                    var scene = GD.Load<PackedScene>(Plants_Bullets_Clone_Request_List[0].Item1);
                    var Bullets_child = (Normal_Plants_Bullets)scene.Instance();
                    Bullets_child.GlobalPosition = Plants_Bullets_Clone_Request_List[0].Item2;
                    Bullets_child.speed_y = Plants_Bullets_Clone_Request_List[0].Item3;
                    GetNode<Control>("/root/In_Game/Object").AddChild(Bullets_child);
                    Plants_Bullets_Clone_Request_List.RemoveAt(0);
                    Clone_Number++;
                    Must_Quit = false;
                }
                if (Zombies_Clone_Request_List.Count != 0)
                {
                    var scene = GD.Load<PackedScene>(Zombies_Clone_Request_List[0].Item1);
                    var plant_child = (Normal_Zombies)scene.Instance();
                    plant_child.ZIndex = Zombies_Clone_Request_List[0].Item3;
                    plant_child.put_position = Zombies_Clone_Request_List[0].Item2;
                    plant_child.player_put = false;
                    GetNode<Control>("/root/In_Game/Object").AddChild(plant_child);
                    Clone_Number++;
                    Zombies_Clone_Request_List.RemoveAt(0);
                    Must_Quit = false;
                }
                if (Plant_Zombies_Clone_Request_List.Count != 0)
                {
                    var scene = GD.Load<PackedScene>(Plant_Zombies_Clone_Request_List[0].Item1);
                    var plant_child = (Normal_Plants_Zombies)scene.Instance();
                    plant_child.ZIndex = Plant_Zombies_Clone_Request_List[0].Item3;
                    plant_child.put_position = Plant_Zombies_Clone_Request_List[0].Item2;
                    plant_child.player_put = false;
                    GetNode<Control>("/root/In_Game/Object").AddChild(plant_child);
                    Clone_Number++;
                    Plant_Zombies_Clone_Request_List.RemoveAt(0);
                    Must_Quit = false;
                }
                if (Must_Quit)
                {
                    break;
                }
            }
        }
        if (is_playing && Wave_Auto_Next && Zombies_Number <= 0)
        {
            Next_Wave_pressed();
            await ToSignal(GetTree().CreateTimer(0.3f), "timeout");
        }
    }
    public void Sun_Timeout()
    {
        if (allow_sun&&is_playing)
        {
            GetNode<Timer>("Timer").WaitTime = (float)GD.RandRange(10d, 40d);
            if (true)
            {
                int sun_value;
                float size;
                float random_number = GD.Randf();
                if (random_number < 0.2f)
                {
                    sun_value = 50;
                    size = 2f;
                }
                else
                {
                    sun_value = 25;
                    size = 1f;
                }
                Sun_Clone_Request(sun_value, new Vector2((float)GD.RandRange(30, 1000), -22f), size, false);
            }
            GetNode<Timer>("Timer").Start();
        }
    }
    public void Wave_Timer_Out()
    {
        ConfigFile file = new ConfigFile();
        Error error = file.Load(level_file);
        if (error == Error.Ok)
        {
            if (now_Flag_number <= Flag_number)
            {
                int result_Number2 = (int)file.GetValue("Flag" + now_Flag_number.ToString(), "Wave", -1);
                if (result_Number2 != -1)
                {
                    if (now_Wave_number == result_Number2)
                    {
                        int Number_in_Wave = 1;
                        int result_Number = -1;
                        GetNode<Timer>("Wave_Timer").WaitTime = (int)file.GetValue("Flag" + now_Flag_number.ToString(), "Time", 60);
                        while (true)
                        {
                            result_Number = (int)file.GetValue("Flag" + now_Flag_number.ToString(), Number_in_Wave.ToString(), -1);
                            if (result_Number == -1)
                            {
                                break;
                            }
                            else
                            {
                                if (background_number == 3 || background_number == 4)
                                {
                                    int i = (int)(GD.Randi() % 6 + 1);
                                    if (true)
                                    {
                                        Vector2 put_position;
                                        int _ZIndex;
                                        if (i == 1)
                                        {
                                            put_position = new Vector2(1024, 124);
                                            _ZIndex = 7;
                                        }
                                        else if (i == 2)
                                        {
                                            put_position = new Vector2(1024, 216);
                                            _ZIndex = 27;
                                        }
                                        else if (i == 3)
                                        {
                                            put_position = new Vector2(1024, 299);
                                            _ZIndex = 47;
                                        }
                                        else if (i == 4)
                                        {
                                            put_position = new Vector2(1024, 376);
                                            _ZIndex = 67;
                                        }
                                        else if (i == 5)
                                        {
                                            put_position = new Vector2(1024, 477);
                                            _ZIndex = 87;
                                        }
                                        else
                                        {
                                            put_position = new Vector2(1024, 558);
                                            _ZIndex = 107;
                                        }
                                        Zombies_Clone_Request(Public_Main.Zombies_Path_List[result_Number - 1], put_position, _ZIndex);
                                    }
                                }
                                else if (background_number == 1 || background_number == 2)
                                {
                                    int i = (int)(GD.Randi() % 5 + 1);
                                    if (true)
                                    {
                                        Vector2 put_position;
                                        int _ZIndex;
                                        if (i == 1)
                                        {
                                            put_position = new Vector2(1024, 138);
                                            _ZIndex = 7;
                                        }
                                        else if (i == 2)
                                        {
                                            put_position = new Vector2(1024, 234);
                                            _ZIndex = 27;
                                        }
                                        else if (i == 3)
                                        {
                                            put_position = new Vector2(1024, 338);
                                            _ZIndex = 47;
                                        }
                                        else if (i == 4)
                                        {
                                            put_position = new Vector2(1024, 434);
                                            _ZIndex = 67;
                                        }
                                        else
                                        {
                                            put_position = new Vector2(1024, 530);
                                            _ZIndex = 87;
                                        }
                                        Zombies_Clone_Request(Public_Main.Zombies_Path_List[result_Number - 1], put_position, _ZIndex);
                                    }
                                }
                            }
                            Number_in_Wave++;
                        }
                        now_Flag_number++;
                        GetNode<Flag_Control>("Main/Info/Flag").Now_Flag_Number = now_Flag_number;
                        GetNode<Flag_Control>("Main/Info/Flag").Now_Wave_Number = now_Wave_number + 1;//诶~!我有一计
                        GetNode<Flag_Control>("Main/Info/Flag").Update_Flag();
                        Wave_Auto_Next = (bool)file.GetValue("Flag" + (now_Flag_number - 1).ToString(), "Auto_Next", false);
                        if (!Ended)
                        {
                            Ended = (bool)file.GetValue("Flag" + (now_Flag_number - 1).ToString(), "End", false);
                            if (Ended)
                            {
                                GetNode<Timer>("End_Cold_Timer").Start();
                            }
                        }
                        return;
                    }
                }
            }
            if (now_Wave_number <= Wave_number)
            {
                int Number_in_Wave = 1;
                int result_Number = -1;
                GetNode<Timer>("Wave_Timer").WaitTime = (int)file.GetValue("Wave" + now_Wave_number.ToString(), "Time", 60);
                while (true)
                {
                    result_Number = (int)file.GetValue("Wave" + now_Wave_number.ToString(), Number_in_Wave.ToString(), -1);
                    if (result_Number == -1)
                    {
                        break;
                    }
                    else
                    {
                        if (background_number == 3 || background_number == 4)
                        {
                            int i = (int)(GD.Randi() % 6 + 1);
                            if (true)
                            {
                                Vector2 put_position;
                                int _ZIndex;
                                if (i == 1)
                                {
                                    put_position = new Vector2(1024, 124);
                                    _ZIndex = 7;
                                }
                                else if (i == 2)
                                {
                                    put_position = new Vector2(1024, 216);
                                    _ZIndex = 27;
                                }
                                else if (i == 3)
                                {
                                    put_position = new Vector2(1024, 299);
                                    _ZIndex = 47;
                                }
                                else if (i == 4)
                                {
                                    put_position = new Vector2(1024, 376);
                                    _ZIndex = 67;
                                }
                                else if (i == 5)
                                {
                                    put_position = new Vector2(1024, 477);
                                    _ZIndex = 87;
                                }
                                else
                                {
                                    put_position = new Vector2(1024, 558);
                                    _ZIndex = 107;
                                }
                                Zombies_Clone_Request(Public_Main.Zombies_Path_List[result_Number - 1], put_position, _ZIndex);
                            }
                        }
                        else if (background_number == 1 || background_number == 2)
                        {
                            int i = (int)(GD.Randi() % 5 + 1);
                            if (true)
                            {
                                Vector2 put_position;
                                int _ZIndex;
                                if (i == 1)
                                {
                                    put_position = new Vector2(1024, 138);
                                    _ZIndex = 7;
                                }
                                else if (i == 2)
                                {
                                    put_position = new Vector2(1024, 234);
                                    _ZIndex = 27;
                                }
                                else if (i == 3)
                                {
                                    put_position = new Vector2(1024, 338);
                                    _ZIndex = 47;
                                }
                                else if (i == 4)
                                {
                                    put_position = new Vector2(1024, 434);
                                    _ZIndex = 67;
                                }
                                else
                                {
                                    put_position = new Vector2(1024, 530);
                                    _ZIndex = 87;
                                }
                                Zombies_Clone_Request(Public_Main.Zombies_Path_List[result_Number - 1], put_position, _ZIndex);
                            }
                        }
                    }
                    Number_in_Wave++;
                }
                now_Wave_number++;
            }
            GetNode<Flag_Control>("Main/Info/Flag").Now_Flag_Number = now_Flag_number;
            GetNode<Flag_Control>("Main/Info/Flag").Now_Wave_Number = now_Wave_number;
            GetNode<Flag_Control>("Main/Info/Flag").Update_Flag();
            Wave_Auto_Next = (bool)file.GetValue("Wave" + (now_Wave_number - 1).ToString(), "Auto_Next", false);
            if (!Ended)
            {
                Ended = (bool)file.GetValue("Wave" + (now_Wave_number - 1).ToString(), "End", false);
                if (Ended)
                {
                    GetNode<Timer>("End_Cold_Timer").Start();
                }
            }
        }
    }
    public void End_Cold_Timer_timeout()
    {
        Cold_Timer_End = true;
    }
    static public void Zombies_Clone_Request(string Clone_String,Vector2 pos,int Z_index)
    {
        Zombies_Clone_Request_List.Add(new Tuple<string, Vector2, int>(Clone_String, pos, Z_index));
    }
    static public void Plants_Zombies_Clone_Request(string Clone_String, Vector2 pos, int Z_index)
    {
        Plant_Zombies_Clone_Request_List.Add(new Tuple<string, Vector2, int>(Clone_String, pos, Z_index));
    }
    static public void Sun_Clone_Request(int Value, Vector2 pos, float size, bool from_Plants)
    {
        Sun_Clone_Request_List.Add(new Tuple<int, Vector2, float, bool>(Value, pos, size, from_Plants));
    }
    static public void Plants_Bullets_Clone_Request(string Path, Vector2 pos, float _y = 0f)
    {
        Plants_Bullets_Clone_Request_List.Add(new Tuple<string, Vector2, float>(Path, pos, _y));
    }
    public void Next_Wave_pressed()
    {
        if (is_playing)
        {
            Wave_Timer_Out();
            GetNode<Timer>("Wave_Timer").Stop();
        }
    }
}
