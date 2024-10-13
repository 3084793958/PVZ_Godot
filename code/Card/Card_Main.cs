using Godot;
using System;

public class Card_Main : Control
{//未知意义的[Export],等我想通了再说
    [Export] public bool Card_enabled = true;
    [Export] public bool Use_Normal = true;
    [Export] public int Sun = 1437;
    [Export] public float now_time = 0f;
    [Export] public float wait_time = 30f;
    [Export] public int Card_Background = 1;
    [Export] public int[] Card_Number = {1,1};
    public override void _Ready()
    {
        Card_Background = Card_Number[0];
        GetNode<Card_Click_Button>("Main/Main/Click_Button").sun = Sun;
        var Sun_Label = GetNode<Label>("Main/Main/Texture/Sun");
        var Shadow_Sun_Label = GetNode<Label>("Shadow/Texture/Sun");
        int Seed_Pos = -1;
        if (Card_Number[0] == 1)
        {
            if (Card_Number[1] <= Public_Main.Plant_list.Count)
            {
                if (Public_Main.Plant_list[Card_Number[1] - 1].Item1 == Card_Number[1])
                {
                    Seed_Pos = Card_Number[1] - 1;
                }
                else
                {
                    for (int i = 0; i < Public_Main.Plant_list.Count; i++)
                    {
                        if (Public_Main.Plant_list[i].Item1 == Card_Number[1])
                        {
                            Seed_Pos = i;
                            break;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < Public_Main.Plant_list.Count; i++)
                {
                    if (Public_Main.Plant_list[i].Item1 == Card_Number[1])
                    {
                        Seed_Pos = i;
                        break;
                    }
                }
            }
        }
        else
        {
            if (Card_Number[1] <= Public_Main.Zombies_list.Count)
            {
                if (Public_Main.Zombies_list[Card_Number[1] - 1].Item1 == Card_Number[1])
                {
                    Seed_Pos = Card_Number[1] - 1;
                }
                else
                {
                    for (int i = 0; i < Public_Main.Zombies_list.Count; i++)
                    {
                        if (Public_Main.Zombies_list[i].Item1 == Card_Number[1])
                        {
                            Seed_Pos = i;
                            break;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < Public_Main.Zombies_list.Count; i++)
                {
                    if (Public_Main.Zombies_list[i].Item1 == Card_Number[1])
                    {
                        Seed_Pos = i;
                        break;
                    }
                }
            }
        }
        if (Seed_Pos==-1)
        {
            Card_enabled = false;
            var Main_Texture = GetNode<Control>("Main/Main/Texture");
            Main_Texture.Hide();
            var Main_Button = GetNode<Button>("Main/Main/Click_Button");
            Main_Button.Hide();
            var Shadow_Main_Texture = GetNode<Control>("Shadow/Texture");
            Shadow_Main_Texture.Hide();
        }
        if (Card_enabled)
        {
            if (Use_Normal)
            {
                if (Card_Number[0] == 1)
                {
                    Sun = Public_Main.Plant_list[Seed_Pos].Rest.Item1;
                    now_time = Public_Main.Plant_list[Seed_Pos].Item6;
                    wait_time = Public_Main.Plant_list[Seed_Pos].Item7;
                }
                else
                {
                    Sun = Public_Main.Zombies_list[Seed_Pos].Rest.Item1;
                    now_time = Public_Main.Zombies_list[Seed_Pos].Item6;
                    wait_time = Public_Main.Zombies_list[Seed_Pos].Item7;
                }
            }
            Sun_Label.Text = Sun.ToString();
            Shadow_Sun_Label.Text = Sun.ToString();
            var Cd_Label = GetNode<Label>("Main/Main/Info/Info/1/2/3/4/Cd");
            Cd_Label.Text = "CD:" + now_time.ToString() + "s";
            var Name_Label = GetNode<Label>("Main/Main/Info/Info/1/2/3/4/Name");
            var Info_Label = GetNode<Label>("Main/Main/Info/Info/1/2/3/4/Info");
            var Main_Button = GetNode<Card_Click_Button>("Main/Main/Click_Button");
            Main_Button.now_time = this.now_time;
            Main_Button.wait_time = this.wait_time;
            GetNode<TextureRect>("Shadow/Texture/Background/1").Visible = Card_Background == 1;
            GetNode<TextureRect>("Main/Main/Texture/Background/1").Visible = Card_Background == 1;
            GetNode<TextureRect>("Shadow/Texture/Background/2").Visible = Card_Background == 2;
            GetNode<TextureRect>("Main/Main/Texture/Background/2").Visible = Card_Background == 2;
            var Main_Card_Texture = GetNode<TextureRect>("Main/Main/Texture/Main/Card_Texture");
            if (Card_Number[0] == 1)
            {
                Main_Card_Texture.Texture = Public_Main.Plant_list[Seed_Pos].Item5;
                Name_Label.Text = Public_Main.Plant_list[Seed_Pos].Item2;
                Info_Label.Text = Public_Main.Plant_list[Seed_Pos].Item3;
                Main_Button.plant_path = Public_Main.Plant_list[Seed_Pos].Rest.Item2;
            }
            else
            {
                Main_Card_Texture.Texture = Public_Main.Zombies_list[Seed_Pos].Item5;
                Name_Label.Text = Public_Main.Zombies_list[Seed_Pos].Item2;
                Info_Label.Text = Public_Main.Zombies_list[Seed_Pos].Item3;
                Main_Button.plant_path = Public_Main.Zombies_list[Seed_Pos].Rest.Item2;
                Main_Button.is_zombies = true;
            }
            var Shadow_Main_Card_Texture = GetNode<TextureRect>("Shadow/Texture/Main/Card_Texture");
            Shadow_Main_Card_Texture.Texture = Main_Card_Texture.Texture;
        }
    }
}
