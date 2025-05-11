using Godot;
using System;
using System.Collections.Generic;

public class In_Game_Main : Node2D
{
    //Clone_List
    static public List<Tuple<string, Vector2,int>> Zombies_Clone_Request_List = new List<Tuple<string, Vector2,int>>();
    static public List<Tuple<string, Vector2, int>> Plant_Zombies_Clone_Request_List = new List<Tuple<string, Vector2, int>>();
    static public List<Tuple<string, Vector2, int>> Plant_Clone_Request_List = new List<Tuple<string, Vector2, int>>();
    static public List<Tuple<int, Vector2, float, bool>> Sun_Clone_Request_List = new List<Tuple<int, Vector2, float, bool>>();
    static public List<Tuple<string, Vector2, float>> Plants_Bullets_Clone_Request_List = new List<Tuple<string, Vector2, float>>();
    static public List<Tuple<string, Vector2, Health_List,string>> Plants_Hypno_Clone_Request_List = new List<Tuple<string, Vector2, Health_List, string>>();
    //Clone_List
    static public List<Node> Choosing_List = new List<Node>();
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
    static public int Card_mode = 1;
    static public bool Belt_Card = false;//传送带
    static public int Belt_Number = 0;
    static public List<int> Belt_List = new List<int>();
    static public int Tomb_Lock_To_Number = -1;
    static public int from_type = 1;
    static public bool Zombies_Died_Sun = false;
    public override async void _Ready()
    {
        GD.Randomize();
        Choosing_List.Clear();
        Zombies_Clone_Request_List.Clear();
        Plant_Zombies_Clone_Request_List.Clear();
        Plant_Clone_Request_List.Clear();
        Sun_Clone_Request_List.Clear();
        Plants_Bullets_Clone_Request_List.Clear();
        Zombies_Number = 0;
        Public_Main.now_card_number = 0;
        Belt_Number = 0;
        Has_Made_Trophy = false;
        Lost_Brain = false;
        has_Lost_Brain = false;
        Ended = false;
        is_playing = false;
        Zombies_Died_Sun = false;
        Normal_Plants.Choosing = false;
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
                GetNode<Label>("Text1").Text = level_file;
                if (level_file != string.Empty)
                {
                    if (from_type == 0 && OS.RequestPermissions()) 
                    {
                        var file_Android1 = new File();
                        var file_Android2 = new File();
                        file_Android1.Open(level_file, File.ModeFlags.Read);
                        string File_Text = file_Android1.GetAsText();
                        file_Android1.Close();
                        string save_path = "user://Users/" + Public_Main.user_name + "/Android_Load_Level.cfg";
                        file_Android2.Open(save_path, File.ModeFlags.Write);
                        file_Android2.StoreString(File_Text);
                        file_Android2.Close();
                        level_file = save_path;
                    }
                    ConfigFile file2 = new ConfigFile();
                    Error error2 = file2.Load(level_file);
                    if (error2==Error.Ok)
                    {
                        GetNode<Label>("Text1").Hide();
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
                        Tomb_Lock_To_Number = (int)file2.GetValue("Tomb", "LockTo", -1);
                        Zombies_Died_Sun = (bool)file2.GetValue("Zombies_Died", "Sun", false);
                        if (Level_Mode == -1)
                        {
                            GD.PushError("错误:关卡文件错误!Level_Mode=-1");
                            GD.Print("错误:关卡文件错误!Level_Mode=-1");
                            GetTree().Quit();
                        }
                        else if (Level_Mode == 0 || Level_Mode == 1) //正常模式
                        {
                            GetNode<TextureRect>("Main/Card/SeedBank").Show();
                            GetNode<TextureRect>("Main/Card/M2Bank").Hide();
                            Card_mode = 1;
                            int Sun = (int)file2.GetValue("Bank", "Sun", 1437);
                            Sun_Number = Sun;
                            bool Shovel = (bool)file2.GetValue("Bank", "Shovel", true);
                            bool Bug = (bool)file2.GetValue("Bank", "Bug", true);
                            bool Hammer = (bool)file2.GetValue("Bank", "Hammer", false);
                            var Sun_Label = GetNode<Label>("Main/Card/SeedBank/Sun/Sun_Text");
                            Sun_Label.Text = Sun.ToString();
                            var Shovel_Main = GetNode<TextureRect>("Main/Card/ShovelBank");
                            var Bug_Main = GetNode<TextureRect>("Main/Card/BugBank");
                            Shovel_Main.Visible = Shovel;
                            Bug_Main.Visible = Bug;
                            if (Hammer)
                            {
                                var scene = GD.Load<PackedScene>("res://scene/Hammer/Hammer.tscn");
                                var plant_child = (Hammer_Main)scene.Instance();
                                GetNode<Control>("/root/In_Game/Object").AddChild(plant_child);
                            }
                            int Card = (int)file2.GetValue("Bank", "Card", 13);
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
                            int Card_Type = (int)file2.GetValue("Card", "Type", 1);
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
                            else if (Card_Type == 3)//id序列
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
                                GetNode<TextureRect>("Main/Card/SeedBank").Show();
                                GetNode<TextureRect>("Main/Card/M2Bank").Hide();
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
                            var Page2 = GetNode<GridContainer>("/root/In_Game/Main/Choose_Card/Background/Page/Page2");
                            if (Card_Type == 3)
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
                                    if (Page1.GetChildCount() < 50)
                                    {
                                        Page1.AddChild(card_child);
                                    }
                                    else if (Page2.GetChildCount() < 50)
                                    {
                                        Page2.AddChild(card_child);
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < Max_Plant_Id; i++)
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
                                    if (Page1.GetChildCount() < 50)
                                    {
                                        Page1.AddChild(card_child);
                                    }
                                    else if (Page2.GetChildCount() < 50)
                                    {
                                        Page2.AddChild(card_child);
                                    }
                                }
                            }
                            if (Card_Type == 1)
                            {
                                for (int i = 0; i < Public_Main.Spec_Plants_list.Count; i++)
                                {
                                    var scene = GD.Load<PackedScene>("res://scene/Card/Card.tscn");
                                    var card_child = (Card_Main)scene.Instance();
                                    card_child.Card_Number[0] = 4;
                                    card_child.Card_Number[1] = i + 1;
                                    if ((bool)file2.GetValue("Level_Mode", "Spec", false))
                                    {
                                        int Adding_Number2 = 1;
                                        while (true)
                                        {
                                            int Ans_Number2 = (int)file2.GetValue("Level_Mode", "S" + Adding_Number2.ToString(), -1);
                                            if (Ans_Number2 != -1)
                                            {
                                                if (Ans_Number2 == i + 1)
                                                {
                                                    card_child.self_Wait_Time_Set = true;
                                                    card_child.wait_time = 0f;
                                                }
                                            }
                                            else
                                            {
                                                break;
                                            }
                                            Adding_Number2++;
                                        }
                                    }
                                    card_child._Ready();
                                    if (Page1.GetChildCount() < 50)
                                    {
                                        Page1.AddChild(card_child);
                                    }
                                    else if (Page2.GetChildCount() < 50)
                                    {
                                        Page2.AddChild(card_child);
                                    }
                                }
                            }
                            else
                            {
                                if ((int)file2.GetValue("Card_Spec", "1", -1) != -1)
                                {
                                    int Adding_Number = 1;
                                    while (true)
                                    {
                                        int Ans_Number = (int)file2.GetValue("Card_Spec", Adding_Number.ToString(), -1);
                                        if (Ans_Number != -1)
                                        {
                                            var scene = GD.Load<PackedScene>("res://scene/Card/Card.tscn");
                                            var card_child = (Card_Main)scene.Instance();
                                            card_child.Card_Number[0] = 4;
                                            card_child.Card_Number[1] = Ans_Number;
                                            if ((bool)file2.GetValue("Level_Mode", "Spec", false))
                                            {
                                                int Adding_Number2 = 1;
                                                while (true)
                                                {
                                                    int Ans_Number2 = (int)file2.GetValue("Level_Mode", "S" + Adding_Number2.ToString(), -1);
                                                    if (Ans_Number2 != -1)
                                                    {
                                                        if (Ans_Number2 == Ans_Number)
                                                        {
                                                            card_child.self_Wait_Time_Set = true;
                                                            card_child.wait_time = 0f;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        break;
                                                    }
                                                    Adding_Number2++;
                                                }
                                            }
                                            card_child._Ready();
                                            if (Page1.GetChildCount() < 50)
                                            {
                                                Page1.AddChild(card_child);
                                            }
                                            else if (Page2.GetChildCount() < 50)
                                            {
                                                Page2.AddChild(card_child);
                                            }
                                        }
                                        else
                                        {
                                            break;
                                        }
                                        Adding_Number++;
                                    }
                                }
                            }
                            for (int i = 0; i < Max_Plants_Zombies_Id; i++)
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
                                if (Page1.GetChildCount() < 50)
                                {
                                    Page1.AddChild(card_child);
                                }
                                else if (Page2.GetChildCount() < 50)
                                {
                                    Page2.AddChild(card_child);
                                }
                            }
                            for (int i = 0; i < Max_Zombies_Id; i++)
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
                                if (Page1.GetChildCount() < 50)
                                {
                                    Page1.AddChild(card_child);
                                }
                                else if (Page2.GetChildCount() < 50)
                                {
                                    Page2.AddChild(card_child);
                                }
                            }
                        }
                        else if (Level_Mode == 2)//传送带
                        {
                            GetNode<TextureRect>("Main/Card/SeedBank").Hide();
                            GetNode<TextureRect>("Main/Card/M2Bank").Show();
                            Card_mode = 2;
                            int Sun = (int)file2.GetValue("Bank", "Sun", 1437);
                            Sun_Number = Sun;
                            bool Shovel = (bool)file2.GetValue("Bank", "Shovel", true);
                            bool Bug = (bool)file2.GetValue("Bank", "Bug", true);
                            bool Hammer = (bool)file2.GetValue("Bank", "Hammer", false);
                            var Sun_Label = GetNode<Label>("Main/Card/M2Bank/Sun/Sun_Text");
                            Sun_Label.Text = Sun.ToString();
                            var Shovel_Main = GetNode<TextureRect>("Main/Card/ShovelBank");
                            var Bug_Main = GetNode<TextureRect>("Main/Card/BugBank");
                            Shovel_Main.Visible = Shovel;
                            Bug_Main.Visible = Bug;
                            if (Hammer)
                            {
                                var scene = GD.Load<PackedScene>("res://scene/Hammer/Hammer.tscn");
                                var plant_child = (Hammer_Main)scene.Instance();
                                GetNode<Control>("/root/In_Game/Object").AddChild(plant_child);
                            }
                            int Card = (int)file2.GetValue("Bank", "Card", 13);
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
                            Belt_Card = (int)file2.GetValue("Card", "Type", 0) == 4;
                            if (Belt_Card)
                            {
                                Belt_List.Clear();
                                int loop_number = 0;
                                while (true)
                                {
                                    int get_card_number = (int)file2.GetValue("Plant_Card", loop_number.ToString(), -1);
                                    if (get_card_number == -1)
                                    {
                                        break;
                                    }
                                    Belt_List.Add(get_card_number);
                                    loop_number++;
                                }
                            }
                            var player1 = GetNode<AnimationPlayer>("Main/AnimationPlayer");
                            var player2 = GetNode<AnimationPlayer>("Main/Card_Player");
                            player1.Play("C_to_R");
                            await ToSignal(player1, "animation_finished");
                            player2.Play("Belt_load");
                            await ToSignal(player2, "animation_finished");
                            player1.Play("R_to_C");
                            await ToSignal(player1, "animation_finished");
                            Game_Start_Effect(this);
                            GetNode<Timer>("Belt_Timer").Start();
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
        node.GetNode<Label>("/root/In_Game/Main/Card/SeedBank/Sun/Sun_Text").Text = Sun_Number.ToString();
        node.GetNode<Label>("/root/In_Game/Main/Card/M2Bank/Sun/Sun_Text").Text = Sun_Number.ToString();
    }
    static public void Warning_Sun(Node node)
    {
        node.GetNode<AudioStreamPlayer>("/root/In_Game/Main/Card/SeedBank/Sun/Pause").Stream.Set("loop", false);
        node.GetNode<AudioStreamPlayer>("/root/In_Game/Main/Card/M2Bank/Sun/Pause").Stream.Set("loop", false);
        if (In_Game_Main.Card_mode == 1)
        {
            var Sun_Warning = node.GetNode<AnimationPlayer>("/root/In_Game/Main/Card/SeedBank/Sun/Warning");
            Sun_Warning.Play("Warning");
            node.GetNode<AudioStreamPlayer>("/root/In_Game/Main/Card/SeedBank/Sun/Pause").Play();
        }
        else if (In_Game_Main.Card_mode == 2)
        {
            var Sun_Warning = node.GetNode<AnimationPlayer>("/root/In_Game/Main/Card/M2Bank/Sun/Warning");
            Sun_Warning.Play("Warning");
            node.GetNode<AudioStreamPlayer>("/root/In_Game/Main/Card/M2Bank/Sun/Pause").Play();
        }
    }
    public override async void _Process(float delta)
    {
        for (int i = 0; i < Choosing_List.Count; i++)
        {
            if (Choosing_List[i] == null || !IsInstanceValid(Choosing_List[i]))
            {
                Choosing_List.RemoveAt(i);
                i--;
                continue;
            }
        }
        if (Choosing_List.Count != 0 && !Normal_Plants.Choosing)
        {
            Normal_Plants.Choosing = true;
        }
        if (Public_Main.debuging && is_playing && Belt_Card) 
        {
            Belt_Timer_timeout();
        }
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
                    sun_child.is_from_plant = Sun_Clone_Request_List[0].Item4;
                    sun_child.sun_value = Sun_Clone_Request_List[0].Item1;
                    GetNode<Control>("/root/In_Game/Object").AddChild(sun_child);
                    sun_child.GlobalPosition = Sun_Clone_Request_List[0].Item2;
                    Sun_Clone_Request_List.RemoveAt(0);
                    Clone_Number++;
                    Must_Quit = false;
                }
                if (Plants_Bullets_Clone_Request_List.Count != 0)
                {
                    var scene = GD.Load<PackedScene>(Plants_Bullets_Clone_Request_List[0].Item1);
                    var Bullets_child = (Normal_Plants_Bullets)scene.Instance();
                    Bullets_child.speed_y = Plants_Bullets_Clone_Request_List[0].Item3;
                    GetNode<Control>("/root/In_Game/Object").AddChild(Bullets_child);
                    Bullets_child.GlobalPosition = Plants_Bullets_Clone_Request_List[0].Item2;
                    Plants_Bullets_Clone_Request_List.RemoveAt(0);
                    Clone_Number++;
                    Must_Quit = false;
                }
                if (Zombies_Clone_Request_List.Count != 0)
                {
                    var scene = GD.Load<PackedScene>(Zombies_Clone_Request_List[0].Item1);
                    if (Zombies_Clone_Request_List[0].Item1 == Public_Main.Zombies_Path_List[8])
                    {
                        var plant_child = (Tomb_Main)scene.Instance();
                        plant_child.ZIndex = Zombies_Clone_Request_List[0].Item3 - 4;
                        plant_child.put_position = new Vector2(710 - 80 * (GD.Randi() % 3), Zombies_Clone_Request_List[0].Item2.y);
                        plant_child.player_put = false;
                        GetNode<Control>("/root/In_Game/Object").AddChild(plant_child);
                    }
                    else
                    {
                        var plant_child = (Normal_Zombies)scene.Instance();
                        plant_child.ZIndex = Zombies_Clone_Request_List[0].Item3;
                        plant_child.put_position = Zombies_Clone_Request_List[0].Item2;
                        plant_child.player_put = false;
                        GetNode<Control>("/root/In_Game/Object").AddChild(plant_child);
                    }
                    Clone_Number++;
                    Zombies_Clone_Request_List.RemoveAt(0);
                    Must_Quit = false;
                }
                if (Plant_Zombies_Clone_Request_List.Count != 0)
                {
                    var scene = GD.Load<PackedScene>(Plant_Zombies_Clone_Request_List[0].Item1);
                    if (Plant_Zombies_Clone_Request_List[0].Item1 == Public_Main.Plants_Zombies_list[8].Rest.Item2)
                    {
                        var plant_child = (Plants_Tomb_Main)scene.Instance();
                        plant_child.ZIndex = Plant_Zombies_Clone_Request_List[0].Item3 - 4;
                        plant_child.put_position = Plant_Zombies_Clone_Request_List[0].Item2;
                        plant_child.player_put = false;
                        GetNode<Control>("/root/In_Game/Object").AddChild(plant_child);
                    }
                    else
                    {
                        var plant_child = (Normal_Plants_Zombies)scene.Instance();
                        plant_child.ZIndex = Plant_Zombies_Clone_Request_List[0].Item3;
                        plant_child.put_position = Plant_Zombies_Clone_Request_List[0].Item2;
                        plant_child.player_put = false;
                        GetNode<Control>("/root/In_Game/Object").AddChild(plant_child);
                    }
                    Clone_Number++;
                    Plant_Zombies_Clone_Request_List.RemoveAt(0);
                    Must_Quit = false;
                }
                if (Plants_Hypno_Clone_Request_List.Count != 0)
                {
                    var scene = GD.Load<PackedScene>(Plants_Hypno_Clone_Request_List[0].Item1);
                    var plant_child = (Normal_Plants_Zombies)scene.Instance();
                    plant_child.Hypno_Health = Plants_Hypno_Clone_Request_List[0].Item3;
                    plant_child.Hypno_Pos = Plants_Hypno_Clone_Request_List[0].Item2;
                    plant_child.Hypno_Spec_Info = Plants_Hypno_Clone_Request_List[0].Item4;
                    plant_child.is_Hypnoed = true;
                    plant_child.player_put = false;
                    GetNode<Control>("/root/In_Game/Object").AddChild(plant_child);
                    Clone_Number++;
                    Plants_Hypno_Clone_Request_List.RemoveAt(0);
                    Must_Quit = false;
                }
                if (Plant_Clone_Request_List.Count != 0)
                {
                    var scene = GD.Load<PackedScene>(Plant_Clone_Request_List[0].Item1);
                    try
                    {
                        var plant_child = (Normal_Plants)scene.Instance();
                        plant_child.ZIndex = Plant_Clone_Request_List[0].Item3;
                        plant_child.put_position = Plant_Clone_Request_List[0].Item2;
                        plant_child.player_put = false;
                        GetNode<Control>("/root/In_Game/Object").AddChild(plant_child);
                    }
                    catch (Exception)
                    {
                        var plant_child = (Limited_Plants)scene.Instance();
                        plant_child.ZIndex = Plant_Clone_Request_List[0].Item3;
                        plant_child.put_position = Plant_Clone_Request_List[0].Item2;
                        plant_child.player_put = false;
                        GetNode<Control>("/root/In_Game/Object").AddChild(plant_child);
                    }
                    Clone_Number++;
                    Plant_Clone_Request_List.RemoveAt(0);
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
        if (is_playing && !GetTree().Paused) 
        {
            int press_number = -1;
            if (Input.IsActionJustPressed("1"))
            {
                press_number = 1;
            }
            else if (Input.IsActionJustPressed("2"))
            {
                press_number = 2;
            }
            else if (Input.IsActionJustPressed("3"))
            {
                press_number = 3;
            }
            else if (Input.IsActionJustPressed("4"))
            {
                press_number = 4;
            }
            else if (Input.IsActionJustPressed("5"))
            {
                press_number = 5;
            }
            else if (Input.IsActionJustPressed("6"))
            {
                press_number = 6;
            }
            else if (Input.IsActionJustPressed("7"))
            {
                press_number = 7;
            }
            else if (Input.IsActionJustPressed("8"))
            {
                press_number = 8;
            }
            else if (Input.IsActionJustPressed("9"))
            {
                press_number = 9;
            }
            else if (Input.IsActionJustPressed("0"))
            {
                press_number = 10;
            }
            else
            {
                press_number = -1;
            }
            if (press_number != -1)
            {
                ConfigFile file2 = new ConfigFile();
                Error error2 = file2.Load(level_file);
                if (error2 == Error.Ok)
                {
                    int Level_Mode = (int)file2.GetValue("Level_Mode", "mode", -1);
                    if (Normal_Plants.Choosing)
                    {
                        Input.ActionPress("Right_Mouse");
                        await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
                        Input.ActionRelease("Right_Mouse");
                    }
                    else
                    {
                        if (Level_Mode == 0 || Level_Mode == 1)
                        {
                            var Seed_Bank = GetNode<HBoxContainer>("/root/In_Game/Main/Card/SeedBank/Seed");
                            if (press_number <= Seed_Bank.GetChildCount())
                            {
                                Seed_Bank.GetChild(press_number - 1).GetNode<Card_Click_Button>("./Main/Click_Button").Button_Pressed();
                            }
                        }
                        else if (Level_Mode == 2)//Belt
                        {
                            var Seed_Bank = GetNode<Control>("/root/In_Game/Main/Card/M2Bank/Seed");
                            if (press_number <= Seed_Bank.GetChildCount())
                            {
                                Seed_Bank.GetChild(press_number - 1).GetNode<Card_Tmp_Main>(".").Button_Click();
                            }
                        }
                    }
                }
            }
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
    public void Belt_Timer_timeout()
    {
        if (is_playing && Belt_Card && Belt_List.Count != 0 && In_Game_Main.Belt_Number < Public_Main.allow_card_number)   
        {
            int id = Belt_List[(int)(GD.Randi() % Belt_List.Count)];
            var scene = GD.Load<PackedScene>("res://scene/Card_Tmp/Card_Tmp.tscn");
            var plant_child = (Card_Tmp_Main)scene.Instance();
            plant_child.Init_Tmp_Card(1, id, (int)(GD.Randi() % 4) <= 2, 0);
            GetNode<Control>("/root/In_Game/Main/Card/M2Bank/Seed").AddChild(plant_child);
        }
    }
    static public void Zombies_Clone_Request(string Clone_String,Vector2 pos,int Z_index)
    {
        Zombies_Clone_Request_List.Add(new Tuple<string, Vector2, int>(Clone_String, pos, Z_index));
    }
    static public void Plants_Zombies_Clone_Request(string Clone_String, Vector2 pos, int Z_index)
    {
        Plant_Zombies_Clone_Request_List.Add(new Tuple<string, Vector2, int>(Clone_String, pos, Z_index));
    }
    static public void Plants_Clone_Request(string Clone_String, Vector2 pos, int Z_index)
    {
        Plant_Clone_Request_List.Add(new Tuple<string, Vector2, int>(Clone_String, pos, Z_index));
    }
    static public void Sun_Clone_Request(int Value, Vector2 pos, float size, bool from_Plants)
    {
        Sun_Clone_Request_List.Add(new Tuple<int, Vector2, float, bool>(Value, pos, size, from_Plants));
    }
    static public void Plants_Bullets_Clone_Request(string Path, Vector2 pos, float _y = 0f)
    {
        Plants_Bullets_Clone_Request_List.Add(new Tuple<string, Vector2, float>(Path, pos, _y));
    }
    static public void Plants_Hypno_Clone_Request(string Path, Vector2 pos, Health_List health_list,string Spec_Info)
    {
        Plants_Hypno_Clone_Request_List.Add(new Tuple<string, Vector2, Health_List, string>(Path, pos, health_list, Spec_Info));
    }
    public void Next_Wave_pressed()
    {
        if (is_playing)
        {
            Wave_Timer_Out();
            GetNode<Timer>("Wave_Timer").Stop();
        }
    }
    static public async void Game_Start_Effect(Node node)
    {
        var Before_bgm = node.GetNode<AudioStreamPlayer>("/root/In_Game/Before_bgm");
        Before_bgm.Stop();
        var player3 = node.GetNode<AnimationPlayer>("/root/In_Game/Ready_Animation");
        player3.Play("start");
        In_Game_Main.is_playing = true;
        await node.ToSignal(node.GetTree().CreateTimer(3), "timeout");
        if (In_Game_Main.background_number == 1)
        {
            var B1_bgm = node.GetNode<AudioStreamPlayer>("/root/In_Game/Music/GrassWalk");
            B1_bgm.Play();
        }
        else if (In_Game_Main.background_number == 2)
        {
            var B2_bgm = node.GetNode<AudioStreamPlayer>("/root/In_Game/Music/MoonGrains");
            B2_bgm.Play();
        }
        else if (In_Game_Main.background_number == 3)
        {
            var B3_bgm = node.GetNode<AudioStreamPlayer>("/root/In_Game/Music/WateryGraves");
            B3_bgm.Play();
        }
    }
}
