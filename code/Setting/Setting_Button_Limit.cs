using Godot;
using System;

public class Setting_Button_Limit : Button
{
    public void Update_This()
    {
        this.Pressed = Public_Main.Using_Clone_Limit;
    }
    public override void _Pressed()
    {
        var Click = GetNode<AudioStreamPlayer>("/root/Setting/Setting/button_Click");
        Click.Play();
        Public_Main.Using_Clone_Limit = this.Pressed;
        ConfigFile file = new ConfigFile();
        file.SetValue("Setting", "debug", Public_Main.debuging);
        file.SetValue("Setting", "Android", Public_Main.for_Android);
        file.SetValue("Setting", "Limit", Public_Main.Using_Clone_Limit);
        file.Save("user://Users/" + Public_Main.user_name + "/Develop_Setting.cfg");
    }
    public void Limit_LineEdit_text_changed(string New_Text)
    {
        if (!New_Text.Empty() && New_Text != null) 
        {
            int Text_Number = 0;
            if (int.TryParse(New_Text, out Text_Number))
            {
                if (Text_Number > 0)
                {
                    Public_Main.Max_Object_Clone_In_F = Text_Number;
                    ConfigFile file = new ConfigFile();
                    file.SetValue("Setting", "debug", Public_Main.debuging);
                    file.SetValue("Setting", "Android", Public_Main.for_Android);
                    file.SetValue("Setting", "Limit", Public_Main.Using_Clone_Limit);
                    file.SetValue("Setting", "Limit_Number", Public_Main.Max_Object_Clone_In_F);
                    file.Save("user://Users/" + Public_Main.user_name + "/Develop_Setting.cfg");
                }
            }
        }
    }
}
