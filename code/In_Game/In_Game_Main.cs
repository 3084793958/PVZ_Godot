using Godot;
using System;

public class In_Game_Main : Node2D
{
    static public int Sun_Number;
    static public int background_number;
    static public bool is_playing=false;
    public bool allow_sun = false;
    private string level_file = null;
    private int Wave_number = 0;
    private int Flag_number = 0;
    private int now_Wave_number = 1;
    private int now_Flag_number = 1;
    static public int Zombies_Number = 0;
    private bool Ended = false;
    private bool Has_Made_Trophy = false;
    static public bool Lost_Brain = false;
    static public bool has_Lost_Brain = false;
    public override async void _Ready()
    {
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
                        else if (Level_Mode==0)//正常模式
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
                                Max_Plant_Id= (int)file2.GetValue("Card", "LockTo", 4);
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
                            for (int i=0;i<Max_Plant_Id;i++)
                            {
                                var Page1 = GetNode<GridContainer>("/root/In_Game/Main/Choose_Card/Background/Page/Page1");
                                if (Page1.GetChildCount()>=50)
                                { }
                                else
                                {
                                    var scene = GD.Load<PackedScene>("res://scene/Card/Card.tscn");
                                    var card_child = (Card_Main)scene.Instance();
                                    card_child.Card_Number[0] = 1;
                                    card_child.Card_Number[1] = i+1;
                                    card_child._Ready();
                                    Page1.AddChild(card_child);
                                }
                            }
                            for (int i = 0; i < Max_Plants_Zombies_Id; i++)
                            {
                                var Page1 = GetNode<GridContainer>("/root/In_Game/Main/Choose_Card/Background/Page/Page1");
                                if (Page1.GetChildCount() >= 50)
                                { }
                                else
                                {
                                    var scene = GD.Load<PackedScene>("res://scene/Card/Card.tscn");
                                    var card_child = (Card_Main)scene.Instance();
                                    card_child.Card_Number[0] = 3;
                                    card_child.Card_Number[1] = i + 1;
                                    card_child._Ready();
                                    Page1.AddChild(card_child);
                                }
                            }
                            for (int i = 0; i < Max_Zombies_Id; i++)
                            {
                                var Page1 = GetNode<GridContainer>("/root/In_Game/Main/Choose_Card/Background/Page/Page1");
                                if (Page1.GetChildCount() >= 50)
                                { }
                                else
                                {
                                    var scene = GD.Load<PackedScene>("res://scene/Card/Card.tscn");
                                    var card_child = (Card_Main)scene.Instance();
                                    card_child.Card_Number[0] = 2;
                                    card_child.Card_Number[1] = i + 1;
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
    public override void _Process(float delta)
    {
        if (is_playing && GetNode<Timer>("Timer").IsStopped() && !GetTree().Paused)
        {
            GetNode<Timer>("Timer").Start();
        }
        if (is_playing && GetNode<Timer>("Wave_Timer").IsStopped() && !GetTree().Paused)
        {
            GetNode<Timer>("Wave_Timer").Start();
        }
        if (Ended && Zombies_Number <= 0 && !Has_Made_Trophy)  
        {
            Has_Made_Trophy = true;
            var scene = GD.Load<PackedScene>("res://scene/In_Game/Trophy/Trophy.tscn");
            var plant_child = (Trophy_Main)scene.Instance();
            GetNode<Control>("/root/In_Game/Object").AddChild(plant_child);
            plant_child.Position = new Vector2(512, 300);
        }
        if (Lost_Brain&&!has_Lost_Brain)
        {
            has_Lost_Brain = true;
            GetNode<AnimationPlayer>("Lost_Brain").Play("Brain");
        }
    }
    public void Sun_Timeout()
    {
        if (allow_sun&&is_playing)
        {
            GetNode<Timer>("Timer").WaitTime = (float)GD.RandRange(10d, 40d);
            var scene = GD.Load<PackedScene>("res://scene/Plants/SunFlower/Sun/Sun.tscn");
            var sun_child = (Sun_Main)scene.Instance();
            sun_child.is_from_plant = false;
            float random_number = GD.Randf();
            if (random_number < 0.2f)
            {
                sun_child.sun_value = 50;
                sun_child.size = 2f;
            }
            else
            {
                sun_child.sun_value = 25;
                sun_child.size = 1f;
            }
            sun_child.Position = new Vector2((float)GD.RandRange(30, 1000), -22f);
            GetNode<Control>("/root/In_Game/Object").AddChild(sun_child);
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
                        GetNode<Timer>("Wave_Timer").WaitTime = (int)file.GetValue("Flag" + now_Flag_number.ToString(), "Time", 5);
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
                                    var scene = GD.Load<PackedScene>(Public_Main.Zombies_Path_List[result_Number - 1]);
                                    var plant_child = (Normal_Zombies)scene.Instance();
                                    if (i == 1)
                                    {
                                        plant_child.put_position = new Vector2(1024, 124);
                                        plant_child.ZIndex = 7;
                                    }
                                    else if (i == 2)
                                    {
                                        plant_child.put_position = new Vector2(1024, 216);
                                        plant_child.ZIndex = 27;
                                    }
                                    else if (i == 3)
                                    {
                                        plant_child.put_position = new Vector2(1024, 299);
                                        plant_child.ZIndex = 47;
                                    }
                                    else if (i == 4)
                                    {
                                        plant_child.put_position = new Vector2(1024, 376);
                                        plant_child.ZIndex = 67;
                                    }
                                    else if (i == 5)
                                    {
                                        plant_child.put_position = new Vector2(1024, 477);
                                        plant_child.ZIndex = 87;
                                    }
                                    else if (i == 6)
                                    {
                                        plant_child.put_position = new Vector2(1024, 558);
                                        plant_child.ZIndex = 107;
                                    }
                                    plant_child.player_put = false;
                                    //plant_child._Ready();//Warning:Can't Add Child
                                    GetNode<Control>("/root/In_Game/Object").AddChild(plant_child);
                                }
                                else if (background_number == 1 || background_number == 2)
                                {
                                    int i = (int)(GD.Randi() % 5 + 1);
                                    var scene = GD.Load<PackedScene>(Public_Main.Zombies_Path_List[result_Number - 1]);
                                    var plant_child = (Normal_Zombies)scene.Instance();
                                    if (i == 1)
                                    {
                                        plant_child.put_position = new Vector2(1024, 138);
                                        plant_child.ZIndex = 7;
                                    }
                                    else if (i == 2)
                                    {
                                        plant_child.put_position = new Vector2(1024, 234);
                                        plant_child.ZIndex = 27;
                                    }
                                    else if (i == 3)
                                    {
                                        plant_child.put_position = new Vector2(1024, 338);
                                        plant_child.ZIndex = 47;
                                    }
                                    else if (i == 4)
                                    {
                                        plant_child.put_position = new Vector2(1024, 434);
                                        plant_child.ZIndex = 67;
                                    }
                                    else if (i == 5)
                                    {
                                        plant_child.put_position = new Vector2(1024, 530);
                                        plant_child.ZIndex = 87;
                                    }
                                    plant_child.player_put = false;
                                    //plant_child._Ready();//Warning:Can't Add Child
                                    GetNode<Control>("/root/In_Game/Object").AddChild(plant_child);
                                }
                            }
                            Number_in_Wave++;
                        }
                        now_Flag_number++;
                        GetNode<Flag_Control>("Main/Info/Flag").Now_Flag_Number = now_Flag_number;
                        GetNode<Flag_Control>("Main/Info/Flag").Now_Wave_Number = now_Wave_number + 1;//诶~!我有一计
                        GetNode<Flag_Control>("Main/Info/Flag").Update_Flag();
                        if (!Ended)
                        {
                            Ended = (bool)file.GetValue("Flag" + now_Flag_number.ToString(), "End", false);
                        }
                        return;
                    }
                }
            }
            if (now_Wave_number <= Wave_number)
            {
                int Number_in_Wave = 1;
                int result_Number = -1;
                GetNode<Timer>("Wave_Timer").WaitTime = (int)file.GetValue("Wave" + now_Wave_number.ToString(), "Time", 5);
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
                            var scene = GD.Load<PackedScene>(Public_Main.Zombies_Path_List[result_Number - 1]);
                            var plant_child = (Normal_Zombies)scene.Instance();
                            if (i == 1)
                            {
                                plant_child.put_position = new Vector2(1024, 124);
                                plant_child.ZIndex = 7;
                            }
                            else if (i == 2)
                            {
                                plant_child.put_position = new Vector2(1024, 216);
                                plant_child.ZIndex = 27;
                            }
                            else if (i == 3)
                            {
                                plant_child.put_position = new Vector2(1024, 299);
                                plant_child.ZIndex = 47;
                            }
                            else if (i == 4)
                            {
                                plant_child.put_position = new Vector2(1024, 376);
                                plant_child.ZIndex = 67;
                            }
                            else if (i == 5)
                            {
                                plant_child.put_position = new Vector2(1024, 477);
                                plant_child.ZIndex = 87;
                            }
                            else if (i == 6)
                            {
                                plant_child.put_position = new Vector2(1024, 558);
                                plant_child.ZIndex = 107;
                            }
                            plant_child.player_put = false;
                            //plant_child._Ready();//Warning:Can't Add Child
                            GetNode<Control>("/root/In_Game/Object").AddChild(plant_child);
                        }
                        else if (background_number == 1 || background_number == 2)
                        {
                            int i = (int)(GD.Randi() % 5 + 1);
                            var scene = GD.Load<PackedScene>(Public_Main.Zombies_Path_List[result_Number - 1]);
                            var plant_child = (Normal_Zombies)scene.Instance();
                            if (i == 1)
                            {
                                plant_child.put_position = new Vector2(1024, 138);
                                plant_child.ZIndex = 7;
                            }
                            else if (i == 2)
                            {
                                plant_child.put_position = new Vector2(1024, 234);
                                plant_child.ZIndex = 27;
                            }
                            else if (i == 3)
                            {
                                plant_child.put_position = new Vector2(1024, 338);
                                plant_child.ZIndex = 47;
                            }
                            else if (i == 4)
                            {
                                plant_child.put_position = new Vector2(1024, 434);
                                plant_child.ZIndex = 67;
                            }
                            else if (i == 5)
                            {
                                plant_child.put_position = new Vector2(1024, 530);
                                plant_child.ZIndex = 87;
                            }
                            plant_child.player_put = false;
                            //plant_child._Ready();//Warning:Can't Add Child
                            GetNode<Control>("/root/In_Game/Object").AddChild(plant_child);
                        }
                    }
                    Number_in_Wave++;
                }
                now_Wave_number++;
            }
            GetNode<Flag_Control>("Main/Info/Flag").Now_Flag_Number = now_Flag_number;
            GetNode<Flag_Control>("Main/Info/Flag").Now_Wave_Number = now_Wave_number;
            GetNode<Flag_Control>("Main/Info/Flag").Update_Flag();
            if (!Ended)
            {
                Ended = (bool)file.GetValue("Wave" + now_Wave_number.ToString(), "End", false);
            }
        }
    }
}
