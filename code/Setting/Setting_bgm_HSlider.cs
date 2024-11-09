using Godot;
using System;

public class Setting_bgm_HSlider : HSlider
{
    public void Update_This()
    {
        this.Value = Public_Main.bgm_value;
    }
    public void Bgm_HSlider_value_changed(float value)
    {
        Public_Main.bgm_value = (int)value;
    }
}
