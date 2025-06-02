using Godot;
using System;

public class Card_Click_Button : Node2D
{
    public bool Card_Is_Running = false;
    public Control Card_Node=null;
    public string plant_path = null;
    public int sun = 0;
    public float now_time = 0f;
    public float wait_time = 30f;
    public Vector2 normal_size;
    public bool is_zombies = false;
    public bool isPlants_zombies = false;
    public bool has_Clicked = false;
    public bool Await_Running = false;
    public Button Click_Button_Button = null;
    //private Object
    public override void _Ready()
    {
        Click_Button_Button = GetNode<Button>("Click_Button");
        has_Clicked = false;
        normal_size.x = 100;
        normal_size.y = 0;
        Card_Node = GetNode<Control>("../../..");
        var button_Click = GetNode<AudioStreamPlayer>("../button_Click");
        button_Click.Stream.Set("loop", false);
        GetNode<AudioStreamPlayer>("Seed").Stream.Set("loop", false);
    }
    public void Mouse_EnterEvent()
    {
        if (GetTree().Paused)
        {
            return;
        }
        var Info = GetNode<Node2D>("../Info");
        Info.Show();
    }
    public void Mouse_ExitEvent()
    {
        var Info = GetNode<Node2D>("../Info");
        Info.Hide();
    }
    public async void Button_Pressed()
    {
        if (has_Clicked)
        {
            return;
        }
        if (In_Game_Main.is_playing) 
        {
            if (In_Game_Main.really_start)
            {
                if (In_Game_Main.Sun_Number < this.sun && !Public_Main.debuging)
                {
                    In_Game_Main.Warning_Sun(this);
                }
                else
                {
                    if (now_time <= 0f || Public_Main.debuging)
                    {
                        if (In_Game_Main.Choosing_List.Count == 0)
                        {
                            Normal_Plants.Choosing = false;
                        }
                        if (!plant_path.Empty() && !Normal_Plants.Choosing)
                        {
                            GetNode<ColorRect>("../Texture/Shadow_2").Show();
                            GetNode<AudioStreamPlayer>("Seed").Play();
                            var scene = GD.Load<PackedScene>(plant_path);
                            Normal_Plants.Choosing = true;
                            if (is_zombies)
                            {//为墓碑施工
                                try
                                {
                                    var plant_child = (Normal_Zombies)scene.Instance();
                                    plant_child.player_put = true;
                                    plant_child.card_parent_Button = this;
                                    GetNode<Control>("/root/In_Game/Object").AddChild(plant_child);
                                }
                                catch (Exception)
                                {
                                    var plant_child = (Tomb_Main)scene.Instance();
                                    plant_child.player_put = true;
                                    plant_child.card_parent_Button = this;
                                    GetNode<Control>("/root/In_Game/Object").AddChild(plant_child);
                                }
                            }
                            else if (isPlants_zombies)
                            {
                                try
                                {
                                    var plant_child = (Normal_Plants_Zombies)scene.Instance();
                                    plant_child.player_put = true;
                                    plant_child.card_parent_Button = this;
                                    GetNode<Control>("/root/In_Game/Object").AddChild(plant_child);
                                }
                                catch (Exception)
                                {
                                    var plant_child = (Plants_Tomb_Main)scene.Instance();
                                    plant_child.player_put = true;
                                    plant_child.card_parent_Button = this;
                                    GetNode<Control>("/root/In_Game/Object").AddChild(plant_child);
                                }
                            }
                            else
                            {
                                try
                                {
                                    var plant_child = (Normal_Plants)scene.Instance();
                                    plant_child.player_put = true;
                                    plant_child.card_parent_Button = this;
                                    GetNode<Control>("/root/In_Game/Object").AddChild(plant_child);
                                }
                                catch (Exception)
                                {
                                    var plant_child = (Limited_Plants)scene.Instance();
                                    plant_child.player_put = true;
                                    plant_child.card_parent_Button = this;
                                    GetNode<Control>("/root/In_Game/Object").AddChild(plant_child);
                                }
                            }
                        }
                        else if (Normal_Plants.Choosing)
                        {
                            Normal_Plants.Choosing = false;
                        }
                    }
                    else
                    {
                        GetNode<AudioStreamPlayer>("/root/In_Game/Main/Card/SeedBank/Sun/Pause").Play();
                    }
                }
            }
        }
        else
        {
            var button_Click = GetNode<AudioStreamPlayer>("../button_Click");
            button_Click.Play();
            if (!Card_Is_Running)
            {
                Card_Is_Running = true;
                if (this.GetParent().GetParent().GetParent() != Card_Node)
                {
                    Public_Main.now_card_number--;
                    var Info = GetNode<Node2D>("../Info");
                    Info.Hide();
                    Vector2 Delta_Vector2 = (Card_Node.RectGlobalPosition - this.GetNode<Control>("../..").RectGlobalPosition) / 5;
                    GetNode<Node2D>("..").ZIndex = 120;
                    for (int i = 0; i < 5; i++)
                    {
                        await ToSignal(GetTree().CreateTimer(0.03f), "timeout");
                        this.GetNode<Control>("../..").RectGlobalPosition += Delta_Vector2;
                    }
                    GetNode<Node2D>("..").ZIndex = 1;
                    this.GetParent().GetParent().GetParent().RemoveChild(this.GetParent().GetParent());
                    Card_Node.AddChild(this.GetParent().GetParent());
                    this.GetNode<Control>("../..").RectPosition = Vector2.Zero;
                    Info.Hide();
                    if (this.GetParent().GetParent().GetParent() != Card_Node)
                    {
                        Public_Main.now_card_number++;
                    }
                }//seed bank
                else
                {
                    var Seed_Bank = GetNode<HBoxContainer>("/root/In_Game/Main/Card/SeedBank/Seed");
                    if (Public_Main.allow_card_number > Public_Main.now_card_number)
                    {
                        var Info = GetNode<Node2D>("../Info");
                        Info.Hide();
                        Vector2 Delta_Vector2;
                        if (Public_Main.now_card_number == 0)
                        {
                            Delta_Vector2 = (Card_Node.RectGlobalPosition - Seed_Bank.RectGlobalPosition) / 5;
                        }
                        else
                        {
                            Delta_Vector2 = (Card_Node.RectGlobalPosition - Seed_Bank.RectGlobalPosition - new Vector2(Public_Main.now_card_number * 55, 0)) / 5;
                        }
                        Public_Main.now_card_number++;
                        GetNode<Node2D>("..").ZIndex = 120;
                        for (int i = 0; i < 5; i++)
                        {
                            await ToSignal(GetTree().CreateTimer(0.03f), "timeout");
                            this.GetNode<Control>("../..").RectGlobalPosition -= Delta_Vector2;
                        }
                        GetNode<Node2D>("..").ZIndex = 1;
                        Card_Node.RemoveChild(this.GetParent().GetParent());
                        Seed_Bank.AddChild(this.GetParent().GetParent());
                        Info.Hide();
                        if (this.GetParent().GetParent().GetParent() != Seed_Bank)
                        {
                            Public_Main.now_card_number--;
                        }
                    }
                }//card
                Card_Is_Running = false;
            }
        }
    }
    public override void _Process(float delta)
    {
        GetNode<CanvasLayer>("Canvas").Offset = this.GlobalPosition;
        if (In_Game_Main.is_playing && Click_Button_Button.GetParent() != GetNode<CanvasLayer>("Canvas")) 
        {
            RemoveChild(Click_Button_Button);
            GetNode<CanvasLayer>("Canvas").AddChild(Click_Button_Button);
        }
        if (In_Game_Main.is_playing && !Public_Main.debuging)
        {
            if (In_Game_Main.Sun_Number >= this.sun&&now_time<=0)
            {
                GetNode<ColorRect>("../Texture/Shadow").Hide();
            }
            else
            {
                GetNode<ColorRect>("../Texture/Shadow").Show();
            }
            if (now_time <= 0)
            {
                now_time = 0;
                GetNode<ColorRect>("../Texture/Wait_Time").Hide();
            }
            else
            {
                GetNode<ColorRect>("../Texture/Wait_Time").Show();
            }
            if (GetNode<ColorRect>("../Texture/Wait_Time").RectSize.y > 140f)
            {
                normal_size.y = 140;
                GetNode<ColorRect>("../Texture/Wait_Time").SetSize(normal_size);
            }
        }
    }
    public void Wait_TimerOut()
    {
        if (In_Game_Main.is_playing&&this.GetParent().GetParent().GetParent() != Card_Node)
        {
            if (now_time > 0)
            {
                now_time -= 0.1f;
            }
            var Cd_Label = GetNode<Label>("../Info/Info/1/2/3/4/Cd");
            Cd_Label.Text = "CD:" + Math.Round(now_time,2).ToString() + "s";
            normal_size.y = (now_time / wait_time) * 140f;
            GetNode<ColorRect>("../Texture/Wait_Time").RectSize = normal_size;
        }
    }
    public void Set_ColorRect_2(bool show)
    {
        GetNode<ColorRect>("../Texture/Shadow_2").Visible = show;
    }
}
