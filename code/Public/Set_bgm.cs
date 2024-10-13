using Godot;
using System;

public class Set_bgm : AudioStreamPlayer
{
    public override void _Ready()
    {
        this.VolumeDb = (float)(10 * Math.Log((float)Public_Main.bgm_value / 100) / Math.Log(10));
        this.PauseMode = PauseModeEnum.Process;
    }
    public override void _Process(float delta)
    {
        this.VolumeDb = (float)(10 * Math.Log((float)Public_Main.bgm_value / 100) / Math.Log(10));
    }
}
