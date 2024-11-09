using Godot;
using System;

public class Card_Click_Button : Button
{
    public Control Card_Node=null;
    public string plant_path = null;
    public int sun = 0;
    public float now_time = 0f;
    public float wait_time = 30f;
    public Vector2 normal_size;
    public bool is_zombies = false;
    public bool isPlants_zombies = false;
    //private Object
    public override void _Ready()
    {
        normal_size.x = 100;
        normal_size.y = 0;
        Card_Node = GetNode<Control>("../../..");
        var button_Click = GetNode<AudioStreamPlayer>("../button_Click");
        button_Click.Stream.Set("loop", false);
        GetNode<AudioStreamPlayer>("Seed").Stream.Set("loop", false);
        Connect("mouse_entered", this, "Mouse_EnterEvent");
        Connect("mouse_exited", this, "Mouse_ExitEvent");
    }
    public void Mouse_EnterEvent()
    {
        var Info = GetNode<Node2D>("../Info");
        Info.Show();
    }
    public void Mouse_ExitEvent()
    {
        var Info = GetNode<Node2D>("../Info");
        Info.Hide();
    }
    public async override void _Pressed()
    {
        if (In_Game_Main.is_playing)
        {
            if (In_Game_Main.Sun_Number < this.sun && !Public_Main.debuging)
            {
                In_Game_Main.Warning_Sun(this);
            }
            else
            {
                if (now_time <= 0f || Public_Main.debuging)
                {
                    if (!plant_path.Empty() && !Normal_Plants.Choosing)
                    {
                        GetNode<ColorRect>("../Texture/Shadow_2").Show();
                        GetNode<AudioStreamPlayer>("Seed").Play();
                        var scene = GD.Load<PackedScene>(plant_path);
                        Normal_Plants.Choosing = true;
                        if (is_zombies)
                        {
                            var plant_child = (Normal_Zombies)scene.Instance();
                            plant_child.player_put = true;
                            plant_child.card_parent_Button = this;
                            GetNode<Control>("/root/In_Game/Object").AddChild(plant_child);
                        }
                        else if (isPlants_zombies)
                        {
                            var plant_child = (Normal_Plants_Zombies)scene.Instance();
                            plant_child.player_put = true;
                            plant_child.card_parent_Button = this;
                            GetNode<Control>("/root/In_Game/Object").AddChild(plant_child);
                        }
                        else
                        {
                            var plant_child = (Normal_Plants)scene.Instance();
                            plant_child.card_parent_Button = this;
                            GetNode<Control>("/root/In_Game/Object").AddChild(plant_child);
                        }
                    }
                }
                else
                {
                    GetNode<AudioStreamPlayer>("/root/In_Game/Main/Card/SeedBank/Sun/Pause").Play();
                }
            }
        }
        else
        {
            var button_Click = GetNode<AudioStreamPlayer>("../button_Click");
            button_Click.Play();
            if (this.GetParent().GetParent().GetParent() != Card_Node)
            {
                var Info = GetNode<Node2D>("../Info");
                Info.Hide();
                Vector2 Delta_Vector2 = (Card_Node.RectGlobalPosition - this.GetNode<Control>("../..").RectGlobalPosition) / 5;
                GetNode<Node2D>("..").ZIndex += 1;
                for (int i = 0; i < 5; i++)
                {
                    await ToSignal(GetTree().CreateTimer(0.03f), "timeout");
                    this.GetNode<Control>("../..").RectGlobalPosition += Delta_Vector2;
                }
                GetNode<Node2D>("..").ZIndex -= 1;
                this.GetParent().GetParent().GetParent().RemoveChild(this.GetParent().GetParent());
                Card_Node.AddChild(this.GetParent().GetParent());
                this.GetNode<Control>("../..").RectPosition = Vector2.Zero;
                Info.Hide();
            }//seed bank
            else
            {
                var Seed_Bank = GetNode<HBoxContainer>("/root/In_Game/Main/Card/SeedBank/Seed");
                if (Public_Main.allow_card_number > Seed_Bank.GetChildCount())
                {
                    var Info = GetNode<Node2D>("../Info");
                    Info.Hide();
                    Vector2 Delta_Vector2;
                    if (Seed_Bank.GetChildCount() == 0)
                    {
                        Delta_Vector2 = (Card_Node.RectGlobalPosition - Seed_Bank.RectGlobalPosition) / 5;
                    }
                    else
                    {
                        Delta_Vector2 = (Card_Node.RectGlobalPosition - Seed_Bank.GetChild<Control>(Seed_Bank.GetChildCount() - 1).RectGlobalPosition - new Vector2(3 + Card_Node.RectSize.x, 0)) / 5;
                    }
                    GetNode<Node2D>("..").ZIndex += 1;
                    for (int i = 0; i < 5; i++)
                    {
                        await ToSignal(GetTree().CreateTimer(0.03f), "timeout");
                        this.GetNode<Control>("../..").RectGlobalPosition -= Delta_Vector2;
                    }
                    GetNode<Node2D>("..").ZIndex -= 1;
                    Card_Node.RemoveChild(this.GetParent().GetParent());
                    Seed_Bank.AddChild(this.GetParent().GetParent());
                    Info.Hide();
                }
            }//card
        }
    }
    public override void _Process(float delta)
    {
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
