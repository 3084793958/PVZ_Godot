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
        Public_Main.Save_Value();
    }
    public void Limit_LineEdit_text_changed(string New_Text)
    {
        if (!New_Text.Empty() && New_Text != null)
        {
            if (int.TryParse(New_Text, out int Text_Number))
            {
                if (Text_Number > 0)
                {
                    Public_Main.Max_Object_Clone_In_F = Text_Number;
                    Public_Main.Save_Value();
                }
            }
        }
    }
}
