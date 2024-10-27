using Godot;
using System;

public class Setting_sound_HSlider : HSlider
{
    public void Update_This()
    {
        this.Value = Public_Main.sound_value;
    }
    public void Sound_HSlider_value_changed(float value)
    {
        Public_Main.sound_value = (int)value;
    }
}
