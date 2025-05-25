using Godot;
using System;
using System.Collections.Generic;
public class In_Game_Choose_Card_Last_Choose : Button
{
    public async override void _Pressed()
    {
        var Click = GetNode<AudioStreamPlayer>("/root/In_Game/button_Click");
        Click.Play();
        var Choose_Card_Bank_Object = GetNode<HBoxContainer>("/root/In_Game/Main/Card/SeedBank/Seed");
        if (Choose_Card_Bank_Object.GetChildCount() > 0)
        {
            int for_time = Choose_Card_Bank_Object.GetChildCount();
            for (int i = 0; i < for_time; i++)
            {
                Choose_Card_Bank_Object.GetChild(for_time - i - 1).GetNode<Card_Click_Button>("Main/Click_Button").Button_Pressed();
            }
            Public_Main.now_card_number = 0;
            return;
        }
        ConfigFile file = new ConfigFile();
        Error error = file.Load("user://Users/" + Public_Main.user_name + "/Card_Data.cfg");
        if (error == Error.Ok)
        {
            int get_number = 0;
            while (true)
            {
                List<int> ans_list = new List<int>();
                ans_list.Add((int)file.GetValue("Plants", get_number.ToString(), -1));
                ans_list.Add((int)file.GetValue("Zombies", get_number.ToString(), -1));
                ans_list.Add((int)file.GetValue("Plants_Zombies", get_number.ToString(), -1));
                ans_list.Add((int)file.GetValue("Spec_Plants", get_number.ToString(), -1));
                get_number++;
                int ans_type = -1;
                int ans_number = -1;
                for (int i = 0; i < ans_list.Count; i++)
                {
                    if (ans_list[i] != -1)
                    {
                        ans_type = i + 1;
                        ans_number = ans_list[i];
                    }
                }
                ans_list.Clear();
                if (ans_type == -1 || ans_number == -1)
                {
                    break;
                }
                var Card_Page_Object = GetNode<Control>("/root/In_Game/Main/Choose_Card/Background/Page");
                for (int i = 0; i < Card_Page_Object.GetChildCount(); i++)
                {
                    var Page_Object = Card_Page_Object.GetChild<Control>(i);
                    for (int j = 0; j < Page_Object.GetChildCount(); j++)
                    {
                        var Card_Object = Page_Object.GetChild<Card_Main>(j);
                        if (Card_Object.Card_Number[1] == ans_number && Card_Object.Card_Number[0] == ans_type)
                        {
                            Card_Object.Card_Click_Button_Object.Button_Pressed();
                            await ToSignal(GetTree().CreateTimer(0.03f), "timeout");
                        }
                    }
                }
            }
        }
    }
}
