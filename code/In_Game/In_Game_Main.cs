using Godot;
using System;

public class In_Game_Main : Node2D
{
    static public int Sun_Number;
    static public int background_number;
    static public bool is_playing=false;
    public bool allow_sun = false;
    public override async void _Ready()
    {
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
                string level_file = (string)file.GetValue("Level", "Level_Name", string.Empty);
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
                            Public_Main.allow_card_number = Card;
                            var player1 = GetNode<AnimationPlayer>("Main/AnimationPlayer");
                            var player2 = GetNode<AnimationPlayer>("Main/Card_Player");
                            player1.Play("C_to_R");
                            await ToSignal(player1, "animation_finished");
                            player2.Play("load");
                            int Max_Plant_Id = 0;
                            int Max_Plants_Zombies_Id = 0;
                            int Max_Zombies_Id = 0;
                            for (int i = 0; i < Public_Main.Plant_list.Count; i++)
                            { 
                                if (Public_Main.Plant_list[i].Item1>Max_Plant_Id)
                                {
                                    Max_Plant_Id = Public_Main.Plant_list[i].Item1;
                                }
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
}
