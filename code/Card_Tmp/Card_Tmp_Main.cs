using Godot;
using System;

public class Card_Tmp_Main : Node2D
{
    protected string Plant_Path = null;
    public int Sun = 0;
    public bool is_zombies = false;
    public bool isPlants_zombies = false;
    public bool has_Clicked = false;
    public void Init_Tmp_Card(int list_number, int id, bool this_sun = false, int sun = 0)
    {
        GetNode<TextureRect>("All/Main/Background/1").Visible = list_number == 1 || list_number == 3;
        GetNode<TextureRect>("All/Main/Background/2").Visible = list_number == 2;
        bool has_id = false;
        if (list_number == 1)
        {
            for (int i = 0; i < Public_Main.Plant_list.Count; i++)
            {
                if (id == Public_Main.Plant_list[i].Item1)
                {
                    Sun = Public_Main.Plant_list[i].Rest.Item1;
                    Plant_Path = Public_Main.Plant_list[i].Rest.Item2;
                    GetNode<TextureRect>("All/Main/Texture/Card_Texture").Texture = Public_Main.Plant_list[i].Item5;
                    GetNode<Label>("All/Info/Info/1/2/3/4/Name").Text = Public_Main.Plant_list[i].Item2;
                    GetNode<Label>("All/Info/Info/1/2/3/4/Info").Text = Public_Main.Plant_list[i].Item3;
                    has_id = true;
                    break;
                }
            }
        }
        else if (list_number == 3)
        {
            for (int i = 0; i < Public_Main.Plants_Zombies_list.Count; i++)
            {
                if (id == Public_Main.Plants_Zombies_list[i].Item1)
                {
                    Sun = Public_Main.Plants_Zombies_list[i].Rest.Item1;
                    Plant_Path = Public_Main.Plants_Zombies_list[i].Rest.Item2;
                    GetNode<TextureRect>("All/Main/Texture/Card_Texture").Texture = Public_Main.Plants_Zombies_list[i].Item5;
                    GetNode<Label>("All/Info/Info/1/2/3/4/Name").Text = Public_Main.Plants_Zombies_list[i].Item2;
                    GetNode<Label>("All/Info/Info/1/2/3/4/Info").Text = Public_Main.Plants_Zombies_list[i].Item3;
                    has_id = true;
                    isPlants_zombies = true;
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < Public_Main.Zombies_list.Count; i++)
            {
                if (id == Public_Main.Zombies_list[i].Item1)
                {
                    Sun = Public_Main.Zombies_list[i].Rest.Item1;
                    Plant_Path = Public_Main.Zombies_list[i].Rest.Item2;
                    GetNode<TextureRect>("All/Main/Texture/Card_Texture").Texture = Public_Main.Zombies_list[i].Item5;
                    GetNode<Label>("All/Info/Info/1/2/3/4/Name").Text = Public_Main.Zombies_list[i].Item2;
                    GetNode<Label>("All/Info/Info/1/2/3/4/Info").Text = Public_Main.Zombies_list[i].Item3;
                    has_id = true;
                    is_zombies = true;
                    break;
                }
            }
        }
        if (!has_id)
        {
            In_Game_Main.Belt_Number--;
            QueueFree();
            return;
        }
        if (this_sun)
        {
            Sun = sun;
        }
        GetNode<Label>("All/Main/Sun").Text = Sun.ToString();
    }
    protected void Mouse_EnterEvent()
    {
        GetNode<CanvasLayer>("All/Info").Show();
    }
    protected void Mouse_ExitEvent()
    {
        GetNode<CanvasLayer>("All/Info").Hide();
    }
    protected void Button_Click()
    {
        if (has_Clicked)//?
        {
            return;
        }
        if (In_Game_Main.is_playing)
        {
            if (In_Game_Main.Sun_Number < this.Sun && !Public_Main.debuging)
            {
                In_Game_Main.Warning_Sun(this);
            }
            else
            {
                if (!Plant_Path.Empty() && !Normal_Plants.Choosing)
                {
                    GetNode<ColorRect>("All/Main/Shadow").Show();
                    GetNode<AudioStreamPlayer>("All/Main/Sound/Seed").Play();
                    var scene = GD.Load<PackedScene>(Plant_Path);
                    Normal_Plants.Choosing = true;
                    if (is_zombies)
                    {
                        var plant_child = (Normal_Zombies)scene.Instance();
                        plant_child.player_put = true;
                        plant_child.Tmp_card_parent = this;
                        plant_child.Tmp_Card_Used = true;
                        GetNode<Control>("/root/In_Game/Object").AddChild(plant_child);
                    }
                    else if (isPlants_zombies)
                    {
                        var plant_child = (Normal_Plants_Zombies)scene.Instance();
                        plant_child.player_put = true;
                        plant_child.Tmp_card_parent = this;
                        plant_child.Tmp_Card_Used = true;
                        GetNode<Control>("/root/In_Game/Object").AddChild(plant_child);
                    }
                    else
                    {
                        var plant_child = (Normal_Plants)scene.Instance();
                        plant_child.Tmp_card_parent = this;
                        plant_child.Tmp_Card_Used = true;
                        GetNode<Control>("/root/In_Game/Object").AddChild(plant_child);
                    }
                }
            }
        }
    }
    public void Hide_Shadow()
    {
        GetNode<ColorRect>("All/Main/Shadow").Hide();
    }
    public void Been_Used()
    {
        In_Game_Main.Belt_Number--;
        QueueFree();
    }
    public override void _Ready()
    {
        GetNode<AudioStreamPlayer>("All/Main/Sound/Seed").Stream.Set("loop", false);
        Position = new Vector2(770, 0);
        In_Game_Main.Belt_Number++;
    }
    public override void _PhysicsProcess(float delta)
    {
        if (In_Game_Main.Sun_Number < this.Sun && !Public_Main.debuging)
        {
            GetNode<ColorRect>("All/Main/Shadow2").Show();
        }
        else
        {
            GetNode<ColorRect>("All/Main/Shadow2").Hide();
        }
        if (this.Position.x > 55 * this.GetIndex()) 
        {
            if (Public_Main.debuging)
            {
                Position -= new Vector2(55f, 0);
            }
            else
            {
                Position -= new Vector2(0.55f, 0);
            }
        }
        GetNode<CanvasLayer>("All/Info").Transform = this.GlobalTransform;
    }
}
