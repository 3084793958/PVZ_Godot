using Godot;
using System;
using System.Collections.Generic;
public class In_Game_Choose_Card_Random_Choose : Button
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
        GD.Randomize();
        var Card_Page_Object = GetNode<Control>("/root/In_Game/Main/Choose_Card/Background/Page");
        int all_card_number = 0;
        for (int i = 0; i < Card_Page_Object.GetChildCount(); i++)
        {
            all_card_number += Card_Page_Object.GetChild(i).GetChildCount();
        }
        List<int> ans_type_list = new List<int>();
        List<int> ans_number_list = new List<int>();
        while (true)
        {
            int card_id_get = (int)(GD.Randi() % all_card_number);
            int ans_type = 0;
            while (card_id_get > 50 * ans_type + 49) 
            {
                ans_type++;
            }
            int ans_number = card_id_get - 50 * ans_type;
            bool can_add = true;
            for (int i = 0; i < ans_number_list.Count; i++)
            {
                if (ans_number_list[i] == ans_number)
                {
                    if (ans_type_list[i] == ans_type)
                    {
                        can_add = false;
                        break;
                    }
                }
            }
            if (can_add)
            {
                ans_type_list.Add(ans_type);
                ans_number_list.Add(ans_number);
            }
            if (ans_number_list.Count >= Public_Main.allow_card_number)
            {
                break;
            }
        }
        for (int i = 0; i < ans_number_list.Count; i++)
        {
            Card_Page_Object.GetChild(ans_type_list[i]).GetChild<Card_Main>(ans_number_list[i]).Card_Click_Button_Object.Button_Pressed();
            await ToSignal(GetTree().CreateTimer(0.03f), "timeout");
        }
    }
}
