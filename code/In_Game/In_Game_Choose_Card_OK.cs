using Godot;
using System;

public class In_Game_Choose_Card_OK : Button
{
    static public bool Has_Pressed = false;
    public override void _Ready()
    {
        Has_Pressed = false;
    }
    public async override void _Pressed()
    {
        Has_Pressed = true;
        var Click = GetNode<AudioStreamPlayer>("/root/In_Game/button_Click");
        Click.Play();
        ConfigFile file = new ConfigFile();
        var Choose_Card_Bank_Object = GetNode<HBoxContainer>("/root/In_Game/Main/Card/SeedBank/Seed");
        for (int i = 0; i < Choose_Card_Bank_Object.GetChildCount(); i++)
        {
            var res_object = Choose_Card_Bank_Object.GetChild(i).GetNode<Card_Click_Button>("Main/Click_Button").Card_Node.GetNode<Card_Main>(".");
            if (res_object.Card_Number[0] == 1)
            {
                file.SetValue("Plants", i.ToString(), res_object.Card_Number[1]);
            }
            else if (res_object.Card_Number[0] == 3)
            {
                file.SetValue("Plants_Zombies", i.ToString(), res_object.Card_Number[1]);
            }
            else if (res_object.Card_Number[0] == 4)
            {
                file.SetValue("Spec_Plants", i.ToString(), res_object.Card_Number[1]);
            }
            else if (res_object.Card_Number[0] == 2)
            {
                file.SetValue("Zombies", i.ToString(), res_object.Card_Number[1]);
            }
        }
        file.Save("user://Users/" + Public_Main.user_name + "/Card_Data.cfg");
        var player1 = GetNode<AnimationPlayer>("/root/In_Game/Main/Card_Player");
        var player2 = GetNode<AnimationPlayer>("/root/In_Game/Main/AnimationPlayer");
        player1.Play("ok");
        await ToSignal(player1, "animation_finished");
        player2.Play("R_to_C");
        await ToSignal(player2, "animation_finished");
        In_Game_Main.Game_Start_Effect(this);
    }
}
